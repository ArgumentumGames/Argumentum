# Runbooks gated-ops — Index (pré-armement au GO jsboige)

> **But** : au GO jsboige, exécution des ops gated = **copier-coller, zéro réflexion**. Cet index consolide les 3 runbooks gated-ops DNN.
> **Contrainte** : rien d'exécuté sans GO jsboige explicite. DNN publish écrase `web.config` prod — ne pas publier. Verdict QA visuelle = ai-01.
> **Auteur** : po-2023 (dispatch ai-01 `msg-20260722T144728-h7msce`).

---

## Index des runbooks

| # | Runbook | Couverture | Statut |
|---|---------|-----------|--------|
| 1 | **Apply Manifests Δ #490/#682** | [`go-live-turnkey-checklist.md`](go-live-turnkey-checklist.md) | ✅ existant (prêt) |
| 2 | **Option C connection-string** | [`dnn-hang-option-c-connection-string-prep.md`](dnn-hang-option-c-connection-string-prep.md) | ✅ existant (prêt) |
| 3 | **Fix skin `tabid=138` (Opt 1)** | [`skin-tabid138-diagnostic-runbook.md`](skin-tabid138-diagnostic-runbook.md) | ⚠ **diagnostic-only** (fix diff deferred au GO, nécessite stack trace live) |

---

## Pré-requis commun aux 3 ops

- **Sauvegarde pré-op** horodatée + hashée (`web.config`, `bin/`, DB). Cf `[[reference-dnn-bin-restore-surgical]]` (restore chirurgical, pas rsync brutal).
- **Backup naming hygiene** : ne JAMAIS restaurer un backup sur la base du nom/date seul — **vérifier la connection string + le count de `dependentAssembly` AVANT** (`[[feedback-dnn-webconfig-bak-trap]]`). `web.config.bak-20260717` = pré-migration IIS, trompeur.
- **CS verification** : la connection string actuelle est `localhost\SQLEXPRESS` (correcte). Ne pas restaurer un backup LocalDB par erreur (`[[feedback-dnn-webconfig-secret-tracked]]`).
- **Runtime** : .NET Framework 4.8 (pas .NET 8/9 — les redirects 9.0.0.0 sont BCL 2sxc-21, cf `[[reference-dnn-2sxc-net48-bcl-stack]]`).

---

## Décisions jsboige en attente (bloqueurs DNN)

1. **Rotation machineKey** (urgent — clés exposées sur branche feature `4b0297ee`). Ops serveur seul.
2. **Skin `tabid=138` Opt 1/2** (reco Opt 1 = v0.9.1, tag non bloqué).
3. **#681 2sxc App export** (hard-unblocker DNN i18n #682-#685). Contenu vit dans DB 2sxc live, aucun worker ne peut le faire.

---

## Post-exec smoke

[`go-live-smoke-test.md`](go-live-smoke-test.md) — homepage 200, `/Argumentum` + `/Règles` rendent 2sxc content, 0 `JsonOptions`/conn-string error, `tabid=138` (post-fix Opt 1).
