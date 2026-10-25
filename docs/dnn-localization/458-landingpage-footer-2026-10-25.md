# Pool #458 item 6 — pied de page `Landingpage.ascx` : pages, cibles servies, inversion latente

**Date :** 2026-10-25 · **Machine :** myia-po-2023 · **Question posée par ai-01** (#1973, c.6395372744) :
quelles pages utilisent `Landingpage.ascx` ; quels `href` sont servis sous chaque libellé ; et si
l'inversion supposée entre libellés et cibles est confirmée, la corriger avec un garde.

## 1. Quelles pages utilisent `Landingpage.ascx` — aucune

Mesuré en base (portail 1, le seul que compte la base) sur les trois surfaces qui peuvent désigner
un gabarit :

| Surface | Rangées nommant `Landingpage` |
|---|---|
| `dbo.Tabs.SkinSrc` | **0** |
| `dbo.Tabs.ContainerSrc` | **0** |
| `dbo.PortalSettings.SettingValue` | **0** |

Les seuls gabarits explicites par onglet sont, hors sujet : `[G]Skins/Xcillion/404.ascx` (onglet 141),
`[G]skins/xcillion/home.ascx` (onglet 153) et `[G]skins/openstorebo/openstorebo.ascx` (onglet 166).
Tout le reste — dont l'accueil — prend le gabarit par défaut du portail,
`[G]skins/2shinebs5/default.ascx`, donc `controls/theme-body.ascx`.

## 2. Quels `href` sont servis sous chaque libellé — 8/8

Bloc `to-shine-footer-imprint`, mesuré sur les 8 routes de culture (cache-bustées, 200 ×8) :

| Culture | libellé `Imprint.Text` | cible | libellé `Privacy.Text` | cible |
|---|---|---|---|---|
| fr | Mentions légales | `/terms` | Confidentialité | `/privacy` |
| en | Imprint | `/terms` | Privacy | `/privacy` |
| es | Aviso legal | `/terms` | Política de privacidad | `/privacy` |
| pt | Informações legais | `/terms` | Privacidade | `/privacy` |
| ru | Выходные данные | `/terms` | Конфиденциальность | `/privacy` |
| zh | 法律声明 | `/terms` | 隐私政策 | `/privacy` |
| ar | إشعار قانوني | `/terms` | الخصوصية | `/privacy` |
| fa | اطلاعات قانونی | `/terms` | حریم خصوصی | `/privacy` |

Les libellés sont **localisés 8/8** ; les cibles sont **littérales** dans `theme-body.ascx`
(lignes 157-158), donc identiques dans toutes les cultures.

## 3. Ce que sont ces cibles — et dans quelle langue

`/terms` et `/privacy` ne sont **pas des onglets** de ce portail (aucune rangée `Tabs`, aucun alias
`TabUrls`) : ce sont les **routes cœur de DNN**, rendues par les contrôles `Terms` et `Privacy`
(`…/Argumentum/ctl/Terms/PortalId/1`, `dnn_ctr_Terms_lblTerms`), sans module dans la page.

Le mapping servi est **sémantiquement correct**, contenu à l'appui : `/terms` rend « MENTIONS
LÉGALES : IDENTIFICATION DU SITE » (l'équivalent de l'imprint) et `/privacy` rend « Déclaration de
confidentialité ».

⚠️ **Constat séparé, non traité ici** : le contenu de ces deux pages est localisé **5/8**
(fr, en, es, pt, ru) et **retombe en anglais** sur `zh`, `ar`, `fa` — alors que leurs libellés sont
localisés 8/8. C'est une surface distincte (contenu de pages cœur DNN), à nommer comme son propre
grain.

## 4. L'inversion — réelle dans le fichier, **non servie**

`Landingpage.ascx` (avant correctif) liait `Imprint.Text` à `PrivacyTabId` et `Privacy.Text` à
`TermsTabId`, à l'inverse de `theme-body.ascx` qui lie les mêmes libellés à `/terms` et `/privacy`.
L'hypothèse d'ai-01 est donc **confirmée dans le source** — mais **pas dans ce que le visiteur
reçoit**, pour deux raisons mesurées :

1. **aucune page n'utilise ce gabarit** (§1) : le bloc n'est rendu nulle part ;
2. les deux réglages qu'il lit sont **absents** de `PortalSettings` sur ce portail — `PrivacyTabId`
   et `TermsTabId` n'ont **aucune rangée** — donc les liens se résoudraient par `NavigateURL(-1)`
   même sans croisement.

Un défaut latent n'est pas un défaut servi : il le devient à la première page qui choisit ce
gabarit. C'est pourquoi l'invariant est épinglé plutôt que laissé à la relecture.

## 5. Correctif et garde

**Correctif** (`Landingpage.ascx`, 2 lignes) : les deux arguments sont remis dans leur famille —
`Imprint.Text` → `TermsTabId`, `Privacy.Text` → `PrivacyTabId`. Une ligne changée par lien, aucune
autre modification ; l'intention du gabarit (lire les réglages du site) est conservée.

**Garde** : `SkinFooterLinkContractTests` — trois faits × deux fichiers, chacun avec un **compte
anti-vacuité** (motif qui ne matcherait rien ne peut pas passer pour un succès) :

- `ImprintLabelReachesTheTermsTarget` — le lien portant `Imprint.Text` atteint une cible « terms » ;
- `PrivacyLabelReachesThePrivacyTarget` — le lien portant `Privacy.Text` atteint une cible « privacy » ;
- `TheTwoLabelsAreDistinctLinks` — un même lien ne porte pas les deux clés (sinon les deux faits
  ci-dessus seraient satisfaits par une seule cible).

**Contrôle inverse exécuté** : garde écrit **avant** le correctif, lancé sur l'arbre non corrigé →
**2 échecs**, tous deux sur `Landingpage.ascx`, `theme-body.ascx` vert (4 réussites). Après
correctif : **6/6**. Le garde n'est donc pas vacant, et il vise bien le fichier fautif. Suite
`Skin` complète après correctif : **24/24**.

## 6. Portée

Aucun changement servi : le gabarit corrigé n'est rendu sur aucune page (§1), donc aucune mesure
servie n'est attendue et aucune ne pourrait distinguer l'avant de l'après — le dire ainsi vaut mieux
que d'annoncer une vérification qui ne mesurerait rien. Le correctif est dans le dépôt ; il prend
effet le jour où une page sélectionne ce gabarit, ou lors d'une réinstallation/du remplacement du
pack de skin.

*po-2023*
