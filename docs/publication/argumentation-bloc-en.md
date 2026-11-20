# “Argumentation” block — English extension (#2181, P24-31)

> **Translation status:** translated for the 20/11/2026 deployment package;
> **no native-speaker review has taken place**. Nothing here is published by
> this file. web2 installs it after the multilingual cutover (#131).

## Where this text goes

Append to the `Text` field (EN) of the “Argumentation” entity (app 33,
`8A8636A4`), immediately after the existing section’s closing `</ul>`. Preserve
the existing value byte for byte. The Virtues mind-map link is localized. The
Virtues OWL link remains excluded until #133 is verified publicly. The five
news links retain their published French titles and URLs.

## Exact value to append

```html
<h3>&nbsp;</h3>
<h3>How to explore the taxonomy</h3>
<p>The mind map can be explored as a tree: each family unfolds into subfamilies, down to the fallacies themselves, and every node carries its reference links. Embedded in a web page, it opens the details of each type of fallacious argument in a modal window.</p>
<p>The ontology can be read with a semantic-web tool: every fallacy is a SKOS concept, connected to its parents, children and neighbouring terms. Branch by branch, the AIF mappings record what the device imitates, what it attacks and how and, where the corpus has already established it, the critical question that exposes it.</p>
<p>The taxonomy of argumentative virtues, in turn, describes the practices that make an exchange sound. It is available in the same formats: <a href="/Virtues_en.html" target="_blank" rel="noopener">its mind map</a>.</p>
<h3>&nbsp;</h3>
<h3>What it is for</h3>
<p>The taxonomy underpins the game’s educational material: its cards, scenarios and memo sheets are directly derived from it. Beyond the game, it provides a reference framework for reviewing a speech, building a course or supporting argument analysis. It already serves several uses:</p>
<ul>
<li><a href="/Actus/details/chaque-sophisme-a-sa-place-l-ontologie-d-argumentum-et-le-format-aif" target="_blank" rel="noopener">Chaque sophisme à sa place : l’ontologie d’Argumentum et le format AIF</a></li>
<li><a href="/Actus/details/la-descente-guidee-dans-l-arbre-le-plugin-argumentum-du-projet-epita" target="_blank" rel="noopener">La descente guidée dans l’arbre : le plugin Argumentum du projet Epita</a></li>
<li><a href="/Actus/details/des-carnets-de-cours-lisent-le-jeu-argumentum-dans-coursia" target="_blank" rel="noopener">Des carnets de cours lisent le jeu : Argumentum dans CoursIA</a></li>
<li><a href="/Actus/details/sophismes-x-scenarios-le-jeu-comme-jeu-de-donnees" target="_blank" rel="noopener">Sophismes × scénarios : le jeu comme jeu de données</a></li>
<li><a href="/Actus/details/la-dette-d-irreversibilite-du-discours-une-prediction-posee-sur-le-corpus-d-argumentum" target="_blank" rel="noopener">La dette d’irréversibilité du discours : une prédiction posée sur le corpus d’Argumentum</a></li>
</ul>
```

No assertion is made here that each virtue corresponds to a fallacy. The
qualified AIF clause is the ai-01 arbitration of 20/11/2026: critical questions
are present for 48 of 175 cards (measured 19/11). No publication or paid API
call was performed for this translation.
