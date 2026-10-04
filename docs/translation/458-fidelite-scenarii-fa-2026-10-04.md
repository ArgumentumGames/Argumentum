# Campagne de fidélité Scénarios — **fa** (0 écriture)

**Mandat** : pool #458, dispatch c.5976781537. **Statut** : **0 écriture** — aucune cellule modifiée.

## 1. Instrument

Écran **multi-langue** comparant chaque cellule à **FR *et* EN** — nécessaire : la passe zh a
montré que la source de traduction peut être l'EN et non le FR (voir §4, où le persan suit
l'inverse). Signaux : manque · identique à une source · chiffre perdu/ajouté · ratio ·
ponctuation finale · latin résiduel · espace CJK.

Trois pièges d'instrument corrigés en route (détail complet dans le dossier `ar`) : jeu de
ponctuation **par écriture** (le jeu latin sur du persan fabriquait 70 faux « PONCT »),
« latin résiduel » **désactivé** sur les langues à écriture latine, et correction de la
comparaison « cellule = FR » sur le bloc EN.

**Borne propre au persan** : il emploie les **chiffres persans** (`۰۱۲۳۴۵۶۷۸۹`). L'écran, qui
sonde `\d`, lit chaque nombre persan comme un chiffre **perdu** *et* comme un chiffre
**ajouté**. Les 9 signaux de ce type mesurés sur fa sont des faux de cette famille.

## 2. Couverture

**1169 / 1169 cellules** (167 × 7 champs rendus) — **0 manquante.**

## 3. Candidats (15) — verdict

| Signal | Cellules | Verdict |
|---|---|---|
| `CHIFFRE-perdu` (5) + `+CHIFFRE` (4) | 1.3.2 (*پنج‌ستاره* = cinq-étoiles), 2.1.3 (۱۰۰۰ / ۱۰۰۱), 4.3.2 (۲۵), 5.3.4 ×2 (۲۰۱۹), 7.2.9 (*ساعت دو*) | **faux** : chiffres persans ou nombres écrits en mots |
| `LATIN` (7) | 1.1.1 *Aurelia Cotta* · 4.3.1 *Ergo sum* · 5.1.5 *Casper* · 5.2.1 *Marcellus Wallace* · 5.2.5 *Ross*, *Rachel* · 6.1.3 *Johnny Hallyday* | **observation**, pas défaut — voir §4 |
| `=FR` (4) · `=EN` (4) | *Ergo sum*, *Casper*, *Ross*, *Rachel* | légitimes : noms propres identiques dans toutes les langues |
| `PONCT` (2) | 1.2.3 contexte + enjeu | lot inter-langues, voir §5 |

**Aucun défaut dur.**

## 4. Observations

**Noms propres laissés en écriture latine.** Sept cellules gardent le latin dans un texte
persan : le tag latin *Ergo sum* (4.3.1), *Casper* (5.1.5), *Ross* / *Rachel* (5.2.5),
*Johnny Hallyday* (6.1.3), *Aurelia Cotta* (1.1.1), *Marcellus Wallace* (5.2.1).
Même famille que la passe zh (où 罗斯 / 瑞秋 voisinaient `Ross` / `Rachel`) : c'est un **choix de
rendu à trancher en relecture native**, pas une erreur — les rôles (`smoothTalker_fa`,
`drawer_fa`) sont **rendus sur la face**.

**Le persan suit le FR, non l'EN** — deux indices mesurés :
- 4.3.2 : FR « 25 **€** » / EN "$25" → fa « ۲۵ **یورو** » (*euro*) ;
- 5.2.1 : FR « **Marcellus** Wallace » / EN « **Marsellus** Wallace » (l'orthographe du film a
  deux s) → fa « Marcellus » (un seul s).

⇒ La source de traduction **varie selon la langue** (zh suit l'EN, fa suit le FR). Un écart
FR↔langue n'est donc jamais, à lui seul, la preuve d'un défaut : c'est la règle que la
campagne applique depuis le début.

## 5. Ponctuation finale — lot **inter-langues**

Restreint aux cellules où **FR et EN ponctuent tous deux** : **1.2.3** (contexte, enjeu) manque
la ponctuation finale en es · ar · **fa** · zh ; **2.1.8** en es · ar · zh ; 1.1.1 en pt ;
7.2.3 en es. Deux cartes fautent donc **dans 3 à 4 langues aux mêmes cellules** : signature de
lot, pas trait persan. Le russe et le portugais ponctuent correctement ces deux cartes.

## 6. Verdict fa

- Couverture **complète**, **0 défaut dur**, **0 contamination**.
- 2 cellules de ponctuation relevant du **lot partagé**.
- Une **observation** de rendu des noms propres (§4), à porter à la relecture native.
- ⚠️ Pour tout écran futur : les **chiffres persans** doivent être normalisés avant comparaison,
  sinon chaque nombre produit deux faux (perdu + ajouté).

*po-2024*
