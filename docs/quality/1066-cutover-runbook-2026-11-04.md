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
| **#1049** | web.config versionné = panne totale | **phase 2, au moment de la bascule** : le site servi vit dans une **copie hors dépôt** (comme la préprod depuis 08/10). Phase 1 faite (fichier sorti du dépôt, 2 tests CI) | **web2** | le dépôt ne peut plus écrire sur le `web.config` vivant ; discriminateur = les 4 comptes de l'étape 5 (rendus par le script du jour J) + taille 83 211 o | n/a — structurel ; la panne existe seulement si le fichier versionné atteint un webroot |
| **#1091** | Compte stocké en mot de passe **Clear** (depuis 2020) | **Étape retirée du déroulé** (décision ai-01 du 14/11, motif au canal privé — cf. §7). La ligne reste comme trace ; le geste Clear → Hashed n'est plus exécuté. ⛔ aucune valeur, empreinte ou sel ne transite par une issue, un commit, le dashboard ou un fichier | — | n/a — étape retirée | ⛔ **jamais** de retour à Clear |
| **#1502** | Règles du site divergé des cartes (4-10 vs 3-10 joueurs) | couvert par la **ré-injection EAV courant** (cf. #457) : la prod ne peut pas servir le corpus tel quel | **web2** (injection prod) ; po-2023 (script + essai à blanc préprod, mesures) | compteurs « de 3 à 10 joueurs » ×5 formes sur `/Règles`, par langue ; cf. garde `RulesExplorerD1PlayerCountGuardTests` | snapshot pré-injection, transaction gardée, rollback SQL borné |
| **#457** | Localiser le site (8 langues) | se **ferme à la mise en ligne** : l'injection EAV courant + les packs de langue installés ce jour-là | **web2** (geste prod) ; po-2023 (script/essai) ; po-2024 (packs) | sondes visa 21/21 par langue ; sélecteur 8 langues ; témoins fr intacts | désinstall langpack **sans** « Delete Files » (R8) ; rollback EAV borné |
| **#135** | Article d'actus + tous les téléchargements | **jour de la mise en ligne** : publier l'article News annonçant la version + rafraîchir tous les liens de téléchargement vers la release #134 ; périmètre mesuré en amont (brouillons + relevé des liens servis) | **web2** (édition + lien prod) ; ai-01 (texte/verdict) ; po-2023 (mesures) | chaque lien servi pointe vers un asset publié de #134 ; `Print.Play` = nom réécrit par GitHub | article = brouillon tant que non annoncé ; liens = tableau de remplacement à blanc |
| **#133** | Publication OWL — **option A** (ai-01, 05/11) | **Répétition faite en préprod le 05/11** : les deux blobs du dépôt déployés à la racine du webroot, **sous les noms que les IRI déclarent** (`argumentum_fallacies.owl`, `argumentum_virtues.owl`) — ⚠️ le fichier du dépôt s'appelle `argumentum.owl`, l'IRI non : la cible est le **nom servi**, l'option A n'est pas un renommage. L'artefact remplacé (4 795 192 o, non suivi, mtime 28/08) était **périmé** et `virtues` **absent (404)** | **web2** (même geste en prod) ; po-2023 (répétition préprod, faite) ; ai-01 (verdict) | `HTTP 200` **et** taille exacte **et** `sha256` égal au blob du dépôt sur les deux URLs (préprod : 5 993 701 / 1 094 832, mesuré 05/11) ; puis lien depuis la **page des téléchargements** (#135, 8 langues — décision ai-01 du 05/11 : la page Argumentation n'est pas modifiée) | restaurer la **copie de préservation** de l'artefact 4,8 Mo (suivi nulle part = non régénérable ici) |
| **#136** | Entités taxonomie 2sxc | état **re-mesuré le 05/11** ([c.6662268609](https://github.com/ArgumentumGames/Argumentum/issues/136#issuecomment-6662268609)) : `Game Rule` (ct **377**, app **60**) = **5 entités** publiées, `Title` 5/5 dans les **8 cultures** — inventaire identique au 30/10, **l'import d'essai n'a pas été exécuté**. ⛔ La « couverture complète par attribut » de la v1 était **fausse** : le rapport du 30/10 n'a mesuré que `Title` et dit lui-même que l'audit par attribut **n'a pas été exécuté**. L'export prêt = **128 entités = 16 règles × 8 cultures** (#2050). Import **post-mise en ligne** : il ferait passer `/Règles` de 5 à 16 fiches, donc il **change une page relue** | ai-01 | `/Règles` fr sert **5 fiches** = les 5 entités ; ⛔ ne pas écrire « rendu = corpus CSV » (16 règles au CSV) | n/a (constat) ou rollback EAV |
| **#1180** (C1) | Filtres date pipeline News cassés sous fa-IR | **reporter le geste base** en prod : bornes typées + 2 filtres neutralisés (PR #1947) — **la prod garde encore `<=`/`>`** ; ligne go-live posée sur [#1180 c.6361658507](https://github.com/ArgumentumGames/Argumentum/issues/1180#issuecomment-6361658507) | **web2** (geste base prod) ; po-2023 (script + essai à blanc préprod) | `/fa/Actus` rend 5 articles (comme fr/ar/ru/zh) | backups entités 9434/9435 : réécrire `"<="`/`">"` puis recycle |

**Partage des lanes** (arbitrage ai-01, 04/11 — la colonne `Lane` le reflète ligne à ligne) :
**web2** porte **toutes les écritures prod** — copie hors dépôt, montée, redirections, alias, filtres
C1, ré-injection EAV, packs de langue, article et liens #135, fichiers E12, wrappers, OWL déployés.
**po-2023** écrit, pour chaque geste base, **le script + l'essai à blanc sur la préprod**, et fait les
**mesures**. **owner** = fenêtre de bascule + valeur du mot de passe #1091 le jour même. **ai-01** =
release finale (#134) + verdicts.

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
   **Source nommée** : l'arbre **servi** de la préprod, **déjà sorti du dépôt** (copie hors dépôt en
   service depuis le 08/10). ⚠️ **Pas** le checkout git lui-même : la préprod **est** un checkout FF
   sur master — copier ce checkout embarquerait le `DNNPlatform/web.config` **versionné** dans le
   webroot = la mine #1049 (500 déterministe). **Contrôle AVANT que le site soit servi** — dernier
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
8. **D2 alias racine** : `PortalAlias` racine `CultureCode` NULL → `fr-FR` **+ recycle** (l'UPDATE seul
   ne suffit pas — cache PortalAlias ; reporté « à la mise en ligne prod » par ai-01, 18/10).
9. **C1/#1180** : reporter le geste base des filtres date (bornes typées / neutralisation) — la prod
   garde encore la config fautive.

### Phase 2 — Contenu (jour J)

10. **Ré-injection EAV courant** (préprod → prod) : la prod sert une 3ᵉ génération (9/96 ancres) ;
    l'injection suit le motif éprouvé (transaction gardée, snapshot pré, anti-rejeu par dimension).
    ⛔ Périmètre déclaré : **aucune autre entité ; aucune table de membres, de commandes ou de comptes.**
11. **Langpacks installés** (R8) : `Install/Install.aspx?mode=installresources` restauré du zip
    d'upgrade, **quarantaine des strays** `Install/*/` (le trigger installe tout), install = crée la
    langue host, **n'active pas le portail** ; contrôle 0 écart `git status` avant/après.
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
6. (web2-A) Étape 5 : **la source de la copie est nommée** — l'arbre **servi** de la préprod, déjà
   sorti du dépôt, et **pas** le checkout git (copier ce checkout embarquerait le
   `DNNPlatform/web.config` versionné dans le webroot = mine #1049, 500 déterministe) ; le contrôle
   par **comptes** (4 comptes + taille, rendus par le script du jour J — étape 5) **précède** la
   mise en service.
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

*po-2023*
