# #1471 grain ① — re-sondage classe E avec instrument distinct

**Auteur** : po-2024 (worker lane) · **Date** : 2026-09-22 · **Base** : master `4b45adc0`
**Issue** : #1471 (Epic liens multilingues) · **Pool** : #458 v10 grain ①
**Statut** : LIVRÉ — instrument + mesure · **0 écriture CSV**

> ⛔ **Résultat majeur** : la classe E (355 cellules « aucune ancre ») publiée par
> l'instrument originel est **massivement sur-estimée**. Sur 50 cartes classe E
> officielles × 8 langues = 400 sondes, **394 sondes (98.5 %, IC95 [96.8 %, 99.3 %])**
> trouvent ≥ 1 article par recherche par titre natif. La décision d'abandonner
> 355 cellules sur l'affirmation originelle **n'est plus défendable** sans passer
> par un audit de **pertinence** (grain ⑤, séparé).

---

## 1. L'instrument — `tools/1471-resond-e-classe.py`

Reprend **exactement** la définition de la classe E du scanner originel
[`tools/1471-langlinks-characterize.py`](../tools/1471-langlinks-characterize.py)
(fonction `wiki_titles_per_card`) : une carte est classe E ssi AUCUNE colonne
`link_<lang>` ne contient une URL `*.wikipedia.org/wiki/...`.

Mais utilise un **chemin de sondage différent** :

| Aspect | Instrument originel | Re-sondage grain ① |
|---|---|---|
| Ancre de départ | titre wiki **d'une autre langue** | titre **natif** `text_<lang>` |
| API | `prop=pageprops` + titre exact | `list=search` + `srsearch` |
| Test | l'article existe-t-il **exactement** sous ce titre ? | l'article existe-t-il **quelconque** dont le titre/texte matche ? |
| Faux négatif typique | titre wiki cible = traduction variable | OK — `list=search` tolère les variantes |

⇒ Le re-sondage trouve des hits **que l'instrument originel ratait** parce qu'il
cherchait un titre exact que la langue cible ne porte pas forcément.

## 2. Mesure — 50 cartes × 8 langues = 400 sondes

Date : 2026-09-22, base master `4b45adc0`.

| Métrique | Valeur |
|---|---|
| Cartes classe E échantillonnées | 50 (toutes les officielles) |
| Sondes effectuées | 400 |
| Trouvailles (≥1 hit) | **394** |
| Misses (0 hits) | **6** |
| Taux de faux « non ancré » | **98.5 %** |
| Intervalle Wilson 95 % | **[96.8 %, 99.3 %]** |
| Contrôle inverse (sujets réputés ancrés) | **5/5 OK** |

### Détail par langue

| Langue | Hits | Misses | Taux |
|---|---:|---:|---:|
| `ar` | 49 | 1 | 98.0 % |
| `en` | 49 | 1 | 98.0 % |
| `es` | 49 | 1 | 98.0 % |
| `fa` | 50 | 0 | 100.0 % |
| `fr` | 50 | 0 | 100.0 % |
| `pt` | 48 | 2 | 96.0 % |
| `ru` | 47 | 3 | 94.0 % |
| `zh` | 50 | 0 | 100.0 % |

### Détail des 6 misses

5 cartes / 50 ont au moins 1 miss sur les 8 langues :

| PK | Langues misses | Requête (titre natif) |
|---|---|---|
| 834  | ru | `Злопотребление сравнением` |
| 1020 | ru | `Логическая ошибка затонувших издержек` |
| 1287 | es, ru | `Pseudoexplicación`, `Псевдообъяснение` |
| 1288 | pt | `Postulado Inconfutável` |
| 1345 | pt | `Complexificação exagerada` |

⇒ Sur les **50 cartes** classe E officielles, **45 cartes trouvent des hits sur les 8 langues**, et **5 cartes ont entre 1 et 2 misses**. **Aucune carte n'est 0/8.**

## 3. Extrapolation et conséquence sur l'arbitrage

L'arbitrage owner sur la **strate 3 (abandon des 355 cellules E)** s'appuyait sur
l'affirmation « ces cartes n'ont aucune ancre wiki dans aucune langue ». **Cette
affirmation est fausse** :

| Métrique | Avant grain ① | Après grain ① |
|---|---:|---:|
| Cellules classe E | 355 | 355 (définition inchangée) |
| Cellules E re-trouvables par recherche titre natif | ~0 (estimation implicite) | **~349** (98.5 % × 355) |
| Cellules **vraiment** non atteignables | ~355 | **~6** (1.5 % × 355) |

**Conséquence** : avant d'arbitrer l'abandon des 355 cellules, **chaque hit de
recherche doit être validé pour pertinence** — un hit ≠ un article pertinent.
C'est précisément le périmètre du **grain ⑤ (#1444 audit de pertinence)**.

## 4. Trois points à ne pas oublier

1. **Hit ≠ article pertinent.** `list=search` rend des articles qui **contiennent
   le terme**, pas nécessairement qui traitent du sophisme. Une PK qui hit
   « Sophisme de l'appel à la nature » peut très bien pointer sur l'article
   « Pithos » ou « Chrysippe ». L'audit de pertinence (grain ⑤) doit faire
   cette discrimination.

2. **Le chemin `srsearch` est biaisé par le titre natif.** Si le titre natif est
   lui-même une **mauvaise traduction** ou un **calque**, la recherche peut
   rater l'article pertinent qui existe sous un titre différent. C'est
   précisément le défaut du scanner originel : il cherche par titre d'une
   autre langue, mon instrument cherche par titre natif, **les deux ont des
   angles morts**.

3. **Les 6 misses ne sont pas des « vrais E ».** Sur les 5 cartes qui ont des
   misses, le reste des 8 langues a des hits. **Aucune carte n'a 0 hit sur les
   8 langues**. ⇒ La classe E **officielle** est presque entièrement due à
   des défauts de l'instrument originel, pas à de vraies lacunes encyclopédiques.

## 5. Ce que ce grain **n'établit pas**

- ⛔ La **pertinence** des hits trouvés — un hit ≠ un article qui parle **du bon
  sujet**. C'est le grain ⑤.
- ⛔ La **décision** owner sur la stratégie (abandon, recherche humaine,
  création) — c'est owner-arbitré.
- ⛔ Le **coût réel** de la campagne de complétion — ~349 cellules à valider,
  plus la pertinence par cellule, plus l'écriture post-validée.
- ⛔ La **correction** de l'instrument originel `1471-langlinks-characterize.py`
  — il faudrait y intégrer `list=search` comme méthode de re-sondage
  avant de publier la classe E.

## 6. Évidence

| Fichier | Contenu |
|---|---|
| `tools/1471-resond-e-classe.py` | instrument re-sondage (lecture seule) |
| `tools/1471-resond-e-classe-officialE50.json` | détail des 400 sondes (PK, langue, query, n_hits, top titres) |
| `tools/1471-resond-e-classe-officialE50.log` | log d'exécution, sortie console |

Reproduction :

```
python tools/1471-resond-e-classe.py --sample 50 --timeout 10 --json-out /tmp/out.json
```

— *po-2024 (worker lane) — pool v10 grain ① livré*
