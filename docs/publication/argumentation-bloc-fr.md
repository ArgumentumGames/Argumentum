# Bloc « L'argumentation » — prolongement du texte français (#2181 point 4)

> **Statut : texte français lu par l'owner le 19/11 ; bloc correspondant en
> ligne.** La clause AIF est adoucie par arbitrage ai-01 du 20/11 (P24-31) ;
> cette retouche servie attend la pose groupée des 7 autres langues après la
> bascule #131. Le lien OWL vertus reste exclu jusqu'à vérification de #133.
> Issue : #2181 · lane : po-2024 · rédigé le 2026-11-10, révisé le 2026-11-20.

## Où va ce texte

Append au champ `Text` (FR) de l'entité « L'argumentation » (app 33,
`8A8636A4`), immédiatement après le `</ul>` final de la section existante
« La taxonomie Argumentum des arguments fallacieux ». La partie existante de
la cellule reste **inchangée à l'octet près** (elle date de 2022, en entités
HTML) ; l'extension suit la convention courante du registre (UTF-8 brut,
apostrophes U+2019, espace insécable U+00A0 avant « : » — mesuré sur les
cellules traduites en 2026 et les articles d'Actus).

## Le texte proposé (valeur exacte à appendre)

```html
<h3>&nbsp;</h3>
<h3>Comment parcourir la taxonomie</h3>
<p>La carte mentale se parcourt comme un arbre : chaque famille se déploie en sous-familles jusqu’aux sophismes eux-mêmes, et chaque nœud porte ses liens de référence. Intégrée dans une page web, elle ouvre dans une modale le détail de chaque type d’argument fallacieux.</p>
<p>L’ontologie se lit avec un outil du web sémantique : chaque sophisme y est un concept SKOS, relié à ses parents, ses enfants et ses termes voisins. Les correspondances AIF notent, branche par branche, ce que le procédé imite, ce qu’il attaque et comment, et, là où le corpus l’a déjà établie, la question critique qui permet de le démasquer.</p>
<p>La taxonomie des vertus de l’argumentation décrit à son tour les pratiques qui rendent un échange solide. Elle est proposée dans les mêmes formats : <a href="/Virtues_fr.html" target="_blank" rel="noopener">sa carte mentale</a>.</p>
<h3>&nbsp;</h3>
<h3>À quoi elle sert</h3>
<p>La taxonomie est le socle du matériel pédagogique du jeu : cartes, scénarios et mémos en dérivent directement. Hors du jeu, elle sert de référentiel pour relire un discours, construire un cours ou outiller l’analyse d’arguments. Plusieurs usages en sont déjà faits :</p>
<ul>
<li><a href="/Actus/details/chaque-sophisme-a-sa-place-l-ontologie-d-argumentum-et-le-format-aif" target="_blank" rel="noopener">Chaque sophisme à sa place : l’ontologie d’Argumentum et le format AIF</a></li>
<li><a href="/Actus/details/la-descente-guidee-dans-l-arbre-le-plugin-argumentum-du-projet-epita" target="_blank" rel="noopener">La descente guidée dans l’arbre : le plugin Argumentum du projet Epita</a></li>
<li><a href="/Actus/details/des-carnets-de-cours-lisent-le-jeu-argumentum-dans-coursia" target="_blank" rel="noopener">Des carnets de cours lisent le jeu : Argumentum dans CoursIA</a></li>
<li><a href="/Actus/details/sophismes-x-scenarios-le-jeu-comme-jeu-de-donnees" target="_blank" rel="noopener">Sophismes × scénarios : le jeu comme jeu de données</a></li>
<li><a href="/Actus/details/la-dette-d-irreversibilite-du-discours-une-prediction-posee-sur-le-corpus-d-argumentum" target="_blank" rel="noopener">La dette d’irréversibilité du discours : une prédiction posée sur le corpus d’Argumentum</a></li>
</ul>
```

*(Dans le bloc ci-dessus, les espaces avant « : » et autour de « × » sont
des U+00A0 insécables ; les apostrophes sont U+2019.)*

## Pourquoi ces choix

- **Deux sections** répondent aux deux demandes de l'issue : « comment la
  parcourir (carte mentale, ontologie, export AIF) » puis « à quoi elle sert »,
  avec les renvois vers les articles 2 à 6 parus le 09/11.
- **Les liens vertus arrivent ici** (point 3 de l'issue : « le lien arrive avec
  le texte du point 4 ») — carte et ontologie des vertus, servies sur la
  préprod depuis le 09/11.
- **⛔ Aucune correspondance vertu↔sophisme** n'est affirmée : les deux
  taxonomies sont présentées côte à côte par leurs formats, jamais l'une comme
  le miroir de l'autre.
- **Aucun chiffre fragile** : pas de compte de familles ni de sophismes dans le
  texte (le compte évolue — #1288 en a déjà retiré un) ; la description reste
  structurelle.
- **« Branche par branche »** (correction ai-01, 10/11) : une correspondance
  AIF (schéma imité, attaque, question critique — les colonnes
  `AIF_skos*`/`AIF_attackType`/`AIF_criticalQuestion`) est posée au plus haut
  nœud où elle est vraie et vaut pour la branche qu'il porte (méthode #498).
  « Sophisme par sophisme » laissait entendre une notation à chaque feuille, ce
  que le corpus ne fait pas. La **troisième clause est adoucie le 20/11**
  (arbitrage ai-01, P24-31) : la question critique est établie pour 48 des 175
  cartes (mesure du 19/11) — le texte ne la promet donc que là où elle existe.
- **Lien OWL vertus exclu de la valeur à servir** : la publication FR du 19/11
  a retiré « et son ontologie » entièrement (route 404 prod ; décision ai-01).
  La valeur ci-dessus suit cette **variante B**, pour que la prochaine pose ne
  réintroduise ni le lien ni la conjonction. L'ontologie des vertus sera liée
  seulement après vérification publique de #133. Les cinq renvois aux Actus
  restent en français : leurs traductions ne sont pas encore publiées.
- Les intitulés des renvois sont **les titres publiés exacts** (mesurés dans le
  registre, espaces insécables compris).

## Vérification des liens (préprod, lecture seule, 2026-11-10)

| Cible | HTTP | Taille | Note |
|---|---|---:|---|
| `/Virtues_fr.html` | 200 | 519 698 o | conforme à la mesure ai-01 du 09/11 |
| `/argumentum_virtues.owl` | 200 | 1 094 832 o | idem (taille du fichier du dépôt) |
| `/Actus/details/chaque-sophisme-a-sa-place-l-ontologie-d-argumentum-et-le-format-aif` | 200 | 51 981 o | article 2 |
| `/Actus/details/la-descente-guidee-dans-l-arbre-le-plugin-argumentum-du-projet-epita` | 200 | 51 178 o | article 3 |
| `/Actus/details/des-carnets-de-cours-lisent-le-jeu-argumentum-dans-coursia` | 200 | 51 634 o | article 4 |
| `/Actus/details/sophismes-x-scenarios-le-jeu-comme-jeu-de-donnees` | 200 | 53 501 o | article 5 |
| `/Actus/details/la-dette-d-irreversibilite-du-discours-une-prediction-posee-sur-le-corpus-d-argumentum` | 200 | 52 997 o | article 6 |

Les 5 tailles d'articles sont distinctes (51-54 Ko) — la page d'erreur DNN,
elle, rend 200 à ~348 Ko constants (piège documenté #2181) : aucune cible ne
présente cette signature.

## Après lecture

1. Le français a été lu par l'owner le 19/11 ; le bloc correspondant est en ligne,
   avec le lien OWL vertus retiré à la pose. La clause AIF est adoucie par
   l'arbitrage ai-01 du 20/11 (P24-31).
2. Les 7 traductions sont préparées dans `argumentation-bloc-<lang>.md`, avec
   leur carte des vertus localisée. Leur absence de relecture native est dite
   dans chaque fichier.
3. La pose des 8 versions est un geste web2 après la bascule #131 : le français
   servi est retouché **au même geste**, pas avant. Aucune injection n'est
   autorisée ici ; la cellule existante reste préservée.
