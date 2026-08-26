# Politique de triage GitHub — les workers portent, ai-01 route

> Demande owner, 2026-08-26 (interactif) : *« pour le traitement des demandes d'Adeline, Thomas, et les miens directement sur GitHub, fais en sorte d'établir une politique qui permet aux workers de faire le gros du boulot pour t'économiser des tokens. »*

## Le coût n'est pas là où on croit

Répondre à une demande coûte peu. **Trouver** la demande coûte cher : relire 40 issues avec leurs commentaires à chaque cycle, c'est des dizaines de milliers de tokens pour, en moyenne, **deux demandes humaines par mois** (mesuré sur 26 jours d'août 2026).

⇒ Le levier n'est pas de lire plus vite, c'est de **ne pas lire ce qui n'est pas une demande**. Puis de **ne pas rédiger soi-même la réponse**.

## ⚠️ Le login ne discrimine rien — c'est le fait structurant

Les agents du cluster poussent avec le **token partagé `jsboige`**. Mesure du 2026-08-26 sur 30 jours :

| login | commentaires | dont rapports d'agents |
|---|---:|---:|
| `jsboige` | 351 | ~239 |

Un filtre « auteur = Adeline / Thomas / jsboige » ramasse donc surtout **nos propres rapports**. Et dans l'autre sens : **Adeline n'a aucun compte GitHub** parmi les 5 collaborateurs (`lpelleti`, `ynnk`, `jsboige`, `ThomasWatanabeVermorel`, `clusterManager-Myia`) — ses demandes arrivent *par le clavier de jsboige*. Chercher « les commentaires d'Adeline » rendrait **0** en permanence, et ce zéro se lirait à tort comme « elle n'a rien demandé ».

Le discriminateur mesuré qui marche est la **longueur**, doublé de la **mention** :

| | humains connus | agents |
|---|---:|---:|
| longueur des commentaires | 129, 155 car. | p10 = 1153, médiane = 2735, max = 47 965 |

## L'organe

```bash
scripts/triage/human-requests.sh              # fenêtre 26 h (défaut)
scripts/triage/human-requests.sh 2026-08-01T00:00:00Z
scripts/triage/human-requests.sh --self-test  # contrôle inverse permanent
```

Trois filets aux **angles morts différents** — A : demande brève · B : mention `@myia-*` quelle que soit la longueur · C : issue ouverte courte. Le `--self-test` rejoue une fenêtre figée et **échoue** si l'organe cesse de voir les deux demandes humaines connues du 26/08, ou s'il se remet à ramasser le ménage d'agent.

⚠️ **Angle mort assumé** : une demande humaine *longue* et *sans mention* échappe aux trois filets. `(aucune)` ne veut donc pas dire « personne n'a rien demandé » — seulement « aucun des trois filets n'a mordu ».

## Matrice de triage — qui porte quoi

Le coordinateur lit la sortie de l'organe, classe, et **s'arrête là** dans trois cas sur quatre.

| Classe | Exemple | Qui porte | Qui **répond sur GitHub** |
|---|---|---|---|
| **M — mesure / reproduction** | « les bandeaux de règles sont incohérents » | **worker** | **le worker**, sous sa bannière |
| **F — correctif** | « le lien de retour dit *Nach oben* » | **worker** (branche + PR) | **le worker** |
| **D — décision** | « quel bandeau est le bon ? » | **personne** — remonte à jsboige | ai-01, en **une** question fermée |
| **V — verdict / arbitrage** | « est-ce que le deck est bon à tirer ? » | **ai-01** (QA visuelle, merge) | ai-01 |

**La bascule qui économise réellement** : jusqu'ici ai-01 rédigeait *toutes* les réponses GitHub. Désormais **M et F sont répondues par le worker lui-même**, sous la bannière `> ⚠️ Agent myia-po-20XX:Argumentum (worker)`. ai-01 n'écrit plus que D et V.

### Règle d'or côté worker

> Un worker répond à une question **mesurable**. Il ne répond **jamais** à une question de **décision**.

Si la demande contient un choix (quelle variante, quel périmètre, quel budget), le worker **mesure les options et les chiffre**, puis renvoie à ai-01 pour remontée. Il ne tranche pas à la place de l'owner. Une décision inventée coûte plus cher que tous les tokens qu'elle économise.

## Le dispatch porte la citation, pas le lien

Un dispatch qui dit « traite #802 » oblige le worker à relire toute l'issue — le coût est déplacé, pas supprimé. Le dispatch doit être **auto-porteur** :

```
[TRIAGE] #802 — demande humaine du 26/08 14:29, classe M

CITATION VERBATIM :
« Revue du deck tarot anglais: - Inconsistance dans les bandeaux de règles
  (fond couleur vs souligné) - overlap images des carrés colorés bas du dos de Memo »

Lien : <url du commentaire>
Périmètre : deck EN, tier boîte (⇒ bloque le BAT)
Attendu : mesure de l'étendue par langue, cause racine, PR si la cause est au gabarit
Tu réponds TOI-MÊME sur l'issue sous ta bannière worker.
Tu ne tranches PAS quel bandeau est le bon — c'est une décision owner, elle remonte.
```

## Ce qui reste irréductiblement sur ai-01

Trois choses, et rien d'autre : le **verdict QA visuelle** (Playwright + vision), le **merge**, et la **remontée d'arbitrage** à jsboige. Le triage lui-même est bon marché : ~1 appel API, ~10 lignes lues.

## Discipline de cycle

1. `scripts/triage/human-requests.sh` **en début de cycle**, avant toute autre lecture GitHub.
2. Classer chaque prise (M / F / D / V).
3. Dispatcher M et F **avec citation verbatim**.
4. Grouper les D en **une seule** remontée owner par cycle — pas une question par issue.
5. Ne relire une issue en entier que si elle est classée V.

⚠️ Un garde-fou muet est un no-op : si `--self-test` échoue, l'organe est aveugle et le cycle doit le dire, pas continuer en silence.
