# #457 T2 — app 33 premier lot réel : le pipeline adapté, bloqué sur la brique 1 (l'export)

**Status: adaptation du pipeline livrée + demande d'export posée — l'exécution du lot
attend l'export app 33, qui n'existe nulle part (mesuré).**

Dispatch ai-01 2026-10-09 13:15Z (addendum au tick, c.6079563534) :
> *"po-2024 : T2, premier lot réel app 33 Content/Link/Video."*

---

## 1. Ce qui bloquait, mesuré avant d'agir

Deux constats préalables, tous deux mesurés le 2026-10-09 :

1. **Aucun export app 33 n'existe.** Le dépôt ne porte que l'export app 60 de juillet
   (`release-validation/exports/DNN-Argumentum-export-2026-07-07/` — vérifié fichier par
   fichier), et le shared-state RooSync également. La brique 1 de la chaîne T1
   (export → JSON) n'a jamais été produite pour app 33.
2. **Le pipeline T1 était épinglé au content-type unique.** `TRANSLATE_FIELDS` (importé du
   traducteur frère) est le set prose de *Game Rule* — or app 33 porte **trois**
   content-types aux sets distincts (I2 §3) :

   | Content type | Entités | Set prose (mesuré I2) | Cellules |
   |---|---:|---|---:|
   | Content | 31 | Title, Text, ImageCaption | 87 |
   | Link | 8 | Title, Description, LinkText | 24 |
   | Video | 8 | Title, Text | 15 |

   Sans adaptation, `to-csv` aurait rempli les cibles de traduction pour `Title` seul
   (le seul attribut commun aux deux sets) et laissé `Text`/`Description`/`LinkText`/
   `ImageCaption` vides — silencieusement : aucun rouge, un CSV « complet » qui encode
   *rien n'est traduisable ici*.

## 2. L'adaptation (le diff fonctionnel)

`tools/dnn_i18n/site_content_pipeline.py` :

1. **`TRANSLATE_FIELDS_BY_CONTENT_TYPE`** — les sets prose déclarés par content type.
   Game Rule reste l'import du traducteur frère (source unique, IC6/IC15) ; les trois
   sets app 33 proviennent de l'inventaire mesuré I2 §3.
2. **Fail-closed sur CT non déclaré** — `to-csv` **refuse** un export dont le
   content-type n'a pas de set déclaré. Conséquence voulue : **Person/Location** (bloc
   contact public de l'association, I2 §5, « 3 entités à traiter à la main ») sont exclus
   de la traduction en masse **par l'outil**, pas par la discipline de l'opérateur.
3. **Garde de cohérence CSV ↔ export dans `to-xml`** — un CSV dont `content_type` ≠ le
   `contentType` de l'export est refusé. Avec plusieurs content-types en existence, un
   CSV fusionné ou mal apparié aurait sinon produit des blocs `<Entity Type>` mal
   étiquetés **sans aucun avertissement** (le `Type` est timbré depuis l'export, la
   colonne `content_type` des rangées ignorée).

## 3. Preuves

- `self-test` : **21 contrôles verts** (17 à T1 + IC12–IC15). Le témoin IC12 porte une
  rangée stockée pour **chaque** question d'appartenance au set (ajouter `Url` au set,
  retirer `Description` — les deux mutent le verdict), et un artefact de traduction qui
  remplit **tous** les attributs y compris le technique, pour prouver que c'est bien le
  set prose (pas l'artefact) qui décide.
- **Contre-épreuve** (mutation → rouge nominatif → restauration par copie,
  sha256-vérifiée, jamais `git checkout --`) : 3 défauts plantés →
  `Url` ajouté au set Link → **IC12 rouge** ; `Description` retiré → **IC12 rouge** ;
  `Person` déclaré → **IC13 rouge**. Chaque mutation produit exactement le rouge prédit.
- IC11 inchangé et vert : le fixture committé reste octet-identique — le chemin Game
  Rule n'est pas dérangé.

## 4. La brique manquante — demande posée

Demande `[CLAIMED]`/`[TASK]` posée sur le dashboard workspace le 2026-10-09 à la lane
`argumentum-31` (la préprod est son domaine d'écriture — *« se demande, ne s'applique
pas »*) : un export **Method B lecture seule** d'app 33, content-types **Content, Link,
Video uniquement**, même forme triple-store plat que `12-game-rule-content-items.json`
(`contentType`/`appId`/`schemaAttributes`/`entities`/`values`). ⛔ Person/Location hors
demande — zéro donnée personnelle **par construction**. Destination préférée : committé
sous `release-validation/exports/` (le pipeline y pointe).

## 5. Runbook quand l'export atterrit

Un export par content-type, un run par content-type (garde §2.3) :

```bash
cd tools/dnn_i18n
python site_content_pipeline.py to-csv \
    --export <app33-Content-export.json> --out app33-content.csv   # fr seul, pivot
python site_content_pipeline.py to-csv \
    --export <app33-Link-export.json> --out app33-link.csv
python site_content_pipeline.py to-csv \
    --export <app33-Video-export.json> --out app33-video.csv
# la preuve sur UNE entité d'abord (le précédent T1), puis le volume :
python site_content_pipeline.py to-xml --csv app33-content.csv \
    --entity <guid> --unconfirmed-ok --out one-entity-app33.xml
```

⚠️ Les **6 cultures non confirmées** (ru/pt/es/ar/fa/zh) restent refusées sans
`--unconfirmed-ok` — inchangé depuis T1 ; leur provision (I1b) est un grain de la lane
préprod. ⚠️ La traduction elle-même (remplir les 7 colonnes cibles) reste **owner-gated**
dans l'attente du choix du traducteur (item ouvert T1 §8.2) — le CSV fr-seul est le
livrable qui débloque cette décision.

## Gates

- ❌ Aucun appel API payé, aucune écriture DNN/DB, zéro donnée personnelle (l'export
  synthétique du self-test est fabriqué, aucune valeur réelle n'y figure).
- ❌ Pas de verdict QA (ai-01).

## Sources

- `tools/dnn_i18n/site_content_pipeline.py` (diff : mapping par CT, fail-closed, garde
  CSV↔export, IC12–IC15), `tools/dnn_i18n/README.md` (section T2)
- [`1781-i2-zone3-inventory.md`](1781-i2-zone3-inventory.md) §3 (sets prose mesurés) et
  §5 (Person/Location = contact public)
- [`457-t1-site-content-pipeline.md`](457-t1-site-content-pipeline.md) (la chaîne à
  4 briques, dont la brique 1 manque pour app 33)
- Dispatch : ai-01 addendum 2026-10-09 13:15Z, commentaire #458 c.6079563534
