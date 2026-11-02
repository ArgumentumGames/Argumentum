# 458 — `security.es-ES.resx` : le typo `fitro` → `filtro` (`IpFilterUpdateError.Text`)

**Date** : 2026-11-02 · **Base** : master `c7437691` · **Item** : pool po-2024 c.6565773786, item 2 (« es-ES fitro »).

## Le défaut, mesuré

`IpFilterUpdateError.Text` portait `No se ha podido actualizar el **fitro**. Vuelva a intentarlo.` — un mot qui n'existe pas. Trois témoins convergents, tous mesurés dans le même fichier :

1. **`fitro` était l'unique occurrence du dépôt entier** (grep insensible à la casse sur `DNNPlatform/` : 1 ligne, cette cellule).
2. Le même fichier épelle **`filtro` correctement 12 fois en minuscule** (avant le fix) — `Añadir nuevo filtro`, `Editar filtro`, `Actualizar filtro`, `FILTROS IP DE IDENTIFICACIÓN`…
3. La clé sœur **`IpFilterDeleteError.Text`** porte la construction parallèle exacte avec l'orthographe correcte : `No se ha podido eliminar el **filtro**. Vuelva a intentarlo.`

La clé est **vivante au neutre** (`Security.resx` : `Could not update the IP filter. Please try later.`) — la traduction espagnole était juste, seul le mot était défectueux.

## Le fix

**1 cellule, +1 octet** (73 845 → 73 846, BOM et LF-seul inchangés, delta = somme des deltas par cellule). Aucune autre ligne touchée ; le pt-BR voisin (`IpFilterUpdateError.Text` = `Não foi possível atualizar o filtro de IP. Tente novamente mais tarde.`) est déjà correct et sert de second témoin de traduction.

## La garde `DnnSecurityEsEsFitroGuardTests` — 4 tests

1. **Épingle littérale** : la cellule corrigée ne doit plus bouger.
2. **Balayage fichier entier** : aucune valeur du `security.es-ES.resx` ne doit contenir `fitro` (insensible à la casse) — le typo ne doit renaître **nulle part**, pas seulement dans la cellule fixée. Ce test généralise : il attraperait aussi une copie future du typo dans une autre clé.
3. **Clé vivante** : le neutre doit continuer de porter la clé avec sa référence anglaise — c'est la source qui fonde l'épingle.
4. **Transport** (BOM, 0 CR) via `DnnResxGuardSocle`.

## Banc de mutations — 3/3

| Cas | Mesuré |
|---|---|
| R1 le typo renaît dans la cellule fixée (`filtro` → `fitro`) | rouge (épingle + balayage) |
| R2 le typo naît dans une **autre** cellule (`IpFilterUpdateSuccess.Text`) | rouge (balayage seul) |
| C1 contrôle inverse : CR dans l'arbre de travail | **vert** (la garde lit le blob `HEAD`) |

R2 est le cas qui distingue le balayage fichier entier de la simple épingle.
