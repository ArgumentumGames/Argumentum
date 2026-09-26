# #458 pool v17 grain 8 — PK 661 « Mauvais argument de la régression infinie » : **deux concepts, pas un doublon** (mesure, ⛔ 0 écriture)

**Date :** 2026-09-26 · **Lane :** po-2024 (worker) · **Dispatch :** pool v17 grain 8 ([#458 c.5843387663](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5843387663)) · **Base :** `880b107d` (origin/master)

## 0. Verdict

**Deux concepts distincts, pas un nœud dupliqué.** PK 661 (`path 3.2.3.2`) est le **nœud parent** de la branche « Infini trompeur » ; PK 664 (`3.2.3.2.2`) est son **fils cadet**. Ils partagent le même titre **français** mais leurs définitions, leurs paths et leurs rôles dans l'arbre diffèrent. Les 6 langues traduites ne « perdent » pas « infinie » par erreur : elles **résolvent** la collision française en suivant l'anglais, qui distingue déjà les deux nœuds. ⛔ Aucune écriture ; un renommage fr de 661 est **nommé comme candidate owner** (§4).

## 1. Les trois nœuds de la branche 3.2.3.2 « Infini trompeur »

| PK | path | titre fr | titre en | desc fr (résumé) |
|---|---|---|---|---|
| **661** | `3.2.3.2` | Mauvais argument de la régression infinie | **Bad regress argument** | votre argument repose sur une mauvaise appréciation d'une chaîne infinie de justifications — *catégorie générique* |
| **662** | `3.2.3.2.1` | Régression infinie vicieuse | Vicious Infinite regress | **vous construisez** une chaîne de causalité infinie qui mène à une incohérence |
| **664** | `3.2.3.2.2` | Mauvais argument de la régression infinie | **Bad infinite regress argument** | **vous dénoncez à tort** une régression infinie comme étant vicieuse |

Lecture logique : 662 et 664 sont les **deux miroirs** de la branche — commettre la régression vicieuse (662) vs accuser à tort d'en commettre une (664) — et 661 les coiffe en catégorie générique « mauvais usage d'un argument de régression ». Trois concepts, trois définitions, une arborescence cohérente.

## 2. La collision de titres : un phénomène systémique, pas une anomalie

Un balayage des 1408 lignes du CSV compte **86 titres fr partagés par ≥2 PK** (« Appel à la nature » 96/108, « Sophisme de régression » 636/1196, « Justification infinie » 665/979/1349, « Homme de paille » 168/894/1365…). Certaines collisions sont de **vrais doublons** jugés tels par l'owner (96/108, décision #1288 : 96 sort du deck) ; d'autres sont **structurelles** — un libellé réutilisé à des étages différents de l'arbre. 661/664 appartient à la **seconde famille** : parent et fils, définitions distinctes, tous deux `carte` vide (hors deck — mindmaps + OWL seulement, prémisse du dispatch vérifiée).

## 3. Pourquoi les 6 langues « perdent » « infinie » — et pourquoi ce n'est pas une perte

| langue | PK 661 | PK 664 | distingués ? |
|---|---|---|---|
| fr | Mauvais argument de la régression **infinie** | Mauvais argument de la régression **infinie** | ✗ (homonymes) |
| en | Bad **regress** argument | Bad **infinite regress** argument | ✓ |
| ru | Плохой аргумент регрессии | Ошибочный аргумент **бесконечной** регрессии | ✓ |
| pt | Argumento Ruim da Regressão | Argumento Ruim de Regressão **Infinita** | ✓ |
| es | Mal argumento de la regresión | Malo argumento de regresión **infinita** | ✓ |
| ar | حجة الانحدار السيئة | حجة التراجع **اللانهائي** السيئة | ✓ |
| fa | استدلال بد بازگشت | استدلال **پسرفت بی‌نهایت** نادرست | ✓ |
| zh | 不良回归论点 | 错误的**无限倒退**论证 | ✓ |

Les traductions suivent l'anglais : 661 y est le **générique** (sans « infinie »), 664 le **spécifique** (avec). Le français est la seule langue à garder les deux libellés identiques — la « perte » de « infinie » en ru/pt/es/ar/fa/zh est la **désambiguïsation correcte** du couple, pas une troncature.

## 4. Candidate owner (⛔ non écrite, non dispatchée)

Renommer le **titre fr de 661** pour réaligner le français sur ce que les 7 autres langues font déjà — p. ex. « Mauvais argument de régression » (calque de « Bad regress argument ») ou une forme équivalente jugée par l'owner. Effet de bord mesurable : les mindmaps fr afficheraient des libellés distincts père/fils dans la branche 3.2.3.2 (les 7 autres langues déjà). C'est un choix éditorial de libellé — délégation 09/09 : un choix linguistique ordinaire **peut** être tranché par ai-01, mais toucher un titre fr de référence touche la langue source : décision rendue ici, exécution sur GO explicite.

## 5. Instrument (rejouable)

```bash
python - <<'EOF'
import csv
from collections import defaultdict
rows = list(csv.DictReader(open("Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv", encoding="utf-8-sig")))
for pk in ("661","662","664"):
    r = next(x for x in rows if x["PK"]==pk)
    print(pk, r["path"], repr(r["text_fr"]), "|", repr(r["text_en"]), "| carte:", repr(r["carte"]))
pairs = defaultdict(list)
for r in rows:
    if (r.get("text_fr") or "").strip(): pairs[r["text_fr"].strip()].append(r["PK"])
print("collisions fr:", sum(1 for v in pairs.values() if len(v)>1))
EOF
```

Mesuré sur `880b107d` : 3 lignes imprimées ci-dessus, **86 collisions** de titres fr, 661/662/664 tous `carte=''`. ⛔ Aucune écriture, aucun appel payant, aucune régénération.
