# #1472 — grains ③ et ④ : chaîne versionnée, premier lot comparable

**Émetteur** : po-2023 (worker). **⛔ Mesure, pas un verdict** — le verdict visuel
est à ai-01, la direction artistique à l'owner seul. Aucune image de ce lot
n'entre dans le deck : le choix des couvertures appartient à l'owner.

## Ce que ce dossier contient

| Fichier | Rôle |
|---|---|
| `run-candidates.py` | construit le graphe API, poste à `/prompt`, attend, télécharge. C'est le workflow **versionné** : il n'existe aucun JSON de workflow à côté de lui, donc rien qui puisse dériver. |
| `postprocess-ink.py` | seuil → alpha → teinte. Déterministe, sans rééchantillonnage. |
| `prompts/cover-kids-01.txt` | le prompt de scène du lot ④, tel que soumis. |

## Lignée : ce qui change par rapport aux essais de septembre

| Essai | Ancre de style | Fond | Sortie |
|---|---|---|---|
| **A** (refusé, c.5770144239) | 2 images, ancre inversée | crème chargé | couleur, opaque |
| **A′** (`A2_20260922.png`, sha256 `7634dacc…cc8ee6`) | `rules-kids.png` seule — registre **moderne minoritaire** | rouge plein cadre | couleur, opaque |
| **③④ (ce lot)** | **12 références médianes** du deck (8 caricature, 4 gravure) | blanc | **encre noire sur blanc**, détourée puis teintée |

La scène est celle de A et A′ : des enfants attablés jouent aux cartes
(couverture de l'*École des menteurs*). Deux écarts, tous deux voulus :

1. **Les ancres sont les médianes du deck**, pas `rules-kids.png` : l'analyse du
   25/09 (c.5830131319) a établi que le registre du deck est à ~40 % de
   caricature de presse 1870-1920 et ~25 % de gravure XIXe, et que
   `rules-kids.png` est un registre moderne minoritaire. C'est la raison pour
   laquelle A′ n'a pas été retenu comme base.
2. **Aucune couleur n'est demandée au modèle.** Le fond rouge plein de A′
   disparaît : la teinte est appliquée **après**, par le script, à l'hexadécimal
   exact de la famille. La couleur devient exacte par construction au lieu d'être
   approximée par le modèle.

## La chaîne (telle qu'exécutée)

| Élément | Valeur | Licence |
|---|---|---|
| Unet | `qwen-image-edit-2511-Q4_K_M.gguf` | Apache-2.0 (carte HF, lue 21/09) |
| Accélération | LoRA `Qwen-Image-Edit-2511-Lightning-4steps-V1.0-bf16`, force 1.0 | Apache-2.0 (idem) |
| Encodeur texte+vision | `qwen_2.5_vl_7b_fp8_scaled`, type `qwen_image`, sur **CPU** | Apache-2.0 (idem) |
| VAE | `qwen_image_vae.safetensors` | Apache-2.0 (idem) |
| Échantillonnage | 4 pas, cfg 1.0, euler/simple | — |
| Résolution | 832 × 1152 (ratio 0,72 ≈ cover Rules) | — |
| Conteneur | `comfyui-qwen`, ComfyUI 0.36.0, RTX 3090 24 Go (GPU 1) | — |

**Licence lue sur les poids, pas sur une fiche de modèle** : sans cette lecture,
le grain ne produit rien (garde #1472). Les trois composants sont Apache-2.0 ;
la chaîne SDXL/openrail++ n'est pas dans ce chemin.

### Invariant du négatif

Le négatif est **inerte à cfg = 1.0** : Lightning n'a pas de CFG, la sortie ne
dépend que du conditionnement positif. Le graphe en câble un quand même
(`CLIPTextEncode`) pour le traçage et pour tout run futur à cfg > 1, mais
**toute contrainte — l'anti-texte en premier — n'existe que si elle est écrite
dans le prompt positif**. C'est un invariant de la chaîne, pas une préférence.

Le schéma de ce build (relevé sur `/object_info` le 15/11) : l'encodeur
`TextEncodeQwenImageEditPlus` prend `clip` et `prompt` (requis), `vae` et
`image1..image3` (optionnels) — il **n'a pas d'entrée `negative_prompt`** et
rend un seul CONDITIONING. Le négatif passe donc par un `CLIPTextEncode`
distinct. Les références sont des entrées IMAGE : chacune est un nœud
`LoadImage` câblé par `["id", 0]`.

## Les références médianes

Douze faces du deck, listées dans `references_mediane_12.txt` (analyse du
25/09) : huit de registre caricature — `1.2.1.2.1`, `1.3`, `2.2.3.1.2`,
`3.1.1.1.1`, `4.3.1.2`, `5`, `5.2.1.3`, `4.3.1.1.1` — et quatre de registre
gravure — `1.2.2.4`, `7`, `3.2.1`, `5.3.3`.

Le lot alterne les paires (`--cycle-refs`) : le candidat *i* prend
`refs[i % 12]` en `image1` (ancre de style) et `refs[(i+1) % 12]` en `image2`.
Sur 24 candidats, chaque référence porte donc l'ancre deux fois. C'est ce qui
rend le lot **comparable** à A et A′ — même scène, même chaîne — tout en
montrant l'éventail réel du registre du deck plutôt qu'un seul point de celui-ci.

## Post-traitement : seuil, alpha, teinte

```
L     = 0,299 R + 0,587 G + 0,114 B          (luminance 8 bits)
alpha = clamp((blanc − L) / (blanc − noir))  (défauts 235 / 40)
RGB   = --ink, uniforme sur tous les pixels
```

Sortie RGBA. La fonction est **pure** : mêmes octets d'entrée et mêmes
paramètres ⇒ sortie identique au bit près, sur n'importe quelle machine (aucun
dithering, aucun rééchantillonnage). `--stats` imprime les déciles d'alpha et
la couverture : une planche blanche ou non détourée se lit dans l'histogramme
avant qu'on la regarde, et le script le signale lui-même au-delà de 99 % dans
un seul seau.

Les deux bornes sont le seul réglage : `--black` est la luminance qui donne un
alpha plein, `--white` celle qui donne un alpha nul. Tout ce qui tombe entre les
deux reste une rampe — c'est ce qui conserve les hachures.

## Doctrine d'exécution (héritée, non négociable)

- **File vide vérifiée avant posting** (`GET /queue`) ; le script refuse de
  poster derrière un job existant. Un run mis en file derrière un job
  co-locataire CoursIA a fait tomber le conteneur deux fois en 12 h (septembre).
- **Runs solo.** Pas de parallélisme, pas de file partagée.
- **Un redémarrage du conteneur est un geste sur une infra partagée** : il
  s'annonce au dashboard workspace **avant**.
- **Aucune API payante** dans ce chemin : tout est local.
- **Aucune image n'entre dans le deck sans sélection de l'owner.**

## Le plafond de VRAM, et pourquoi le débit est bimodal

Formule lue dans le conteneur (`comfy/model_management.py:1025`) :
`usable = mémoire_libre − minimum_memory_required` (le ratio 0,4 est neutralisé
sur NVIDIA, `:459`). Les poids font 12 739 Mo.

| État du serveur | usable | Conséquence | Débit mesuré |
|---|---:|---|---|
| Fraîchement redémarré | 14 358 Mo | chargé complètement | 4 pas ≈ 64 s, **256 s/image** |
| Usé | 9 903 Mo | 2 927 Mo offloadés | 7-12 min/pas, **≈ 34 min/image** |

`POST /free` **ne restaure pas** cette comptabilité : seul un redémarrage du
conteneur le fait. La dégradation (~4,5 Go) vit dans l'état CUDA/allocator du
processus serveur. Leviers : redémarrer avant un lot (annoncé), `--highvram`
(décision owner sur l'infra), ou dimensionner le lot au premier run
post-redémarrage.

⚠️ La VRAM libre au moment du lot dépend aussi de ce que l'hôte y met : le
15/11 à 21:55, GPU 1 = 15 157 / 24 576 Mo utilisés, soit **9 419 Mo libres**
avant chargement — le régime « usé » est donc le pronostic, à confirmer par la
mesure du premier candidat.

## Reproduire

```powershell
# 1. un candidat de calibration, chronométré
python -I tools/1472-cover-chain/run-candidates.py `
  --refs <ref1.png> <ref2.png> `
  --prompt-file tools/1472-cover-chain/prompts/cover-kids-01.txt `
  --seeds 20261116 --out-dir <sortie>

# 2. le lot, paires tournantes
python -I tools/1472-cover-chain/run-candidates.py `
  --refs @(les 12) --cycle-refs `
  --prompt-file tools/1472-cover-chain/prompts/cover-kids-01.txt `
  --seeds <24 graines> --out-dir <sortie>

# 3. la teinte d'une famille
python -I tools/1472-cover-chain/postprocess-ink.py `
  <sortie>/argu1472_s<graine>.png <sortie>/argu1472_s<graine>-ink.png `
  --ink 1A1A1A --stats
```

Le jeton Bearer se lit dans le fichier de secrets du conteneur ; il n'est jamais
affiché, jamais journalisé, jamais commité.

## Ce que ce grain ne fait pas

- Il ne choisit pas la couverture : il produit des candidats comparables.
- Il ne fixe pas la couleur d'encre définitive : `--ink` est un paramètre, pas
  une constante du dépôt — l'hexadécimal vient de la famille, qui est une
  décision d'owner.
- Il ne touche ni au deck, ni aux PDF, ni à la régénération.

## Mesures de ce lot

À compléter par la mesure du run de calibration (voir le commentaire de
livraison sur #1472) : régime du serveur, durée par image, nombre de candidats
effectivement produits, et répartition d'alpha de la planche après teinte.