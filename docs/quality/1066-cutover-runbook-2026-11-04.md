# Runbook de bascule consolidé — mise en ligne du site DNN Argumentum (2026-11-04)

> **Statut** : document de consolidation, établi à master `8c74ee13`. Aucun geste prod n'est exécuté
> par ce document — la production est **gelée** (GO owner, #972, discipline maintenue après sa clôture).
> **Remplace** : [`1066-cutover-runbook-2026-09-22.md`](1066-cutover-runbook-2026-09-22.md) (R1-R8) et
> [`../dnn-localization/132-deployment-runbook.md`](../dnn-localization/132-deployment-runbook.md)
> (phase 5 VPS) — remplacés, **aucun supprimé**. La famille `131-*` (sandbox/montées historiques)
> reste un archives, non remplacée.
>
> Ce runbook consolide ce que les deux précédents ne couvraient pas : l'**EventLog** comme instrument
> de contrôle, le **texte légal anglais** en production, la porte **#2132** (relecture humaine),
> **#135/#1502/#457** (contenu publié), le **nouveau VPS** (`myia-web1`) et l'état **DNN 10.4.1 +
> 2sxc 22.01** issu du rang 2 (02/11).

## 0. État de référence (mesuré)

| Étage | État | Preuve |
|---|---|---|
| Préprod `dnn.argumentum.myia.io` | webroot master `8c74ee13` (CDV 381), DNN **10.4.1** + 2sxc **22.01**, 8 langues servies | rang 2 exécuté 02/11 ; DB `Version` 10.4.1/1048 ; 8/8 routes 200 |
| Production `www.argumentum.games` | **gelée** : DNN 9.11.1.19, 2sxc 15.02.00, accueil 200, `/en/` = 404 (état gelé, pas incident) | web.config 83 211 o, sha256 `211be984…` (contrôle à chaque tick) |
| Contenu prod | **3ᵉ génération** antérieure aux marqueurs emoji — 9/96 ancres CSV présentes | [#458 c.6260750566](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-6260750566) |
| Relecture humaine #2132 | **0/3** (Adeline, Thomas) | ⛔ pas de relance |
| Release GitHub #134 | draft `v2.1.0-review` (80 PDF, 27/10) — verdict ai-01 attendu | #2025 |

## 1. Table maîtresse — geste, lane, contrôle, retour arrière, issue fermée

| Issue | Objet | Geste | Lane | Contrôle (DoD) | Retour arrière |
|---|---|---|---|---|---|
| **#2132** | Relecture avant sortie | **Porte d'entrée** : 3/3 validations humaines AVANT toute bascule | owner (humains) | les 3 réponses sur la page #2132 | n/a — porte, pas un geste |
| **#134** | Release GitHub | publier la release (80 PDF de `v2.1.0-review`) ; déclenche #135 | ai-01 | assets publiés, tailles = bundle | release = draft, retirage possible avant annonce |
| **#131** | Montée DNN + mise en ligne | déployer sur le VPS `myia-web1` la génération validée en rang 2 (10.4.1 + 2sxc 22.01), séquence A→J du plan de montée (`docs/dnn/2026-10-31-montee-dnn1041-2sxc2201-plan.md`) | owner (ops VPS) + ai-01 | §2 DoD ci-dessous (versions, redirections, EventLog, routes) | snapshot VPS + backup DB pré-geste ; DNS retour arrière |
| **#1049** | web.config versionné = panne totale | **phase 2, au moment de la bascule** : le site servi vit dans une **copie hors dépôt** (comme la préprod depuis 08/10). Phase 1 faite (fichier sorti du dépôt, 2 tests CI) | lane qui déploie | le dépôt ne peut plus écrire sur le `web.config` vivant ; `dependentAssembly` ~176 (pas 2) | n/a — structurel ; la panne existe seulement si le fichier versionné atteint un webroot |
| **#1091** | Compte stocké en mot de passe **Clear** (depuis 2020) | passer Clear → **Hashed** **pendant la séance de bascule uniquement** ; la valeur est choisie par l'owner — ⛔ aucune valeur, empreinte ou sel ne transite par une issue, un commit, le dashboard ou un fichier | owner (valeur) + lane (geste admin DNN) | type de membership `Hashed` en base ; login OK avec la valeur owner | ⛔ **jamais** de retour à Clear |
| **#1502** | Règles du site divergé des cartes (4-10 vs 3-10 joueurs) | couvert par la **ré-injection EAV courant** (cf. #457) : la prod ne peut pas servir le corpus tel quel | po-2023 (injection) | compteurs « de 3 à 10 joueurs » ×5 formes sur `/Règles`, par langue ; cf. garde `RulesExplorerD1PlayerCountGuardTests` | snapshot pré-injection, transaction gardée, rollback SQL borné |
| **#457** | Localiser le site (8 langues) | se **ferme à la mise en ligne** : l'injection EAV courant + les packs de langue installés ce jour-là | po-2023 / po-2024 | sondes visa 21/21 par langue ; sélecteur 8 langues ; témoins fr intacts | désinstall langpack **sans** « Delete Files » (R8) ; rollback EAV borné |
| **#135** | Article d'actus + tous les téléchargements | **jour de la mise en ligne** : publier l'article News annonçant la version + rafraîchir tous les liens de téléchargement vers la release #134 ; périmètre mesuré en amont (brouillons + relevé des liens servis) | ai-01 (édition) + po-2023 (mesures) | chaque lien servi pointe vers un asset publié de #134 ; `Print.Play` = nom réécrit par GitHub | article = brouillon tant que non annoncé ; liens = tableau de remplacement à blanc |
| **#133** | Publication OWL | gated sur #134 : publier/déployer les ontologies avec la release ; l'`fallacies.owl` **servi** aujourd'hui en préprod est un artefact non suivi (mars-2024), `virtues.owl` (1,1 Mo) déployé nulle part | ai-01 | ontologies servies depuis un chemin suivi ; liens mindmap → OWL vérifiés | retrait des fichiers déployés |
| **#136** | Entités taxonomie 2sxc | état mesuré 30/10 : `Game Rule` = 5 entités × 8 langues en base, couverture complète par attribut ; le type `Rule` niveau ligne CSV n'existe pas dans l'app 60 (import non requis pour le rendu actuel) — à confirmer au go-live | ai-01 | rendu `/Règles` par langue = corpus CSV ; ancres du sweep | n/a (constat) ou rollback EAV |
| **#1180** (C1) | Filtres date pipeline News cassés sous fa-IR | **reporter le geste base** en prod : bornes typées + 2 filtres neutralisés (PR #1947) — **la prod garde encore `<=`/`>`** ; ligne go-live posée sur [#1180 c.6361658507](https://github.com/ArgumentumGames/Argumentum/issues/1180#issuecomment-6361658507) | po-2023 | `/fa/Actus` rend 5 articles (comme fr/ar/ru/zh) | backups entités 9434/9435 : réécrire `"<="`/`">"` puis recycle |

## 2. Séquence de bascule (jour J)

### Phase 0 — Pré-requis (avant J-J, réversibles)

1. **#2132 = 3/3** (porte humaine, ⛔ relance). Sans elle, rien ne suit.
2. **#134 publié** : release 80 PDF validée par verdict ai-01 sur la draft `v2.1.0-review`.
3. **Sauvegardes** : snapshot VPS `myia-web1` + `BACKUP DATABASE ... WITH COPY_ONLY` sur la base prod
   (vérifier via `msdb.dbo.backupset`, jamais par le filesystem — le répertoire par défaut est ACL service) ;
   empreinte sha256 du `web.config` prod attendu (83 211 o).
4. **Langpacks construits au hash du go-live** : `tools/458-langpack-build.py --culture <xx-XX> --commit <hash>`
   pour chaque culture publiée ; fraîcheur = `git log <hash>..master -- '*.<culture>.resx'` **vide**.
   Les packs ar-SA/fa-IR/zh-CN/ru-RU/pt-BR déjà livrés ne couvrent que les familles livrées — vérifier le
   résidu (volet module-level en cours, #2111-#2113 et suivants).

### Phase 1 — Déploiement technique (VPS, fenêtre owner)

5. **Copie hors dépôt** du webroot validé (#1049 phase 2) — la copie devient le site servi. Jamais
   `git checkout` sur un webroot servi ; jamais de copie du `web.config` préprod → prod (le secret et
   les `machineKey` diffèrent).
6. **Montée DNN 9.11.1 → 10.4.1 + 2sxc 15.02 → 22.01** sur la copie, selon le plan de montée rang 2 :
   installeurs natifs (assistant DNN puis installeur d'extensions 2sxc) → **mesurer `bin/`** → une
   redirection par nom à la **version mesurée** (38 redirections sur la copie validée ;
   `DnnBindingRedirectContractTests`). Publier **seulement** `<runtime><assemblyBinding>`.
   Leçons intégrées : bindingRedirect **superposés** (HTTP 200 malgré des `FileLoadException` — seul
   l'EventLog les voit, mesuré 02/11) ; pile BCL 2sxc (B1-inversion, 132 §5.5(a)).
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
12. **#135** : article News + liens de téléchargement (→ assets #134, `Print.Play` = nom réécrit par
    GitHub — `--clobber` cible le nom **publié**), méta/sitemap/og:image.
13. **Texte légal anglais** : restaurer/vérifier le texte légal EN en production (DoD #2132 — la
    relecture l'exige).
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

- #2132 : 0/3 — la seule porte purement humaine.
- #134 : verdict ai-01 sur la draft `v2.1.0-review`.
- Volet module-level des packs (#458) : résidu en cours de livraison — la phase 0.4 liste les cultures
  complètes au moment du go-live.
- Grains #135 (brouillons, liens servis) : mesurés en parallèle ce cycle — résultats à joindre au jour J.

*po-2023*
