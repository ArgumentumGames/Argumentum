# Campagne de fidélité Scénarios — **ar** (0 écriture)

**Mandat** : pool #458, dispatch c.5976781537 — « campagne de fidélité des Scénarios
(jamais relus) : 1 dossier 0 écriture par langue, zh → ar → fa → es → ru → pt → en ».
**Statut** : **0 écriture** — aucune cellule CSV modifiée.

## 1. Instrument

Écran **multi-langue** comparant chaque cellule à **FR *et* EN** (la passe zh a montré que
la source de traduction peut être l'EN et non le FR : un écart FR seul ne prouve rien).
Signaux : cellule manquante · identique à une source · **chiffre perdu / ajouté** · ratio de
longueur · ponctuation finale · latin résiduel · espace adjacent à du CJK.

Trois pièges d'instrument corrigés en route, consignés parce qu'ils ont produit des faux
massifs avant correction :

| Piège | Faux produit | Correction |
|---|---|---|
| Jeu de ponctuation **latin** testé sur de l'arabe/persan | **74** « PONCT » sur ar (les vrais : 4) | jeu de ponctuation **par écriture** (`؟`, `۔`) |
| « latin résiduel » cherché dans une langue **à écriture latine** | **1169/1169** sur es/pt/en | signal désactivé pour ces langues |
| Comparaison « cellule = FR » sur le bloc EN | **+167** faux (un par carte) | le bloc EN porte `suggestion_en` et des colonnes **nues** ailleurs ; la coupure générique du suffixe comparait la colonne FR à elle-même |

## 2. Couverture

**1169 / 1169 cellules** (167 cartes × 7 champs rendus par le gabarit) — **0 manquante.**

## 3. Candidats (11) — verdict

| Signal | Cellules | Verdict |
|---|---|---|
| `CHIFFRE-perdu` (7) | 1.3.2 ×1 (général **5** étoiles → *خمس نجوم*), 5.3.4 ×4 (**5G** → *الجيل الخامس*), 7.2.9 ×2 (2 h → *الثانية صباحًا*) | **faux** : l'arabe écrit ces nombres **en mots** ; l'écran ne sonde que les chiffres |
| `PONCT` (4) | 1.2.3 contexte + enjeu · 2.1.8 suggestion · 2.2.1 suggestion | voir §4 |

**Aucun défaut dur.** Les 7 « chiffres perdus » sont une borne de l'instrument, pas une
perte de contenu : *الجيل الخامس* (« la cinquième génération ») est le terme arabe **correct**
pour la 5G, et 2019 est conservé dans les mêmes cellules.

## 4. Ponctuation finale — lot **inter-langues**, pas trait de l'arabe

L'écran, restreint aux cellules où **FR et EN ponctuent tous deux**, donne :

| Carte | Champ | Langues sans ponctuation finale |
|---|---|---|
| **1.2.3** | contexte | es · **ar** · fa · zh |
| **1.2.3** | enjeu | es · **ar** · fa · zh |
| **2.1.8** | suggestion | es · **ar** · zh |
| 1.1.1 | enjeu | pt |
| 7.2.3 | contexte | es |

Deux cartes portent donc **le même défaut sur les mêmes cellules dans 3 à 4 langues** : c'est
une **signature de lot**, pas une faute propre à l'arabe. `2.2.1` est d'un autre ordre — c'est
l'**EN** qui omet le point (`Vade retro Satanas`), et ar comme es le suivent.
Le russe et le portugais ponctuent correctement les deux cartes du lot.

## 5. Verdict ar

- Couverture **complète**, **0 défaut dur**, **0 contamination**.
- 3 cellules de ponctuation relevant d'un **lot partagé** (§4), 1 héritée de l'EN.
- L'arabe écrit les nombres en mots : tout écran futur qui sonde les chiffres doit le savoir,
  sinon il produira 7 faux par passe sur cette langue.

*po-2024*
