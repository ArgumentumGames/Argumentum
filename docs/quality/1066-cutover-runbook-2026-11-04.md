# Runbook de bascule consolidé — mise en ligne du site DNN Argumentum (2026-11-04)

> **Statut** : document de consolidation, établi à master `8c74ee13`. Aucun geste prod n'est exécuté
> par ce document — la production est **gelée** (GO owner, #972, discipline maintenue après sa clôture).
> **Remplace** : [`1066-cutover-runbook-2026-09-22.md`](1066-cutover-runbook-2026-09-22.md) (R1-R8) et
> [`../dnn-localization/132-deployment-runbook.md`](../dnn-localization/132-deployment-runbook.md)
> (phase 5 VPS) — remplacés, **aucun supprimé**. La famille `131-*` (sandbox/montées historiques)
> reste une archive, non remplacée.
>
> Ce runbook consolide ce que les deux précédents ne couvraient pas : l'**EventLog** comme instrument
> de contrôle, le **texte légal anglais** en production, la porte **#2132** (relecture humaine),
> **#135/#1502/#457** (contenu publié), le **nouveau VPS** — servi par la lane **`myia-web2`** — et
> l'état **DNN 10.4.1 + 2sxc 22.01** issu du rang 2 (02/11).

## 0. État de référence (mesuré)

| Étage | État | Preuve |
|---|---|---|
| Préprod `dnn.argumentum.myia.io` | webroot master `8c74ee13` (CDV 381), DNN **10.4.1** + 2sxc **22.01**, 8 langues servies | rang 2 exécuté 02/11 ; DB `Version` 10.4.1/1048 ; 8/8 routes 200 |
| Production `www.argumentum.games` | **gelée** : DNN 9.11.1.19, 2sxc 15.02.00, accueil 200, `/en/` = 404 (état gelé, pas incident) — servie par la lane **`myia-web2`** | `web.config` **83 211 o** (relevé avant/après chaque geste) |
| Contenu prod | **3ᵉ génération** antérieure aux marqueurs emoji — 9/96 ancres CSV présentes | [#458 c.6260750566](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-6260750566) |
| Relecture humaine #2132 | **5 cases, toutes décochées** — site **0/3** (Jean-Sylvain, Adeline, Thomas) · PDF **0/2** (Adeline, Thomas) | ⛔ pas de relance |
| Release GitHub #134 | **préversion publique** `v2.1.0-review` (80 PDF, publiée 27/10 23:54Z) — release **finale** à créer par ai-01 | #2025 |

## 1. Table maîtresse — geste, lane, contrôle, retour arrière, issue fermée

| Issue | Objet | Geste | Lane | Contrôle (DoD) | Retour arrière |
|---|---|---|---|---|---|
| **#2132** | Relecture avant sortie | **Porte d'entrée** : les **5 cases** cochées AVANT toute bascule (site ×3, PDF ×2) | owner (humains) | les 5 cases sur la page #2132 | n/a — porte, pas un geste |
| **#134** | Release GitHub | **créer la release finale** à partir du lot validé (80 PDF) ; déclenche #135 | ai-01 (release + verdicts) | assets publiés, tailles = bundle | `v2.1.0-review` **reste en l'état** (⛔ pas de `--clobber`) ; la finale est un objet distinct |
| **#131** | Montée DNN + mise en ligne | déployer sur le VPS de prod (exploité par la lane **`myia-web2`**) la génération validée en rang 2 (10.4.1 + 2sxc 22.01), séquence A→J du plan de montée (`docs/dnn/2026-10-31-montee-dnn1041-2sxc2201-plan.md`) | **web2** (toutes les écritures prod) ; owner = fenêtre ; ai-01 = verdicts | §2 DoD ci-dessous (versions, redirections, EventLog, routes) | snapshot VPS + backup DB pré-geste ; DNS retour arrière |
| **#1049** | web.config versionné = panne totale | **phase 2, au moment de la bascule** : le site servi vivra dans une **copie hors dépôt**. ⚠️ **Mesuré le 22/11 (P23-05)** : la **préprod est encore servie par le checkout git**, pas par une copie — le « comme la préprod depuis 08/10 » était faux (corrigé à l'étape 5 et au §8). Phase 1 faite (fichier sorti du dépôt, 2 tests CI) | **web2** | le dépôt ne peut plus écrire sur le `web.config` vivant ; discriminateur = les 4 comptes de l'étape 5 (rendus par le script du jour J) + taille 83 211 o | n/a — structurel ; la panne existe seulement si le fichier versionné atteint un webroot |
| **#1091** | Compte stocké en mot de passe **Clear** (depuis 2020) | **Étape retirée du déroulé** (décision ai-01 du 14/11, motif au canal privé — cf. §7). La ligne reste comme trace ; le geste Clear → Hashed n'est plus exécuté. ⛔ aucune valeur, empreinte ou sel ne transite par une issue, un commit, le dashboard ou un fichier | — | n/a — étape retirée | ⛔ **jamais** de retour à Clear |
| **#1502** | Règles du site divergé des cartes (4-10 vs 3-10 joueurs) | couvert par la **ré-injection EAV courant** (cf. #457) : la prod ne peut pas servir le corpus tel quel | **web2** (injection prod) ; po-2023 (script + essai à blanc préprod, mesures) | compteurs « de 3 à 10 joueurs » ×5 formes sur `/Règles`, par langue ; cf. garde `RulesExplorerD1PlayerCountGuardTests` | snapshot pré-injection, transaction gardée, rollback SQL borné |
| **#457** | Localiser le site (8 langues) | se **ferme à la mise en ligne** : l'injection EAV courant + les packs de langue + l'activation portail + les **dimensions 2sxc** (étape 11 bis, P23-25) installés ce jour-là. ⚠️ **Préalable app 48, à chaque copie prod→staging** (arbitrage ai-01 du 22/11, option d) : la prod porte les 2 coquilles typographiques (`&nbsp;` → espace) de la cellule fr du formulaire newsletter (`FB8362B1-…`/Html, ValueId 23452) — réaligner le staging **avant le dry-run v4** (sinon refus G2) : sha `491ff712c7fb8c9b` → `da87843404410f1a` (13 303 → 13 314 o), `@@ROWCOUNT`=1, backup + inverse écrits **avant** le geste — geste détaillé : `ai01-arbitrage-app48-20261122`. Puis **dériver la copie d'exécution** du registre avant le dry-run v4 : `python tools/458-derive-run-registry.py <registre officiel> <sortie>` (arbitrage ai-01 du 22/11, option iii) — 673 lignes → **22 retirées / 651 conservées**, sortie sha `92bf1e0624256e7f…`, **1 166 864 o** ; l'outil refuse toute entrée qui n'est pas l'officiel (empreinte d'entrée `277aac83e0b71e64`) et la CI (`eav-derive-run-registry`) épingle l'empreinte de sortie, si bien qu'une dérive **légitime** du registre rougit **avant** d'atteindre le staging — outil et test se mettent à jour ensemble, même commit. ⚠️ **Puis les compteurs de joueurs, seconde étape de la même famille, elle aussi à chaque copie prod→staging** (arbitrage ai-01 du 23/11, **P23-31**) : ils viennent de `MinNbPlayers`/`MaxNbPlayers` (type **Number**, non traduisibles, donc **exclus du registre par construction** — aucune réinjection ne les transporte) et la **carte imprimée fait foi** : *L'école des menteurs* **4 → 3**, *Le Bingo* **10 → 20** (École et Bingo : lecture seule préalable, **une** valeur servie par attribut attendue, STOP sinon ; sauvegarde + inverse écrits avant ; `@@ROWCOUNT`=1 ; relecture in-tx ; COMMIT ; recyclage mtime). Preuve : intervalles servis `{1→8, 1→20, 2→8, 3→10}` sur les 8 routes `/{lang}/Règles`, parlote en forme simple (« 4 »). ⚠️ Si la parlote rend « de 4 à 4 », le gabarit servi est celui de la prod — **STOP** et poster le sha256 du `_RulesExplorer_RuleList.cshtml` servi à côté de celui du blob master (`7961b591…`) | **web2** (geste prod) ; po-2023 (script/essai + relevé préprod) ; po-2024 (packs) | sondes visa 21/21 par langue ; sélecteur 8 langues ; témoins fr intacts ; dims résolues sans REFUS au dry-run ; app 48 alignée (relecture sha `da878434…`) | désinstall langpack **sans** « Delete Files » (R8) ; DELETE dims par id mesuré (inverse écrit avant COMMIT) ; rollback EAV borné |
| **#135** | Article d'actus + tous les téléchargements | **jour de la mise en ligne** : publier l'article News annonçant la version + rafraîchir tous les liens de téléchargement vers la release #134 ; périmètre mesuré en amont (brouillons + relevé des liens servis) | **web2** (édition + lien prod) ; ai-01 (texte/verdict) ; po-2023 (mesures) | chaque lien servi pointe vers un asset publié de #134 ; `Print.Play` = nom réécrit par GitHub | article = brouillon tant que non annoncé ; liens = tableau de remplacement à blanc |
| **#133** | Publication OWL — **option A** (ai-01, 05/11) | **Répétition faite en préprod le 05/11** : les deux blobs du dépôt déployés à la racine du webroot, **sous les noms que les IRI déclarent** (`argumentum_fallacies.owl`, `argumentum_virtues.owl`) — ⚠️ le fichier du dépôt s'appelle `argumentum.owl`, l'IRI non : la cible est le **nom servi**, l'option A n'est pas un renommage. L'artefact remplacé (4 795 192 o, non suivi, mtime 28/08) était **périmé** et `virtues` **absent (404)** | **web2** (même geste en prod) ; po-2023 (répétition préprod, faite) ; ai-01 (verdict) | `HTTP 200` **et** taille exacte **et** `sha256` égal au blob du dépôt **et** `Content-Type: application/xml` (§6, mesuré 21/11) sur les deux URLs (préprod : 5 993 701 / 1 094 832, mesuré 05/11) ; puis lien depuis la **page des téléchargements** (#135, 8 langues — décision ai-01 du 05/11 : la page Argumentation n'est pas modifiée) | restaurer la **copie de préservation** de l'artefact 4,8 Mo (suivi nulle part = non régénérable ici) |
| **#136** | Entités taxonomie 2sxc | état **re-mesuré le 05/11** ([c.6662268609](https://github.com/ArgumentumGames/Argumentum/issues/136#issuecomment-6662268609)) : `Game Rule` (ct **377**, app **60**) = **5 entités** publiées, `Title` 5/5 dans les **8 cultures** — inventaire identique au 30/10, **l'import d'essai n'a pas été exécuté**. ⛔ La « couverture complète par attribut » de la v1 était **fausse** : le rapport du 30/10 n'a mesuré que `Title` et dit lui-même que l'audit par attribut **n'a pas été exécuté**. L'export prêt = **128 entités = 16 par culture × 8** (#2050) — et « 16 » y est un compte d'**entités, pas de règles** ([#2075](https://github.com/ArgumentumGames/Argumentum/issues/2075), qui tranche) : par culture, la **racine `Rules`** + `Rules_01…Rules_15`, quand le CSV porte **15 lignes plates** (`Dnn2sxcConfig.cs:256`, revérifié 22/11) — la formulation « 16 » de [#2157](https://github.com/ArgumentumGames/Argumentum/issues/2157) lit ce compte d'entités comme un compte de règles : **c'est la note contredite**. La base porte la **fiche de jeu** (`Game Rule`, 5), l'export le **texte par ligne CSV** — ni redondants, ni équivalents ; l'import **n'est pas nécessaire au rendu actuel** et **créerait un 14ᵉ content-type** (app 60 : 13, aucun `Rule`). Import **post-mise en ligne** : il **change une page relue** (fiches plus nombreuses qu'aujourd'hui — compte servi non mesuré, l'import n'ayant jamais tourné) | ai-01 | `/Règles` fr sert **5 fiches** = les 5 entités ; ⛔ ne pas écrire « rendu = corpus CSV » (15 lignes au CSV ; 16 entités/culture en comptant la racine) | n/a (constat) ou rollback EAV |
| **#1180** (C1) | Filtres date pipeline News cassés sous fa-IR | **reporter le geste base** en prod : bornes typées + 2 filtres neutralisés (PR #1947) — **la prod garde encore `<=`/`>`** ; ligne go-live posée sur [#1180 c.6361658507](https://github.com/ArgumentumGames/Argumentum/issues/1180#issuecomment-6361658507) | **web2** (geste base prod) ; po-2023 (script + essai à blanc préprod) | `/fa/Actus` rend 5 articles (comme fr/ar/ru/zh) | backups entités 9434/9435 : réécrire `"<="`/`">"` puis recycle |

**Partage des lanes** (arbitrage ai-01, 04/11 — la colonne `Lane` le reflète ligne à ligne) :
**web2** porte **toutes les écritures prod** — copie hors dépôt, montée, redirections, alias, filtres
C1, ré-injection EAV, packs de langue, article et liens #135, fichiers E12, wrappers, OWL déployés.
**po-2023** écrit, pour chaque geste base, **le script + l'essai à blanc sur la préprod**, et fait les
**mesures**. **owner** = fenêtre de bascule. **ai-01** = release finale (#134) + verdicts.

## 2. Séquence de bascule (jour J)

### Phase 0 — Pré-requis (avant J-J, réversibles)

1. **#2132 = 5 cases cochées** (site ×3, PDF ×2 — porte humaine, ⛔ relance). Sans elle, rien ne suit.
2. **#134 : release finale créée** par ai-01 à partir du lot validé ; `v2.1.0-review` reste la préversion
   publique (80 PDF, lue par les relecteurs — ⛔ pas de `--clobber` dessus).
3. **Sauvegardes** : snapshot VPS `myia-web2` + `BACKUP DATABASE ... WITH COPY_ONLY` sur la base prod
   (vérifier via `msdb.dbo.backupset`, jamais par le filesystem — le répertoire par défaut est ACL service) ;
   taille du `web.config` prod relevée avant le geste (**83 211 o** attendu).
4. **Langpacks construits au hash du go-live** : `tools/458-langpack-build.py --culture <xx-XX> --commit <hash>`
   pour chaque culture publiée ; fraîcheur = `git log <hash>..master -- '*.<culture>.resx'` **vide**.
   Les packs ar-SA/fa-IR/zh-CN/ru-RU/pt-BR déjà livrés ne couvrent que les familles livrées — vérifier le
   résidu (volet module-level en cours, #2111-#2113 et suivants).

### Phase 1 — Déploiement technique (VPS, fenêtre owner)

5. **Copie hors dépôt** (#1049 phase 2), par **web2** — la copie devient le site servi.
   **Source nommée** : l'arbre **servi** de la préprod. ⚠️ **Mesuré le 22/11 (P23-05, lecture
   seule)** : cet arbre servi **est** le checkout git — site IIS « dnn.argumentum.myia.io »
   (id 52), le `physicalPath` lu dans `applicationHost.config` pointe sur le dossier
   `DNNPlatform` **du checkout** ; « copie hors dépôt en service depuis le 08/10 » était
   **faux** ([c.6256818241](https://github.com/ArgumentumGames/Argumentum/issues/1049#issuecomment-6256818241)
   du 19/10 avait raison). La distinction « arbre servi ≠ checkout » ne tient donc plus : ce
   qui ferme la mine #1049 n'est pas le choix de la source mais les **comptes ci-dessous** —
   d'autant que le `web.config` **versionné** n'existe plus depuis #1055 (le fichier du
   checkout est l'original vivant, non suivi) : le résidu attrapé par les comptes est un
   template/`REPLACE` ou l'`example`, pas un fichier suivi. **Contrôle AVANT que le site soit servi** — dernier
   point où reculer ne coûte rien — par **comptes** sur le `web.config` de la copie :
   `Data Source=REPLACE` = **0** · `validationKey="REPLACE"` = **0** ·
   `Initial Catalog=ArgumentumGames` = **1** · `fileExtension=".owl"` = **1** — et une taille de
   **83 211 o**. Un seul de ces comptes qui dévie = le fichier **versionné** a atteint la copie :
   reculer (remplacer par le `web.config` vivant sauvegardé), ne pas « réparer ».
   ⛔ **Comptes seulement, jamais les lignes trouvées** — le fichier porte la chaîne de connexion
   (avec son mot de passe) et les `machineKey`.
   **Comment on l'exécute** : par le **script du jour J** (`tools/1105-cutover-day-controls.py`,
   section `web.config` — étendue par #2165, cinq comptes + verdicts), ⛔ **pas** en tapant un
   `grep -c` dans PowerShell : sur le VPS de la prod `grep` est absent du PATH et `bash` est un WSL
   **sans distribution** (mesuré, #2165) — les quatre commandes y échouent au seul endroit où
   reculer ne coûte rien (même famille que le lanceur écrit pour le mauvais interpréteur).
   Commande exacte, avec ⚠️ `--require-webconfig` — son oubli sort en **code 2**, jamais en 0 :
   un vert machine-lisible sans la mesure n'existe pas (remarque 🟡 de la review #2165).

   ```bash
   python tools/1105-cutover-day-controls.py --webconfig "<copie>\web.config" \
       --skip-web --skip-eventlog --require-webconfig --label jour-j-etape5
   ```

   Codes de sortie : **0** = passe · **1** = échec dur — un compte qui dévie ⇒ reculer ·
   **2** = `--require-webconfig` posé sans `--webconfig` (mesure pivot absente).
   **Un rouge sur la taille SEULE se qualifie avant de reculer** : une retouche légitime du
   fichier vivant (BOM, fin de ligne, réindentation IIS) fait bouger la taille — quand les
   cinq comptes passent, vérifier la date et l'origine du fichier, ajuster si besoin
   (`--webconfig-size`), puis continuer. Un compte qui dévie, lui, ne se qualifie pas.
   Jamais `git checkout` sur un webroot servi ; jamais de copie du
   `web.config` préprod → prod (le secret et les `machineKey` diffèrent).
   **Phase 2 à blanc — #1049, mesuré le 22/11 (P23-05), lecture seule, rien copié** : la copie
   de secours hors dépôt (rafraîchie le 20/09, phase 1-bis) est **périmée au point de
   régresser #133** si on basculait dessus telle quelle : elle porte `argumentum_fallacies.owl`
   **périmé** (4 795 192 o, mtime 28/08 — l'artefact remplacé le 05/11) et
   `argumentum_virtues.owl` **absent**, quand l'arbre servi tient la paire du 05/11
   (5 993 701 / 1 094 832 o) ; **2 349 fichiers suivis** du webroot ont changé depuis le
   20/09 (DesktopModules 1089 · Icons 551 · Providers 234 · admin 179 · controls 69 ·
   Portals 53 · App_GlobalResources 44 — liste jointe à la livraison P23-05). Ordre imposé :
   **refresh de la copie d'abord** (recette phase 1-bis — robocopy à exclusions, jamais
   `/MIR`), puis le geste serait `appcmd set vdir "dnn.argumentum.myia.io/"
   -physicalPath:"<copie>"` (rollback symétrique vers le chemin du checkout, lu dans
   `applicationHost.config`) — **jamais exécuté** : la bascule de la préprod reste un
   arbitrage owner ([c.5314245647](https://github.com/ArgumentumGames/Argumentum/issues/1049#issuecomment-5314245647)).
6. **Montée DNN 9.11.1 → 10.4.1 + 2sxc 15.02 → 22.01** sur la copie, selon le plan de montée rang 2 :
   installeurs natifs (assistant DNN puis installeur d'extensions 2sxc) → **mesurer `bin/`** → une
   redirection par nom à la **version mesurée** (**38 redirections à poser** sur la copie validée —
   objet **distinct** des `dependentAssembly` **déjà présents** dans le `web.config` vivant, ce ne
   sont pas deux mesures du même nombre ; `DnnBindingRedirectContractTests`). Publier **seulement**
   `<runtime><assemblyBinding>`.
   Leçons intégrées : bindingRedirect **superposés** (HTTP 200 malgré des `FileLoadException` — seul
   l'EventLog les voit, mesuré 02/11) ; pile BCL 2sxc (B1-inversion, 132 §5.5(a)).
6 bis. **Texte légal anglais — restaurer les 2 valeurs neutres** (web2), **juste après l'étape 6**.
   Cause mesurée : l'installeur DNN **écrase le neutre `GlobalResources.resx`**, et il **n'existe
   aucun `GlobalResources.en-US.resx`** — le neutre **est** le texte anglais
   (`MESSAGE_PORTAL_TERMS.Text` + `MESSAGE_PORTAL_PRIVACY.Text`). Sans ce geste, `/en/` sert le
   texte **stock** de DNN (« AGREEMENT BETWEEN USER AND ») **entre la montée et la phase 2**.
   Geste : restaurer les 2 valeurs depuis le blob de master.
   **Contrôle — un marqueur PROPRE à chaque page, pas le même aux deux** (mesuré 05/11 sur les
   valeurs du dépôt, cf. [`controles-jour-j-baseline-preprod-2026-11-05.md`](controles-jour-j-baseline-preprod-2026-11-05.md)) :
   `/en/terms` porte « ARGUMENTUM SAS » (×1 dans le neutre) ; `/en/privacy` **ne le porte pas** — ni
   dans le neutre, ni dans le fr-FR — son marqueur est la phrase de primauté du français
   (« The French version of this page is the authoritative version »). Négatif commun aux deux
   pages : « AGREEMENT BETWEEN USER AND » **absent**. ⛔ Exiger « ARGUMENTUM SAS » sur `privacy`
   serait rouge sur un site sain — un faux rouge au jour J coûte une fenêtre.
7. **#1091 — étape retirée** (décision ai-01 du 14/11 ; motif au canal privé, cf. §7). Le numéro est
   conservé pour que les étapes 8 à 16 gardent le leur (le §2 et le §4 y renvoient). ⛔ L'interdiction
   de retour à un mot de passe Clear reste en vigueur (voir « Interdictions permanentes »).
8. **D2 alias racine ET alias de langue** (étendu P23-25, arbitrages ai-01 du 21/11) — par **web2**,
   d'abord répété sur le site de test ; **avant/après posté au dashboard AVANT le COMMIT**, inverse
   écrit **avant le COMMIT**, puis **recycle** (cache PortalAlias — l'UPDATE seul ne suffit pas,
   mesuré 18/10). État de référence = la préprod relevée le 21/11 (lecture seule), avec l'hôte de
   prod :
   - **Racine** : `www.argumentum.games` en `CultureCode` fr-FR, `IsPrimary=1`.
   - **Les 7 alias de langue** : `www.argumentum.games/{en,es,pt,ru,zh,ar,fa}` — sous-chemins en
     **2 lettres** (comme la préprod et les liens de #2324), `CultureCode` = codes **complets**
     mesurés sur la préprod : `/en`→en-US · `/es`→es-ES · `/pt`→**pt-BR** · `/ru`→ru-RU ·
     `/zh`→zh-CN · `/ar`→ar-SA · `/fa`→fa-IR · `IsPrimary=0` pour les 7.
   - **Les rangées `argumentum2.dnndev.me/<cc>` créées par l'activation** : ⛔ **aucun DELETE**.
     Les 7 non-fr sont **réécrites en place** (`UPDATE HTTPAlias`, le `PortalAliasID` est gardé)
     vers l'alias cible ; la rangée `fr-fr` passe à `IsPrimary=0`.
   - **Réglages du portail** (même transaction) : `DefaultPortalAlias` aligné sur
     `www.argumentum.games`. Mesure préprod 21/11 : `'argumentum2.dnndev.me'` + `PortalAliasMapping
     = 'CANONICALURL'` — la préprod porte elle-même le default sur l'hôte dnndev et sert pourtant
     depuis un autre hôte (CANONICALURL ne force pas la redirection) ; l'état cible reste **plus
     sûr** que la préprod. web2 mesure et rapporte les **deux** réglages : **R5 (étape 20) n'est
     une mesure que si 0 redirection vers un autre hôte** que le site interrogé.
   - **Fallbacks de langues** (`Languages.FallbackCulture`), selon la préprod mesurée le 21/11 :
     en-US→fr-FR · ru-RU→en-US · fr-FR/es-ES/pt-BR/zh-CN/ar-SA/fa-IR → **vides**. ⚠️ Ce n'est pas
     le défaut DNN laissé par l'activation (« anglais pour les 6 autres ») : les 5 fallbacks
     parasites se **vident** explicitement.
   - **Répétition (site de test seulement)** : alias **non primaires** `localhost:8123` et
     `localhost:8123/<lang>` avec les mêmes `CultureCode` — inoffensifs après le swap (joignables
     depuis le serveur seulement) ; le déroulé les liste.
   - **Ordre d'exécution sur la prod** : après l'**étape 11 bis** (activation) — les rangées
     `<hôte>/<cc>` à réécrire n'existent qu'après elle (détail au 11 bis).
9. **C1/#1180** : reporter le geste base des filtres date (bornes typées / neutralisation) — la prod
   garde encore la config fautive.

### Phase 2 — Contenu (jour J)

10. **Ré-injection EAV courant** (préprod → prod) : la prod sert une 3ᵉ génération (9/96 ancres) ;
    l'injection suit le motif éprouvé (transaction gardée, snapshot pré, anti-rejeu par dimension).
    ⛔ Périmètre déclaré : **aucune autre entité ; aucune table de membres, de commandes ou de comptes.**
    ⚠️ **Ordre d'exécution : l'étape 11 bis précède CETTE étape** — l'outil de ré-injection
    (`tools/458-eav-reinject.py`, paquet P23-24 v4) **refuse** toute dimension de langue absente de
    la zone des apps cibles (REFUS explicite, pas un skip) : sans les dimensions, le dry-run même
    ne passe pas. Exécuter dans l'ordre **11 → 11 bis → 10**.
11. **Langpacks installés** (R8) : `Install/Install.aspx?mode=installresources` restauré du zip
    d'upgrade, **quarantaine des strays** `Install/*/` (le trigger installe tout), install = crée la
    langue host, **n'active pas le portail** ; contrôle 0 écart `git status` avant/après.
11 bis. **Activation portail des langues + dimensions 2sxc de la zone des apps** (P23-25,
    arbitrages ai-01 du 21/11 — par **web2**, d'abord répété sur le site de test). Deux gestes
    distincts dans cet ordre :
    - **Activation portail** : l'étape 11 crée la langue host sans activer le portail — activer
      ici les 8 langues du portail (fr-FR incluse). ⚠️ L'activation crée des rangées
      `PortalAlias` `<hôte-courant>/<cc>` — c'est ce que l'étape 8 réécrit en place. D'où l'ordre :
      sur le site de test les rangées existent déjà (R8 répété) et le geste 8 s'y répète avant ;
      sur la prod, l'étape 8 s'applique **après** cette activation — réécrire avant, c'est
      réécrire des rangées qui n'existent pas encore.
    - **Dimensions 2sxc** : recette d'origine =
      [`../dnn-localization/1781-i1b-culture-dimensions-2026-10-10.md`](../dnn-localization/1781-i1b-culture-dimensions-2026-10-10.md)
      — **INSERT SQL direct documenté**, pas l'UI 2sxc. Préprod mesurée 21/11 (lecture seule) :
      une racine `SystemKey='Culture'`, `ExternalKey` vide, puis **8 cultures** fr-FR, en-US,
      ru-RU, **pt-BR** (re-keyée depuis pt-PT le 11/10 — pas pt-PT), es-ES, ar-SA, fa-IR, zh-CN,
      toutes `Active=1`, toutes filles de la racine.
      ⚠️ **Ids et zones se mesurent sur la base cible, jamais copiés** : la préprod porte zone 3
      + racine 5 + ids 6-13 ; la zone cible = le `ZoneId` des **apps cibles** de la carte
      d'apps imprimée par le dry-run du paquet P23-24 v4 (`TsDynDataApp.AppId` → `ZoneId`),
      les `DimensionId` viennent de l'IDENTITY de la base cible. Une culture identique existe
      dans plusieurs zones (défaut DNN zone 2 vs apps Argumentum zone 3 en préprod) — ne jamais
      prendre le premier `en-US` venu.
      Garde : **UNE transaction** · **inverse écrit AVANT le COMMIT** (DELETE par `DimensionId`
      mesuré) · avant/après posté au dashboard **avant le COMMIT** · recycle après (cache 2sxc) ·
      **re-dry-run de l'outil P23-24** juste après : les dimensions doivent y être résolues sans
      REFUS — c'est la porte d'entrée de l'étape 10.
    - **La boucle refus → zone → nouveau dry-run** (P23-26) : sur une base où les dimensions
      n'existent pas encore, le premier dry-run du paquet v4 termine en refus — message exact,
      **sans** préfixe « REFUS » : `dimension <lang> absente de la zone <zone> (app <app>)`, avec
      `<lang>` en **forme courte** (`en`, `ru`, `pt`, `es`, `ar`, `fa`, `zh` — l'outil ne nomme
      jamais fr : fr = dim-0, absent de `LANGS` ; **mais la zone doit porter les 8 cultures,
      fr-FR comprise** — préprod zone 3 id 6 : la recette insère celles qui manquent). ⚠️ Ce
      refus est levé dans `fetch_target`, **avant** l'impression de la carte d'apps : la zone se
      lit **dans le message**, pas dans la carte. ⚠️ Le message ne nomme que la **première**
      langue manquante, dans l'ordre en, ru, pt, es, ar, fa, zh — **chaque tour insère toutes
      les cultures de la recette absentes de la zone nommée** (mesurées par le SELECT avant),
      jamais la seule langue du message : sinon le tour suivant nomme la langue suivante et
      retombe sur l'issue 3, un STOP sans défaut réel. Chaque itération : créer les dimensions
      de la zone nommée (recette ci-dessus — une transaction, inverse, avant/après, recycle)
      puis relancer le dry-run. Trois issues, et elles seules :
      1. **Le dry-run passe** (plus aucun refus de dimension) — la boucle est close : la carte
         d'apps imprimée confirme la zone de chaque app cible, l'étape 10 peut suivre.
      2. **Le refus nomme une autre zone** — reprendre l'itération pour elle, même recette
         (toutes ses cultures absentes, donc). Le nombre de zones est fini et petit (en préprod,
         toutes les apps cibles vivent en zone 3 ; la zone 2, défaut DNN, n'en porte aucune) :
         la boucle converge en peu de tours.
      3. **Le refus nomme une zone déjà traitée** — **STOP** : les cultures y ont été insérées
         au tour précédent et le refus persiste. Ré-insérer créerait un **doublon de
         dimension** — deux `en-US` dans la même zone, précisément ce que la garde « jamais le
         premier `en-US` venu » interdit. Première hypothèse à lire : la **casse**
         d'`ExternalKey` — l'outil compare par égalité **exacte** (`en-US`, `pt-BR`) quand le
         SELECT de l'opérateur suit la collation de la colonne, peut-être insensible à la casse
         (supposé, non mesuré) : une culture stockée `en-us` serait vue par le SELECT et
         refusée par l'outil. Puis : INSERT non pris, ou zone mal lue. Diagnostiquer en lecture
         seule, sortie complète au dashboard.
      ⛔ **Tout autre refus = STOP immédiat**, hors boucle : `REFUS (G1)` / `REFUS (G2)` /
      `REFUS (G3)` — gardes d'entités (paire (app, guid) multi-entités · vainqueur de carte
      sans marge · carte non injective) ; `REFUS: app <a> du registre introuvable sur la cible
      (aucun GUID reconnu)` — émis par `build_app_map`, qui tourne **avant** `fetch_target` ;
      ou **tout message hors du gabarit `dimension … absente de la zone …`**. Aucun ne se
      résout par des INSERT de dimensions. Sortie complète au dashboard.
12. **#135** : article News + liens de téléchargement (→ assets de la release finale, `Print.Play` =
    nom réécrit par GitHub — `--clobber` cible le nom **publié**), méta/sitemap/og:image.
    ⛔ **Aucun `--clobber` sur les assets de `v2.1.0-review`** : les relecteurs la lisent. La release
    finale de #134 est créée **à part**, et c'est elle que les liens servent.
13. **Texte légal anglais — re-vérifier** (le geste est en 6 bis) : les 2 pages `/en/` tiennent après
    la phase 2 — la ré-injection EAV ne doit pas les avoir écrasées (cache-buster, mêmes marqueurs).
14. **R1 — les 2 fichiers E12** (classe « prod 200 + cible 404 ») :
    `Portals/1/Images/github-mark.png` (**6 393 o**, `sha256` `367d5cb2…`) et
    `Portals/1/adam/News5/4bLeKJXdFUGdPtYu4lCPnA/Image/Open-Store.png` (**source brute 40 766 o**,
    `sha256` `5964179c…` — ⛔ **pas** le dérivé 14 708 o : déposer la source reproduit les deux
    formes d'URL, le handler dérive à la volée).
    **Source** : l'**arbre servi de la préprod** — donc la copie de l'étape 5 les porte déjà. Ces
    deux chemins **n'existent pas dans le dépôt** (`git ls-files` : 0 fichier suivi), d'où
    l'imprécision que l'audit web2 a relevée : rien à récupérer depuis un checkout. Mesuré le
    14/11 : **200 sur les deux hôtes, octets identiques** (6 393 o et 40 766 o, `sha256` égaux).
    **Contrôle** : `sha256` cible == `sha256` de la **prod lue le jour J** (curl, cache-buster) ·
    200 des deux côtés, statique sans query string · re-jeu É2 home/actus : les bandes causales de
    1 148 px disparaissent (**contrôle inverse** : si l'écart persiste, la cause était fausse — le dire).
    **Rollback** : supprimer les 2 entrées (fichier + répertoire GUID pour le second). Aucun autre levier.
15. **R2 — les 16 wrappers mindmap inline** (famille `included`, **pas** `_ext`) : 8 langues ×
    {`Fallacies_<lang>.html`, `Argumentation_Virtues_<lang>.html`}.
    ⚠️ **L'arbre en compte 17** : le 17ᵉ, `Fallacies_cards_fr.html` (FR seul), est **404 sur les
    deux hôtes** (re-mesuré le 14/11 ; déjà 404/404 au relevé de parité du 02/11). Ce n'est donc
    **pas** une route à restaurer — le déployer créerait une route **neuve**, hors périmètre de
    bascule. ⇒ **le périmètre reste 16** : il compte les routes **servies**, pas les fichiers
    **committés**. Contrôle : **16/16 en 200** avec tailles substantielles (0,50 → 5,32 Mo) ; les
    2 liens de `/Acheter-le-jeu` résolvent.
    ⚠️ Les 16 `_ext` sont, eux, servis **par la prod** (32 = 16 inline + 16 `_ext` au relevé du
    02/11) mais **pas** par la préprod : leur exclusion est une **décision** du runbook du 22/09
    (famille `<object>` non autoportante → 16 pages vides), **pas** un oubli. Conséquence assumée :
    ces 16 routes passeront en 404 au cutover. Aucun fichier du dépôt ne les référence.
16. **R3/#133** : OWL avec la release (gated).

### Phase 3 — Contrôles post-bascule (jamais déclarés sans mesure)

17. **Les 4 contrôles en une commande** — [`tools/1105-cutover-day-controls.py`](../../tools/1105-cutover-day-controls.py) :
    routes 8 cultures × 4 surfaces, texte légal EN (deux sens, marqueur par page), compteurs Règles
    par langue, journal Application. Lecture seule, cache-buster et `-L` compris, code de sortie 1
    au premier contrôle dur en échec. Ligne de base préprod mesurée le 05/11 : 32/32 routes 200,
    0 `FileLoadException` sur 24 h — c'est à cette mesure-là que la prod se compare, pas au vide
    ([rapport](controles-jour-j-baseline-preprod-2026-11-05.md)).

    ```powershell
    python tools/1105-cutover-day-controls.py --base-url https://www.argumentum.games `
        --label prod-jour-j --out docs/quality/controles-jour-j-prod-<date>.md
    ```

    ⚠️ **Site endormi** : une requête d'**éveil** précède la passe, avec sa propre borne
    (`--wake-max-time`, défaut **240 s** — mesuré 93 à 107 s sur la prod endormie le
    24/11, rapporté par web2 ; la borne de passe reste 60 s). Le rapport la nomme
    (« Réveil : `GET /` → 200 en N s »). **Aucune** réponse HTTP dans cette borne ⇒
    contrôles 1 à 3 **non mesurés**, code **1**, et la passe ne tourne pas : une borne
    au lieu de 42 requêtes. Un statut HTTP à l'éveil (même 503) n'est **pas** ce cas —
    le site répond, la passe le juge.

    ⚠️ **Le journal lu est celui de la machine qui exécute le script**, et le script nomme
    désormais cette machine dans sa sortie (`machine dont le journal est lu : …`). Le jour J,
    il doit donc tourner **sur le VPS de la prod** : lancé depuis un autre siège, son
    `0 FileLoadException` décrit le mauvais journal et le contrôle est **vert à tort** — la
    forme exacte du défaut que ce contrôle existe pour attraper (`HTTP 200` avec 32
    `FileLoadException`). Si la ligne ne nomme pas l'hôte attendu, le verdict du §4 ne vaut
    rien, même vert.

18. **Routes** : accueil 200 ×8 langues + `/terms`/`/privacy` par culture + `/fa/Actus` = 5 articles (C1).
    (Le script couvre les trois premiers ; `/fa/Actus` reste manuel.)
19. **Contenu** : compteur Règles 3→10 par langue (#1502) ; 21/21 sondes visa par langue (#457) ;
    ancres CSV du sweep 2c. ⚠️ Mesuré en préprod le 05/11 : **1 occurrence** du 3→10 par page
    `/Règles`, et les intervalles servis sont 1→8, 1→20, 2→8, 3→10 — le « ×5 formes » de #1502 ne se
    reproduit pas tel quel ; c'est la ligne de base qui tranche au jour J, pas le chiffre de l'issue.
    ⚠️ Ces intervalles **ne viennent pas** de la réinjection : ils sont posés par l'étape **compteurs
    de joueurs** de la ligne #457 (dim-0 sur `MinNbPlayers`/`MaxNbPlayers`, P23-31). La prod sert
    aujourd'hui École 4→10 et Bingo 1→10 ; la carte imprimée dit 3→10 et 1→20. Un jour J qui omet
    cette étape passe les routes au vert **et laisse les deux mauvais compteurs en ligne** — c'est le
    contrôle de contenu, pas celui des routes, qui le voit.
20. **Balayage 2c** (R5) : prod-200 + cible-404 = gap ; **témoins obligatoires** (un marqueur connu
    présent + un absent), cache-buster `?cb=` sur chaque sonde, `-L` systématique, statiques sans
    query string.

### Interdictions permanentes (R7, inchangées)

⛔ zéro écriture prod hors fenêtre de bascule · ⛔ `/MIR` et synchronisation de répertoire ·
⛔ `git checkout` sur un webroot · ⛔ copie du `web.config` préprod → prod · ⛔ `iisreset` (recycle =
touch `web.config` mtime, sha inchangé) · ⛔ GTM/GA à rejouer (R4 — clos 08/10) ·
⛔ retour à un mot de passe Clear (#1091) · ⛔ suppression sans preuve de preservation.

## 3. Ce qui reste ouvert avant que ce runbook soit exécutable

- #2132 : **site 0/3 + PDF 0/2** (5 cases) — la seule porte purement humaine.
- #134 : **release finale à créer** par ai-01 à partir de `v2.1.0-review` (préversion publique, 80 PDF) ;
  la préversion reste servie et lisible en l'état — ⛔ pas de `--clobber`.
- Volet module-level des packs (#458) : résidu en cours de livraison — la phase 0.4 liste les cultures
  complètes au moment du go-live.
- Grains #135 (brouillons, liens servis) : mesurés en parallèle ce cycle — résultats à joindre au jour J.

### Ordre d'impression du lot PDF (#1187) — mesuré le 22/11 (P23-07)

Source mesurée des consignes de tirage. Aucun prix ni volume de fabrication ici : ils vivent sur
#1187. Mesure sur l'**asset publié** de `v2.1.0-review`, pas sur un arbre de build.

- **Poker (`Argumentum_PokerCards_fr.pdf`, 32 251 668 o — mesuré sur `fr` ; les 7 autres
  langues suivent le même générateur, non mesurées)** : **334 pages = 167 couples (dos,
  face)** — dos **toujours en page impaire**, face au pair, une image par page, 180 × 252 pt
  (63,5 × 89 mm), rotation 0 sur les 334 pages. **Recto/verso, une carte par couple de pages.**
- **7 dos distincts, un par catégorie** — effectifs mesurés dans le PDF = effectifs du CSV
  `Argumentum Scenarii - Cards.csv` : relation intime **36** (p. 89-159) · vie
  professionnelle **30** (p. 161-221) · mythologie **27** (p. 35-87) · vie personnelle
  **25** (p. 287-333 — 24 dos dans la plage, le 25ᵉ est l'exception p. 201 ; symétriquement,
  la plage vie pro ci-dessus l'inclut) · pop culture **18** (p. 223-257) · histoire **17**
  (p. 1-33) · politique **14** (p. 259-285) — Σ = 167. L'ordre des cartes suit l'ordre des
  lignes du CSV, vérifié aux **8** frontières de blocs (9 blocs — l'exception scinde
  « vie professionnelle ») ; **une exception mesurée** : la carte 101
  « La promotion ratée » (vie personnelle) est imprimée aux pages 201-202 **au milieu du
  bloc « vie professionnelle »**, avec le dos de sa propre catégorie — c'est l'ordre du CSV,
  pas un défaut de mise en page.
- **Tarot (`Argumentum_TarotCards_{lang}.pdf`)** : 379 pages — **ordre physique mesuré**
  (contre-revue po-2024 du 22/11, instrument indépendant) : **Rules p. 1-15 en tête**, recto
  seul (aucun dos natif) · **Memo p. 16-29** = **7 couples identiques** (2 images alternées
  ×7) · **Fallacies p. 30-379** = **175 couples, dos en pages PAIRES** — ⚠️ **parité inverse
  du Poker** (dos impairs) : un repérage par parité réutilisé d'un deck à l'autre se trompe
  de moitié de deck. La décomposition arithmétique du 28/10 (po-2023, confirmée ai-01) reste
  exacte ; les feuilles Rules sortent recto seul ou verso blanc.
- **Sens de retournement duplex : non mesuré** — les profils d'encre des dos sont quasi
  symétriques, l'instrument ne tranche pas ; à confirmer sur **épreuve** avant tirage.

## 4. Révision du 2026-11-05 — delta de la contre-relecture

Sept corrections appliquées à la v1 du 04/11 (contre-relecture ai-01 + web2). La v1 n'est pas
conservée à part : ce paragraphe **est** le delta.

1. Le VPS est nommé par **la lane qui l'exploite** (`myia-web2`), plus par un nom de machine ambigu
   — `myia-web1` désigne l'ancien VPS — 5 endroits, bandeaux de remplacement compris.
2. Colonne **Lane** réécrite selon le partage : web2 = toutes les écritures prod ; po-2023 = les
   scripts d'essai à blanc sur la préprod et les mesures ; owner = fenêtre de bascule et valeur
   #1091 ; ai-01 = release finale et verdicts.
3. `v2.1.0-review` est une **préversion publique** (`isDraft=false`, `isPrerelease=true`, publiée le
   27/10 à 23:54:30Z, 80 assets) — **pas** un brouillon, 4 endroits corrigés. Le retour arrière
   « draft, retirage possible avant annonce » ne tient donc pas : le geste #134 est de **créer la
   release finale** à côté, la préversion restant telle quelle.
4. **Texte légal EN** déplacé en phase 1, juste après l'étape 6 (nouvelle étape **6 bis**) : la cause
   est l'installeur DNN qui **écrase le neutre** `GlobalResources.resx` — il n'existe **aucun**
   `GlobalResources.en-US.resx`, le neutre *est* le texte anglais. Contrôle dans les deux sens.
5. **#2132** = **5 cases** (site ×3 : Jean-Sylvain, Adeline, Thomas ; PDF ×2 : Adeline, Thomas),
   toutes décochées — et non « 0/3 ».
6. (web2-A) Étape 5 : **la source de la copie est nommée** — l'arbre **servi** de la préprod, qui
   **est** le checkout git (mesuré le 22/11, P23-05 : site id 52, `physicalPath` → `DNNPlatform`
   du checkout — l'« hors dépôt depuis le 08/10 » était faux) ; ce qui ferme la mine #1049 reste
   le contrôle par **comptes** (4 comptes + taille, rendus par le script du jour J — étape 5),
   qui **précède** la mise en service. La copie du jour J part **refresh** : 2 349 fichiers
   suivis + les blobs OWL ont changé depuis le rafraîchissement du 20/09 (phase 2 répétée à
   blanc, P23-05).
7. (web2-B) ⛔ **Aucun `--clobber` sur les assets de `v2.1.0-review`** — posé au retour arrière de
   #134 et à l'étape 12.

Deux mineurs en plus : « reste **une** archive » ; et les **38 redirections à poser** vs les
`dependentAssembly` **déjà présents** dans le `web.config` vivant sont **deux objets distincts** (le
runbook les opposait comme un désaccord de mesure). Le préfixe `sha256` du `web.config` prod est
retiré ; le **compte `dependentAssembly`** (~176) est retiré aussi, remplacé comme discriminateur
par les **4 comptes** de l'étape 5 (rendus par le script du jour J, jamais par `grep` — absent du
PATH PowerShell du VPS) : la preuve utile sur un fichier qui porte des secrets
est un **compte**, jamais une ligne affichée — et jamais un décompte de balises fragiles à
re-mesurer. La **taille** (83 211 o) reste le cinquième témoin.

*po-2023*

## 5. Mesures du 2026-11-05 — le geste d'avance préprod, exécuté et vérifié

L'avance du clone préprod a été faite **de jour** (11:23, fenêtre 10 h-17 h), à la tête de master
au moment du geste — `8c74ee13` → **`4332d26a`** (resx `ru-RU`, `ar-SA`, `fa-IR`).

| Contrôle | Mesure |
|---|---|
| Empreinte de l'arbre sale | `sha256(porcelain trié)` = `ed3c4242…a33a38e` — **identique avant et après** |
| Arbre sale | **22 `M` + 49 `??`** (un *compte* ne dit pas lesquels : le témoin est l'empreinte) |
| Blob-égalité des 3 resx servis | `ebaa6d00` (ru-RU) · `e7892ac1` (ar-SA) · `4295c958` (fa-IR) — identiques à `origin/master` |
| Accueils | **8/8 → 200** après stabilisation (0,23-0,33 s) |
| Contrôles jour J rejoués | 32/32 routes, 8/8 Règles portent 3→10, **0 `FileLoadException`** (530 événements / 24 h) |
| `web.config` | **non touché** — mtime du 04/11, 110 419 o (préprod ≠ les 83 211 o de la prod) |

⭐ **Fait opérationnel à retenir pour le jour J** : écrire dans `App_GlobalResources` déclenche un
**redémarrage d'app-domain ASP.NET** (surveillance de fichiers). Pendant quelques secondes, 2 sondes
sur 8 ont rendu `000` — connexion non aboutie, **jamais** un `500` — puis 8/8 à `200`. Aucun
événement WAS `5074`/`5186` pour ce pool (un recycle de *worker process* en produirait) : c'est donc
un app-domain, pas un processus. Deux conséquences :

- tout geste qui touche un `resx` **coupe le service quelques secondes** — acceptable en fenêtre de
  jour, à ne jamais laisser tomber pendant une relecture du soir ;
- une sonde lancée **pendant** la fenêtre lit `000` : le `000` d'abord, le `200` ensuite, ce n'est
  pas une instabilité du site mais l'ordre des mesures. Rejouer après stabilisation, et ne jamais
  conclure sur la première volée.

## 6. Répétition OWL en préprod (#133, option A) — 2026-11-05

Item ③ du pool. Geste exécuté **de jour**, annoncé au dashboard avant application.

**Ce que la mesure a changé dans le geste** : les blobs du dépôt déclarent
`ontologyIRI="https://www.argumentum.games/argumentum_fallacies.owl#"` et
`…argumentum_virtues.owl#`. Le fichier du dépôt s'appelle `argumentum.owl`, mais **l'IRI
désigne `argumentum_fallacies.owl`** : la cible du déploiement est donc le **nom déjà servi**.
L'option A n'est pas un renommage — c'est le remplacement d'un artefact périmé, plus l'ajout
du fichier absent.

| Objet (racine du webroot) | Avant | Après |
|---|---|---|
| `argumentum_fallacies.owl` | 4 795 192 o, non suivi, mtime 28/08 (**périmé**) | **5 993 701 o** — blob du dépôt |
| `argumentum_virtues.owl` | **404** | **1 094 832 o** — blob du dépôt |

Contrôles joués, dans cet ordre :

1. **Préservation avant remplacement** — l'artefact de 4,8 Mo n'est suivi nulle part : copie
   nommée hors arbre servi, `sha256` de la copie comparé à l'original (`258a9e43…`, identiques).
   C'est aussi le retour arrière.
2. **Blob-égalité disque** — `sha256` du fichier servi comparé au blob du dépôt : `85e4c593…`
   (fallacies) et `d4099c79…` (virtues), identiques.
3. **Octets réellement servis** (pas ceux du disque) — deux `GET` avec cache-buster :
   `HTTP 200` · **5 993 701 o** · `85e4c593…` et `HTTP 200` · **1 094 832 o** · `d4099c79…` :
   taille exacte **et** `sha256` égal au dépôt. Un 200 de bonne taille ne prouve pas le contenu ;
   les trois témoins sont demandés.
4. **`Content-Type` servi** (P23-06, mesuré le 21/11 en préprod) — **`application/xml`** sur les
   deux URLs, avec `X-Content-Type-Options: nosniff` (lecture d'en-tête `curl -sI` avec
   cache-buster, jouable en même temps que le témoin 3). C'est le **quatrième témoin** : les
   trois premiers restent verts si la carte MIME dérive — un `web.config` dont la carte
   `fileExtension=".owl"` (l'un des 4 comptes de l'étape 5) disparaît à la copie ou à la montée
   DNN répond **404.3** (carte MIME absente), et une carte différente sert
   `application/octet-stream` sur un `200` de bonne taille : l'OWL devient des octets opaques
   pour tout consommateur. `application/xml` **et** `nosniff` : les deux, sur les deux URLs.

⚠️ **Reste à faire, et ce n'est pas un geste de déploiement** : aucune page ne pointe encore vers
ces fichiers (mesuré : 0 lien servi). Le lien se pose sur la **page des
téléchargements** (#135), qui est réécrite dans les 8 langues pour la mise en ligne (décision
ai-01 du 05/11) ; la page Argumentation n'est pas modifiée. Sans ce lien, l'OWL est correct et invisible.

## 7. Révision du 2026-11-14 — les 3 retours de l'audit web2

Trois corrections relevées par l'audit en lecture seule de la lane web2
([c.6863975698](https://github.com/ArgumentumGames/Argumentum/pull/2148#issuecomment-6863975698)).
Chacune est **re-mesurée** avant d'être écrite ici ; aucune n'ajoute de valeur de production.

1. **Étape 7 (#1091) — retirée.** Décision ai-01 du 14/11. Le motif est au **canal privé** : il
   porte un état de compte, et le dépôt n'en reçoit aucun. La ligne de la table maîtresse reste
   comme trace, le geste n'est plus exécuté, et l'interdiction de retour à Clear reste en vigueur.
   Le numéro 7 est conservé pour ne pas décaler les étapes 8 à 16, auxquelles §2 et §4 renvoient.
2. **Étape 14 (R1) — la source des 2 fichiers E12 est nommée.** Le runbook du 22/09 la portait
   (« copie depuis prod fraîchement lus, `curl`, cache-buster ») ; la consolidation du 04/11
   l'avait perdue, d'où la question de l'audit. Re-mesuré le 14/11 : les deux chemins répondent
   **200 sur la prod *et* sur la préprod**, **octets identiques** (`367d5cb2…` · 6 393 o ;
   `5964179c…` · 40 766 o), et `git ls-files` n'en suit **aucun**. La source du geste est donc
   l'**arbre servi de la préprod** (déjà copié à l'étape 5) ; la référence d'octets reste la
   **prod lue le jour même**.
3. **Étape 15 (R2) — 16, et le 17ᵉ est nommé.** L'arbre compte **17** wrappers inline ; le 17ᵉ,
   `Fallacies_cards_fr.html`, est **404 sur les deux hôtes** (14/11, et déjà au relevé de parité
   du 02/11) : il n'est pas dans le périmètre jour J, et le déployer créerait une route neuve.
   Le décompte **16** est donc exact — il décrit les routes **servies**, pas les fichiers
   **committés**. Même distinction pour les 16 `_ext` : servies par la prod, absentes de la
   préprod, exclues **par décision** (famille `<object>` non autoportante), pas par oubli.

## 8. Révision du 2026-11-21 → 22 (P23-25, P23-26, P23-07, P23-05) — dimensions 2sxc, alias de langue, ordre d'impression, racine servie de la préprod

Deux gestes manquaient au déroulé ; ils sont écrits aux étapes **8 (étendue)** et **11 bis**
(nouvelle), sur arbitrages ai-01 du 21/11. L'état de référence cité dans les deux étapes est le
**relevé préprod en lecture seule du 21/11** (dashboard workspace po-2023) : dimensions zone 3
complètes, `PortalAlias` du portail 1, fallbacks de langues, `DefaultPortalAlias` +
`PortalAliasMapping`.

1. **Étape 8 étendue** — l'ancienne étape ne couvrait que la culture de l'alias racine (D2).
   L'état cible complet : racine `www.argumentum.games` fr-FR primaire ; 7 alias de langue en
   sous-chemins 2 lettres avec les codes complets mesurés (`/pt`→**pt-BR**, pas pt-PT) ; les
   rangées `argumentum2.dnndev.me/<cc>` créées par l'activation **réécrites en place** (⛔ aucun
   DELETE) ; `DefaultPortalAlias` aligné sur www ; fallbacks selon la préprod (en→fr, ru→en,
   5 vides — ⚠️ ≠ défaut DNN) ; alias `localhost:8123` non primaires pour la répétition.
   Mesures qui ont fait écrire l'étape : la préprod porte `DefaultPortalAlias` sur l'hôte
   dnndev et sert pourtant correctement (`PortalAliasMapping='CANONICALURL'`) — l'état cible
   reste plus sûr que la préprod, et R5 (étape 20) n'est une mesure qu'avec 0 redirection
   vers un autre hôte.
2. **Étape 11 bis (nouvelle)** — activation portail des 8 langues + création des dimensions
   2sxc de la zone des apps, recette `1781-i1b` (INSERT SQL documenté, pas l'UI). Les ids et
   zones se **mesurent** sur la base cible (zone = `ZoneId` des apps cibles de la carte
   d'apps imprimée par le dry-run P23-24 v4), jamais copiés de la préprod. Dépendance posée
   dans les deux sens : l'étape 10 (outil de ré-injection) **refuse** une dimension absente
   (ordre 11 → 11 bis → 10) et l'étape 8 s'applique **après** l'activation sur la prod.
3. **Étape 11 bis (P23-26) — la boucle refus → zone → nouveau dry-run écrite dans l'étape** :
   le refus attendu du premier dry-run cite la zone **dans son message** (`dimension <lang>
   absente de la zone <zone> (app <app>)`, sans préfixe — l'outil refuse avant d'imprimer la
   carte d'apps). Le message ne nomme que la **première** langue manquante : chaque tour insère
   **toutes** les cultures de la recette absentes de la zone nommée — **8 au total, fr-FR
   comprise** (l'outil ne nomme jamais fr, absent de `LANGS`). Conditions d'arrêt : dry-run qui
   passe (la carte imprimée confirme les zones) · refus nommant une zone **déjà traitée** = stop
   (ré-insérer créerait un doublon de dimension ; première hypothèse = la casse d'`ExternalKey`,
   l'outil comparant par égalité exacte) · **tout autre refus** (`REFUS (G1)/(G2)/(G3)`,
   `REFUS: app … introuvable sur la cible`, message hors gabarit) = stop, hors boucle.
4. **Résidu #1091 retiré du partage des lanes (§1)** : la ligne attribuait encore à l'owner la
   « valeur du mot de passe #1091 le jour même », en contradiction avec le retrait de l'étape 7
   (§7, décision ai-01 du 14/11) — l'owner ne garde que la **fenêtre de bascule**. Aucune autre
   mention de #1091 ne change : la ligne #1091 de la table maîtresse (§1) et le §4 restent des
   traces historiques.
5. **Section « Ordre d'impression » + correction du sous-code MIME (P23-07, 22/11)** : le §3
   reçoit la structure de tirage mesurée du lot PDF (Poker fr : 334 pages = 167 couples
   dos-impair/face-pair, 7 dos par catégorie, effectifs = CSV ; Tarot : Rules recto seul —
   mesure du 28/10). **Retouche du 22/11 au matin (contre-revue po-2024, mesurée — push à 05:59Z)** : l'ordre
   **physique** du Tarot est précisé — Rules **en tête** (p. 1-15) · Memo p. 16-29 = **7
   couples identiques** · Fallacies p. 30-379, dos en pages **paires** (parité inverse du
   Poker) ; Poker : la plage vie personnelle est annotée (24 dos + l'exception p. 201) et les
   frontières recomptées **8** (9 blocs). Le témoin 4 (§6) citait pour une carte MIME absente
   le sous-code IIS d'un **autre** défaut (« URL trop longue ») — le sous-code d'une carte
   MIME absente est **404.3** (relecture ai-01 du 21/11,
   [c.7071193468](https://github.com/ArgumentumGames/Argumentum/pull/2335#issuecomment-7071193468)).
   Corrigé au §6 ; le sous-code erroné n'est plus cité nulle part dans ce document.
6. **Racine servie de la préprod + phase 2 à blanc (P23-05, 22/11)** : le runbook disait « copie
   hors dépôt en service depuis le 08/10 » (table §1, étape 5, §8-A) — **faux** : mesuré dans
   `applicationHost.config` (site « dnn.argumentum.myia.io », id 52), le `physicalPath` sert le
   dossier `DNNPlatform` **du checkout git**. Corrigé aux trois endroits pour que le document ne
   dise qu'une chose ; le contrôle par comptes de l'étape 5 reste le garde anti-mine #1049 (le
   `web.config` versionné n'existe plus depuis #1055 — le résidu attrapé est un
   template/`REPLACE`). Phase 2 répétée **à blanc** : la copie du 20/09 porte l'artefact OWL
   périmé et `virtues` absent — refresh obligatoire avant tout geste (2 349 fichiers suivis +
   blobs). Rien copié, aucune écriture IIS, lecture seule.

## 9. Révision du 2026-11-23 (P23-33) — portes et sondes du jour J : arrêt à 14:54Z, décision à 20:40Z

Sources : les posts dashboard `ai01-repetition-validee-20261123` (validation de 12:40Z) et
`ai01-g1g3-tranches-20261123` (décision de 20:40Z, après l'arrêt du jour J à 14:54Z). Ce qui suit
**inscrit les portes et leur mode de défaillance**, sans recopier le script du jour J : celui-ci est
`tools/1105-cutover-day-controls.py` (17) et le déroulé de web2, dont les empreintes ne changent pas.
Le déroulé ci-dessus n'est pas modifié : chaque porte ci-dessous dit **où elle se place**.

Ce que `1105` ne voit pas, et pourquoi : il mesure le **rendu HTTP**, le texte légal, les compteurs de
contenu et le journal Application **de la machine qui l'exécute**. Il ne lit ni les **ACL du dossier
servi**, ni les **réglages des pools**, ni les **réglages de base** (`PortalSettings`/`HostSettings`).
Ces trois surfaces sont donc des portes de lecture **avant** le swap — un écart s'y produit sans que
`1105` rougisse, et parfois sans que la préprod l'ait jamais rencontré.

### G1 — droits du pool de prod sur le dossier servi (avant le swap)

- **Porte** : le pool qui servira la prod au swap (`Argumentum`) doit porter, sur le dossier du staging,
  **le même droit** qu'il porte sur `wwwroot` — `(OI)(CI)(M)` à la racine, hérité. C'est aussi celui que
  le pool de test porte déjà sur ce dossier, éprouvé en R6.
- **Mode de défaillance mesuré (arrêt du 23/11 14:54Z)** : le pool de prod n'avait **aucun `(M)`** sur le
  dossier du staging — la répétition sur le site de test ne le montre jamais, puisqu'elle s'exécute avec
  le pool de test. Après le swap, le site de prod aurait refusé toute écriture du pool : `App_Data`,
  `Portals`, `DesktopModules`, `bin`… DNN écrit dans **toute sa racine**, pas dans un seul sous-dossier
  (mesure po-2023 du 23/11 sur la préprod : **851 fichiers appartenant au pool** sur 30 jours, répartis
  sur 5 zones de premier niveau et 11 dossiers de second niveau — un octroi borné à un sous-dossier ne
  suffirait pas).
- **Critère d'arrêt** : l'absence du droit, ou un réglage de pool qui diffère, arrête la séquence — on ne
  swap pas « pour voir ».
- **G1.a — réglages des deux pools (lecture)** : `managedRuntimeVersion`, `managedPipelineMode`,
  `enable32BitAppOnWin64` comparés entre les deux pools — **STOP** si l'un diffère (le dossier n'a été
  éprouvé qu'avec les réglages du pool de test) ; `processModel.loadUserProfile` listé seulement.
- **G1.b — dossiers dont l'héritage est coupé (lecture)** : un droit posé à la racine **n'atteint pas** un
  dossier dont l'héritage est rompu. Passe si `protégés = 0` **et** les deux comptes d'erreurs à 0 : une
  erreur de lecture rend l'instrument aveugle, un 0 lu comme « rien » n'est alors pas une mesure.
- **G1.c — sauvegarde avant le geste** : `icacls <racine> /save`, empreinte du fichier de sauvegarde et
  `icacls <racine>` postés **avant** G1.d. C'est le dry-run du geste.
- **G1.d — le geste borné** : `icacls <racine> /grant 'IIS APPPOOL\Argumentum:(OI)(CI)(M)'`. **Pas de
  `/T`** : c'est l'héritage qui propage. Le droit s'ajoute, rien n'est retiré ; ni `wwwroot` ni le gel
  #972 ne sont touchés. Attendu : 1 fichier traité, 0 échec.
- **G1.e — relecture** : racine `(OI)(CI)(M)` sans `(I)` ; `App_Data`, `Portals\1`, `Portals\_default`,
  `bin` : `(I)(OI)(CI)(M)` ; `web.config` : `(I)(M)`. **STOP** sur une ligne vide. ⚠️ Le filtre de
  recherche se termine par **`:`** — `IIS APPPOOL\Argumentum` sans les deux-points matche aussi
  `IIS APPPOOL\Argumentum-staging`, et la relecture validerait alors le mauvais principal (cause de la
  première lecture de G1).
- **Inverse, pour mémoire** : `icacls <racine> /remove:g 'IIS APPPOOL\Argumentum'`. ⛔ **Ne pas le jouer au
  retour arrière** : le droit est inoffensif quand la prod resert `wwwroot`. Aucun nettoyage sans décision.

### G1b — mode d'authentification de la base du staging (avant le swap)

- **Porte** : si la base du staging est jointe en sécurité intégrée, l'**identité du pool** devient un
  principal de base — et le pool de prod doit y exister. Compte seulement, jamais de contenu :
  occurrences de `integrated security`/`trusted_connection` dans les deux `web.config`.
- **Passe** si le staging rend `integrated=0` (authentification SQL : l'identité du pool ne joue pas).
  Sinon, `SELECT name FROM ArgumentumGames_staging.sys.database_principals WHERE name =
  N'IIS APPPOOL\Argumentum'` doit rendre **1** ligne ; **sinon STOP**.
- **Mode de défaillance** : après le swap, le site de prod joint la base du staging avec **son** pool. Une
  connexion intégrée sans principal correspondant = refus de connexion pour toutes les requêtes — un
  défaut que la préprod ne reproduit pas, son pool y existant déjà.

### G2 — comptes `<rewrite>`/`<rule ` égaux entre les deux `web.config` (avant le swap)

- **Porte** : même nombre de blocs de réécriture et de règles des deux côtés. Mesuré au jour J : **0 = 0**.
- **Mode de défaillance** : une règle présente d'un seul côté change le routage sans toucher au code —
  et `1105` ne mesure que le résultat final, pas la règle qui l'a produit.
- **Limite** : G2 compare des **comptes**. Une **valeur** de réglage peut différer à comptes égaux : c'est
  exactement le cas de `SSLSetup`, porté par G3.

### G3 — réglages de base qui diffèrent, dont `SSLSetup` (avant le swap, geste après l'étape 6)

- **Porte** : noms des réglages `PortalSettings`/`HostSettings` qui diffèrent entre les deux bases.
  **STOP** si l'un appartient à `ssl/alias/url/domain/redirect/https/smtp` (hors `DefaultPortalAlias`,
  traité à l'étape 8). Mesuré au jour J : STOP sur `SSLSetup`.
- **Ce que `SSLSetup` fait (source upstream DNN, lu le 23/11)** : `SiteSslSetup` = `Off` (0), `On` (1),
  `Advanced` (2) ; chargé depuis `PortalSettings` (défaut `Off`). Sous `On`, le réécrivain d'URL de DNN
  redirige toute requête non sécurisée vers HTTPS ; sous `Advanced`, seules les pages marquées sécurisées
  (`BasicUrlRewriter`, `PortalSettingsController`).
- **Mesure ai-01 du 23/11 20:20Z, depuis l'extérieur** : la 301 HTTP→HTTPS de la prod est **émise par
  DNN** (cookies `dnn_IsMobile`/`language`, corps « Object moved » d'ASP.NET), pas par IIS — cohérent
  avec G2 (0 règle des deux côtés). Elle dépend donc de `SSLSetup` **dans la base servie**. La valeur du
  staging avait été posée pour un site de test **en HTTP** ; servie en prod, elle aurait laissé le site en
  HTTP clair — et la sonde P1 ne l'aurait vu qu'après la bascule.
- **Geste (décision de 20:40Z)** : `SSLSetup` du staging **recopié depuis la base de prod**, en transaction
  gardée (`LEN ≤ 10`, `@@ROWCOUNT = 1`, `XACT_ABORT`), avec les lignes `avant`/`apres` postées. ⚠️
  **Placement : dans la phase 1, après l'étape 6 bis, avant l'étape 7** (post ai-01 : « dans le §6bis,
  après le §6 ») — avec la valeur de la prod, le site de test redirige vers HTTPS : le faire plus tôt casse
  la répétition. La valeur se lit depuis la prod ; elle ne transite jamais par le dashboard dans l'autre sens. **Inverse écrit dans un fichier avant** la
  transaction (la valeur `avant` du staging). Ce réglage ne joue pas au retour arrière, qui resert
  `wwwroot` avec la base de prod.
- **Les autres écarts de G3 n'arrêtent rien** : ce sont ceux d'une montée de version (clé de zone 2sxc
  renommée, skins par défaut neufs, caches, version CRM).

### Sondes après la bascule — P1, P1b, P2 (un écart est un `[WARN]`, pas un retour arrière)

- **P1 — HTTP→HTTPS** : `http://www.argumentum.games/` doit répondre 301 vers HTTPS. C'est la sonde qui
  aurait découvert `SSLSetup` trop tard ; elle reste nécessaire, parce qu'elle mesure ce que le visiteur
  reçoit et pas ce que la base déclare.
- **P1b — domaine nu** : `http://argumentum.games/` et `https://argumentum.games/` doivent répondre 301
  vers `www` **avec le même schéma** (mesure ai-01 du 23/11 20:20Z). Un domaine nu qui répondrait
  autrement sortirait une partie des visiteurs du site sans que la route `www` rougisse.
- **P2 — rendu et contenu des 8 accueils** : `dir="rtl"` en ar et fa seulement, description non vide
  (référence préprod du 23/11 13:00Z : 38 à 104 caractères), 7 liens de langue, et **≥ 1 occurrence** du
  marqueur d'audience `G-VHLTL18PEW` sur chacune des 8 pages (servi par la prod **et** par la préprod,
  mesuré 20:22Z).
- **Discipline commune** : ces sondes **informent** — elles ne déclenchent pas de retour arrière. Un retour
  arrière reste régi par les critères du script du jour J, pas par une sonde de contenu.

### Limite de cette inscription

Ces portes mesurent le jour J **de cette bascule**, sur les hôtes et les bases nommés par la décision de
20:40Z. Elles ne sont pas une recette générale : une autre bascule re-mesure ses propres valeurs. La
mesure de surface d'écriture de G1 (851 fichiers) vient de la **préprod**, pas du staging — elle établit
la forme du droit (racine, hérité), pas un état de la prod.
