> ### ⚠️ Brouillon daté — **à re-générer au lancement du run**, jamais à citer tel quel
>
> Mesuré le **2026-10-08** de `89f78bcd` à `b7e89c32` (master du jour). Les pools continuent de
> vivre : **ce chiffre dérive**. Au lancement de la régénération, rejouer la commande du
> § Reproductibilité de [`README.md`](README.md) et **remplacer ce fichier**.
>
> ⛔ **Sans publication** : ce document n'est pas envoyé aux associés en l'état — il alimente la
> relecture d'ai-01, qui choisit ce qui sort.
>
> **Re-mesuré à la tête ce jour : 429 cellules · 196 cartes**, 4 decks, 8 langues.
>
> ---
>
> ✅ **Réconciliation census — CLOSE (08/10, grain S1).** L'en-tête précédent déclarait cet écart
> « OUVERTE, non tranchée ». C'était **faux** : les deux nombres mesurent le **même objet à deux
> dates** — même unité (cellules de texte sur rangées cartes), même base (`89f78bcd`), mêmes
> discriminants.
>
> | Relevé | Tête | F | V | S | R | Total |
> |---|---|---:|---:|---:|---:|---:|
> | census manuel n°7, tel que porté au status (soir 05/10) | `2969f4b3` | 123 | 42 | 78 | **30** | 273 |
> | idem, double-addition retirée | `2969f4b3` | 123 | 42 | 78 | **28** | **271** |
> | ce brouillon, mesure d'origine | `edb39554` | 123 | 122 | 145 | 28 | 418 |
> | re-mesure du correctif S1 | `b7e89c32` | 123 | **133** | 145 | 28 | **429** |
>
> **Le 273 avait deux défauts, tous deux identifiés.** Son `R:30` = **19 cellules mesurées + les
> « 11 » de #1778 en vol** — dont **2 cellules `Rules_13` (`Text_ar`, `Text_es`) comptées deux
> fois** (changées par #1768 « un terme pour un objet », rechangées par #1778 « 32→28 »). `R:30`
> n'existe à **aucune** tête (plafond outil : 28) ; l'union réelle vaut **28**, donc le total exact
> à cet instant est **271**. Et `F123/V42/S78` ne tiennent **simultanément** que dans la fenêtre
> `8103baa6` (05/10 12:49) → `2969f4b3` (06/10 01:08) : c'est bien un **instantané du 05/10**, pas
> la tête du run.
>
> **La dérive est la preuve, et ce correctif l'a rejouée.** Entre `edb39554` (08/10 00:26) et
> `b7e89c32` (08/10 12:17), le total passe de 418 à **429** — **+11 cellules, toutes sur les
> Vertus** — et ce sont **nominalement** celles du lot de merges ai-01 de 12:17 : #1807 (pk 205
> ru/pt/es = 3) + #1808 (pk 175 pt/es = 2) + #1814 (pk 172 ru/pt = 2 ; pk 208 ×4 langues = 4).
> `3 + 2 + 6 = 11` ✔, et par langue : **ar +1 · es +3 · pt +4 · ru +3**. ⇒ **Contrôle inverse
> satisfait : la dérive du census égale le diff mergé, au chiffre près.** Un instrument qui suit
> les merges est un instrument qui mesure.
>
> ⇒ **Au lancement du run : re-mesurer à la tête, jamais recopier un chiffre.** Ni 273, ni 418, ni
> 429 ne sont « le » census — c'est un **instantané vivant** (l'outil documente ~40 cellules/jour
> tant que les pools vivent). Commande :
> `python tools/what-changed-associates.py 89f78bcd <tête-du-run>` ; **les pages à couvrir sont le
> listing de l'outil à cette tête**, pas un total. Les relevés ci-dessus restent valables comme
> **traces historiques**, jamais comme périmètres.
>
> ⚠️ **Un point d'instrument reste ouvert, et il n'est pas tranché ici.** Le chemin `--self-test`
> de l'outil n'exécute **pas** `diff_deck` : il recopie sa logique (l.202 : « même logique que
> `diff_deck`, sans git ») — et cette copie a **dérivé sur deux discriminants** : le self-test
> détermine « carte » sur la **tête seule** (l.208) là où `diff_deck` accepte base **ou** tête
> (l.134), et il compte les cellules **hors** de la garde `if card:` (l.215) là où `diff_deck` ne
> les compte que **dedans** (l.143). Son assertion l.223 passe donc **grâce à** la divergence au
> lieu de la détecter. Sans effet sur les chiffres ci-dessus (mesurés par `diff_deck`, un seul
> chemin) ; **à trancher par le propriétaire de l'outil**, car un self-test qui valide une copie
> laisse le chemin de production non vérifié.

---

# Quoi de neuf — cartes dont le texte change dans votre édition

*Mesuré `89f78bcd → b7e89c32` par `tools/what-changed-associates.py` — généré à chaque appel, jamais de chiffres gelés. Ancres = titre FR de la tête.*

## Vue d'ensemble (reconciliation census)

| Deck | Cellules | Cartes | Par langue (cellules/cartes) |
|---|---:|---:|---|
| Sophismes (Tarot + Web) | 123 | 89 | ar 6/6 · en 6/6 · es 80/78 · fa 8/8 · pt 3/3 · ru 16/16 · zh 4/4 |
| Vertus (Tarot) | 133 | 44 | ar 26/23 · en 17/17 · es 17/17 · fa 28/26 · fr 3/3 · pt 19/19 · ru 13/13 · zh 10/10 |
| Scénarios (Poker) | 145 | 55 | ar 18/16 · en 27/25 · es 17/16 · fa 20/18 · pt 14/14 · ru 28/27 · zh 21/18 |
| Règles (Tarot) | 28 | 8 | ar 5/5 · en 4/4 · es 5/5 · fa 4/4 · fr 1/1 · pt 1/1 · ru 4/4 · zh 4/4 |
| **TOTAL** | **429** | **196** | ar 55/50 · en 54/52 · es 119/116 · fa 60/56 · fr 4/4 · pt 37/37 · ru 61/60 · zh 39/36 |

## Français — 4 carte(s) touchée(s)

### Vertus (Tarot) (3 carte(s))

- **Définitions claires** (pk 135) : subfamily
- **Causalités bien identifiées** (pk 80) : subfamily
- **Énoncés rigoureux** (pk 84) : subfamily

### Règles (Tarot) (1 carte(s))

- **Rules_13** (pk Rules_13) : texte

## English — 52 carte(s) touchée(s)

### Sophismes (Tarot + Web) (6 carte(s))

- **Traiter d’hypocrite** (PK 1362) : description
- **622** (PK 622) : description
- **625** (PK 625) : description
- **673** (PK 673) : exemple
- **784** (PK 784) : description
- **Citation hors contexte** (PK 943) : exemple

### Vertus (Tarot) (17 carte(s))

- **Prémisses fiables** (pk 12) : description
- **Logique informelle solide** (pk 128) : titre
- **Définitions claires** (pk 135) : subfamily
- **Niveau de preuve approprié** (pk 162) : description
- **Tenir compte des biais idéologiques** (pk 176) : titre
- **Argument fondé** (pk 2) : description
- **Principe de charité** (pk 205) : description
- **Ne pas interrompre** (pk 220) : description
- **Ton respectueux** (pk 221) : description
- **Argument déductif** (pk 4) : description
- **Argument inductif** (pk 5) : description
- **Sans chantage aux conséquences** (pk 54) : description
- **Preuves tangibles** (pk 6) : description
- **Objectif clair** (pk 7) : description
- **Illustrer par des exemples** (pk 8) : description
- **Raisonnement jalonné** (pk 89) : description
- **Démonstration cohérente** (pk 90) : description

### Scénarios (Poker) (25 carte(s))

- **Truman et la bombe A** (path 1.3.2) : baratineur
- **Salomon** (path 2.2.5) : baratineur
- **Dom Juan** (path 2.3.5) : piocheur
- **Rencontres en ligne** (path 3.1.6) : piocheur
- **Le t-shirt taché** (path 3.2.15) : titre
- **L’anniversaire oublié** (path 3.2.8) : contexte
- **Rouler des mécaniques** (path 4.1.1) : titre
- **Ergo sum** (path 4.3.1) : suggestion
- **La commande de trop** (path 4.3.3) : contexte
- **Le coup d’État** (path 5.1.2) : baratineur, titre
- **Une fiction pulp** (path 5.2.1) : suggestion
- **On était en pause** (path 5.2.5) : enjeu
- **Débat avec un terraplaniste** (path 5.3.1) : enjeu, titre
- **Refus du vaccin** (path 5.3.2) : enjeu
- **La conspiration de la 5G** (path 5.3.4) : titre
- **Moralisation** (path 6.1.1) : suggestion
- **Bas les masques** (path 6.1.4) : suggestion
- **Juste un doigt** (path 6.2.2) : contexte
- **L’escalier** (path 7.1.2) : suggestion
- **La kermesse** (path 7.1.5) : titre
- **L’héritage** (path 7.1.6) : titre
- **Foie gras végétarien** (path 7.2.5) : enjeu
- **T’as pas mille euros ?** (path 7.2.8) : suggestion
- **La dernière cigarette** (path 7.3.2) : piocheur
- **Buuut !** (path 7.3.4) : titre

### Règles (Tarot) (4 carte(s))

- **Rules_09** (pk Rules_09) : texte
- **Rules_10** (pk Rules_10) : texte
- **Rules_11** (pk Rules_11) : texte
- **Rules_13** (pk Rules_13) : texte

## Русский — 60 carte(s) touchée(s)

### Sophismes (Tarot + Web) (16 carte(s))

- **121** (PK 121) : exemple
- **1352** (PK 1352) : exemple
- **Attaque de l’estime de soi** (PK 1388) : exemple
- **1398** (PK 1398) : description
- **182** (PK 182) : exemple
- **361** (PK 361) : exemple
- **51** (PK 51) : exemple
- **726** (PK 726) : exemple
- **735** (PK 735) : exemple
- **784** (PK 784) : exemple
- **813** (PK 813) : exemple
- **844** (PK 844) : exemple
- **848** (PK 848) : exemple
- **Citation hors contexte** (PK 943) : exemple
- **Monter la barre** (PK 974) : exemple
- **989** (PK 989) : description

### Vertus (Tarot) (13 carte(s))

- **Prémisses fiables** (pk 12) : description
- **Logique informelle solide** (pk 128) : titre
- **Définitions claires** (pk 135) : subfamily
- **Analogie appropriée** (pk 144) : description
- **Clarté des enjeux** (pk 159) : subfamily
- **Reconnaître ses biais culturels** (pk 172) : description
- **Reconnaître ses biais idéologiques** (pk 175) : titre
- **Tenir compte des biais idéologiques** (pk 176) : titre
- **Principe de charité** (pk 205) : description
- **Évaluation loyale de la position adverse** (pk 208) : remarque
- **Hypothèse plausible** (pk 24) : description
- **Sans chantage aux conséquences** (pk 54) : description
- **Illustrer par des exemples** (pk 8) : description

### Scénarios (Poker) (27 carte(s))

- **La controverse de Valladolid** (path 1.2.2) : enjeu
- **Truman et la bombe A** (path 1.3.2) : titre
- **Maréchal, nous voilà** (path 1.3.3) : suggestion
- **Le prétendant de Pénélope** (path 2.2.7) : titre
- **Déjà-vu** (path 2.2.9) : contexte
- **Le pizzaïolo** (path 3.1.5) : contexte, piocheur
- **Emménager ensemble** (path 3.2.1) : enjeu
- **Le t-shirt taché** (path 3.2.15) : titre
- **Le flambeur** (path 3.2.16) : enjeu
- **Bébé** (path 3.2.4) : contexte
- **L’anniversaire oublié** (path 3.2.8) : contexte
- **Mariage ? Non merci** (path 3.3.10) : titre
- **Mariage de pierre** (path 3.3.2) : enjeu
- **Ciel, mon mari !** (path 3.3.6) : titre
- **Rouler des mécaniques** (path 4.1.1) : titre
- **Destination Mars** (path 4.1.11) : enjeu
- **Le babysitter et le chat chauve** (path 4.1.12) : enjeu
- **Le martinet** (path 4.1.2) : titre
- **Une IA vraiment éthique ?** (path 4.3.4) : contexte
- **Le coup d’État** (path 5.1.2) : baratineur
- **Une fiction pulp** (path 5.2.1) : suggestion
- **Débat avec un terraplaniste** (path 5.3.1) : titre
- **Retrait négocié** (path 6.2.1) : titre
- **La kermesse** (path 7.1.5) : titre
- **Millésime de foie gras** (path 7.2.6) : titre
- **Un ami qui vous veut du bien** (path 7.2.7) : suggestion
- **Le fromage vivant** (path 7.3.5) : suggestion

### Règles (Tarot) (4 carte(s))

- **Rules_02** (pk Rules_02) : texte
- **Rules_11** (pk Rules_11) : texte
- **Rules_12** (pk Rules_12) : texte
- **Rules_13** (pk Rules_13) : texte

## Português — 37 carte(s) touchée(s)

### Sophismes (Tarot + Web) (3 carte(s))

- **Attaque de l’estime de soi** (PK 1388) : exemple
- **Citation hors contexte** (PK 943) : exemple
- **Monter la barre** (PK 974) : exemple

### Vertus (Tarot) (19 carte(s))

- **Prémisses fiables** (pk 12) : description
- **Définitions claires** (pk 135) : subfamily
- **Analogie appropriée** (pk 144) : description
- **Clarté des enjeux** (pk 159) : subfamily
- **Reconnaître ses biais culturels** (pk 172) : description
- **Reconnaître ses biais idéologiques** (pk 175) : titre
- **Tenir compte des biais idéologiques** (pk 176) : titre
- **Argument fondé** (pk 2) : description
- **Principe de charité** (pk 205) : description
- **Évaluation loyale de la position adverse** (pk 208) : remarque
- **Critique axée sur les arguments** (pk 209) : description
- **Hypothèse plausible** (pk 24) : description
- **Rasoir de Hanlon** (pk 33) : description
- **Présentation intègre** (pk 34) : description
- **Argument déductif** (pk 4) : description
- **Sans chantage aux conséquences** (pk 54) : description
- **Preuves tangibles** (pk 6) : description
- **Objectif clair** (pk 7) : description
- **Illustrer par des exemples** (pk 8) : description

### Scénarios (Poker) (14 carte(s))

- **La mère de César et Cléopâtre** (path 1.1.1) : enjeu
- **La controverse de Valladolid** (path 1.2.2) : baratineur
- **Truman et la bombe A** (path 1.3.2) : piocheur
- **Le prétendant de Pénélope** (path 2.2.7) : titre
- **Déjà-vu** (path 2.2.9) : contexte
- **Le t-shirt taché** (path 3.2.15) : titre
- **L’amour à la plage** (path 3.2.7) : titre
- **L’anniversaire oublié** (path 3.2.8) : contexte
- **Le babysitter et le chat chauve** (path 4.1.12) : enjeu
- **Le coup d’État** (path 5.1.2) : baratineur
- **Refus du vaccin** (path 5.3.2) : baratineur
- **La conspiration de la 5G** (path 5.3.4) : piocheur
- **Austérité** (path 6.1.2) : baratineur
- **T’as pas mille euros ?** (path 7.2.8) : suggestion

### Règles (Tarot) (1 carte(s))

- **Rules_13** (pk Rules_13) : texte

## Español — 116 carte(s) touchée(s)

### Sophismes (Tarot + Web) (78 carte(s))

- **1023** (PK 1023) : description
- **Biais naturel** (PK 1024) : description
- **108** (PK 108) : description
- **1092** (PK 1092) : description
- **1120** (PK 1120) : description
- **Biais culturel** (PK 1174) : description
- **121** (PK 121) : description
- **Biais dogmatique** (PK 1242) : description
- **128** (PK 128) : description
- **Relativisme** (PK 1282) : description
- **1287** (PK 1287) : description
- **Sabotage** (PK 1312) : description
- **133** (PK 133) : description
- **Défense Chewbacca** (PK 1330) : description
- **134** (PK 134) : description
- **Couper les cheveux en quatre** (PK 1345) : description
- **1357** (PK 1357) : description
- **1360** (PK 1360) : description
- **1361** (PK 1361) : description
- **Attaque de l’estime de soi** (PK 1388) : description
- **153** (PK 153) : exemple
- **176** (PK 176) : description
- **184** (PK 184) : description
- **185** (PK 185) : description
- **2** (PK 2) : description
- **219** (PK 219) : description
- **299** (PK 299) : description
- **3** (PK 3) : description
- **300** (PK 300) : description
- **33** (PK 33) : description
- **356** (PK 356) : description
- **357** (PK 357) : description
- **358** (PK 358) : description
- **420** (PK 420) : description
- **421** (PK 421) : description
- **43** (PK 43) : description
- **432** (PK 432) : description
- **51** (PK 51) : description
- **511** (PK 511) : description
- **55** (PK 55) : description
- **603** (PK 603) : description
- **621** (PK 621) : description
- **622** (PK 622) : description
- **625** (PK 625) : description
- **633** (PK 633) : description
- **636** (PK 636) : description
- **653** (PK 653) : description
- **673** (PK 673) : exemple
- **70** (PK 70) : description
- **707** (PK 707) : description
- **708** (PK 708) : description
- **713** (PK 713) : description
- **719** (PK 719) : description
- **726** (PK 726) : description
- **727** (PK 727) : description
- **729** (PK 729) : description
- **733** (PK 733) : description
- **735** (PK 735) : description
- **740** (PK 740) : description
- **759** (PK 759) : description
- **784** (PK 784) : description
- **798** (PK 798) : description
- **799** (PK 799) : description
- **809** (PK 809) : description
- **813** (PK 813) : description, exemple
- **887** (PK 887) : description
- **888** (PK 888) : description
- **889** (PK 889) : description
- **900** (PK 900) : description
- **908** (PK 908) : description
- **942** (PK 942) : description
- **Citation hors contexte** (PK 943) : exemple
- **953** (PK 953) : description
- **Plaider l’exception** (PK 956) : description
- **973** (PK 973) : description
- **Monter la barre** (PK 974) : description, exemple
- **977** (PK 977) : description
- **994** (PK 994) : description

### Vertus (Tarot) (17 carte(s))

- **Prémisses fiables** (pk 12) : description
- **Logique informelle solide** (pk 128) : titre
- **Définitions claires** (pk 135) : subfamily
- **Analogie appropriée** (pk 144) : description
- **Reconnaître ses biais idéologiques** (pk 175) : titre
- **Tenir compte des biais idéologiques** (pk 176) : titre
- **Argument fondé** (pk 2) : description
- **Principe de charité** (pk 205) : description
- **Évaluation loyale de la position adverse** (pk 208) : remarque
- **Ne pas interrompre** (pk 220) : description
- **Ton respectueux** (pk 221) : description
- **Hypothèse plausible** (pk 24) : description
- **Argument déductif** (pk 4) : description
- **Sans chantage aux conséquences** (pk 54) : description
- **Preuves tangibles** (pk 6) : description
- **Objectif clair** (pk 7) : description
- **Illustrer par des exemples** (pk 8) : description

### Scénarios (Poker) (16 carte(s))

- **Veto** (path 1.1.3) : suggestion
- **Napoléon et la campagne de Russie** (path 1.2.3) : contexte, enjeu
- **Le loup et l’agneau** (path 2.1.8) : suggestion
- **Déjà-vu** (path 2.2.9) : contexte
- **Le t-shirt taché** (path 3.2.15) : titre
- **Le flambeur** (path 3.2.16) : enjeu
- **Mariage de pierre** (path 3.3.2) : enjeu
- **Destination Mars** (path 4.1.11) : enjeu
- **Réveil compromis** (path 4.2.8) : titre
- **Une IA vraiment éthique ?** (path 4.3.4) : contexte
- **On était en pause** (path 5.2.5) : enjeu
- **Refus du vaccin** (path 5.3.2) : enjeu
- **Moralisation** (path 6.1.1) : suggestion
- **Le chien dévastateur** (path 7.2.3) : contexte
- **Foie gras végétarien** (path 7.2.5) : enjeu
- **Le fromage vivant** (path 7.3.5) : suggestion

### Règles (Tarot) (5 carte(s))

- **Rules_08** (pk Rules_08) : texte
- **Rules_11** (pk Rules_11) : texte
- **Rules_12** (pk Rules_12) : texte
- **Rules_13** (pk Rules_13) : texte
- **Rules_15** (pk Rules_15) : texte

## العربية — 50 carte(s) touchée(s)

### Sophismes (Tarot + Web) (6 carte(s))

- **Attaque de l’estime de soi** (PK 1388) : exemple
- **622** (PK 622) : exemple
- **784** (PK 784) : description
- **900** (PK 900) : exemple
- **Citation hors contexte** (PK 943) : exemple
- **Monter la barre** (PK 974) : exemple

### Vertus (Tarot) (23 carte(s))

- **Citer ses sources** (pk 10) : description
- **Logique informelle solide** (pk 128) : titre
- **Définitions claires** (pk 135) : subfamily
- **Effort d’objectivité** (pk 167) : subfamily, titre
- **Universalisme** (pk 168) : subfamily
- **Reconnaître ses biais** (pk 169) : subfamily
- **Comprendre les biais d’autrui** (pk 170) : subfamily
- **Conscience interculturelle** (pk 171) : subfamily
- **Reconnaître ses biais culturels** (pk 172) : subfamily
- **Comprendre les biais culturels d’autrui** (pk 173) : subfamily
- **Mettre les idéologies à distance** (pk 174) : subfamily
- **Reconnaître ses biais idéologiques** (pk 175) : subfamily, titre
- **Tenir compte des biais idéologiques** (pk 176) : subfamily, titre
- **Acceptation de l’incertitude** (pk 177) : subfamily
- **Principe de charité** (pk 205) : description
- **Évaluation loyale de la position adverse** (pk 208) : remarque
- **Argument déductif** (pk 4) : description
- **Argument inductif** (pk 5) : description
- **Preuves tangibles** (pk 6) : description
- **Objectif clair** (pk 7) : description
- **Illustrer par des exemples** (pk 8) : description
- **Causalités bien identifiées** (pk 80) : subfamily
- **Énoncés rigoureux** (pk 84) : subfamily

### Scénarios (Poker) (16 carte(s))

- **Veto** (path 1.1.3) : suggestion
- **Napoléon et la campagne de Russie** (path 1.2.3) : contexte, enjeu
- **Le loup et l’agneau** (path 2.1.8) : suggestion
- **Déjà-vu** (path 2.2.9) : contexte
- **Le pizzaïolo** (path 3.1.5) : contexte, piocheur
- **Le t-shirt taché** (path 3.2.15) : titre
- **Le flambeur** (path 3.2.16) : enjeu
- **Mariage de pierre** (path 3.3.2) : enjeu
- **Destination Mars** (path 4.1.11) : enjeu
- **Le babysitter et le chat chauve** (path 4.1.12) : enjeu
- **Réveil compromis** (path 4.2.8) : titre
- **On était en pause** (path 5.2.5) : enjeu
- **Refus du vaccin** (path 5.3.2) : enjeu
- **Moralisation** (path 6.1.1) : suggestion
- **Foie gras végétarien** (path 7.2.5) : enjeu
- **Le fromage vivant** (path 7.3.5) : suggestion

### Règles (Tarot) (5 carte(s))

- **Rules_08** (pk Rules_08) : texte
- **Rules_09** (pk Rules_09) : texte
- **Rules_12** (pk Rules_12) : texte
- **Rules_13** (pk Rules_13) : texte
- **Rules_15** (pk Rules_15) : texte

## فارسی — 56 carte(s) touchée(s)

### Sophismes (Tarot + Web) (8 carte(s))

- **Défense Chewbacca** (PK 1330) : exemple
- **323** (PK 323) : description
- **361** (PK 361) : description
- **362** (PK 362) : description
- **696** (PK 696) : description
- **813** (PK 813) : exemple
- **Citation hors contexte** (PK 943) : exemple
- **Monter la barre** (PK 974) : exemple

### Vertus (Tarot) (26 carte(s))

- **Citer ses sources** (pk 10) : description
- **Prémisses fiables** (pk 12) : description
- **Définitions claires** (pk 135) : subfamily
- **Niveau de preuve approprié** (pk 162) : titre
- **Objectif non complaisant** (pk 164) : subsubfamily, titre
- **Identifier les points de vigilance** (pk 165) : subsubfamily
- **Universalisme** (pk 168) : subsubfamily, titre
- **Reconnaître ses biais** (pk 169) : subsubfamily
- **Comprendre les biais d’autrui** (pk 170) : subsubfamily
- **Reconnaître ses biais idéologiques** (pk 175) : titre
- **Tenir compte des biais idéologiques** (pk 176) : titre
- **Argument fondé** (pk 2) : description
- **Principe de charité** (pk 205) : description
- **Interprétation juste** (pk 28) : description
- **Complexité adaptée** (pk 29) : description
- **Représentation parcimonieuse** (pk 31) : description
- **Rasoir d’Ockham** (pk 32) : description
- **Rasoir de Hanlon** (pk 33) : description
- **Argument déductif** (pk 4) : description
- **Argument inductif** (pk 5) : description
- **Preuves tangibles** (pk 6) : description
- **Objectif clair** (pk 7) : description
- **Illustrer par des exemples** (pk 8) : description
- **Causalités bien identifiées** (pk 80) : subfamily
- **Énoncés rigoureux** (pk 84) : subfamily
- **Appuyer par des citations** (pk 9) : description

### Scénarios (Poker) (18 carte(s))

- **Veto** (path 1.1.3) : suggestion
- **Napoléon et la campagne de Russie** (path 1.2.3) : contexte, enjeu
- **Déjà-vu** (path 2.2.9) : contexte
- **Le pizzaïolo** (path 3.1.5) : contexte, piocheur
- **Le t-shirt taché** (path 3.2.15) : titre
- **Le flambeur** (path 3.2.16) : enjeu
- **L’anniversaire oublié** (path 3.2.8) : contexte
- **Mariage de pierre** (path 3.3.2) : enjeu
- **Destination Mars** (path 4.1.11) : enjeu
- **Le babysitter et le chat chauve** (path 4.1.12) : enjeu
- **Réveil compromis** (path 4.2.8) : titre
- **Une IA vraiment éthique ?** (path 4.3.4) : contexte
- **On était en pause** (path 5.2.5) : enjeu
- **Refus du vaccin** (path 5.3.2) : enjeu
- **Gravité inversée** (path 5.3.5) : suggestion
- **Moralisation** (path 6.1.1) : suggestion
- **Foie gras végétarien** (path 7.2.5) : enjeu
- **Le fromage vivant** (path 7.3.5) : suggestion

### Règles (Tarot) (4 carte(s))

- **Rules_08** (pk Rules_08) : texte
- **Rules_12** (pk Rules_12) : texte
- **Rules_13** (pk Rules_13) : texte
- **Rules_15** (pk Rules_15) : texte

## 中文 — 36 carte(s) touchée(s)

### Sophismes (Tarot + Web) (4 carte(s))

- **813** (PK 813) : exemple
- **Citation hors contexte** (PK 943) : exemple
- **Monter la barre** (PK 974) : exemple
- **989** (PK 989) : description

### Vertus (Tarot) (10 carte(s))

- **Citer ses sources** (pk 10) : description
- **Définitions claires** (pk 135) : subfamily
- **Clarté des enjeux** (pk 159) : subfamily
- **Argument déductif** (pk 4) : description
- **Preuves tangibles** (pk 6) : description
- **Objectif clair** (pk 7) : description
- **Illustrer par des exemples** (pk 8) : description
- **Causalités bien identifiées** (pk 80) : subfamily
- **Énoncés rigoureux** (pk 84) : subfamily
- **Appuyer par des citations** (pk 9) : description

### Scénarios (Poker) (18 carte(s))

- **Veto** (path 1.1.3) : suggestion
- **Napoléon et la campagne de Russie** (path 1.2.3) : contexte, enjeu
- **Le loup et l’agneau** (path 2.1.8) : suggestion
- **Déjà-vu** (path 2.2.9) : contexte
- **Le flambeur** (path 3.2.16) : enjeu
- **L’anniversaire oublié** (path 3.2.8) : suggestion, titre
- **Mariage de pierre** (path 3.3.2) : enjeu
- **Destination Mars** (path 4.1.11) : enjeu
- **Le babysitter et le chat chauve** (path 4.1.12) : enjeu
- **Réveil compromis** (path 4.2.8) : titre
- **La commande de trop** (path 4.3.3) : contexte, enjeu
- **Une IA vraiment éthique ?** (path 4.3.4) : contexte
- **Vous comprendrez... que dalle** (path 4.3.5) : contexte
- **Ski extrême : le défi écureuil** (path 4.3.6) : contexte
- **On était en pause** (path 5.2.5) : enjeu
- **Moralisation** (path 6.1.1) : suggestion
- **Retrait négocié** (path 6.2.1) : titre
- **Le fromage vivant** (path 7.3.5) : suggestion

### Règles (Tarot) (4 carte(s))

- **Rules_08** (pk Rules_08) : texte
- **Rules_12** (pk Rules_12) : texte
- **Rules_13** (pk Rules_13) : texte
- **Rules_15** (pk Rules_15) : texte

