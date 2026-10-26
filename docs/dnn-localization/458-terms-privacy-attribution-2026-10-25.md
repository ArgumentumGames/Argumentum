# Item 6 — Attribution `/terms` `/privacy` : le contenu vit dans `GlobalResources.<culture>.resx`, clés `MESSAGE_PORTAL_TERMS.Text` / `MESSAGE_PORTAL_PRIVACY.Text` (mesuré 25/10, lecture seule)

Pool #458, item 6 : « attribution `/terms` `/privacy` (lecture seule, fichier/ressource
par culture → texte pour po-2024) ». Contexte dashboard : « localisé 5/8 (fr, en, es, pt,
ru), retombe EN sur zh/ar/fa alors que libellés 8/8 — grain à part (contenu pages cœur
DNN), pas un défaut de pied de page ».

**Verdict : l'état dashboard est confirmé et attribué en entier.** Une seule mécanique,
cinq fichiers, trois absents — le retombe-EN est prouvé par sha du bloc servi.

## 1. La chaîne, du clic au texte (mesurée sur l'arbre servi + source au SHA du binaire)

1. **Page** : `/terms` et `/privacy` rendent chacune **un seul pseudo-module** DNN
   (`DnnModule--1 DnnModule-Admin` dans `dnn_ContentPane` — id -1 = module core, pas une
   instance).
2. **Contrôle servi** : `admin/Portal/terms.ascx` (classe `DotNetNuke.Common.Controls.Terms`,
   code-behind `Terms.ascx.cs` relu au SHA `4b0214b8` du binaire servi) — markup
   `<div class="dnnTerms dnnClear"><asp:Label ID="lblTerms">`, **identique au servi**
   (`dnnTerms dnnClear` + `dnn_ctr_Terms_lblTerms`, mesuré). Privacy : jumeau
   `dnnPrivacy dnnClear` + `dnn_ctr_Privacy_lblPrivacy`.
3. **Une seule ligne de code charge le texte** :
   `lblTerms.Text = Localization.GetSystemMessage(PortalSettings, "MESSAGE_PORTAL_TERMS");`
   (privacy : `MESSAGE_PORTAL_PRIVACY`).
4. **Résolution** : `GetSystemMessage` lit la clé **+ suffixe `.Text`** dans les
   *GlobalResources* du portail, par culture de la requête :
   `App_GlobalResources/GlobalResources.<culture>.resx`.
   ⚠️ La clé stockée est `MESSAGE_PORTAL_TERMS.Text` — chercher le nom nu rend « absent »
   en silence.

Ne pas confondre : `admin/Skins/terms.ascx` est **l'objet skin lien** du pied de page
(`<asp:hyperlink>`, le monde #1973/#1984 — libellés 8/8) ; les
`admin/Portal/App_LocalResources/Terms.ascx.*.resx` ne portent **pas** le contenu (le
contrôle n'a pas d'autre chaîne que le message système).

## 2. État par culture (extraction XPath/InnerText, XML parsé — jamais regex le brut)

| Fichier (`App_GlobalResources/`) | `…TERMS.Text` | `…PRIVACY.Text` | Nature |
|---|---|---|---|
| `GlobalResources.resx` (défaut = en) | 16 816 c / 16 836 o | 7 840 c / 7 840 o | générique DNN, jetons `[Portal:PortalName]` |
| `GlobalResources.fr-FR.resx` | **17 956 c / 18 723 o** | **16 704 c / 17 001 o** | **rédaction custom Argumentum** (LCEN : SAS, SIREN, hébergeur en dur ; début « MENTIONS LÉGALES » ; aucun jeton) |
| `GlobalResources.es-ES.resx` | 17 011 c / 17 250 o | 7 741 c / 7 853 o | générique DNN |
| `GlobalResources.pt-BR.resx` | 16 323 c / 16 839 o | 7 670 c / 7 904 o | générique DNN |
| `GlobalResources.ru-RU.resx` | 15 742 c / 26 645 o | 7 577 c / 12 593 o | générique DNN |
| `.zh-CN` / `.ar-SA` / `.fa-IR` | **fichier absent** | **fichier absent** | — |

**Retombe-EN prouvé au sha du bloc servi** (`/terms`, empreinte du bloc `dnnTerms`) :
fr racine = 18 815 o (`A3159F227B01`, « MENTIONS LÉGALES : ») · **en, zh, ar, fa = bloc
octet-identique 16 323 o (`7992E63C335B`, « LEGAL NOTICE: »)** — le fallback ASP.NET
(fichier culture absent → fichier invariant = en) rend les trois cultures **strictement
anglaises**, conforme au constat dashboard.

⚠️ Asymétrie éditoriale à arbitrer avant traduction : **fr est la seule rédaction
custom** (mentions légales réelles du site, plus longues, sans jeton) ; en/es/pt/ru sont
les textes génériques du produit DNN. Traduire zh/ar/fa **depuis fr** donne les mentions
réelles ; traduire depuis en donne la version générique. Ce choix n'est pas instrumental.

## 3. Le grain pour po-2024 (attribution, pas exécution)

- Créer `App_GlobalResources/GlobalResources.zh-CN.resx`, `.ar-SA.resx`, `.fa-IR.resx`,
  chacun avec les 2 clés `MESSAGE_PORTAL_TERMS.Text` / `MESSAGE_PORTAL_PRIVACY.Text`
  (2 clés × 3 fichiers = 6 valeurs à traduire).
- ⚠️ **resx = restart ~33 s** (écriture `App_GlobalResources` = recompile app-domain —
  leçon mémoire) : la pose suit le protocole recycle-mtime du lane, annonce `[INFO]`
  avant geste, non-régression 16 routes après.
- La resx ne doit contenir **que les clés utiles** (un fichier réduit aux 2 clés suffit —
  le fallback par clé couvre le reste) ; encodage UTF-8 **sans BOM** à vérifier par
  bytes, `xml:space="preserve"` sur chaque `<value>`.
- Vérification post-geste attendue : `/zh/terms` ≠ sha `7992E63C335B` et 8/8 blocs
  distincts.

## 4. Pièges d'instrument rencontrés

- **Suffixe `.Text`** : la clé resx est `MESSAGE_PORTAL_TERMS.Text` ; sonder le nom nu
  rend « absent » alors que le fichier porte la valeur.
- **Adaptateur XML PowerShell** : `$node.value` sur un `<data>` ne rend pas l'InnerText
  du `<value>` enfant de façon fiable (sur 2 fichiers il a rendu un objet XmlElement dont
  `.Length` = 1 → « 1 char » aberrant). Instrument correct :
  `SelectSingleNode("//data[@name='…']/value").InnerText`.
- **Comparer les blocs servis au sha, pas à l'œil** : « LEGAL NOTICE » identique sur
  4 cultures ne dit pas byte-identique ; le sha12 du bloc le dit.
