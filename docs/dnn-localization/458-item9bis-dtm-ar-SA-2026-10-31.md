# 458 item 9bis — lot « modules » ar-SA (DesktopModules) — 2026-10-31

Jumeau ar-SA du lot zh-CN « modules » (#2089) : **27 fichiers resx / 668 clés** sur
les HUIT répertoires `DesktopModules/{…}/App_LocalResources`, traduits par la lane
(0 $ d'API), avec garde, vérification indépendante et contrôles inverses mesurés.
Branche : `feat/458-modulelevel-dtm-modules-ar`.

## Périmètre

| Répertoire | Fichiers | Clés |
|---|---:|---:|
| AuthenticationServices/DNN | 1 (Settings) | 5 |
| CoreMessaging | 2 (SharedResources, View) | 140 |
| DDRMenu | 2 (MenuSettings, MenuView) | 5 |
| HTML | 6 (EditHtml, HtmlModule, MyWork, Settings, SharedResources, Workflow) | 136 |
| Journal | 4 (Edit, Settings, View, SharedResources) | 131 |
| MemberDirectory | 2 (Settings, View) | 113 |
| RazorModules/RazorHost | 5 (AddScript, CreateModule, EditScript, RazorHost, Settings) | 51 |
| SocialGroups | 5 (CreateSettings, Edit, ListSettings, SharedResources, View) | 87 |
| **Total** | **27** | **668** |

PersonaBar et NBright restent **hors périmètre** (#2072). Mêmes 10 clés déclarées
non livrées que le zh (repli neutre DNN) : `CoreMessaging/SR` ResultCount +
SingleResultCount (`{0}` pur) · `Journal/SR` Notification_Mention.Body + les 4
gabarits `journal_status/link/photo/file` (zéro prose) · `RazorHost/CreateModule`
ManifestText + ModuleControlText (manifest XML, directive) · `SocialGroups/SR`
GroupListTemplate (la prose vit dans des `{resx:…}` déjà livrés). Zéro verbatim :
les 668 valeurs portent l'écriture arabe.

## Livraison

8 JSON de traduction dans `docs/dnn-localization/`
(`458-item9bis-dtm-ar-SA.{authservices,coremessaging,ddrmenu,html,journal,
memberdirectory,razorhost,socialgroups}.translations.json`), puis le constructeur
exécuté **une fois par répertoire** :

```
python docs/dnn-localization/458-item9bis-langpack-build.py \
  --lang ar-SA --root "DNNPlatform/DesktopModules/<dir>/App_LocalResources" --bom mirror
```

- **Gabarits Journal** (11 clés) : le resx neutre stocke le HTML **échappé**
  (`&lt;span&gt;`, `&amp;#149;`) — les valeurs injectées dans le JSON viennent du
  neutre **parsé** (ElementTree), substitution chirurgicale de 5 fragments
  (« replied to: », « created the topic: », « created the group », « joined the
  group », « >Read More< »), jetons `[JournalProfile:relativeurl|32]` /
  `[journaldata:Url]` / `[journalitem:Title]` et tabulations/retours à la ligne
  comptés avant/après. Le `esc()` du constructeur ré-échappe à l'écriture
  (round-trip octet, même convention que le zh).
- **Fidélité des espaces significatifs** : espace initial
  `NewUnreadMessages.Text`/`TotalNotifications.Text`, «  من  » pour `Of.Text`
  (View), espace final `ServerErrorWithDescription.Text` et
  `GroupName.Required`, doubles espaces finals `GroupIdNotFound.Text`/
  `GroupViewPage.Help`, double espace avant `<br />` dans `SetupIntro.Text`,
  `[GROUPEDITBUTTON] ` final et espacement `groupViewTabId ="…"` dans le gabarit
  SocialGroups — tous préservés à l'octet.
- **Terminologie du lot** : script→البرنامج النصي, module→وحدة,
  workflow→سير العمل, Journal→اليوميات, group→مجموعة,
  member directory→دليل الأعضاء.

## Vérification indépendante (hors constructeur)

`verify_ar.py` (parse XML + octets bruts), 8 contrôles : jeux de clés == lot zh
livré par (dir, stem) · valeurs vides · copies anglaises · couverture écriture
arabe · multisets placeholders · identité jetons DNN · BOM miroir · LF uniquement.
**Résultat : 0 écart sur les 8 contrôles, 668 clés vérifiées.**

## Garde

`DnnModuleLevelDtmModulesArResxGuardTests` — **138 tests** (3 facts + 5 theories
× 27 fichiers) : comptes épinglés (27/668 dérivés), jeu livré = neutre − 10
déclarées manquantes, propreté par clé (multiset placeholders, identité jetons,
≠ anglais sauf verbatim déclaré), **écriture arabe présente** (U+0600–U+06FF),
BOM miroir le blob committé + LF (indépendant de `core.autocrlf` — règle payée
2× : #2078, #2087), listes vivantes épinglées.

⚠️ Classe **autonome miroir** de la garde zh, PAS un socle factorisé : la
séquence zh n'est pas close (4ᵉ garde zh gatée sur le merge de #2098 — règle
« un lot par garde ») ; la factorisation est le grain désigné une fois la
séquence close, comme `controls` et `admin` l'ont fait.

**Frange de la règle d'écriture arabe** (leçon #2098, énoncée dans la garde) :
elle ferme « diffère de l'anglais SANS aucun caractère arabe » (espacement,
ponctuation, marque latine, anglais reformulé) ; elle ne ferme PAS une valeur à
moitié traduite — le fragment traduit porte déjà un caractère arabe et passe.

## Contrôles inverses (mesurés)

| # | Mutation (cible) | Attendu | Mesuré |
|---|---|---|---|
| ① | `{0}`→`{9}` sur `CoreMessaging/View` `DismissAllConfirm.Text` | 1 rouge nommant la clé | **1 rouge** `DismissAllConfirm.Text — multiset des placeholders modifié` ✓ |
| ② | copie anglaise exacte de la même clé | 2 rouges (copie + écriture arabe) | **2 rouges** ✓ |
| ③ | latin NON anglais (« Aperçu des notifications {0} — latin non anglais ») | 1 rouge écriture arabe seule — le cas que « ≠ anglais » ne voit pas | **1 rouge** « ne porte aucun caractère arabe » ✓ |
| ④a | BOM préfixé, **arbre de travail seul** (non committé) | 138 verts — la garde lit le blob committé, pas l'arbre | **138/138 verts** ✓ |
| ④b | BOM **committé** (commit temporaire) | 1 rouge nommant le fichier | **1 rouge** `AuthenticationServices/DNN/Settings.ascx.ar-SA.resx : le BOM doit suivre celui du neutre (neutre=False)` ✓ |

Chaque restauration vérifiée : mutation sur le XML **brut** interne validée
well-formed avant écriture, puis `cp` depuis le backup scratchpad (jamais
`git checkout --`) + empreinte sha256 ; ④b défait par `reset --soft HEAD~1` +
`restore --staged` + cp, `git status` sans aucun `M` sur les resx du lot, garde
re-mesurée 138/138.

## Tests

- Garde seule : **138/138 verts** (mesuré 3× au fil des contrôles inverses).
- Suite complète : voir le corps de la PR (compte du jour, skips GUI/infrastructure
  nommés — la variance locale 6→5 est connue).

## Non-objectifs

- **Aucune installation en préprod** : un resx = restart DNN — geste po-2023
  avec `[INFO]` préalable, jamais dans un lot de traduction.
- Pas de factorisation de garde (séquence zh non close, ci-dessus).
- Pas de lot PersonaBar/NBright (#2072) ; RTL front déjà servi (#1932), ce lot
  n'y touche pas.

*po-2023*
