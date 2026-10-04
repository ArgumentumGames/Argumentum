# Pool #458 v23 — grain ㉝ (g39) : registre es des définitions — tableau de propositions « tú » (0 écriture)

**Date** : 2026-10-04 · **Branche** : `docs/458-registre-es-tu-tableau` (depuis master, non empilée) · **Machine** : myia-po-2024
**Dispatch** : c.5971513525 item 4 — « pour chaque définition es au vosotros / ustedes / usted (≈ 80 cellules), écrire la forme proposée au « tú » (verbes, possessifs, pronoms), avec le texte avant / après. C'est un tableau à relire, pas une écriture. »
**Écritures CSV : 0** (`git status --porcelain Cards/` vide après la passe).

> **Erratum d'exécution (04/10, grain 1 du pool c.5975630522, arbitrage c.5975624315).** Le tableau est **exécuté** : **74 cellules `desc_es` écrites** — §2 (39) tel quel · §3+§4 (25) **sans le « Tú » initial** (verbe en majuscule : « Fundamentas tus argumentos… », « Te apoyas… », « Das peso… ») · §5 (10) **tranché au tú** avec la forme proposée. Les tableaux ci-dessous demeurent l'état de la **proposition** : pour §3/§4, la forme écrite retire le « Tú » en tête qu'ils portent encore. Garde : `EsDeckRegisterTuGuardTests` (4 faits — zéro marqueur vosotros/ustedes/usted et zéro désinence -áis/-éis/-ís sur les 175 définitions, zéro « Tú » initial, 5 irrégulières pleine cellule, 4 conservations 3ᵉ personne ; 3 mutations toutes rouges, restauration byte-exacte). L'espagnol n'a jamais été imprimé : le changement entrera dans les PDF es à la prochaine régénération, après le verdict des associés.

## 1. Recensement mesuré (175 définitions `desc_es` du deck)

| registre | n | statut |
|---|---:|---|
| `tú` explicite (pronom) | 36 | conforme — aucune écriture |
| `tú` morphologique (verbe en `-as`/`-es`, GRIS pour l'instrument) | 41 | conforme — aucune écriture (l'instrument ne le voit pas : l'écran explicite ne couvre que le pronom) |
| `vosotros` (pronom/désinences `-áis/-éis/-ís`) | 39 | **à convertir** (§2) |
| `ustedes` (pronom) | 20 | **à convertir** (§3) |
| `usted` explicite (pronom) | 5 | **à convertir** (§4) |
| 3ᵉ personne descriptive (`Formula…`, `Supone…` — GRIS) | 10 | **à trancher** (§5) : adresse au lecteur ou description du sophiste ? |
| `ZERO_SUJET` (infinitif/gérondif initial — neutre) | 13 | compatible tú — aucune écriture |
| GRIS pur (aucun marqueur) | 11 | compatible tú — aucune écriture |

**Réconciliation avec l'arbitrage (« tú 82, vosotros 39, ustedes 22, usted 19 »)** : vosotros 39 = exact ; le « tú 82 » recouvre nos 36 explicites + 41 morphologiques + quelques neutres (coupe différente, même matière) ; « ustedes 22 / usted 19 » rapprochent nos 20 + 5 explicites des 10 descriptives tacites — la coupe de l'arbitrage comptait « toute cellule non-vosotros s'adressant en 3ᵉ personne » comme usted. La worklist ci-dessous (64 + 10 = 74 cellules) couvre l'intégralité des deux coupes. Le recensement somme à 175 (36+41+39+20+5+10+13+11).

## 2. vosotros → tú (39) — désinences `-áis/-éis/-ís` → `-as/-es/-es`, possessifs, pronoms

| PK | avant | proposé |
|---|---|---|
| 33 | Atribuís a un hábito, una impresión o un ejemplo el valor de una prueba. | Atribuyes a un hábito, una impresión o un ejemplo el valor de una prueba. |
| 51 | Confundís vuestra perspectiva individual con una objetividad universal. | Confundes tu perspectiva individual con una objetividad universal. |
| 55 | Ante objeciones válidas, improvisáis justificaciones no fundadas para escapar a la crítica. | Ante objeciones válidas, improvisas justificaciones no fundadas para escapar a la crítica. |
| 121 | Rechazáis o desviáis un argumento por temor a que pueda ofender a una parte del público. | Rechazas o desvías un argumento por temor a que pueda ofender a una parte del público. |
| 128 | Pensáis que una idea es válida solo porque quien la presenta es rico. | Piensas que una idea es válida solo porque quien la presenta es rico. |
| 133 | Sacáis conclusiones exageradas de los elementos de los que disponéis. | Sacas conclusiones exageradas de los elementos de los que dispones. |
| 134 | Abordáis cuestiones complejas como si fueran simples juegos o problemas fácilmente resolubles, a menudo utilizando modelos simplistas o descuidando la complejidad real. | Abordas cuestiones complejas como si fueran simples juegos o problemas fácilmente resolubles, a menudo utilizando modelos simplistas o descuidando la complejidad real. |
| 219 | Usáis el humor para hacer que vuestra argumentación sea más atractiva, sin basaros en hechos. | Usas el humor para hacer que tu argumentación sea más atractiva, sin basarte en hechos. |
| 300 | Establecéis un vínculo emocional con vuestro auditorio para ganar su apoyo. | Estableces un vínculo emocional con tu auditorio para ganar su apoyo. |
| 357 | Creáis asociaciones entre ideas y emociones, influyendo sutilmente en los comportamientos sin que sea evidente. | Creas asociaciones entre ideas y emociones, influyendo sutilmente en los comportamientos sin que sea evidente. |
| 421 | Utilizáis vuestro carisma para conseguir que vuestro auditorio adhiera a vuestras ideas. | Utilizas tu carisma para conseguir que tu auditorio adhiera a tus ideas. |
| 432 | Buscáis influir en el comportamiento de vuestro auditorio invitándolo a asumir una responsabilidad inducida por vuestra propuesta. | Buscas influir en el comportamiento de tu auditorio invitándolo a asumir una responsabilidad inducida por tu propuesta. |
| 603 | Ignoráis los hechos que no apoyan vuestra argumentación. | Ignoras los hechos que no apoyan tu argumentación. |
| 621 | Atribuís a un grupo entero las características de sus elementos, o a la inversa. | Atribuyes a un grupo entero las características de sus elementos, o a la inversa. |
| 622 | Pensáis erróneamente que una característica común a los miembros de un grupo se extiende a todo el grupo. | Piensas erróneamente que una característica común a los miembros de un grupo se extiende a todo el grupo. |
| 625 | Atribuís erróneamente ciertas características de un grupo a cada uno de sus miembros. | Atribuyes erróneamente ciertas características de un grupo a cada uno de sus miembros. |
| 707 | Confundís la causa con el efecto, invirtiendo así la dinámica causal. | Confundes la causa con el efecto, invirtiendo así la dinámica causal. |
| 708 | Deducís una causa a partir de su efecto sin tener en cuenta las otras causas posibles. | Deduces una causa a partir de su efecto sin tener en cuenta las otras causas posibles. |
| 729 | Rechazáis una conclusión simplemente porque una causa posible no se ha realizado, confundiendo así causa y condición necesaria. | Rechazas una conclusión simplemente porque una causa posible no se ha realizado, confundiendo así causa y condición necesaria. |
| 733 | Afirmáis que dos opciones son mutuamente exclusivas cuando podrían coexistir. | Afirmas que dos opciones son mutuamente exclusivas cuando podrían coexistir. |
| 759 | Sacáis conclusiones precipitadas sin disponer de suficientes pruebas. | Sacas conclusiones precipitadas sin disponer de suficientes pruebas. |
| 784 | Construís un razonamiento lógico en tres partes (premisa mayor, premisa menor, conclusión) de manera incorrecta. | Construyes un razonamiento lógico en tres partes (premisa mayor, premisa menor, conclusión) de manera incorrecta. |
| 887 | Os liberáis de las reglas tácitas que rigen un debate racional. | Te liberas de las reglas tácitas que rigen un debate racional. |
| 888 | Presentáis eventos o hechos de manera engañosa. | Presentas eventos o hechos de manera engañosa. |
| 900 | Omitís decir lo que realmente pensáis. | Omites decir lo que realmente piensas. |
| 953 | No presentáis más que los hechos que apoyan vuestra tesis, ocultando aquellos que la contradicen. | No presentas más que los hechos que apoyan tu tesis, ocultando aquellos que la contradicen. |
| 956 | Pedís una excepción a una regla sin razón válida. | Pides una excepción a una regla sin razón válida. |
| 973 | Cambiáis los criterios del debate sin decirlo, a fin de evitar admitir vuestro error. | Cambias los criterios del debate sin decirlo, a fin de evitar admitir tu error. |
| 974 | Aumentáis vuestras pretensiones a medida que vuestro interlocutor las va cumpliendo. | Aumentas tus pretensiones a medida que tu interlocutor las va cumpliendo. |
| 977 | Rechazáis las soluciones realistas pidiendo un ideal inalcanzable. | Rechazas las soluciones realistas pidiendo un ideal inalcanzable. |
| 994 | Ahogáis vuestras proposiciones en una corriente de generalidades, evitando así comprometeros en algo específico. | Ahogas tus proposiciones en una corriente de generalidades, evitando así comprometerte en algo específico. |
| 1023 | Vuestros argumentos están influenciados por vuestros propios sesgos, lo que dificulta vuestra capacidad de juzgar de manera equilibrada. | Tus argumentos están influenciados por tus propios sesgos, lo que dificulta tu capacidad de juzgar de manera equilibrada. |
| 1024 | Vuestros argumentos están teñidos por vuestra perspectiva humana, perdiendo así la objetividad científica. | Tus argumentos están teñidos por tu perspectiva humana, perdiendo así la objetividad científica. |
| 1092 | Os concentráis en los elementos desfavorables y pesimistas, en detrimento de los posibles aspectos positivos. | Te concentras en los elementos desfavorables y pesimistas, en detrimento de los posibles aspectos positivos. |
| 1120 | Razonáis en términos de todo o nada, sin matices: si no sois perfectos, os consideráis un fracaso total. | Razonas en términos de todo o nada, sin matices: si no eres perfecto, te consideras un fracaso total. |
| 1174 | Vuestras posiciones están influenciadas por vuestro contexto cultural. | Tus posiciones están influenciadas por tu contexto cultural. |
| 1242 | Razonáis en un contexto sesgado por la aplicación estricta de conceptos teóricos. | Razonas en un contexto sesgado por la aplicación estricta de conceptos teóricos. |
| 1330 | Inundáis la conversación con un flujo de información sin relación para enmascarar la debilidad de vuestra posición. | Inundas la conversación con un flujo de información sin relación para enmascarar la debilidad de tu posición. |
| 1388 | Desacreditáis las ideas de alguien destacando su falta de confianza en sí mismo, en lugar de refutar su argumentación. | Desacreditas las ideas de alguien destacando su falta de confianza en sí mismo, en lugar de refutar su argumentación. |

## 3. ustedes → tú (20) — pronom + verbe principal + possessifs ; les 3ᵉ personnes objets restent

⚠️ *Arbitrage c.5975624315 : la forme écrite retire le « Tú » initial ci-dessous — verbe en majuscule (« Fundamentas tus argumentos… »), le reste inchangé. Aucune définition du deck ne commence par « Tú » ; le pronom explicite en tête serait emphatique.*

| PK | avant | proposé |
|---|---|---|
| 70 | Ustedes fundamentan sus argumentos en ideas recibidas, sin examinarlas de forma crítica. | Tú fundamentas tus argumentos en ideas recibidas, sin examinarlas de forma crítica. |
| 176 | Ustedes utilizan fórmulas seductoras y un estilo persuasivo para convencer, sin fundamentar necesariamente su discurso en un razonamiento lógico. | Tú utilizas fórmulas seductoras y un estilo persuasivo para convencer, sin fundamentar necesariamente tu discurso en un razonamiento lógico. |
| 184 | Ustedes definen los términos de manera sesgada para orientar el debate a su favor. | Tú defines los términos de manera sesgada para orientar el debate a tu favor. |
| 185 | Ustedes utilizan clichés impactantes para cortocircuitar el espíritu crítico o evitar un debate argumentado. | Tú utilizas clichés impactantes para cortocircuitar el espíritu crítico o evitar un debate argumentado. |
| 299 | Ustedes suscitan emociones fuertes para desviar a su auditorio de una reflexión racional. | Tú suscitas emociones fuertes para desviar a tu auditorio de una reflexión racional. |
| 356 | Ustedes utilizan procedimientos que orientan sutilmente el punto de vista de sus interlocutores, a menudo sin que ellos se den cuenta. | Tú utilizas procedimientos que orientan sutilmente el punto de vista de tus interlocutores, a menudo sin que ellos se den cuenta. |
| 358 | Ustedes imponen con su formulación un contexto que orienta las conclusiones. | Tú impones con tu formulación un contexto que orienta las conclusiones. |
| 420 | Ustedes buscan influir en su auditorio ejerciendo un ascendiente psicológico sobre él. | Tú buscas influir en tu auditorio ejerciendo un ascendiente psicológico sobre él. |
| 511 | Ustedes buscan convencer más allá del sentido de las palabras, especialmente mediante el lenguaje corporal o las inflexiones de la voz. | Tú buscas convencer más allá del sentido de las palabras, especialmente mediante el lenguaje corporal o las inflexiones de la voz. |
| 633 | Ustedes atribuyen una relación significativa a lo que no es más que una simple coincidencia. | Tú atribuyes una relación significativa a lo que no es más que una simple coincidencia. |
| 636 | Ustedes atribuyen una causa errónea a lo que no es más que el resultado de una fluctuación completamente normal. | Tú atribuyes una causa errónea a lo que no es más que el resultado de una fluctuación completamente normal. |
| 713 | Ustedes creen erróneamente que dos errores combinados pueden producir un resultado justo. | Tú crees erróneamente que dos errores combinados pueden producir un resultado justo. |
| 799 | Ustedes definen los términos de manera que favorezcan su argumento, descartando su sentido establecido. | Tú defines los términos de manera que favorezcan tu argumento, descartando su sentido establecido. |
| 889 | Ustedes afirman algo que saben que es falso. | Tú afirmas algo que sabes que es falso. |
| 908 | Ustedes se apoyan en hechos ampliamente aceptados, pero inexactos, para sostener sus afirmaciones. | Tú te apoyas en hechos ampliamente aceptados, pero inexactos, para sostener tus afirmaciones. |
| 1282 | Ustedes afirman que la verdad es subjetiva y propia de cada individuo. | Tú afirmas que la verdad es subjetiva y propia de cada individuo. |
| 1287 | Ustedes falsean el debate al fingir que explican hechos o conceptos. | Tú falseas el debate al fingir que explicas hechos o conceptos. |
| 1312 | Ustedes perturban el debate para orientarlo a su favor o limitar su alcance. | Tú perturbas el debate para orientarlo a tu favor o limitar su alcance. |
| 1360 | Ustedes apuntan contra su adversario mismo en lugar de refutar sus argumentos. | Tú apuntas contra tu adversario mismo en lugar de refutar sus argumentos. |
| 1361 | Ustedes señalan las incoherencias o contradicciones de su interlocutor, o le oponen argumentos erróneos que, no obstante, pueden parecerle válidos. | Tú señalas las incoherencias o contradicciones de tu interlocutor, o le opones argumentos erróneos que, no obstante, pueden parecerle válidos. |

## 4. usted → tú (5)

⚠️ *Même arbitrage que §3 : « Tú » initial retiré à l'écriture (« Consideras… », « Crees… », « Cometes… », « Sacas… », « Das peso… »).*

| PK | avant | proposé |
|---|---|---|
| 43 | Usted considera que un comportamiento está justificado porque es comúnmente adoptado. | Tú consideras que un comportamiento está justificado porque es comúnmente adoptado. |
| 719 | Usted cree erróneamente que una correlación entre dos eventos implica necesariamente una relación de causa y efecto. | Tú crees erróneamente que una correlación entre dos eventos implica necesariamente una relación de causa y efecto. |
| 735 | Usted comete un error al usar cuantificadores como «todos», «ninguno» o «algunos», lo que impacta el argumento. | Tú cometes un error al usar cuantificadores como «todos», «ninguno» o «algunos», lo que impacta el argumento. |
| 740 | Usted saca una conclusión directamente de una premisa de manera incorrecta, atribuyendo propiedades o conclusiones que no están lógicamente respaldadas por esa premisa. | Tú sacas una conclusión directamente de una premisa de manera incorrecta, atribuyendo propiedades o conclusiones que no están lógicamente respaldadas por esa premisa. |
| 942 | Usted da peso a su argumento citando una fuente falsa, mal identificada, incompetente o inventada. | Tú das peso a tu argumento citando una fuente falsa, mal identificada, incompetente o inventada. |

## 5. À trancher — 3ᵉ personne descriptive (10)

Ces définitions décrivent le sophiste (« Formula un argumento… ») sans pronom : adressées au lecteur (→ convertir comme §2-4) ou description neutre (→ statu quo) ? **Erratum (c.5975624315) : l'affirmation initiale « le français utilise l'infinitif » est FAUSSE pour ces 10 cartes — le français s'y adresse au lecteur** (2 « Vous construisez un argument… », 108 « Vous supposez… », 653 « Vous croyez que vos succès récents… », 813 « …l'idée générale que vous défendez »). **Tranché au tú, avec la forme proposée** : « Formulas… » se lirait en 3ᵉ personne ou en impératif ; le tú aligne les 10 cartes sur le français et sur le reste de la colonne.

| PK | avant | proposé (si adresse) |
|---|---|---|
| 2 | Formula un argumento basado en impresiones o anécdotas, sin evidencia sólida. | Formulas un argumento basado en impresiones o anécdotas, sin evidencia sólida. |
| 3 | Utiliza afirmaciones que no contribuyen a establecer un argumento convincente. | Utilizas afirmaciones que no contribuyen a establecer un argumento convincente. |
| 108 | Supone que algo es bueno porque es natural, o malo porque no es natural. | Supones que algo es bueno porque es natural, o malo porque no es natural. |
| 653 | Cree que su reciente éxito influirá positivamente en sus futuras oportunidades. | Crees que tu reciente éxito influirá positivamente en tus futuras oportunidades. |
| 726 | Encadena incorrectamente proposiciones lógicas, llevando a un razonamiento erróneo. | Encadenas incorrectamente proposiciones lógicas, llevando a un razonamiento erróneo. |
| 727 | Utiliza incorrectamente las proposiciones lógicas básicas conectadas por «y», «o», «si... entonces», lo que lleva a conclusiones falsas. | Utilizas incorrectamente las proposiciones lógicas básicas conectadas por «y», «o», «si... entonces», lo que lleva a conclusiones falsas. |
| 798 | Usa sutilezas lingüísticas para convencer. | Usas sutilezas lingüísticas para convencer. |
| 809 | Define los términos del debate de manera que se difuminen sus diferencias. | Defines los términos del debate de manera que se difuminen sus diferencias. |
| 813 | Para refutar un contraejemplo, modifica los contornos de la idea general que defiende. | Para refutar un contraejemplo, modificas los contornos de la idea general que defiendes. |
| 1345 | Complica deliberadamente el debate para impedir llegar a una resolución clara. | Complicas deliberadamente el debate para impedir llegar a una resolución clara. |

## 6. Jugements portés (à relire en priorité)

- **889** : « saben » → « sabes » — le second verbe est addresseur (« algo que **saben** que es falso »), pas objet.
- **908** : « se apoyan » → « te apoyas » — réfléchi addresseur.
- **1287** : « explican » → « explicas » — « al fingir que **explican** » (l'addresseur feint d'expliquer).
- **1361** : « oponen » → « opones » — mais « le » et « parecerle » restent 3ᵉ (l'interlocuteur).
- **1360** : « sus argumentos » **reste** — ce sont les arguments de l'adversaire (3ᵉ), seul « su adversario » devient « tu adversario ».
- **799** : « su sentido establecido » **reste** — le sens établi des termes (3ᵉ).
- **1312** : « su alcance » **reste** (l'ampleur du débat) ; « a su favor » → « a tu favor ».
- **356** : « orientan » et « se den cuenta » **restent** — les procédés orientent, les interlocuteurs s'en rendent compte (3ᵉ).
- **713/719** : « pueden producir » et « implica » restent 3ᵉ (sujets : deux erreurs, la corrélation).
- **956** : « Pedís » → « Pides » (pedir, diphtongue e→i) — la règle générique `-ís`→`-es` produirait « Pedes » : la plus forte irrégularité du lot.
- **128/622/900** : « Pensáis » → « **Piensas** » (pensar, diphtongue e→ie) — la règle générique produirait « Pensas ».
- **121** : « desviáis » → « **desvías** » (accent : desviar, llana finissant en -s) — la règle générique produirait « desvias » sans accent.
- **219/994** : enclitiques « basar**os** » → « basar**te** », « comprometer**os** » → « comprometer**te** » — le pronom atone soudé à l'infinitif.
- **1120** : accord — « si no sois perfectos » → « si no eres perfect**o** » (le pluriel de l'attribut suit le sujet converti).
- **809 (§5)** : « sus diferencias » reste — les différences des termes.
- **813 (§5)** : « que defiende » → « que defiendes » — la proposition suppose l'adresse (le second verbe est addresseur).

## N'établit pas

- **Aucune écriture** n'a eu lieu et aucune n'est exécutée ici — le tableau attend la relecture cellule par cellule d'ai-01 (dispatch : « que je relis cellule par cellule »).
- La conversion des 10 descriptives (§5) — question ouverte, proposition mécanique seulement.
- Les exemples es (registre des répliques : statu quo arbitré) et le portugais (imprimé : registre gardé, sauf décision owner).
- Les 1233 rangées hors deck.

⛔ Gel `v2.0.0-review` respecté — aucune republication.

## Reproductibilité

```bash
python docs/corpus/register-consistency-instrument.py --self-test
# classification rejouée : classify(desc_es) par rangée deck (175) -- même instrument que #1732
git status --porcelain Cards/   # vide
```

*po-2024*
