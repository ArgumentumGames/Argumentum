# #2182 — plan d'injection des 147 cellules : rejouable depuis le dépôt

**Date** : 2026-11-11 · **Lane** : po-2023 · **Pool** : #458 c.6803706574 · **Base** : master `53652ce5`
**Portée** : préprod uniquement. ⛔ **Ce plan n'a été exécuté nulle part** — l'injection est **gatée
sur la réponse de l'owner à Q-2026-11-09-2** (ou au plus tard l'annonce de la date de mise en ligne) ;
⛔ ai-01 ne lève pas ce gate. La fenêtre du 12/11 10h-17h Paris est **lecture seule**.

Ce document remplace le plan qui vivait en scratch (`hors dépôt`) : il ne porte **aucune chaîne de
connexion, aucune IP, aucun chemin de machine, aucun compte**. La connexion se lit du `web.config`
du checkout au moment du geste — précédent d'établissement : `tools/458-eav-content-export.py
--config DNNPlatform/web.config` fait exactement cela pour l'export.

## 1. Le delta, re-dérable du dépôt seul

La source de l'injection est le **registre** `docs/dnn-localization/eav-content.csv` — le registre
est le **producteur**, pas un miroir (leçon `458-inject42` : corriger la base sans repasser par le
pivot ré-injecte l'erreur à l'export suivant).

```bash
git diff 16126d26^..16126d26 -- docs/dnn-localization/eav-content.csv
```

`16126d26` = merge de **#2227** (10/11). Mesuré sur ce diff :

| Mesure | Valeur |
|---|---|
| Rangées logiques modifiées | **21** = 7 entités × 3 champs (`Title`, `Teaser`, `Content`) |
| Lignes physiques | 21 retirées / 1596 insérées (les champs portent du multi-ligne) |
| Cellules non vides changées | **147** = 21 × 7 langues (en, ru, pt, es, ar, fa, zh) |
| `fr` (dim-0) | **0 cellule changée** — les articles sont natifs FR |
| Volumétrie | **342 983 octets** |
| Rangées ajoutées/retirées | **0 / 0** |

Les 7 entités (type 325, app 52 — créées le 09/11, `2182-articles-actus-et-liens-mindmap-2026-11-09.md`,
EntityIds **12011-12017** attendus, à re-confirmer par la sonde 1) :

| GUID | EntityId attendu | Title (fr) | octets rangée |
|---|---:|---|---:|
| `2E10F534-92EC-41FD-A9BE-767FD992DB78` | 12017 | Le site Argumentum, en huit langues | 38 344 |
| `39355C1D-4E9E-47F3-822E-92B1B9880770` | 12012 | Chaque sophisme à sa place : l'ontologie d'Argumentum | 46 978 |
| `3F913BF3-F0F7-47F7-AB49-6256F76AA55E` | 12015 | Sophismes × scénarios : le jeu comme jeu de données | 57 229 |
| `84094BAE-CA49-4881-A99E-9F11E83671B1` | 12014 | Des carnets de cours lisent le jeu | 46 690 |
| `8EC968E1-8B0C-47CC-9B9C-FC2892CA6BEF` | 12016 | La dette d'irréversibilité du discours | 49 471 |
| `D6A2A0CF-B256-4545-AAFC-DF8D222E282B` | 12011 | Argumentum : du jeu de cartes au dépôt reproductible | 63 847 |
| `F2D87598-EEC5-44DA-A10B-E4CFEC2B8491` | 12013 | La descente guidée dans l'arbre : le plugin Argumentum | 40 424 |

Dimensions 2sxc (publiées au dépôt depuis l'injection app 59) : `7=en-US`, `8=ru-RU`, `9=pt-BR`,
`10=es-ES`, `11=ar-SA`, `12=fa-IR`, `13=zh-CN`.

## 2. Prérequis — 4 sondes en lecture seule (fenêtre annoncée, 10h-17h jour ouvré)

Chaque sonde tourne avec la connexion lue du `web.config` du checkout — jamais depuis ce document.

1. **GUID → EntityId** (valide la table du §1) :
   ```sql
   SELECT EntityId, EntityGuid FROM TsDynDataEntity
   WHERE EntityGuid IN ('2E10F534-92EC-41FD-A9BE-767FD992DB78', '39355C1D-4E9E-47F3-822E-92B1B9880770',
                        '3F913BF3-F0F7-47F7-AB49-6256F76AA55E', '84094BAE-CA49-4881-A99E-9F11E83671B1',
                        '8EC968E1-8B0C-47CC-9B9C-FC2892CA6BEF', 'D6A2A0CF-B256-4545-AAFC-DF8D222E282B',
                        'F2D87598-EEC5-44DA-A10B-E4CFEC2B8491')
   ```
2. **ValueIds par dimension** — ⚠️ la valeur de **base n'a pas de ligne de dimension** (piège mesuré,
   `LEFT JOIN` obligatoire, dim NULL = dim-0). ⚠️ Une rangée de langue a son **propre ValueId** —
   requêter `EntityId + dimension`, jamais « la » valeur de l'attribut :
   ```sql
   SELECT v.ValueId, v.EntityId, a.StaticName AS Attribute, d.DimensionId, d.SystemKey AS Culture, LEN(v.Value) AS Len
   FROM TsDynDataValue v
   JOIN TsDynDataAttribute a ON a.AttributeId = v.AttributeId
   LEFT JOIN TsDynDataValueDimension vd ON vd.ValueId = v.ValueId
   LEFT JOIN TsDynDataDimension d ON d.DimensionId = vd.DimensionId
   WHERE v.EntityId BETWEEN 12011 AND 12017 AND a.StaticName IN ('Title','Teaser','Content')
   ORDER BY v.EntityId, a.StaticName, DimensionId
   ```
   ⚠️ **Corrigé le 12/11 après exécution** : la culture vit dans `d.SystemKey` (le `d.[Key]`
   initiale n'existe pas — `Nom de colonne non valide`, colonnes réelles : `DimensionId, Parent,
   Name, SystemKey, ExternalKey, Active, ZoneId`) ; l'`ORDER BY` porte sur `DimensionId`.
   **Cette sonde tranche l'écriture** : rangée de dimension **présente mais vide** ⇒ `UPDATE` ;
   **absente** ⇒ `INSERT` (`TsDynDataValue` + `TsDynDataValueDimension`, motif app 59 : une rangée
   de valeur **par dimension**, jamais une valeur partagée multi-langues).
3. **Témoin « pas encore injectées »** : chaque cellule DB actuelle **égale** la valeur registre au
   parent `16126d26^`. Toute cellule déjà traduite = **arrêt et constat**, jamais d'écrasement aveugle.
4. **Témoin servi avant** : matrice 7 slugs × 7 langues (`/en/actus/details/…` … `/zh/actus/details/…`)
   — attendu : repli FR partout (traductions absentes de la base). ⚠️ Le site sert `actus` en
   **minuscules** (mesuré 12/11 : le motif `/Actus/` majuscule ne matche aucun lien servi ; sur ce
   DNN une URL à la mauvaise casse peut répondre 200 avec une page d'erreur).

### 2.1 Résultat d'exécution — fenêtre du 12/11 (lecture seule, 11h23-11h4x)

| Sonde | Résultat mesuré |
|---|---|
| 1. GUID → EntityId | **7/7 conformes** à la table §1 (12011-12017) |
| 2. Valeurs par dimension | **21 valeurs base (dim-0), 0 rangée de dimension** ⇒ les 147 cellules sont toutes des **INSERT** (motif app 59 : une rangée de valeur par dimension) |
| 3. Base == registre parent `16126d26^` | **21/21 égales au Len ET au SHA-256** ; 147 cellules cibles non vides au merge ; prémisses tenues (fr inchangé parent→merge, cellules langues vides au parent) |
| 4. Matrice servie 7×7 | **49/49 repli FR** (200 + fragment du titre FR partout, `fa` compris — repli #2220 intact) |

ValueIds des 21 valeurs base (mesurés 12/11, à re-sonder au moment du geste) : Title/Teaser/Content
de 12011 = 30776/30781/30783, 12012 = 30787/30792/30794, 12013 = 30798/30803/30805,
12014 = 30809/30814/30816, 12015 = 30820/30825/30827, 12016 = 30831/30836/30838,
12017 = 30842/30847/30849.

**Verdict : « 147 cellules injectables, aucune déjà traduite » (INSERT pur).** L'injection reste
gatée sur la réponse de l'owner à Q-2026-11-09-2.

Sortie attendue : table `ValueId × dimension × (Len DB, Len registre)` + verdict « 147 cellules
injectables, aucune déjà traduite », ou écart nommé.

## 3. Procédure d'application (recette #2181-OWL, éprouvée)

1. **Sauvegarde AVANT tout geste** : un JSON par valeur `{ValueId, Len, Sha256, Value}` — les 147
   cibles **et** les valeurs dim-0 attenantes — écrit dans un **répertoire de sauvegarde hors dépôt
   désigné par la lane qui exécute** (jamais committé, jamais nommé ici).
2. **Répétition ROLLBACK** : transaction d'essai + retour arrière, contrôle aller-retour octet par
   valeur sur un échantillon nommé.
3. **COMMIT** : une seule `TsDynDataTransaction` pour les 147 (UPDATE ou INSERT selon la sonde 2),
   bump `TransModifiedId`, puis **recyclage app-domain par mtime `web.config`** — sha256 du fichier
   **vérifié inchangé** après le touch. ⛔ **Jamais `iisreset`**.
4. **Témoin servi après** : la matrice du §2.4 rend les 7 titres traduits par langue, fr inchangé
   octet pour octet.
5. **Gardes** : #2182 « aucun filtre de langue dans Actus » (la liste reste 12 articles × 8 langues) ;
   repli #2220 intact (slug inconnu ⇒ 404 en `fa`) ; garde CI #2235 (288 cellules, 0 vide) verte.

## 4. Retour arrière

Restaurer les 147 valeurs depuis la sauvegarde (mêmes transactions), **correspondance ligne à
ligne** (#2171) : chaque valeur revient sur **son** ValueId, sha256 re-vérifié après écriture ;
re-sonde de la matrice servie (retour au repli FR attendu).

## 5. Après injection

- **Ré-export autorisé** (la traduction mergée EST alors injectée — la porte « ⛔ aucun ré-export
  commité tant qu'une traduction mergée n'est pas injectée » tombe) :
  `python tools/458-eav-content-export.py --config DNNPlatform/web.config export` puis `verify
  --classify`. **Diff attendu : 0** sur les 147 cellules — elles ÉTAIENT le delta. Tout autre
  écart = dérive à qualifier avant commit.
- PR éventuelle uniquement si un écart hors-147 apparaît.

*po-2023*
