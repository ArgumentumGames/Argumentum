# -*- coding: utf-8 -*-
"""Ecran de fidelite des REGLES du deck : instrument de mesure, 0 ecriture.

POURQUOI CE DOCUMENT EXISTE
---------------------------
Les TITRES ont eu leurs passes de fidelite (grains (8)-(17), 7 langues) et les DEFINITIONS
et EXEMPLES les leurs (`definitions-fidelity-instrument.py`). Le corpus des REGLES
(`Cards/Rules/Argumentum Rules - Cards.csv`, 15 rangees dont 6 miroirs P&P, 105 cellules sur
7 langues cibles) n'avait pas d'instrument. Celui-ci en est un, et il est ecrit APRES
reparation : la premiere version, ecrite le 04/10 au matin, portait **cinq defauts de mesure**
dont un faisait lever un signal sur *chaque* rangee par construction, et un autre faisait
**taire un vrai defaut**. La mesure avant/apres et les temoins sont en bas de ce fichier.

⛔ CE QUE L'INSTRUMENT N'EST PAS. Les drapeaux sont des PRIORITES DE LECTURE, pas des
verdicts. Sur ce corpus, il rend **21 drapeaux**, qui se decomposent en **13 faux positifs
mesures** (6 `DUEL?` et 1 `MOT-NOMBRE?` -- le nombre est ecrit en mots ou au duel, 6
`CHIFFRE?` -- la forme employee n'est pas celle qu'annonce la table, cf. defaut 6) et **8
cellules reelles** (la famille `PONCT`, voir plus bas). Un ecran mecanique ne sait pas voir un
contresens : c'est la lecture qui porte le verdict, et 8 cellules sur 105 ne jugent pas 129 Ko
de texte.

⛔ LA CELLULE EST UN DOCUMENT ENTIER, PAS UNE CHAINE. `Text_<lang>` contient plusieurs
centaines d'octets de markdown (titres `##`, listes `*`, emoji `❌🏆➜✅1🎴`). Un ecran
cellule-par-cellule ne peut donc RIEN dire de la fidelite d'un paragraphe : il ne compare
que des nombres et des ponctuations de fin. C'est exactement ce qu'il fait, et rien de plus.

LES CINQ DEFAUTS REPARES OU NOMMES (04/10/2026)
-----------------------------------------------
 (1) `=EN` COMPARE A LUI-MEME -- repere et repare. La boucle parcourait la liste
     `['en','ru',...]`, donc pour `lg == 'en'` la cellule `Text_en` etait comparee a
     `Text_en` : **toujours vraie**. Chaque rangee recevait un `en[=EN]` par construction, et
     le total annoncait `=EN : 15` -- un « 15 » qui ne mesurait rien. Garde `lg != 'en'`.
     ⭐ *Un signal qui se leve partout ne distingue rien : c'est un bruit, pas une mesure.*
 (2) CHIFFRES NATIFS NON NORMALISES -- repere et repare. `\\d+` ne voit que l'ASCII : un
     nombre ecrit en chiffres arabo-indiens (U+0660-0669) ou persans (U+06F0-06F9) etait vu
     comme ABSENT (faux `CHIFFRE?`) et un nombre natif en trop etait invisible (`+CHIFFRE`
     manque). `norm_digits()` ramene les deux plages a l'ASCII avant comparaison.
 (3) `NUMWORD` DECLARE ET JAMAIS CABLE -- repere et cable. Les tables de nombres en mots
     (7 langues x 10) existaient depuis la premiere version mais n'etaient lues nulle part :
     un instrument annonce mais inerte. Un nombre absent en chiffres mais present en MOT est
     desormais rendu `MOT-NOMBRE?n`, pas `CHIFFRE?n`.
 (4) LE NOMBRE PEUT ETRE DANS UNE AUTRE CLASSE QUE LE CARDINAL -- NOMME, PAS REPARE ENTIER.
     C'est la limite qui reste. `MOT-NOMBRE?` ne compare que des cardinaux, et deux langues
     du corpus expriment l'entier autrement :
       * **arabe -- le DUEL** : « رُزمتان » (deux piles) pour `2`, au lieu de « اثنان ».
         Repare par `DUEL?` (motif `تان|تين`). Le duel nu en `ان` est **volontairement
         exclu** : il termine des mots arabes courants et masquerait une vraie omission --
         c'est le temoin 9, controle inverse obligatoire de cette garde.
       * **russe -- le COLLECTIF ADVERBIAL** : « вчетвером » (a quatre) pour `4`, au lieu de
         « четыре ». **NON repare** -- voir defaut (6).
     ⭐ *Regle generale : un faux positif NOMME et borne vaut mieux qu'une garde elargie qui
     avale aussi les vrais defauts.*
 (5) LE MOT-NOMBRE SE RECONNAISSAIT DANS UN AUTRE MOT -- repere et repare. ⚠️ C'est LE SEUL
     DES CINQ QUI POUVAIT FAIRE TAIRE UN VRAI DEFAUT, et il ne l'a pas fait par chance : il
     l'a fait par construction. Le test etait `words[n-1] in v.lower()`, une sous-chaine nue.
     En russe, « иллюс**три**рует » (illustre) contient les lettres т-р-и, donc le 3 de
     Rules_06 -- absent en chiffres -- etait reclasse « ecrit en mots » et ne sortait plus
     qu'en `MOT-NOMBRE?3`. Mesure : `'три' in v` -> True, frontiere de mot -> False.
     Repare par `whole_word()` (lookaround par script d'ecriture ; `zh` reste en sous-chaine,
     le chinois n'ayant pas de frontiere de mot).
     ⚠️ La reparation a un COUT MESURE, assume : elle reclasse 6 cellules de `MOT-NOMBRE?` en
     `CHIFFRE?` -- donc plus bruyantes, et moins precises. C'est le bon sens de l'erreur.
 (6) LA FORME DU NOMBRE N'EST PAS CELLE QU'ANNONCE LA TABLE -- NOMME, NON REPARE. Residu
     borne de (4) et (5), mesure cellule par cellule : **6 cellules** ou le nombre EST bien
     ecrit, dans une forme que `NUMWORD` ne liste pas --
       * arabe, 5 cellules : « رزمة **واحدة** » (un paquet -- FEMININ de « واحد ») aux
         Rules_02/11/13/15, « اللاعبين **الأربعة** » (les quatre -- ARTICLE defini) au
         Rules_14 ;
       * russe, 1 cellule : « играют **втроем** или **вчетвером** » (collectif adverbial) au
         Rules_06.
     ⛔ NON REPARE, ET C'EST UN CHOIX. Completer la table par les formes attesteES serait du
     reglage sur le corpus : une autre forme produirait demain le meme faux positif, et une
     table elargie sans preuve commence a avaler de vraies omissions. Le drapeau reste DONC
     LEVE et NOMME : il coute a la lectrice un coup d'oeil par cellule, et il ne cache rien.

CE QUE L'INSTRUMENT TROUVE (et que rien d'autre ne voyait)
----------------------------------------------------------
`PONCT` : le dernier bloc de **Rules_08** et **Rules_15** se termine sans ponctuation finale
en **ar, es, zh, fa** -- 8 cellules. Le FR, l'EN, le ru et le pt y terminent tous par `.` ;
ar finit sur `ن` (R08) / `ة` (R15), es sur `s`, **zh sur `者` / `胜`**, fa sur `د`. C'est la
seule famille reelle que cet ecran leve, et elle est invisible a toute lecture qui ne va pas
jusqu'au dernier caractere du document.

⚠️ Le caractere zh avait d'abord ete consigne `)` -- FAUX, et corrige ici apres re-mesure de
`rstrip()[-1:]` : la cellule finit sur `者` (dernier caractere de « vainqueur »). Lecon : un
caractere cite de memoire n'est pas un caractere mesure, meme quand le COMPTE autour de lui
est juste -- les deux etaient justes (8 cellules), le detail ne l'etait pas.

LE CONTROLE DE CITATION (grain 9 du pool #458, corrige le 04/10 soir)
---------------------------------------------------------------------
Le dossier #1758 citait ses fragments ; au merge, ai-01 a mesure que l'un d'eux -- l'atout
farsi, cite «اتو» -- n'existe NULLE PART dans le CSV : la cellule ecrit «اَتو», AVEC la fatha
(U+064E), 5 fois en Rules_14, 0 fois ailleurs ; la forme nue : 0 fois dans tout le fichier.
Le controle d'alors annoncait « 56 fragments presents, 0 absent » -- il ne comparait donc pas
mot pour mot. Son script n'a pas survécu a la session (scratchpad vide a la reprise), on ne
peut plus dire s'il retirait les voyelles ou si la forme nue n'y figurait pas : les deux
hypoteses restent ouvertes, aucune n'est etablie. Ce qui est etabli : la re-mesure, et le
controle COMMITTE ici, qui compare **mot pour mot** -- aucune normalisation, ni harakat
retires, ni chiffres ramenes a l'ASCII, ni ZWNJ confondu avec l'espace.

La re-mesure a sorti deux autres ecarts de la MEME classe, corriges dans le dossier (§10) :
  * le compte « المغلوطة (×24) » : la mesure donne **22** (8 cellules : 4+3+2+5+5+1+1+1) ;
  * le contraste « 3 یا 4 contre ۳ یا ۴ » : la forme de droite apparait **0 fois** -- le
    contraste reel est `3 یا 4` (Rules_05/06, ASCII) contre les chiffres persans du reste du
    corpus (`۳۲`, `۲۰`...). Le dossier citait une forme ideale, pas une cellule.

⚠️ Les deux formes kaf/keheh coexistent dans le CSV et les DEUX sont presentes : حکm en
KEHEH (U+06A9, ×6 -- l'atout fa, Rules_15) et حكم en KAF (U+0643, ×12 -- le mot arabe
« regle »). Un controle de presence ne peut pas les distinguer : l'entree citee est donc
ecrite en \\uXXXX et le temoin 16 l'epingle.

USAGE
-----
    python docs/corpus/regles-fidelity-instrument.py      # self-test, l'ecran, puis les citations
Le self-test tourne TOUJOURS avant l'ecran : si un temoin tombe, le code de sortie n'est pas
nul et l'ecran ne doit pas etre cite. Le controle de citation court ensuite sur le texte du
CSV ; un ecart (fragment cite absent, ou forme « absente » retrouvee) sort NOMME et met aussi
le code de sortie a 1.
"""
import io, csv, os, re, sys

# Chemin derive du script (reprise 1 du merge #1758, c.5980906972) : la constante absolue
# `D:\Dev\Argumentum\...` ne tenait que sur la machine qui avait ecrit l'outil -- ailleurs il
# plantait apres le self-test (FileNotFoundError). Le script vit a <racine>/docs/corpus/,
# donc le CSV est deux niveaux plus haut.
CSV = os.path.normpath(os.path.join(os.path.dirname(os.path.abspath(__file__)),
                                    os.pardir, os.pardir,
                                    'Cards', 'Rules', 'Argumentum Rules - Cards.csv'))
LANGS = ['en', 'ru', 'pt', 'ar', 'es', 'zh', 'fa']
NUM = re.compile(r'[0-9]+')
TERM = {'ru': set('.!?\u2026\u00bb'), 'pt': set('.!?\u2026\u00bb"'), 'es': set('.!?\u2026\u00bb"'),
        'en': set('.!?\u2026"'), 'ar': set('.!?\u061f\u06d4\u2026'), 'fa': set('.!?\u061f\u2026'),
        'zh': set('\u3002\uff01\uff1f\u2026\u201d\u300f\u300d\uff1a\uff1b')}
# Frontiere de mot par langue (defaut 5). 'zh' est volontairement VIDE : le chinois n'a pas
# de frontiere de mot, la sous-chaine y EST le mot (cf. whole_word()).
LETTERS = {'ru': 'а-яёА-ЯЁ',
           'ar': 'ء-ي',
           'fa': 'ء-يپچژکگی',
           'pt': 'a-zA-ZÀ-ÿ', 'es': 'a-zA-ZÀ-ÿ', 'en': 'a-zA-Z',
           'zh': ''}
# Script attendu par langue (cf. check_scripts). pt, es et en sont ABSENTS a dessein : ils
# s'ecrivent en latin comme le fr, donc le script ne peut pas y decider d'un glissement.
SCRIPT = {'ru': 'Ѐ-ӿ', 'ar': '؀-ۿ', 'fa': '؀-ۿ',
          'zh': '㐀-鿿'}
NUMWORD = {'ru': ['один', 'два', 'три', 'четыре', 'пять', 'шесть', 'семь', 'восемь', 'девять', 'десять'],
           'ar': ['واحد', 'اثنان', 'ثلاثة', 'أربعة', 'خمسة', 'ستة', 'سبعة', 'ثمانية', 'تسعة', 'عشرة'],
           'fa': ['یک', 'دو', 'سه', 'چهار', 'پنج', 'شش', 'هفت', 'هشت', 'نه', 'ده'],
           'zh': ['一', '二', '三', '四', '五', '六', '七', '八', '九', '十'],
           'pt': ['um', 'dois', 'três', 'quatro', 'cinco', 'seis', 'sete', 'oito', 'nove', 'dez'],
           'es': ['uno', 'dos', 'tres', 'cuatro', 'cinco', 'seis', 'siete', 'ocho', 'nueve', 'diez'],
           'en': ['one', 'two', 'three', 'four', 'five', 'six', 'seven', 'eight', 'nine', 'ten']}

# Les fragments PORTANTS cites par le dossier #1758 (§3-§6), re-mesures puis recopies DEPUIS
# LA MESURE (jamais frappes a la main) : True = attendu PRESENT dans le CSV, False = attendu
# ABSENT (forme correcte non appliquee, forme citee a tort, ou temoin inverse). Les caracteres
# invisibles sont ecrits en \uXXXX (ZWNJ) ; le keheh est epingle par le temoin 16.
CITED = [
    # --- ru (dossier §6.1) ---
    ('ые в середине стола.', True, 'fragment orphelin, Rules_11'),
    ('вытазить', True, 'coquille Rules_12'),
    ('вытянуть', False, 'forme correcte, absente du CSV'),
    ('слудующую', True, 'coquille Rules_12'),
    ('следующую', False, 'forme correcte, absente'),
    ('набравший на 20 очков', True, 'le NA superflu, Rules_12'),
    ('набравший 20', False, 'forme sans NA, absente'),
    ('3 семей', True, 'Rules_09/11 -- sous-chaine de семейства, cf dossier §9.1'),
    ('семейства', True, 'Rules_02'),
    ('ложными аргументами', True, 'Rules_14, seule cellule hors софизмы'),
    ('софизм', True, 'x32 ailleurs'),
    ('В комплект входит:', True, 'en-tete Rules_02'),
    ('Потребуется:', True, 'les quatre autres en-tetes'),
    ('Карты Мемо', True, 'Rules_02, liste'),
    ('Карты-памятки', True, 'Rules_02, prose -- deux noms, un document'),
    ('колода из 32 карт', True, 'Rules_13, fusion des deux enonces du FR'),
    ('✅+1🎴', True, 'Rules_06, sequence emoji duelle'),
    ('✅ 1🎴', True, 'les 7 autres langues'),
    # --- pt (dossier §6.2) ---
    ('do esquete', True, 'Rules_05, masculin fautif'),
    ('uma esquete', True, 'Rules_02'),
    ('a esquete', True, 'Rules_04/06'),
    ('Cartas de ajuda-memória', True, 'Rules_11'),
    ('Cartas de ajuda', True, 'Rules_02/09/13'),
    # --- es (dossier §6.3) ---
    ('Cartas recordatorio', True, 'Rules_02/09'),
    ('Cartas de ayuda', True, 'Rules_11'),
    ('Cartas de memo', True, 'Rules_13 -- trois noms, un objet'),
    ('falacias argumentativas', True, 'ilot lexical Rules_07'),
    ('argumento falaz', True, 'partout ailleurs'),
    ('palos de triunfo', True, 'Rules_15'),
    ('colores de triunfo', True, 'Rules_14'),
    ('Empieza a robar', True, 'Rules_12, acteur supprime'),
    # --- ar (dossier §6.4) ---
    ('المغالِطة', True, 'Rules_09 x6 + Rules_13 x5 = 11 -- kasra U+0650 sous le lam'),
    ('المغلوطة', True, '8 cellules, x22 MESUREES (le dossier disait x24, erratum §10)'),
    ('الرصيد', True, 'Rules_06, solde/credit pour la reserve'),
    ('المخزون', True, 'Rules_03, la reserve'),
    ('حزمة', True, 'Rules_09, x4 corpus'),
    ('رزمة', True, 'Rules_09, x7 corpus -- meme paquet, meme document'),
    ('رُزمتان', True, 'Rules_03/09/13, damma U+064F, x3'),
    ('كومتان', True, 'Rules_11, kaf'),
    ('المواد', True, 'en-tete Rules_02'),
    ('المكوّنات', True, 'les quatre autres en-tetes, shadda U+0651, x4'),
    # --- fa (dossier §6.5) ---
    ('اَتو', True, "l'atout fa Rules_14, AVEC fatha U+064E, x5 -- la citation corrigee"),
    ('اتو', False, 'forme NUE citee par le dossier : 0 fois dans le CSV -- erratum §10'),
    ('حکم', True, "l'atout fa Rules_15, KEHEH U+06A9 x6 (pas le kaf U+0643 du mot arabe regle, x12)"),
    ('مغالطی', True, 'Rules_02/13'),
    ('مغالطه‌آمیز', True, 'ZWNJ U+200C entre he et alef-madda, x25'),
    ('مغالطه آمیز', False, 'meme forme AVEC ESPACE au lieu du ZWNJ : 0 fois -- temoin inverse'),
    ('تجهیزات', True, 'en-tete Rules_02'),
    ('محتویات', True, 'les quatre autres en-tetes'),
    ('بسته', True, 'Rules_11, le paquet'),
    ('دسته', True, 'Rules_02/09/13, le paquet ailleurs'),
    ('قرار دهید', True, 'imperatif Rules_13'),
    ('می‌دهیم', True, 'le nous, ZWNJ, x11'),
    ('3 یا 4', True, 'Rules_05/06, chiffres ASCII dans le corps, x2'),
    ('۳ یا ۴', False, 'forme du contraste citee par le dossier : 0 fois -- erratum §10'),
    # --- zh (dossier §6.6 + §3.1) ---
    ('序列', True, 'Rules_09, ordre'),
    ('阶层', True, 'Rules_02/13, ordre ailleurs'),
    ('论证谬误', True, 'compose inverse, Rules_11'),
    ('谬误论证', True, 'partout ailleurs'),
    ('诡辩者', True, 'le role, 27/27 occurrences suivies de 者'),
    ('诡辩', True, '33/27 partage lu par les referents, dossier §3.1'),
]


def check_citations(text, cited):
    """Chaque fragment cite est-il present/absent du CSV, MOT POUR MOT ?

    Aucune normalisation -- c'est le contrat : la fatha de « اَتو », le ZWNJ de
    « مغالطه‌آمیز », le keheh de « حکم », le script des chiffres : tout fait la
    difference, et DOIT la faire. Une citation qui ne survive pas a la comparaison
    character-par-character n'etait pas la citation d'une cellule.

    Retourne la liste des ecarts [(fragment, attendu, obtenu, ou)] ; vide = fidele.
    """
    fails = []
    for frag, want, ou in cited:
        got = frag in text
        if got != want:
            fails.append((frag, want, got, ou))
    return fails


def norm_digits(s):
    """Chiffres arabo-indiens (U+0660-0669) et persans (U+06F0-06F9) -> ASCII (defaut 2)."""
    out = []
    for ch in s:
        o = ord(ch)
        if 0x0660 <= o <= 0x0669:
            out.append(chr(o - 0x0660 + 48))
        elif 0x06F0 <= o <= 0x06F9:
            out.append(chr(o - 0x06F0 + 48))
        else:
            out.append(ch)
    return ''.join(out)


def whole_word(w, v, lg):
    """Le mot-nombre est-il present COMME MOT, pas comme morceau d'un autre mot ?

    Defaut (5), repere apres publication du premier jet -- c'est le seul des cinq qui
    pouvait FAIRE TAIRE un vrai defaut. Le test initial etait `words[n-1] in v.lower()`,
    une sous-chaine nue : en russe, « иллюс**три**рует » (illustre) contient les lettres
    т-р-и, donc le 3 de Rules_06 -- absent en chiffres -- etait reclasse « ecrit en mots »
    et ne sortait plus qu'en `MOT-NOMBRE?3`. Le nombre reellement manquant disparaissait du
    rapport. Mesure : `'три' in v` -> True, frontiere de mot -> False.

    ⛔ Le chinois n'a PAS de frontiere de mot : les caracteres se concatenent, la
    sous-chaine EST le mot, et `\\b`/lookaround y sont inoperants. La classe de lettres est
    donc vide pour `zh` et le test y reste une sous-chaine -- voulu, pas un oubli.
    """
    cls = LETTERS.get(lg, '')
    if not cls:
        return w in v
    return re.search('(?<![%s])%s(?![%s])' % (cls, re.escape(w), cls), v) is not None


def flags_for(v, fr, en, lg):
    """Signaux d'une cellule. `v` = texte de la langue visee (`lg`), ou l'EN."""
    f = []
    if not v.strip():
        return ['VIDE']
    if v.strip() == fr.strip():
        f.append('=FR')
    if lg != 'en' and v.strip() == en.strip():
        f.append('=EN')                      # defaut (1) : jamais pour lg == 'en'
    nfr = set(NUM.findall(norm_digits(fr))) | set(NUM.findall(norm_digits(en)))
    nl = set(NUM.findall(norm_digits(v)))    # defaut (2) : chiffres natifs normalises
    if nfr - nl:
        words = NUMWORD.get(lg, [])
        as_word = [n for n in sorted(nfr - nl)
                   if (n.isdigit() and int(n) - 1 < len(words)
                       and whole_word(words[int(n) - 1], v.lower(), lg))]  # defaut (5)
        # defaut (4) : le duel arabe. Motif restreint a `تان|تين` (nominatif/oblique) ;
        # le duel nu en `ان` est exclu pour ne pas masquer une vraie omission (temoin 9).
        dual = ('2' in (nfr - nl)) and lg == 'ar' and re.search('[ء-ي]{2,}(تان|تين)', v)
        if dual:
            f.append('DUEL?2')
        rest = [n for n in as_word if not (dual and n == '2')]
        if rest:
            f.append('MOT-NOMBRE?%s' % ','.join(rest))   # defaut (3) : table cablee
        leftovers = [n for n in sorted(nfr - nl)
                     if n not in as_word and not (dual and n == '2')]
        if leftovers:
            # faux positif CONNU ici : les collectifs adverbiaux (ru « вчетвером »).
            f.append('CHIFFRE?%s' % ','.join(leftovers))
    if nl - nfr:
        f.append('+CHIFFRE:%s' % ','.join(sorted(nl - nfr)))
    src = fr or en
    if src and src.rstrip()[-1:] in '.!?"' and v.rstrip()[-1:] not in TERM[lg]:
        f.append('PONCT')
    return f


def check_rows(h, data):
    """Une rangee courte n'est inoffensive que si le champ manquant est le DERNIER.

    Mesure 2026-10-04 : **10 des 15 rangees** portent **10 champs pour 11 colonnes** declarees
    (le champ manquant est `variant_class`, en FIN de ligne ; les 5 autres rangees le portent
    et nomment une couverture). `dict(zip(h, r))` aligne alors correctement le PREFIXE, et
    l'ecran lit juste -- c'est un faux positif de forme, pas une corruption.

    ⛔ CE QUE CETTE GARDE NE SAIT **PAS** VOIR, et il faut le dire : une suppression d'UN SEUL
    champ AU MILIEU. La ligne reste « courte d'un », le champ manquant reste le dernier du
    compte, et **les valeurs glissent d'une colonne** -- `Text_ru` porterait le chinois. Le
    comptage ne peut pas l'atteindre : le discriminant est le **script**, et c'est
    `check_scripts()` qui le porte. *Un compteur de champs ne mesure pas une langue.*

    Retourne None si la forme est sure, sinon le message d'erreur.
    """
    for r in data:
        if len(r) == len(h):
            continue
        if len(r) > len(h):
            return "rangee %s : %d champs pour %d colonnes (trop longue)" % (r[0], len(r), len(h))
        manquants = h[len(r):]
        if len(manquants) > 1 or manquants[0] != h[-1]:
            return ("rangee %s : champ manquant AU MILIEU (%s) -- tout ce qui suit est decale"
                    % (r[0], ','.join(manquants)))
    return None


def check_scripts(h, data):
    """Le discriminant du glissement de colonnes : le SCRIPT de chaque cellule.

    Un champ supprime au milieu fait glisser les valeurs d'une position, et le comptage n'y
    voit rien (cf. `check_rows`). Le script, lui, decide -- **pour ru, ar, fa et zh**. Il ne
    decide **pas** pour pt, es et en, qui s'ecrivent en latin comme le fr : c'est une limite
    nommee, pas une couverture supposee.

    Retourne la liste des cellules fautives ('pk.colonne').
    """
    bad = []
    for r in data:
        d = dict(zip(h, r))
        for lg, rng in SCRIPT.items():
            col = 'Text_' + lg
            if col not in d or not d[col].strip():
                continue
            if not re.search('[%s]' % rng, d[col]):
                bad.append('%s.%s' % (r[0], lg))
    return bad


def screen(rows):
    h, data = rows[0], rows[1:]
    err = check_rows(h, data)
    if err:
        print('SHAPE GUARD:', err)
        sys.exit(1)
    slip = check_scripts(h, data)
    for x in slip:
        print('SCRIPT GUARD: %s ne porte pas l\'ecriture attendue (glissement de colonnes ?)' % x)
    if slip:
        sys.exit(1)
    tot = {}
    for r in data:
        d = dict(zip(h, r))
        line = []
        for lg in LANGS:
            f = flags_for(d['Text_' + lg], d['Text'], d['Text_en'], lg)
            if f:
                line.append('%s[%s]' % (lg, ' '.join(f)))
            for x in f:
                tot[x.split(':')[0].split('?')[0]] = tot.get(x.split(':')[0].split('?')[0], 0) + 1
        if line:
            print('%-10s %s' % (d['pk'], '  '.join(line)))
    print()
    print('=== totaux par signal ===', tot)
    print('rangees : %d | langues : %d | cellules : %d'
          % (len(data), len(LANGS), len(data) * len(LANGS)))
    print('remplies :', sum(1 for r in data for lg in LANGS
                            if dict(zip(h, r))['Text_' + lg].strip()))
    print('P&P : 6 rangees (miroir)')
    return tot


def self_test():
    """Mute des litteraux et exige que chaque signal se leve -- et pas a tort.

    Le temoin 9 est le CONTROLE INVERSE du temoin 8 : sans lui, une garde `DUEL?` elargie
    a tout passerait le temoin 8 et resterait verte en avalant les vraies omissions.
    """
    ok = True

    def check(nom, cond, detail):
        nonlocal ok
        if not cond:
            print('SELF-TEST ECHEC (%s): %s' % (nom, detail))
            ok = False

    check('1 =EN/soi', '=EN' not in flags_for('same text.', 'different.', 'same text.', 'en'),
          '=EN leve pour lg=en -- comparaison de soi a soi (defaut 1)')
    check('2 =EN/cible', '=EN' in flags_for('same text.', 'different.', 'same text.', 'ru'),
          '=EN ne se leve plus pour une langue cible')
    check('3 chiffre persan', not any(x.startswith('CHIFFRE?')
          for x in flags_for('il y a ۳ joueurs.', 'il y a 3 joueurs.', 'x', 'fa')),
          'chiffre persan non normalise (defaut 2)')
    check('4 chiffre arabo-indien', not any(x.startswith('CHIFFRE?')
          for x in flags_for('٣', '3', 'x', 'ar')), 'chiffre arabo-indien non normalise (defaut 2)')
    check('5 nombre en mots', any(x.startswith('MOT-NOMBRE?')
          for x in flags_for('три joueurs', '3 joueurs', 'x', 'ru')),
          'nombre en mots non reconnu (defaut 3)')
    check('6 manque reel', any(x.startswith('CHIFFRE?')
          for x in flags_for('aucun nombre ici', '3 joueurs', 'x', 'ru')),
          'manque reel non signale')
    check('7 nombre en trop', any(x.startswith('+CHIFFRE:')
          for x in flags_for('il y a 7 joueurs', '3 joueurs', 'x', 'ru')),
          'nombre en trop non signale')
    k = flags_for('تُشكَّل رُزمتان من بطاقات', '2 piles', 'x', 'ar')
    check('8 duel arabe', any(x.startswith('DUEL?') for x in k)
          and not any(x.startswith('CHIFFRE?') for x in k), 'duel arabe mal classe -> %s' % k)
    m = flags_for('أربعة لاعبين', '2 piles', 'x', 'ar')
    check('9 CONTROLE INVERSE', any(x.startswith('CHIFFRE?') for x in m),
          'garde DUEL? vacue : un manque reel du 2 passe en silence -> %s' % m)
    # 10 : le mot-nombre ne doit pas se reconnaitre DANS un autre mot (defaut 5).
    # « иллюстрирует » contient т-р-и : un test de sous-chaine reclasse le 3 manquant en
    # MOT-NOMBRE? et le fait TAIRE. C'est le controle inverse du temoin 5 (mot reel ->
    # MOT-NOMBRE?) : sans lui, un whole_word() devenu sous-chaine resterait vert.
    n = flags_for('он иллюстрирует карту', '3 joueurs', 'x', 'ru')
    check('10 mot-nombre en sous-chaine', any(x.startswith('CHIFFRE?') for x in n)
          and not any(x.startswith('MOT-NOMBRE?') for x in n),
          'le 3 manquant est masque par un morceau de mot -> %s' % n)
    # 11/12 : la forme des rangees. Un champ manquant EN FIN est inoffensif (zip aligne le
    # prefixe -- mesure : 10 des 15 rangees du corpus sont ainsi) ; il ne faut donc PAS le
    # refuser, sinon la garde rejette le corpus reel. Un manque de PLUSIEURS champs, lui,
    # est refuse. Les deux temoins vont ensemble : accepter le premier sans accepter le second.
    H4 = ['pk', 'Text', 'Text_en', 'variant_class']
    tail = check_rows(H4, [['R1', 'a', 'b']])
    check('11 champ manquant en FIN', tail is None,
          'rangee courte en fin de ligne refusee -> %s' % tail)
    multi = check_rows(H4, [['R1', 'a']])
    check('12 deux champs manquants', bool(multi),
          'un manque de deux champs passe en silence -> %s' % multi)
    # 13 : le glissement de colonnes, que le comptage NE PEUT PAS voir (cf. check_rows).
    # Le script, lui, decide : du chinois dans Text_ru est une faute, pas une variante.
    slip = check_scripts(H4 + ['Text_ru'], [['R1', 'a', 'b', 'c', '中文测试']])
    check('13 glissement de colonnes', len(slip) == 1 and slip[0].endswith('.ru'),
          'un glissement de colonnes passe en silence -> %s' % slip)
    ok_slip = check_scripts(H4 + ['Text_ru'], [['R1', 'a', 'b', 'c', 'русский текст']])
    check('14 pas de faux positif', ok_slip == [],
          'le script attendu est signale a tort -> %s' % ok_slip)
    # 15 : le controle de citation, sur un mini-corpus synthetique. Il doit rendre zero ecart
    # sur des attentes justes -- dont la paire fatha/nue (اَتو present, اتو absent) et la paire
    # ZWNJ/espace -- et NOMMER l'ecart quand une attente est inversee (controle falsifiant :
    # sans lui, un controle toujours-green passerait pour voyant).
    mini = 'abc اَتو مغالطه‌آمیز def'
    ok15 = check_citations(mini, [('اَتو', True, 'a'), ('اتو', False, 'a'),
                                  ('مغالطه‌آمیز', True, 'a'), ('مغالطه آمیز', False, 'a')])
    bad15 = check_citations(mini, [('اتو', True, 'attente inversee')])
    check('15 controle de citation', ok15 == [] and len(bad15) == 1 and bad15[0][0] == 'اتو',
          'le controle de citation ne voit pas la fatha, le ZWNJ, ou ne nomme pas son ecart -> %s / %s'
          % (ok15, bad15))
    # 16 : la fatha du temoin. Si un editeur depouille les harakat du present fichier source,
    # l'entree CITED de l'atout devient la forme nue et ce temoin tombe -- c'est exactement le
    # defaut qu'a mesure ai-01 au merge de #1758, garde ici en epi.
    pairs = [(f, w) for f, w, o in CITED]
    check('16 fatha et keheh epingles',
          ('اَتو', True) in pairs and ('اتو', False) in pairs
          and ('حکم', True) in pairs,
          'CITED ne porte plus la fatha de l atout, sa forme nue, ou son keheh')
    # 17 : le tableau CITED dit ce qu'il mesure -- chaque entree attendue PRESENTE a bien une
    # attente booleenne et une localisation ecrite (une entree mal formee serait muette).
    check('17 entrees CITED bien formees',
          all(isinstance(w, bool) and ou for _, w, ou in CITED) and len(CITED) >= 50,
          'entree CITED mal formee ou tableau ampute : %d entrees' % len(CITED))
    print('SELF-TEST:', 'OK (17 temoins)' if ok else 'ECHEC')
    return ok


if __name__ == '__main__':
    if not self_test():
        sys.exit(1)
    print()
    text = io.open(CSV, encoding='utf-8-sig', newline='').read()
    screen(list(csv.reader(io.StringIO(text))))
    fails = check_citations(text, CITED)
    print('CITATIONS: %d fragments, %d conformes, %d ecarts'
          % (len(CITED), len(CITED) - len(fails), len(fails)))
    for frag, want, got, ou in fails:
        print('  ECART: %r attendu=%s obtenu=%s (%s)'
              % (frag, 'present' if want else 'absent', 'present' if got else 'absent', ou))
    if fails:
        sys.exit(1)
