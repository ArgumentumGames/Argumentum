# Prompt de sélection — essai #1520 (nouveau fichier, propre à l'essai)
# ⛔ Ne pas modifier les prompts de DatasetUpdater/Resources/ — ce prompt vit dans l'essai.

Tu reçois une grappe de la taxonomie des sophismes Argumentum : sa hiérarchie
(chemins), le titre FR de chaque nœud, ses champs FR (desc, example) qui font foi,
et leurs traductions dans 7 langues (en ru pt es ar zh fa).

Ta tâche : identifier les cellules traduites les plus défectueuses, et les classer
du pire au moins grave.

Règles :
- Cellules éligibles : `desc_<l>` et `example_<l>` pour l ∈ {en, ru, pt, es, ar, zh, fa}.
  ⛔ Jamais les champs FR (ils font foi, lecture seule). Jamais `text_<l>`, jamais
  `Sous-Famille`/`Soussousfamille`, jamais `example_en_bis`/`political_example_en`.
- Au plus UNE cellule par nœud et par passe.
- « Rien à corriger » est une réponse valide.
- Ne tronque JAMAIS une cellule : cite-la en entier.
- Compare chaque traduction au FR du même nœud, et au contexte de la grappe
  (les nœuds frères partagent un thème — une traduction qui confond deux nœuds
  est un défaut).

Réponds STRICTEMENT en JSON :
{"propositions": [
  {"rang": 1, "pk": <PK>, "langue": "<l>", "champ": "desc|example",
   "severite": "grossier|moyen|nuance",
   "probleme": "<une phrase>",
   "extrait_fautif": "<cellule actuelle ENTIÈRE>",
   "correction": "<cellule corrigée ENTIÈRE>"}
 ],
 "rien_a_corriger": false}
