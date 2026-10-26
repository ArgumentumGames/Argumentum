# #457 T2 — brique 2 : pivots CSV app 33 prouvés (une entité), volume livré, worklist mesurée

**Statut : exécution de la brique 2 de T2 (`to-csv`), 0 appel payé, 0 écriture préprod —
fichiers locaux uniquement.** Suite directe de #1837 (pipeline, mergé) et #1843 (export app 33,
mergé). Le geste demandé : *« un `to-csv` par content-type sur cet export, preuve sur une
entité avant le volume »* (po-2023, 09/10 19:05Z).

## 1. Preuve sur une entité — 3 témoins, comptes exacts

Témoin choisi **réel et rendu** par content-type (les entités démo 2sxc sont exclues du choix,
cf. manifeste §rendering) :

| Content-type | Témoin | Rangées prédites | Rangées obtenues |
|---|---|---:|---:|
| Content (348) | ent 11392 « Les règles d'Argumentum » | 4 | **4** |
| Link (352) | ent 10076 « MindMap de la Taxonomie des arguments fallacieux » | 7 | **7** |
| Video (358) | ent 9905 « Shortlink Test » | 2 | **2** |

**Sets prose vérifiés champ à champ sur les 13 rangées témoins** : exactement les bons champs
sont éligibles à la traduction. Les rangées non-prose sont **présentes** (la présence encode
« attribut posé ») mais **non éligibles** : `Image` (`file:800`), `Link`
(`/Portals/1/Downloads/…`), `Window` (`auto`), `Icon`, `VideoLink` (`https://youtu.be/…`) —
références d'assets et réglages, pas de la prose.

## 2. Round-trip `to-xml` — le pivot redevient un XML d'import pour UNE entité

```
to-csv  --entity 11392                          -> 4 rangées
to-xml  --entity <GUID réel> --unconfirmed-ok   -> 8 blocs <Entity> (une par culture)
```

Le XML produit porte la forme attendue (`<Entity Type="Content"><Guid>…</Guid>
<Language>fr-FR</Language><Title>…`) — sortie **locale**, rien d'injecté.

⚠️ **Piège d'instrument mesuré** : un `--entity` GUID **inventé** (reconstitué depuis le
préfixe affiché) produit un XML **vide mais valide, exit 0** — un vert silencieux de la même
famille que ceux déjà consignés. Le GUID se **lit dans l'export** (`entities[].EntityGUID`),
jamais ne se reconstitue. Mesuré : GUID réel `2538A3F0-9785-…`, 8 blocs ; GUID paddé `2538A3F0-
6E08-…`, 0 bloc.

## 3. Volume — les trois content-types

| Content-type | Rangées | Manifeste (#1843) |
|---|---:|---|
| Content | 116 | 116 valeurs |
| Link | 56 | 56 valeurs |
| Video | 30 | 30 valeurs |
| **Total** | **202** | **202** |

**Égalité exacte avec le manifeste** — une rangée par valeur stockée, la correspondance est
bijective.

## 4. La worklist de traduction, mesurée

| | rangées | prose | prose FR non vide | non-prose |
|---|---:|---:|---:|---:|
| Content | 116 | 87 | 57 | 29 |
| Link | 56 | 24 | 23 | 32 |
| Video | 30 | 15 | 10 | 15 |
| **Total** | **202** | **126** | **90** | **76** |

**Entités démo 2sxc** (marqueurs du manifeste : `Demo content` ×2, `Demo content without
Caption`, `Demo link`, `New video`, `Demo VIdeo 2`, `Mentions légales`/Musterfirma AG) :
**31 rangées** dont **19 prose**. La worklist **réelle** après exclusion candidates :
**126 − 19 = 107 rangées prose**, dont **~71 à FR non vide** ⇒ ≈ **500 unités de traduction**
pour 7 langues — le volume « faible » qu'annonçait le plan, très loin des 8 316 cellules resx.

⚠️ **Video est un content-type sans contenu réel** : **8/8 entités** sont démo/test
(`New video`, `Shortlink Test`, `Test` ×2, `Test 2` ×2, `2sxc Text`, `Demo VIdeo 2`) —
10 valeurs prose FR non vides dont 4 démo. Traduire Video ≈ ne rien traduire de servi.

⚠️ **Les valeurs `Text` portent du HTML échappé** (`&lt;p&gt;…&eacute;…`) : l'étape de
traduction devra gérer l'unescape/re-escape (le manifeste de rendu le fait déjà pour sa sonde).

## 5. Livrables et régénération

Les trois pivots sont committés sous
`release-validation/exports/DNN-Argumentum-export-app33-2026-10-09/pivots/`
(BOM + CRLF, conformes aux CSV du corpus). Ils sont **dérivés déterministes** de l'export
committé — régénération :

```
python tools/dnn_i18n/site_content_pipeline.py to-csv \
  --export docs/dnn-localization/release-validation/exports/DNN-Argumentum-export-app33-2026-10-09/12-app33-content-items.json \
  --out <pivots/content-pivot.csv>   # idem 14-…link…, 16-…video…
```

## 6. Routage

| Volet | État | Geste |
|---|---|---|
| Brique 2 (pivots) | ✅ **fait** (cette PR) | — |
| Remplissage des 7 cibles | ⏳ **owner-gated** (appel payé / traduction) | GO owner + clé nommée |
| Entités démo (19 rangées prose) | ❓ exclusion candidate | arbitrage owner avec le manifeste |
| Injection préprod (`to-xml` → import) | ⛔ hors ce grain | GO owner séparé, écriture préprod |

## Déclaré NON mesuré

- La qualité de traduction (aucune cible remplie — par construction, 0 avec au moins une
  traduction).
- L'injection : le `to-xml` de §2 est une sortie locale, **aucun** import n'a été exécuté en
  préprod.
- Que les 7 cultures provisionnées suffisent au rendu (non sondé ici).

---

## Sources

- `release-validation/exports/DNN-Argumentum-export-app33-2026-10-09/` — l'export #1843
  (manifeste : 47 entités, 202 valeurs, 0 dimensionnée)
- `tools/dnn_i18n/site_content_pipeline.py` — le pipeline #1837 (`to-csv`/`to-xml`,
  `TRANSLATE_FIELDS_BY_CONTENT_TYPE` fail-closed)
- `docs/dnn-localization/457-t2-app33-first-lot.md` — la brique 1 (#1837)
