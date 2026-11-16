#!/usr/bin/env python3
"""#1472 grain ③④ — ComfyUI candidate runner for Argumentum covers.

Versioned chain (Apache-2.0 models only, licences read on the weight cards,
2026-09-21, #1472 c.5766061660):

    Unet      qwen-image-edit-2511-Q4_K_M.gguf          (apache-2.0)
    LoRA      Qwen-Image-Edit-2511-Lightning-4steps      (apache-2.0), strength 1.0
    CLIP/VL   qwen_2.5_vl_7b_fp8_scaled  (type qwen_image, CPU)
    VAE       qwen_image_vae                              (apache-2.0)

Sampling invariants (measured 2026-09-22, c.5771451311 / CONDITIONS.md):
    4 steps, cfg 1.0, euler/simple — Lightning has NO CFG, so the negative
    prompt is inert: every constraint (anti-text above all) lives in the
    POSITIVE prompt or nowhere.

The API-format graph is built HERE, in code — there is no side-car workflow
JSON to drift from this file.

Usage:
    python run-candidates.py --refs <img> [<img> ...] --prompt-file PROMPT.txt \
        --seeds 20261116,20261117,... --out-dir OUT [--ink RRGGBB]

Reference images are uploaded via POST /upload/image before the run. The
queue must be EMPTY before posting (solo-run doctrine, 2026-09-22: a run
queued behind a co-tenant job crashed the container once).

Auth: bearer token read from the file at --token (no default; the path can
also come from the ARGUMENTUM_COMFYUI_TOKEN_FILE env var). Never printed,
never logged. Deliberately NO machine-specific default (AGENTS.md §6.4).
"""

import argparse
import io
import json
import os
import sys
import time
import urllib.error
import urllib.parse
import urllib.request

UNET = "qwen-image-edit-2511-Q4_K_M.gguf"
LORA = "Qwen-Image-Edit-2511-Lightning-4steps-V1.0-bf16.safetensors"
CLIP = "qwen_2.5_vl_7b_fp8_scaled.safetensors"
VAE = "qwen_image_vae.safetensors"
WIDTH, HEIGHT = 832, 1152  # Rules-cover ratio, same as essais A/A' (2026-09-22)
STEPS, CFG = 4, 1.0


def http(base, path, token, payload=None, method=None, timeout=30):
    url = base.rstrip("/") + path
    data = None
    headers = {"Authorization": "Bearer " + token}
    if payload is not None:
        data = json.dumps(payload).encode("utf-8")
        headers["Content-Type"] = "application/json"
    req = urllib.request.Request(url, data=data, headers=headers,
                                 method=method or ("POST" if data else "GET"))
    try:
        with urllib.request.urlopen(req, timeout=timeout) as resp:
            return json.loads(resp.read().decode("utf-8") or "{}")
    except urllib.error.HTTPError as exc:
        # ComfyUI answers 400 with node_errors when the graph is rejected —
        # that body is the whole diagnosis, so surface it instead of a bare code.
        body = exc.read().decode("utf-8", "replace")
        raise SystemExit(f"HTTP {exc.code} on {path}: {body[:4000]}") from exc


def build_graph(prompt_positive, prompt_negative, seed, ref_names):
    """API-format prompt graph.

    Schema read on this build's /object_info (2026-11-15): the encoder takes
    `clip`, `prompt` (required) and `vae`, `image1..image3` (optional) — there
    is NO `negative_prompt` input and a single CONDITIONING output, so the
    sampler's negative is a plain CLIPTextEncode. Reference images are IMAGE
    inputs: each one is a LoadImage node, wired as ["id", 0].
    """
    refs = list(ref_names[:2])
    graph = {
        "1": {"class_type": "UnetLoaderGGUF", "inputs": {"unet_name": UNET}},
        "2": {"class_type": "LoraLoaderModelOnly", "inputs": {
            "lora_name": LORA, "strength_model": 1.0, "model": ["1", 0]}},
        "3": {"class_type": "CLIPLoader", "inputs": {
            "clip_name": CLIP, "type": "qwen_image", "device": "cpu"}},
        "4": {"class_type": "VAELoader", "inputs": {"vae_name": VAE}},
        "10": {"class_type": "LoadImage", "inputs": {"image": refs[0]}},
        "5": {"class_type": "TextEncodeQwenImageEditPlus", "inputs": {
            "clip": ["3", 0], "prompt": prompt_positive, "vae": ["4", 0],
            "image1": ["10", 0]}},
        "11": {"class_type": "CLIPTextEncode", "inputs": {
            "clip": ["3", 0], "text": prompt_negative}},
        "6": {"class_type": "EmptySD3LatentImage", "inputs": {
            "width": WIDTH, "height": HEIGHT, "batch_size": 1}},
        "7": {"class_type": "KSampler", "inputs": {
            "model": ["2", 0], "positive": ["5", 0], "negative": ["11", 0],
            "latent_image": ["6", 0], "seed": seed, "steps": STEPS, "cfg": CFG,
            "sampler_name": "euler", "scheduler": "simple", "denoise": 1.0}},
        "8": {"class_type": "VAEDecode", "inputs": {
            "samples": ["7", 0], "vae": ["4", 0]}},
        "9": {"class_type": "SaveImage", "inputs": {
            "filename_prefix": "argu1472", "images": ["8", 0]}},
    }
    if len(refs) > 1:
        graph["12"] = {"class_type": "LoadImage", "inputs": {"image": refs[1]}}
        graph["5"]["inputs"]["image2"] = ["12", 0]
    return graph


def upload_ref(base, token, path):
    name = os.path.basename(path).replace(" ", "_")
    with open(path, "rb") as fh:
        blob = fh.read()
    boundary = "----argu1472boundary"
    body = io.BytesIO()
    body.write(("--%s\r\nContent-Disposition: form-data; name=\"image\"; "
                "filename=\"%s\"\r\nContent-Type: image/png\r\n\r\n"
                % (boundary, name)).encode("utf-8"))
    body.write(blob)
    body.write(("\r\n--%s--\r\n" % boundary).encode("utf-8"))
    req = urllib.request.Request(
        base.rstrip("/") + "/upload/image?overwrite=true", data=body.getvalue(),
        headers={"Authorization": "Bearer " + token,
                 "Content-Type": "multipart/form-data; boundary=" + boundary},
        method="POST")
    with urllib.request.urlopen(req, timeout=120) as resp:
        out = json.loads(resp.read().decode("utf-8"))
    return out["name"]


def main():
    ap = argparse.ArgumentParser(description=__doc__)
    ap.add_argument("--base", default="http://127.0.0.1:8188")
    ap.add_argument("--token", default=os.environ.get("ARGUMENTUM_COMFYUI_TOKEN_FILE"),
                    help="path to the bearer-token file (or set "
                         "ARGUMENTUM_COMFYUI_TOKEN_FILE); no machine default")
    ap.add_argument("--refs", nargs="+", required=True,
                    help="reference images (median deck faces)")
    ap.add_argument("--prompt-file", required=True)
    ap.add_argument("--negative", default="no text, no letters, no title, "
                     "no watermark, no typography")
    ap.add_argument("--seeds", required=True,
                    help="comma-separated seed list")
    ap.add_argument("--cycle-refs", action="store_true",
                    help="rotate the reference pair across candidates "
                         "(candidate i uses refs[i%%n] + refs[(i+1)%%n]); "
                         "default: the first two refs for every candidate")
    ap.add_argument("--out-dir", required=True)
    ap.add_argument("--dry-run", action="store_true",
                    help="build and print the graph, post nothing")
    args = ap.parse_args()

    if not args.token:
        ap.error("--token is required (or set ARGUMENTUM_COMFYUI_TOKEN_FILE)")

    with open(args.token, encoding="ascii") as fh:
        token = fh.read().strip()

    with open(args.prompt_file, encoding="utf-8") as fh:
        prompt = fh.read().strip()

    os.makedirs(args.out_dir, exist_ok=True)
    seeds = [int(s) for s in args.seeds.split(",")]

    ref_names = []
    for r in args.refs:
        n = upload_ref(args.base, token, r)
        ref_names.append(n)
        print("uploaded ref:", n, flush=True)

    if args.dry_run:
        print(json.dumps(build_graph(prompt, args.negative, seeds[0], ref_names),
                         indent=1, ensure_ascii=False))
        return

    # Solo-run doctrine: refuse to post behind an existing job.
    queue = http(args.base, "/queue", token)
    pending = len(queue.get("queue_running", [])) + len(queue.get("queue_pending", []))
    if pending:
        print(f"REFUSING: queue not empty ({pending} job(s)) — solo-run doctrine",
              file=sys.stderr)
        sys.exit(3)

    for i, seed in enumerate(seeds):
        if args.cycle_refs and len(ref_names) > 1:
            n = len(ref_names)
            pair = [ref_names[i % n], ref_names[(i + 1) % n]]
        else:
            pair = ref_names
        graph = build_graph(prompt, args.negative, seed, pair)
        t0 = time.time()
        queued = http(args.base, "/prompt", token,
                      {"prompt": graph, "client_id": "argu1472-runner"})
        pid = queued["prompt_id"]
        print(f"seed {seed}: queued as {pid}", flush=True)
        failures = 0
        while True:
            time.sleep(5)
            try:
                hist = http(args.base, "/history/" + pid, token, timeout=60)
                failures = 0
            except (TimeoutError, urllib.error.URLError, OSError) as exc:
                # A 52-minute render (16/11) outlived one 30 s poll: a single
                # transient timeout must not kill the batch (measured: the
                # job COMPLETED server-side while the runner was crashing).
                failures += 1
                print(f"seed {seed}: poll failed ({exc}) {failures}/12",
                      file=sys.stderr, flush=True)
                if failures >= 12:
                    print(f"seed {seed}: giving up after 12 failed polls",
                          file=sys.stderr)
                    sys.exit(5)
                continue
            entry = hist.get(pid)
            if not entry:
                continue
            status = entry.get("status", {})
            if status.get("completed") or status.get("status_str") == "success":
                for node_out in entry["outputs"].values():
                    for img in node_out.get("images", []):
                        q = urllib.parse.urlencode(
                            {"filename": img["filename"],
                             "subfolder": img.get("subfolder", ""),
                             "type": img.get("type", "output")})
                        req = urllib.request.Request(
                            args.base.rstrip("/") + "/view?" + q,
                            headers={"Authorization": "Bearer " + token})
                        with urllib.request.urlopen(req, timeout=120) as resp:
                            blob = resp.read()
                        dest = os.path.join(args.out_dir,
                                            f"argu1472_s{seed}.png")
                        with open(dest, "wb") as fh:
                            fh.write(blob)
                        print(f"seed {seed}: saved {dest} "
                              f"({time.time()-t0:.0f}s)", flush=True)
                break
            if status.get("status_str") == "error":
                print(f"seed {seed}: ERROR {json.dumps(status)[:400]}",
                      file=sys.stderr)
                sys.exit(4)


if __name__ == "__main__":
    main()
