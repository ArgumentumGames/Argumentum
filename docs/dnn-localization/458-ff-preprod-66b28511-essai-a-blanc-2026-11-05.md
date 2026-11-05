# FF préprod → `66b28511` — essai à blanc (2026-11-05, avant la fenêtre de jour)

> **Nature** : mesure seule, lecture git et HTTP. **Aucun geste servi** : la préprod n'est pas
> modifiée par ce document. L'avance du clone est annoncée pour le premier tick de jour
> (règle 10 h-17 h Paris, intervention préprod annoncée avant).

## 1. État mesuré au moment de l'essai (05/11, 06:30 local)

| Objet | Mesure |
|---|---|
| Tête du clone webroot | `8c74ee13` |
| Cible | `66b28511` (merges #2147 ru-RU `9a86b9e7`, #2149 ar-SA `66b28511`) |
| Arbre sale délibéré | **22 `M` + 49 `??` = 71 entrées** `git status --porcelain` |
| Empreinte de l'arbre sale | `sha256(sort(porcelain))` = `ed3c4242…a33a38e` |
| Delta `HEAD → 66b28511` | **7 fichiers**, dont **2 servis** |
| Collision delta ∩ arbre sale | **0** |
| Préprod | **8/8 accueils → 200** (fr, en, es, pt, ru, zh, ar, fa) |

Les 7 fichiers du delta, et leur portée :

- **servis (2)** — `DNNPlatform/App_GlobalResources/GlobalResources.ru-RU.resx`,
  `…ar-SA.resx` ;
- **hors service (5)** — 3 gardes de test (`DnnAgrCoreResxIntegrityGuardTests`,
  `DnnGlobalResourcesArSaGuardTests`, `DnnGlobalResourcesRuRuGuardTests`) et 2 notes
  `docs/dnn-localization/458-globalresources-{arsa,ruru}-30-cles-2026-11-04.md`.

⇒ L'avance ne change **que deux fichiers servis**, tous deux absents de l'arbre sale :
le FF est un fast-forward propre, sans conflit possible.

## 2. Correction d'une imprécision de mon annonce

L'annonce `[INFO]` du 05/11 parlait de « 70 fichiers dirty ». La mesure dit **22 modifiés
suivis + 49 non suivis**. Un compte est de toute façon le mauvais témoin (il ne dit pas
*lesquels*) : le DoD porte désormais sur l'**empreinte** de la liste triée, qui ne bouge
que si un chemin ou un statut change.

## 3. Séquence du geste (à exécuter dans la fenêtre de jour uniquement)

```bash
# 0. avant : capturer l'invariant
git -C <clone> status --porcelain | sort | sha256sum      # attendu ed3c4242…a33a38e

# 1. avance fast-forward (jamais de stash, checkout --, reset --hard sur ce clone)
git -C <clone> merge --ff-only origin/master

# 2. après : la tête ET l'invariant
git -C <clone> rev-parse --short HEAD                      # attendu 66b28511
git -C <clone> status --porcelain | sort | sha256sum      # attendu ed3c4242…a33a38e (inchangé)
```

Puis les deux contrôles de service : `git hash-object --path <f> <f>` sur chacun des 2
resx comparé à `git rev-parse 66b28511:<f>` (blob-égalité), et **8/8 accueils 200**.

## 4. ⚠️ À re-mesurer au moment du geste

Si master avance d'ici la fenêtre (par exemple #2151 fa-IR, ouverte, CI verte), la cible
n'est plus `66b28511` mais la **nouvelle tête** : le delta et l'empreinte de l'arbre sale
se re-mesurent **avant** le `merge --ff-only`, jamais sur la foi de ce document. L'invariant
d'arbre sale, lui, ne change pas tant qu'aucune main ne touche le clone.

⛔ Ce que le geste ne fait pas : aucun `stash`, aucun `checkout --`, aucun `reset --hard`,
aucun `iisreset` (le recycle vit sur la mtime de `web.config`, pas sur le SHA), aucun
`--clobber` d'asset de `v2.1.0-review`.

*po-2023 · instrument : `git rev-parse`/`status --porcelain`/`diff --name-only` + 8 sondes HTTP cache-buster*
