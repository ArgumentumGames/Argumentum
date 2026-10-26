# Pool #458 v23 — grain ㉞ (g35) : tirets typographiques — inventaire par colonne × langue (mesure, 0 écriture)

**Date** : 2026-10-03 · **Branche** : `docs/458-g35-tirets-mesure` (depuis master, non empilée) · **Machine** : myia-po-2024
**Dispatch** : c.5968567990 item 7 — « tirets typographiques — mesure 0 écriture (106 lignes CSV " - "), compter par colonne × langue, correction PR à part ».
**Écritures CSV : 0** (`git status --porcelain Cards/` vide).

> **Erratum d'exécution (03/10, grain 2 du dispatch c.5971513525, PR #1736 de typographie des dialogues ; recompté au renvoi c.5974397377, 04/10).** Le volet **deck** de cet inventaire est exécuté : **31 cellules** — dialogues **17** (974 rétabli en 3 lignes ru/pt/es/ar/fa/zh : `— ` espacé, **es `—` collé usage RAE**, zh `——` doublé ; 943 en 2 lignes ×7 langues non-fr, es collé + « —¡…! » ; **813 découpage par langue conservé**, marqueurs normalisés es/fa/ru/zh — l'archive imprimée v3 ne porte pas les marques ru, « — » ajouté en ligne comme le FR) ; **12 tirets ASCII → cadratin espacé** (example_ru 51/121/182/726/735/784/844/1388, example_pt et example_ar 1388, desc_ru 989/1398) ; 784 desc_en/desc_ar énumérations en demi-cadratin ` – `. **en 658/796 restent en forme espacée de master** : le tiret y suit un point final (changement d'interlocuteur / glose), pas une incise — le collé était une erreur d'arbitrage, retirée au renvoi ; il ne se justifie qu'au milieu d'une phrase (847/855/1388, non touchés). ⚠️ Comptes initiaux erronés (« 35 cellules », « dialogues 19 ») : le blob portait 33 cellules, les dialogues 17, le total après retrait 31. ⚠️ La worklist mesurait example_ru à 10 : **361 était déjà soldé par #1725** (tiret long arbitré c.5968567847) ; sur les 9 restantes, 974 passe par le rétablissement du dialogue (comptée dans les 17), les **8 autres** sont converties cadratin espacé. Garde : `CorpusDialogueTypographyGuardTests` (8 faits, dont un balayage zéro ` - `/`.-` sur les 16 colonnes du deck). Restent pour le grain « hors-deck » : les 84 rangées non imprimées (dont les doublons 1055/1341 de 51/121 example_ru, ASCII conservé volontairement), les 37 cellules Remarques, **et les 24 sauts de ligne intra-phrase — PR à part depuis master après le merge de #1736** (pool c.5974403468). La mesure ci-dessous demeure l'état pré-exécution.

---

## 1. Réconciliation du chiffre du dispatch (106)

| périmètre | compte |
|---|---|
| lignes **brutes** du fichier contenant ` - ` | **106** (le chiffre du dispatch ✓ — une cellule CSV entre guillemets peut s'étaler sur 2 lignes physiques) |
| **rangées** CSV concernées | 104 |
| … dont deck (`carte` non vide, imprimé) | **20** |
| … dont hors-deck (non imprimé) | 84 |

Cellules ` - ` toutes rangées confondues (119) : example_ru 56 · Remarques 37 · desc_ru 10 · example_ar 6 · example_pt 3 · example_fa 2 · example_fr 1 · text_fa 1 · desc_en 1 · desc_ar 1 · text_ru 1. **Le gros du volume est hors-deck ou interne** (Remarques) : la surface imprimée est petite.

## 2. Le deck (175) — cellules par colonne × patron

| colonne | ` - ` ASCII | ` — ` espacé | `—` collé (total) | `——` doublé |
|---|---:|---:|---:|---:|
| desc_en | 1 | 0 | 0 | 0 |
| desc_ru | 2 | 0 | 0 | 0 |
| desc_ar | 1 | 0 | 0 | 0 |
| example_en | 0 | 2 | 7 | 0 |
| example_ru | **10** | **13** | 13 | 0 |
| example_pt | 1 | 2 | 3 | 0 |
| example_es | 0 | 2 | 2 | 0 |
| example_ar | 1 | 3 | 3 | 0 |
| example_fa | 1 | 1 | 2 | 0 |
| example_zh | 0 | 0 | 5 | 5 |
| Remarques (interne, non rendu) | 11 | 0 | 0 | 0 |

Tiret demi-cadratin ` – ` : **0 cellule** dans toutes les colonnes du deck (mesuré). FR (`title`/`desc`/`example` nus) : **zéro tiret de toute espèce** sur le deck — la ponctuation forte française passe par « : » / « ; ».

### Worklist PK (matière de la PR de correction, à décider)

- **example_ru ` - ` (10)** : 51, 121, 182, 361, 726, 735, 784, 844, 974, 1388 — disjoint de l'ensemble em-dash (aucune rangée ne mélange les deux) :
  ` — ` (13) : 595, 596, 658, 698, 796, 804, 833, 834, 847, 855, 942, 1301, 1355
- **desc ` - ` (4)** : desc_en 784 · desc_ru 989, 1398 · desc_ar 784 (784 touche desc_en ET desc_ar — rangée jumelle de la série exemples)
- **example_en** : espacé 658, 796 · collé-sans-espace 813, 847, 855, 974, 1388
- **example_pt** : ` - ` 1388 · ` — ` 658, 796 · collé-sans-espace (1388 contient les deux formes)
- **example_es** : ` — ` 658, 796 (tout espacé — conforme à l'usage du rayado)
- **example_ar** : ` - ` 1388 · ` — ` 658, 796, 813
- **example_fa** : ` - ` 974 · ` — ` 658 · collé-sans-espace 1388
- **example_zh** : 658, 796, 847, 848, 1388 — les 5 cellules à tiret portent toutes le `——` doublé standard
- **Remarques** (11, interne) : 55, 713, 844, 1314, 1360, 1361, 1362, 1365, 1373, 1388, 1398

## 3. Lecture (mesurée, pas arbitrée)

1. **Le russe porte toute l'incohérence** : 10 cellules tiret ASCII contre 13 cellules tiret cadratin espacé, **pour le même rôle grammatical** (tiret-copule et tiret d'incise) — p. ex. 51 « Дети - это монстры… », 121 « Религия - это личное дело… », 182 « Если «орел» - я выиграл. » face à 595 « … — … ». Deux ensembles disjoints : la normalisation ru est mécanique une fois la cible décidée (` — ` espacé, usage dominant du corpus).
2. **658 / 796 = famille transversale** : leurs exemples portent ` — ` espacé en en/ru/pt/es/ar/fa et `——` en zh — ce sont les rangées à calembour/dialogue (« avocat »-avocat). Le tiret cadratin est là la ponctuation du dialogue traduite uniformément — pas un défaut.
3. **zh est conforme** : le 破折号 standard `——` (doublé, collé) est la seule forme utilisée.
4. **en mélange les espacements** : 2 cellules espacées (658, 796) contre 5 collées (813, 847, 855, 974, 1388) — l'usage américain ferme le cadratin (« word—word ») ; la correction éventuelle = 2 cellules, sens inverse de ru.
5. **Le côté définitions est quasi vierge** : 4 cellules ASCII seulement, zéro cadratin — la question des tirets est une question d'exemples.
6. **La règle « ne pas confondre avec les traits d'union » tient** : `contre-exemple`, `sous-famille` etc. sont collés (sans espaces) et ne matchent aucun des patrons ci-dessus — les trois patrons espacés/typographiques ne capturent que la ponctuation, pas la composition.

## 4. Ce que le grain n'établit pas

- **Aucune décision typographique** : ASCII→cadratin (ru), espacé→collé (en) ou l'inverse sont des arbitrages de langue — la PR de correction (à part, après arbitrage) a ici sa worklist complète, PK par PK.
- **Hors-deck (84 rangées) et Remarques (37 cellules) ne sont pas une worklist** : non imprimés, ils ne bougent que si l'owner décide une normalisation du corpus entier, pas du deck.
- Les tirets dans les **autres corpus** (Scenarii, Rules, Virtues) ne sont pas mesurés ici — périmètre = `Argumentum Fallacies - Taxonomy.csv`.

## 5. Reproductibilité (0 écriture)

```bash
python - <<'EOF'
import csv, io
rows = list(csv.DictReader(io.open('Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv',
                                   encoding='utf-8-sig', newline='')))
deck = [r for r in rows if (r.get('carte') or '').strip()]
pats = {'ASCII': ' - ', 'EM espace': ' — ', 'EM total': '—', 'EM double': '——'}
for col in ['desc_en','desc_ru','desc_ar','example_en','example_ru','example_pt',
            'example_es','example_ar','example_fa','example_zh','Remarques']:
    for name, pat in pats.items():
        pks = [r['PK'] for r in deck if pat in (r.get(col) or '')]
        if pks: print('%-12s %-9s %d : %s' % (col, name, len(pks), ' '.join(pks)))
EOF
# 106 lignes brutes : grep -c ' - ' sur le fichier (attention BOM/quotage multi-lignes)
git status --porcelain Cards/   # vide
```

Deck = 175 rangées `carte` non vide ; hors-deck (65, 476, 1300) hors périmètre.

---

*po-2024*
