# #458 grain module-level — contrôle d'écriture chinoise sur la 4ᵉ garde zh-CN (28 tests)

**Date :** 2026-10-31 · **Lane :** po-2023 · **Culture :** `zh-CN` seule
**Périmètre :** `DnnModuleLevelDtmModulesZhResxGuardTests` — **1 fichier de test, 0 `.resx` touché**
**Origine :** pool po-203 c.6547058416, item 1 — débloqué par le merge de #2098 (« la 4ᵉ garde zh est débloquée »).

---

## 1. Ce que ce lot ferme — et ce qu'il ne ferme pas

#2098 a ajouté le contrôle d'écriture chinoise aux gardes `controls`, `admin` et
`DtmAdmin`. La garde du lot `DesktopModules` « modules » (#2089, 27 fichiers /
668 clés) en restait dépourvue : son contrôle « ≠ anglais » laisse traverser une
valeur qui **diffère** de l'anglais **sans porter un seul caractère chinois**
(espacement, ponctuation, marque latine, anglais reformulé). Ce lot referme ce
trou sur la 4ᵉ et dernière garde zh livrée à ce jour.

L'énoncé reprend la forme **corrigée** de #2098 (commit `f765e187`) : la règle
ferme « diffère de l'anglais sans aucun caractère chinois » ; elle **ne ferme
pas** une valeur à moitié traduite — le fragment chinois porte déjà un caractère
CJK et la règle passe. Le message d'échec dit « la valeur ne porte aucun
caractère chinois », jamais « traduction partielle ».

Le motif `[㐀-䶿一-鿿豈-﫿]` reprend les trois plages de
#2098 (Ext-A, unifié, compatibilité) : les deux plages hors bloc unifié restent
une **marge défensive** — la mesure #2098 (3 018 valeurs zh du dépôt, 2 997 en
écriture chinoise, toutes dans le bloc unifié) n'en justifie aucune.

## 2. Dénominateur mesuré avant écriture

Census (`python -I` + ElementTree, les 27 `*.zh-CN.resx` du lot) : **668 valeurs,
668 portent l'écriture chinoise, 0 sans CJK** — même profil que le lot `admin`
zh dans #2098. **Aucune exemption nécessaire** : `ScriptExempt` = `VerbatimByDesign`
(déjà vide) et le fait d'épinglage l'exige **vide** (`HaveCount == 0`) — la
première dispense sera une décision consignée, jamais un constat pour faire
passer un rouge.

## 3. La garde

| | Avant | Après | Nouveaux |
|---|---:|---:|---:|
| `DnnModuleLevelDtmModulesZhResxGuardTests` | 111 | **139** | 27 théories + 1 fait |

- `EveryNonExemptValueCarriesChineseScript` (théorie × 27 fichiers) : chaque
  valeur livrée non dispensée porte au moins un caractère CJK.
- `TheScriptExemptListCannotGrowSilently` (fait) : la liste des dispenses est
  **vide et épinglée** — son ajout est le seul chemin pour faire passer une
  valeur sans CJK, et ce chemin rougit.

Doc de classe mise à jour (paragraphe « Depuis #2098 »).

## 4. Contrôles inverses — mesurés avant push

Mutation sur le **XML brut interne** (jamais la valeur parsée — piège #2101),
validée well-formed avant écriture, restaurée par copie + sha256 identique
(`899737d9c32b305b`), arbre suivi propre après.

| # | Mutation | Attendu | Mesuré |
|---|---|---|---|
| M1 | `CoreMessaging/View.ascx` `DismissAllConfirm.Text` → `Aperçu des notifications {0} — latin non anglais` (latin ≠ anglais, zéro CJK) | 1 rouge nommant la clé — la valeur **diffère** de l'anglais donc le socle reste vert, seule la nouvelle règle la voit | ✅ **1 échec / 138 verts** — *« …View.ascx.zh-CN.resx:DismissAllConfirm.Text — la valeur ne porte aucun caractère chinois ; valeur = « Aperçu des notifications {0} — latin non anglais » »* |
| M2 | **+1 dispense** (`ScriptExempt.Append(("CoreMessaging","View.ascx","DismissAllConfirm.Text"))`) avec la valeur TOUJOURS mutée | la théorie passe pour la clé (la dispense est ce qui autorise) et le fait d'épinglage rougit seul | ✅ **1 échec / 138 verts** — *`TheScriptExemptListCannotGrowSilently` [FAIL] « Expected ScriptExempt to be empty … but found at least one item »* |

**M1 + M2 forment la paire de contrôle** : rouge sans dispense, vert-avec-rouge
en l'ajoutant — l'exemption est le seul chemin de contournement et il est
police. La valeur mutée de M1 diffère de l'anglais ET porte un placeholder
intact : les 138 autres tests du même run restent verts, y compris
`EveryDeliveredKeyIsSaneAgainstTheNeutral` — preuve que le socle seul ne voyait
pas cette famille de défaut.

⚠️ Piège mtime (#2098, 4ᵉ occurrence) : après la mutation du `.cs` (M2),
rebuild explicite (`dotnet build -t:Rebuild`) avant de lire un compte — le build
incrémental sert l'assembly précédent.

Après restauration : **139/139 verts** re-mesurés, `git status` vide sur
`DNNPlatform/DesktopModules/CoreMessaging`.

## 5. Suite complète

Voir le corps de la PR — master `8c1f99b7` mesuré à 3 394 (3 388/0/6), + 28 =
**3 422** attendus (variance locale de skips 6→5 possible, déjà consignée
#2096/#2097/#2103, aucune cause inventée).

## 6. Ce que cette PR ne fait pas

- **Aucun `.resx` modifié**, aucune traduction produite : lot de garde uniquement. **Dépense API : 0 $.**
- **`ar-SA` / `fa-IR` non touchés** — leurs gardes DtmModules portent déjà leur
  contrôle d'écriture propre (ar : #2103 ; fa : à venir avec le lot fa-IR).
- **Pas de factorisation** : la 4ᵉ garde reste autonome — le socle des gardes
  `DesktopModules` est un grain séparé du pool (item 5), à prendre une fois la
  séquence zh close (règle « un lot par garde »).
- **Rien côté préprod** — aucun artefact servi n'est touché.

## 7. Suite

La séquence zh de `DesktopModules` reste ouverte (lot 3c, 9 fichiers / 381
clés — non claimé à la prise de ce grain). Le socle des gardes `DesktopModules`
(item 5) devient éligible quand la séquence zh ferme.

*po-2023*
