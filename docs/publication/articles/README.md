# Articles de la page Actus — comment ils s'écrivent

Ce dossier porte les sources françaises des articles de la page **Actus** du site (Epic #2173). Chaque fichier `<rang>-<sujet>.fr.md` devient une actualité : le titre, le chapô (le résumé affiché dans la liste), le corps, l'illustration et la ligne « Sources ».

Les règles communes de l'Epic restent valables : un chapô de deux phrases au plus, une lecture de 3 à 5 minutes, chaque terme technique expliqué à sa première apparition, des liens qui répondent, la typographie française corrigée, et ni nom de personne privée, ni chemin de machine, ni secret. Ce fichier ajoute les deux règles que l'owner a données après avoir lu les sept premiers articles, le 14/11/2026.

## 1. Le récit d'abord, les chiffres ensuite

> « C'est plutôt bon, avec juste toujours une pointe d'obsession pour les chiffres qu'on avait déjà discuté dans d'autres contextes. Je réduirais un poil ce côté là (même si certains chiffres font partie de l'info de l'article, ne pas en faire le coeur de la narration) » — l'owner, 14/11/2026

- Un chiffre entre dans le corps quand il **est** l'information que le lecteur vient chercher. Les autres vont dans la ligne « Sources », ou disparaissent.
- Chaque paragraphe doit se comprendre si on retire ses chiffres. Il dit ce qui change, pour qui, et pourquoi cela compte.
- Une suite de calculs n'est pas un récit. La section « Deux échelles » de l'article 5 enchaîne trois multiplications, alors que l'idée tient en une phrase : chaque scénario se croise avec chaque nœud de l'arbre, et l'étiquette de chaque couple est connue d'avance.
- Un chiffre qu'on garde garde aussi sa base et sa date, mesurées sur un commit nommé. Cette exigence porte sur les chiffres retenus : ce n'est pas une raison d'en ajouter.
- Il n'y a pas de quota. On allège, on ne vide pas.

## 2. L'illustration montre le sujet

> « et puis surtout les images ont été choisies de façon assez arbitraire et ne collent pas vraiment. Il faudrait voir pour proposer mieux. » — l'owner, 14/11/2026

- Le test : un lecteur qui ne voit que l'image devine-t-il de quoi parle l'article ? Le logo du jeu, ou une carte prise au hasard, ne passent pas ce test.
- L'image doit rester lisible à la taille où la page Actus l'affiche, dans la liste comme dans l'article. Une carte mentale entière, réduite à une vignette, n'est plus qu'une tache.
- Les sources possibles :
  - les illustrations du jeu (cartes, scénarios) ;
  - un détail recadré d'une carte mentale ;
  - une capture du site, sans barre d'administration ni nom de compte ;
  - un carnet public de CoursIA ;
  - un montage de plusieurs de ces éléments.

  La source est nommée sous l'image.
- Une image générée ne vient qu'en option, signalée comme telle : c'est l'owner qui décide d'en utiliser une.
- ⛔ Jamais une carte de sophisme à côté d'une carte de vertu, présentées comme une paire. La correspondance entre les deux paquets n'est jamais publique.
- Changer l'illustration d'un article passe par une proposition de deux ou trois images, chacune avec une ligne qui dit pourquoi elle colle. L'owner choisit en les regardant.

## 3. Le chemin d'un article

1. Le fichier français est écrit ici, une PR par article ou par passe de relecture.
2. L'owner le lit.
3. Le texte entre dans le registre de traduction, `docs/dnn-localization/eav-content.csv` (#2182).
4. Après une modification du français, `tools/458-cascade-drift-scan.py` liste les traductions devenues périmées. Seules celles-là sont retraduites, puis contre-lues.
5. Les huit langues sont injectées sur la préprod, puis publiées le jour de la mise en ligne, dans l'ordre de l'Epic, l'article 7 en dernier.
