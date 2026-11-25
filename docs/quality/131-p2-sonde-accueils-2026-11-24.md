# Sonde P2 des 8 accueils — première lecture rendue par le script du jour J (2026-11-24)

**Grain** : P24-45 (#131), lane po-2024. **Objet** : la sonde P2 du runbook
(`1066-cutover-runbook-2026-11-04.md`, § « Sondes après la bascule »), jusqu'ici faite à la
main, est désormais rendue par `tools/1105-cutover-day-controls.py` — section 6 du rapport.
Ce document est la première lecture de cette sonde sur le site servi, et ses bornes.

## Ce qui a été mesuré

Depuis un checkout de la branche, en lecture seule (32 `GET`, cache-buster `?cb=` sur chaque
sonde, `-L`) :

```
python -I tools/1105-cutover-day-controls.py --base-url https://www.argumentum.games \
    --skip-eventlog --label relecture-p2-2026-11-24
```

Mesuré le 24/11/2026 à 21:05 local (20:05Z), **code de sortie 0**.

| Langue | `dir` | Description (car.) | Sélecteur de langue | Marqueur d'audience | Verdict |
|---|---|---|---|---|---|
| fr | ✅ ltr | ✅ 103 | ✅ 8 entrées · 7 liens | ✅ ×2 | ✅ |
| en | ✅ ltr | ✅ 87 | ✅ 8 entrées · 7 liens | ✅ ×2 | ✅ |
| es | ✅ ltr | ✅ 95 | ✅ 8 entrées · 7 liens | ✅ ×2 | ✅ |
| pt | ✅ ltr | ✅ 96 | ✅ 8 entrées · 7 liens | ✅ ×2 | ✅ |
| ru | ✅ ltr | ✅ 104 | ✅ 8 entrées · 7 liens | ✅ ×2 | ✅ |
| zh | ✅ ltr | ✅ 38 | ✅ 8 entrées · 7 liens | ✅ ×2 | ✅ |
| ar | ✅ rtl | ✅ 59 | ✅ 8 entrées · 7 liens | ✅ ×2 | ✅ |
| fa | ✅ rtl | ✅ 88 | ✅ 8 entrées · 7 liens | ✅ ×2 | ✅ |

Dans la même passe (mesuré) : 32 routes sur 32 en 200, les deux pages légales anglaises
conformes, un compteur de joueurs au moins sur chacune des 8 pages Règles.

## Lecture

- **La référence préprod du 23/11 13:00Z est rejouée verte.** Elle annonçait des descriptions
  de 38 à 104 caractères : ce sont, à l'unité près, la plus courte (zh, 38) et la plus longue
  (ru, 104) de cette lecture. L'arbre servi depuis la bascule du 24/11 à 04:56Z est celui que
  la préprod servait le 23/11.
- `dir="rtl"` sur **ar** et **fa** seulement ; `ltr` sur les 6 autres.
- Le sélecteur porte 8 entrées, une seule `active`, et c'est celle de la langue de la page —
  donc 7 liens vers les autres langues, sur chacune des 8 pages.
- Le marqueur d'audience `G-VHLTL18PEW` est présent 2 fois sur chaque accueil : c'est l'état
  après la réparation de web2 du 24/11 à 10:00Z.

## Bornes de l'instrument

- **Le marqueur compte des occurrences dans le HTML servi.** Il ne prouve pas que la mesure
  arrive chez l'éditeur : la réception dans les consoles n'est pas vérifiée par cette sonde.
- **La longueur de la description est comparée à la référence, pas jugée.** La règle est
  « non vide » (sur le texte décodé : une description réduite à `&nbsp;` est vide). La
  longueur affichée est celle de l'attribut brut, l'instrument de la référence.
- **Le sélecteur est identifié par une classe du thème** (`to-shine-page-language`) et par
  les codes `nav-<culture>` de ses entrées. Un thème renommé donne 0 bloc : c'est un signal
  d'instrument, pas un résultat, et le rapport le dit.
- **`dir` est lu sur la balise `<html>`** : un `dir` posé plus bas ne dirige pas la page, et
  une balise sans `dir` n'est pas `ltr`.
- **Le cache-buster se retrouve dans les liens du sélecteur** : DNN réécrit `?cb=<valeur>` en
  `…/Argumentum/cb/<valeur>`. La sonde lit donc la structure et les classes du sélecteur, pas
  la cible de ses liens.
- **Un écart de cette sonde ne change pas le code de sortie.** La discipline du runbook tient :
  ces sondes informent, elles ne déclenchent pas de retour arrière. L'écart est nommé dans la
  ligne « Verdict » du rapport.
- **Non mesuré dans cette passe** : le journal Application (`--skip-eventlog` : la lane n'est
  pas sur le VPS, et un « 0 FileLoadException » lu ailleurs décrit la mauvaise machine) et le
  `web.config` (aucun `--webconfig` fourni).

## Le contrat hors ligne

`tools/test_1105-home-render-p2.py` : 25 cas, std-lib seulement, hors ligne (aucun `curl`,
aucun réseau). Il épingle le témoin (8 accueils sains, 4 lectures vertes), chaque défaut
injeté sur son contrôle et lui seul, les deux sens de la discipline (écart P2 ⇒ code 0,
route rouge ⇒ code 1), et les formes attendues en littéraux. La preuve que chaque clause
porte : 25 mutations du script, chacune sur une copie, 25 rouges.
