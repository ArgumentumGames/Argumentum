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
| **#1049** | web.config versionné = panne totale | **phase 2, au moment de la bascule** : le site servi vit dans une **copie hors dépôt** (comme la préprod depuis 08/10). Phase 1 faite (fichier sorti du dépôt, 2 tests CI) | **web2** | le dépôt ne peut plus écrire sur le `web.config` vivant ; `dependentAssembly` ~176 (pas 2) | n/a — structurel ; la panne existe seulement si le fichier versionné atteint un webroot |
| **#1091** | Compte stocké en mot de passe **Clear** (depuis 2020) | passer Clear → **Hashed** **pendant la séance de bascule uniquement** ; la valeur est choisie par l'owner — ⛔ aucune valeur, empreinte ou sel ne transite par une issue, un commit, le dashboard ou un fichier | owner (valeur, le jour même) ; **web2** (geste admin DNN) | type de membership `Hashed` en base ; login OK avec la valeur owner | ⛔ **jamais** de retour à Clear |
| **#1502** | Règles du site divergé des cartes (4-10 vs 3-10 joueurs) | couvert par la **ré-injection EAV courant** (cf. #457) : la prod ne peut pas servir le corpus tel quel | **web2** (injection prod) ; po-2023 (script + essai à blanc préprod, mesures) | compteurs « de 3 à 10 joueurs » ×5 formes sur `/Règles`, par langue ; cf. garde `RulesExplorerD1PlayerCountGuardTests` | snapshot pré-injection, transaction gardée, rollback SQL borné |
| **#457** | Localiser le site (8 langues) | se **ferme à la mise en ligne** : l'injection EAV courant + les packs de langue installés ce jour-là | **web2** (geste prod) ; po-2023 (script/essai) ; po-2024 (packs) | sondes visa 21/21 par langue ; sélecteur 8 langues ; témoins fr intacts | désinstall langpack **sans** « Delete Files » (R8) ; rollback EAV borné |
| **#135** | Article d'actus + tous les téléchargements | **jour de la mise en ligne** : publier l'article News annonçant la version + rafraîchir tous les liens de téléchargement vers la release #134 ; périmètre mesuré en amont (brouillons + relevé des liens servis) | **web2** (édition + lien prod) ; ai-01 (texte/verdict) ; po-2023 (mesures) | chaque lien servi pointe vers un asset publié de #134 ; `Print.Play` = nom réécrit par GitHub | article = brouillon tant que non annoncé ; liens = tableau de remplacement à blanc |
| **#133** | Publication OWL | gated sur #134 : publier/déployer les ontologies avec la release ; l'`fallacies.owl` **servi** aujourd'hui en préprod est un artefact non suivi (mars-2024), `virtues.owl` (1,1 Mo) déployé nulle part | **web2** (déploiement prod) ; ai-01 (verdict) | ontologies servies depuis un chemin suivi ; liens mindmap → OWL vérifiés | retrait des fichiers déployés |
| **#136** | Entités taxonomie 2sxc | état mesuré 30/10 : `Game Rule` = 5 entités × 8 langues en base, couverture complète par attribut ; le type `Rule` niveau ligne CSV n'existe pas dans l'app 60 (import non requis pour le rendu actuel) — à confirmer au go-live | ai-01 | rendu `/Règles` par langue = corpus CSV ; ancres du sweep | n/a (constat) ou rollback EAV |
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
   point où reculer ne coûte rien : le `web.config` de la copie porte **~176 `dependentAssembly`**
   (pas 2) et **83 211 o**. Jamais `git checkout` sur un webroot servi ; jamais de copie du
   `web.config` préprod → prod (le secret et les `machineKey` diffèrent).
6. **Montée DNN 9.11.1 → 10.4.1 + 2sxc 15.02 → 22.01** sur la copie, selon le plan de montée rang 2 :
   installeurs natifs (assistant DNN puis installeur d'extensions 2sxc) → **mesurer `bin/`** → une
   redirection par nom à la **version mesurée** (**38 redirections à poser** sur la copie validée —
   objet **distinct** des ~176 `dependentAssembly` déjà présents dans le `web.config` vivant, ce ne
   sont pas deux mesures du même nombre ; `DnnBindingRedirectContractTests`). Publier **seulement**
   `<runtime><assemblyBinding>`.
   Leçons intégrées : bindingRedirect **superposés** (HTTP 200 malgré des `FileLoadException` — seul
   l'EventLog les voit, mesuré 02/11) ; pile BCL 2sxc (B1-inversion, 132 §5.5(a)).
6 bis. **Texte légal anglais — restaurer les 2 valeurs neutres** (web2), **juste après l'étape 6**.
   Cause mesurée : l'installeur DNN **écrase le neutre `GlobalResources.resx`**, et il **n'existe
   aucun `GlobalResources.en-US.resx`** — le neutre **est** le texte anglais
   (`MESSAGE_PORTAL_TERMS.Text` + `MESSAGE_PORTAL_PRIVACY.Text`). Sans ce geste, `/en/` sert le
   gabarit DNN (« AGREEMENT BETWEEN USER AND ») **entre la montée et la phase 2**. Geste : restaurer
   les 2 valeurs depuis le blob de master. Contrôle (cache-buster) : `/en/terms` et `/en/privacy`
   contiennent « ARGUMENTUM SAS » et **plus** « AGREEMENT BETWEEN USER AND » — vérifié dans les deux
   sens sur le master courant.
7. **#1091** : Clear → Hashed, valeur owner, pendant la séance.
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
14. **R1** : déployer les 2 fichiers E12 (`Portals/1/Images/github-mark.png` 6 393 o ;
    `Portals/1/adam/News5/4bLeKJXdFUGdPtYu4lCPnA/Image/Open-Store.png` source 40 766 o).
15. **R2** : les 16 wrappers mindmap (famille `included`, **pas** `_ext`).
16. **R3/#133** : OWL avec la release (gated).

### Phase 3 — Contrôles post-bascule (jamais déclarés sans mesure)

17. **EventLog** : 0 exception 2sxc, 0 `FileLoadException`, 0 échec de login inattendu — c'est le seul
    instrument qui voit les redirections ratées (#2132 DoD : EventLog sur les 3 DOWN signalés).
18. **Routes** : accueil 200 ×8 langues + `/terms`/`/privacy` par culture + `/fa/Actus` = 5 articles (C1).
19. **Contenu** : compteurs Règles ×5 formes par langue (#1502) ; 21/21 sondes visa par langue (#457) ;
    ancres CSV du sweep 2c.
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
   `dependentAssembly` ~176 **précède** la mise en service.
7. (web2-B) ⛔ **Aucun `--clobber` sur les assets de `v2.1.0-review`** — posé au retour arrière de
   #134 et à l'étape 12.

Deux mineurs en plus : « reste **une** archive » ; et les **38 redirections à poser** vs les **~176
`dependentAssembly`** du `web.config` vivant sont **deux objets distincts** (le runbook les opposait
comme un désaccord de mesure). Le préfixe `sha256` du `web.config` prod est retiré : la preuve utile
est la **taille**, 83 211 o.

*po-2023*
