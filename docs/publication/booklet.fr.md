# Livret de publication — *placeholder*

> ⏳ **En attente de clarification de scope** — dispatch ai‑01 2026‑05‑30, message `[ASK]` posté sur le dashboard workspace.

## Contexte

Le dispatch initial mentionnait un livret PDF compilé via LaTeX, intitulé « The Liars' School », hébergé dans un **dépôt distinct** de l'organisation `ArgumentumGames` sur GitHub.

## Constat de l'état des lieux (2026‑05‑30)

| Source vérifiée | Résultat |
|---|---|
| `gh repo list ArgumentumGames` | 2 dépôts seulement : `Argumentum` (ce dépôt) et `Fallacies` (Python, dernière maj 2017) |
| `gh search repos "argumentum" --owner=ArgumentumGames` | Idem, 1 résultat |
| Recherche `*.tex`, `Liar*`, `livret*`, `booklet*` dans ce dépôt | 0 résultat |
| Mention dans `README.md`, `CLAUDE.md`, `docs/` | Aucune |

## Hypothèses en attente d'arbitrage

1. **Dépôt personnel.** Le livret pourrait être chez un mainteneur individuel (jsboige ou autre handle).
2. **Pas encore créé.** Volet 1 = définir un cahier des charges pour un livret futur.
3. **Hors Git.** Le livret existe en Drive/local et ce document doit décrire le workflow d'intégration.
4. **Renommage sémantique.** Le terme « livret » pourrait désigner en réalité les règles imprimées (`Argumentum_Rules.csv` → cartes Rules dans le deck Tarot).

## Plan d'action une fois le scope précisé

À compléter par la suite ; structure prévue :

- **Identité.** Titre, sous‑titre, auteur·rice·s, ISBN éventuel.
- **Source.** URL du dépôt, branche canonique, fichier LaTeX racine, fichiers de bibliographie.
- **Build.** Distribution LaTeX requise (TeX Live / MiKTeX), commandes de compilation, dépendances graphiques (TikZ, fontes).
- **Versions.** Politique de versioning (tag git, date de publication, ISSN/DOI éventuel).
- **Distribution.** Canaux (site `argumentum.games`, GitHub Release, impression à la demande).
- **Localisation.** Politique multilingue (FR canonique, EN miroir, autres langues TODO).
- **Lien avec les cartes.** Comment le livret référence le catalogue de cartes ([cards-catalog.fr.md](cards-catalog.fr.md)).

## Référence dispatch

- **Dashboard workspace** — message `[ACK]` puis `[ASK]` 2026‑05‑30 par `po-2023`.
- **PR doc-only associée** — ce fichier.
