# Réveil d'un site endormi — la requête d'éveil du script du jour J (2026-11-24)

**Grain** : P24-50 (#131), lane po-2024. **Objet** : la première passe de
`tools/1105-cutover-day-controls.py` sur la prod a échoué le 24/11 parce que le site
endormi a mis 93 à 107 s à répondre, au-delà de la borne de 60 s (rapporté par web2,
porté dans le corps de #458, ligne P24-50). Ce document pose la mesure, le choix fait
et ses bornes. Y est joint le cas de comportement de la sonde P2 demandé par la revue
ai-01 de #2367 (c.7143482432).

## Ce qui est mesuré, et par qui

| Fait | Nature | Source |
|---|---|---|
| La prod endormie a mis **93 à 107 s** à répondre à sa première requête | **rapporté** — non mesuré par cette lane | web2, 24/11, repris dans #458 |
| La borne de chaque requête était **60 s**, en dur, une seule pour tout le script | **mesuré** | `git show 0543ab69:tools/1105-cutover-day-controls.py \| grep -n max-time` → ligne 257 |
| Une passe compte **42 requêtes** : 32 routes, 2 pages légales, 8 pages Règles | **dérivé** de `control_routes` (8 × 4), `control_legal_en` (2), `control_rules_d1` (8) ; **mesuré** deux fois en contre | 43 appels sur le faux transport du contrat, 43 requêtes vues par un vrai serveur en boucle (réveil compris) |
| Sur un site muet, l'ancienne forme attendait jusqu'à 42 × 60 s avant le verdict | **dérivé** des deux lignes ci-dessus |

## Le choix

Deux bornes, deux rôles :

| Requête(s) | Borne | Pourquoi |
|---|---|---|
| la **passe** (42 requêtes) | `--max-time`, 60 s — inchangée | une route chaude qui met 60 s est un défaut ; la borne ne doit pas le couvrir |
| l'**éveil** (1 requête, `GET /`, avant la passe) | `--wake-max-time`, 240 s | couvre la mesure rapportée (107 s) avec marge ; un site muet coûte **une** borne (4 min), pas 42 |

L'éveil n'est pas un contrôle : il paie le démarrage, la passe juge ensuite. Le rapport
le nomme en tête (« Réveil : `GET /` → 200 en N s ») et la console le dit pendant
l'attente.

**Un seul état fait échec dur** : aucune réponse HTTP dans la borne de réveil (curl n'a
rien reçu). Les contrôles 1 à 3 sont alors non mesurés, le code est 1, et la passe ne
tourne pas — un site qui ne répond pas n'est pas un site dont on peut juger 32 routes.

**Un statut HTTP à l'éveil — même un 503 — n'est pas cet état** : le site répond, la
passe le juge comme la route qu'il est. C'est le refus d'un critère « différent de 200 »,
qui lirait un site présent comme un site absent.

**Écarté** : relever la borne unique pour toutes les requêtes. Un site muet coûterait
42 × 240 s avant son verdict, et une route lente se cacherait derrière la borne de
démarrage.

## Vérifié

- `python -I tools/test_1105-home-render-p2.py` : **33 cas, OK** (25 avant). Le serveur
  à démarrage à froid est un **état du faux transport** avec horloge simulée — aucun
  `sleep`, aucun curl, aucun réseau.
- **Batterie de mutations** (8, sur des copies du module) — chacune rougit sur le cas
  nommé :

| Mutation | Cas rouge |
|---|---|
| M2 — `fa` retiré de `RTL_LANGS` | `test_fa_served_ltr_is_red_and_fa_served_rtl_is_green` **et** `test_expected_shapes_are_pinned_as_literals` |
| M8 — l'éveil envoyé sans sa borne | `test_cold_server_after_the_old_bound_passes_thanks_to_the_wake`, `test_mute_server_exits_one_without_running_the_pass` |
| M9 — un éveil muet hors des échecs durs | `test_mute_server_exits_one_without_running_the_pass` |
| M10 — la passe tourne après un éveil muet | `test_mute_server_exits_one_without_running_the_pass` |
| M11 — le réveil absent du rapport | 5 cas |
| M12 — un statut HTTP à l'éveil lu comme un site muet | `test_an_http_status_at_the_wake_is_not_a_mute_site` |
| M13 — `--skip-web` envoie un éveil | `test_skip_web_sends_no_wake` |
| M14 — durée de l'éveil non mesurée (0 s) | `test_cold_server_after_the_old_bound_passes_thanks_to_the_wake` |

  Jusqu'ici, M2 n'était vue **que** par l'épingle des constantes (revue ai-01 de #2367).
  Le cas de comportement la voit aussi : une page `fa` servie en `ltr` est rouge, une
  page `fa` servie en `rtl` est verte — page construite en littéral, non dérivée de
  `RTL_LANGS` comme l'est le témoin.

- **Contrôle réel, en boucle** (contrôle local, hors CI) : un vrai `http.server` qui met
  3 s à répondre à sa première requête, un vrai `curl`, bornes 1 s / 10 s → code 0,
  « Réveil : `GET /` → 200 en 3 s », 32/32 routes. Une prise TCP qui accepte et ne
  répond jamais, bornes 1 s / 2 s → code 1 en 2,2 s, **une** connexion vue par le
  serveur.

## Non vérifié

- **Contre la prod ou la préprod endormies** : la mesure de la vraie durée d'éveil
  appartient à qui opère (web2, ai-01). Cette lane n'a pas sonné un site endormi.
- **Qu'une seule requête suffit sur le vrai site** : la passe suit immédiatement. Si la
  deuxième requête est encore lente, la passe rougit — honnêtement, mais sans nommer le
  démarrage. Non mesuré.
- Le journal Application et le `web.config` : non exercés par ce grain.

## Reproduire

```
python -I tools/test_1105-home-render-p2.py
git show 0543ab69:tools/1105-cutover-day-controls.py | grep -n max-time
```

Le runbook porte la règle d'usage : `1066-cutover-runbook-2026-11-04.md`, phase 3,
étape 17 (« Site endormi »).
