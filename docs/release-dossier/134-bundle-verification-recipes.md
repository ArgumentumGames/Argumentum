# Bundle v2.0.0 — recettes de vérification (reproductibles par un tiers)

> **But** : permettre à un tiers de **refaire** les mesures du bundle sans croire personne sur
> parole. Ce document ne remplace pas le manifeste (`manifest-v2.0.0.md`, sha256 80/80) : le
> manifeste prouve la **provenance** ; ce document donne les **recettes** et nomme les pièges
> d'instrument qui font qu'une mesure verte peut ne rien prouver.
>
> **Auteur** : po-2023 (dispatch ai-01, [DONE] du 2026-09-13 01:2x). **Verdict** : ai-01 only.

---

## 1. Artefacts de référence

| Artefact | Chemin | Contenu |
|---|---|---|
| Bundle livré | `G:\Mon Drive\Argumentum\review-v2.0.0-regen-20260912` | 80 PDF (8 langues × 10 types), **3 745,6 Mo** |
| Manifeste | `<bundle>\manifest-v2.0.0.md` | 80/80, sha256 par fichier, base `65dd4742`, statut FINAL |
| Bundle précédent (**préservé**) | `G:\Mon Drive\Argumentum\review-v2.0.0-regen-20260911` | 80 PDF, 7 071,9 Mo, base `6108194d` |
| Commit de génération | `65dd4742` | arbre de régén `.regen-final-worktree` (recyclé depuis) |

Layout du bundle : `{lang}/Argumentum_{type}_{lang}.pdf` avec
`lang ∈ {ar,en,es,fa,fr,pt,ru,zh}` et `type` ∈ les 10 de `package-v2.0.0.ps1`.

---

## 2. Ce qui est re-mesurable — et ce qui ne l'est plus

⚠️ **À lire avant de conclure quoi que ce soit.**

- ✅ **Re-mesurable sur le bundle livré** : dimensions des objets image, nombre de pages,
  conformité CMYK, sha256 (via le manifeste).
- ❌ **Plus re-mesurable** : la sonde `/SMask = 0` **pré-CMYK**. Elle portait sur l'arbre de build
  `Target/` (le seul endroit où elle signifie quelque chose) ; **cet arbre a été recyclé par le
  recycleur de flotte** après la livraison. La mesure reste consignée, mais elle n'est plus
  reproductible.

C'est précisément pourquoi le critère de non-défectuosité du bundle doit être la **dimension**
(§3.1), pas la présence de `/SMask` (§4.1).

---

## 3. Recettes

### 3.1 Dimensions des objets image — **le critère qui survit**

```
python -c "
import fitz, collections, os
B = r'G:\Mon Drive\Argumentum\review-v2.0.0-regen-20260912'
p = os.path.join(B, 'fr', 'Argumentum_TarotCards_fr.pdf')
d = fitz.open(p)
c = collections.Counter()
for pno in range(len(d)):
    for i in d[pno].get_image_info(xrefs=True):
        c[(i['width'], i['height'])] += 1
print(len(d), 'pages'); [print(k, 'x', v) for k, v in c.most_common(5)]
"
```

**Attendu** : `826x1417` pour les documents cartes (Tarot / Virtues), `779x779` pour les cartes du
poster A0, plus une bannière `1316x475`.

**Valeurs enregistrées** (mesurées sur le bundle livré) :

| Document | Pages | Objets | Dimension |
|---|---:|---:|---|
| `Argumentum_TarotCards_fr.pdf` | 379 | 379 | **826×1417** (379/379) |
| `Argumentum_TarotCards_zh.pdf` | 379 | 379 | **826×1417** (379/379) — témoin non-latin |
| `Argumentum_TarotCards_Virtues_fr.pdf` | 262 | 262 | **826×1417** (262/262) |
| `Argumentum_Fallacies_Web_A0_fr.pdf` | 1 | 176 | **779×779** ×175 + `1316×475` ×1 |

**Le seuil défectueux à discriminer** : `1982×3401` (upsample nearest-neighbor ×991/413 du bug
720 dpi, corrigé par #1338) et, pour le poster A0, un raster unique `23 840×33 710`. La recette
ci-dessus rend ces valeurs impossibles à confondre avec les valeurs saines.

### 3.2 Conformité imprimeur (CMYK)

Le contrôle est **le log du post-traitement**, jamais le code de sortie :

- Chercher dans le log de la passe `--pdf-cmyk` la ligne
  `PDF CMYK post-process complete: N/80 converted` — **N = 80** attendu, **0 warning**.
- Sur le PDF : présence de `/DeviceCMYK` et de `/OutputIntent` (2 par fichier).

⛔ Ghostscript absent ⇒ **warning par PDF et exit 0** : un exit 0 est vert que la passe ait
converti 80 fichiers ou zéro.

### 3.3 Provenance (manifeste)

Recalculer les sha256 du bundle et les comparer au manifeste. **Contrôle inverse obligatoire** :
comparer un hash à la mauvaise langue doit rendre un MISMATCH — sinon le comparateur est aveugle.

---

## 4. Pièges d'instrument (chacun payé par un incident)

### 4.1 `/SMask = 0` est **aveugle** sur le PDF livré

L'aplatissement PDF/X par Ghostscript **consomme l'alpha**. Un bundle *connu défectueux* (720 dpi)
et un bundle correct mesurent donc `/SMask = 0` **à l'identique** — contrôlé en positif et en
négatif. Sur l'artefact livré, le seul discriminateur survivant est la **dimension** (§3.1).

Corollaire : la sonde `/SMask` de po-2023 sur l'arbre **pré-CMYK** `Target/` reste correcte **à son
emplacement** — les deux mesures ne se contredisent pas, elles ne portent pas sur le même objet.

### 4.2 `get_images()` déduplique — mesurer les **placements**

`page.get_images()` d'une planche de dos **homogène** (9 fois le même dos) rend **1** entrée, pas 9 :
les xrefs identiques sont dédupliqués. Pour compter les cartes réellement posées, utiliser
`page.get_image_info(xrefs=True)` (une entrée par **placement**, avec `bbox`).

C'est ce piège qui a fait conclure à tort « planche de dos = 1 image » avant calibration.

### 4.3 Un `grep` de chaîne sur un PDF est un instrument, pas une preuve

Un motif absent parce que le producteur ne l'écrit pas ressemble exactement à une non-conformité.
Exemple mesuré : la chaîne `SWOP` est **absente** des PDF produits alors que le profil
`CGATS TR 001` + `/GTS_PDFX` sont bien présents — un `grep SWOP` rendant 0 est une **panne
d'instrument**, pas un défaut du bundle.

### 4.4 Rejeu d'un comparateur contre un **autre** arbre

Variable d'environnement `ARGUMENTUM_PDF_BUNDLE_ROOT` (tests VisualTests) : force la racine de
bundle et **échoue bruyamment** si le chemin n'existe pas (jamais de repli silencieux). Layout
attendu : `{lang}/Documents/density-0/*.pdf` — un bundle publié **à plat** ne matche pas.

---

## 5. Ce que ce document ne fait pas

- ❌ Ne déclare pas de verdict (ai-01 only) et ne remplace pas le manifeste.
- ❌ Ne re-génère rien et ne re-mesure pas le pré-CMYK (arbre recyclé, §2).
- ❌ Ne tranche pas le couplage DNN (décision jsboige, §D3 de `DECISION-v2.0.0-jsboige.md`).
