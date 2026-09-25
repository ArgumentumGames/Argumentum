# -*- coding: utf-8 -*-
"""#1499 G4-C-W — garde de la décision Q-13 : PK 94 « Validité formelle », description ×8.

Décision owner Q-13 (« Q13 OK », c.5816834208) : la nuance « valide ≠ vrai » entre dans
`description_<lang>` ×8 — PAS dans remark (la remark remplit l'emplacement de l'exemple
dans le gabarit Virtues ; elle reste la règle modus ponens inchangée).

La description doit porter les DEUX hémistiches :
  (1) évaluation selon la forme logique, indépendamment du contenu ;
  (2) le transfert de vérité : ce qui est conditionné est la GARANTIE qu'apporte la
      forme valide — « ne garantit une conclusion vraie que si les prémisses le sont »
      (condition SUFFISANTE sur la garantie). Jamais une condition NÉCESSAIRE sur la
      vérité de la conclusion (« la conclusion n'est vraie que si… »), qui est FAUSSE :
      « Tous les chats sont des chiens ; tous les chiens sont des mammifères ; donc
      tous les chats sont des mammifères » est valide, a une prémisse fausse et une
      conclusion vraie. La v1 portait cette erreur ; rejetée par la revue ai-01
      (pulls/1540/reviews, 2026-09-24T19:59Z), corrigée ici en v2.

TÉMOINS À CHAQUE RUN
  (a) en-têtes : `pk` + `description_{fr,en,ru,pt,ar,es,fa,zh}` + `remark_fr` présents
      (défaut M2 : `r.get(col)` rend None sur colonne renommée → faux vert) ;
  (b) PK 94 présent, unique ;
  (c) les 8 descriptions ÉGALES aux textes attendus ci-dessous (garde d'égalité,
      pas de sous-chaîne — un fragment qui passe n'est pas un sélecteur, #1138) ;
  (d) remark_fr de PK 94 inchangée (la règle « Si A alors B ; A ; donc B ») ;
  (e) longueur ≤ 158 (plafond mesuré dispatch ; re-mesure fraîche 2026-09-24 :
      max 243, p90 135, médiane 95).

--self-test : 5 mutations in-memory, rc attendu [0, 2, 2, 2, 2]
  (1) sain                     -> 0
  (2) description_fr renommée  -> 2 (header guard, défaut M2)
  (3) description_fr revertée  -> 2 (valeur 2024 courte)
  (4) remark_fr mutée          -> 2
  (5) desc_fr = v1 (fausse)    -> 2 (condition nécessaire sur la vérité ; la garde
                                     distingue v1 de v2 — sinon elle ne prouve rien)
"""
import csv
import io
import os
import sys

BASE = os.path.abspath(os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', '..'))
CSV_PATH = 'Cards/Fallacies/Argumentum Virtues - Taxonomy.csv'
LANGS = ['fr', 'en', 'ru', 'pt', 'ar', 'es', 'fa', 'zh']
REQUIRED = {'pk', 'remark_fr'} | {'description_' + l for l in LANGS}

EXPECTED_DESC = {
    'fr': "Évaluer un raisonnement selon sa forme logique, indépendamment du contenu ; une forme valide ne garantit une conclusion vraie que si les prémisses le sont.",
    'en': "Evaluating reasoning according to its logical form, regardless of content; a valid form guarantees a true conclusion only if its premises are true.",
    'ru': "Оценивать рассуждение по его логической форме, независимо от содержания; валидная форма гарантирует истинное заключение только при истинности посылок.",
    'pt': "Avaliar um raciocínio segundo sua forma lógica, independentemente do conteúdo; uma forma válida só garante uma conclusão verdadeira se as premissas o forem.",
    'es': "Evaluar un razonamiento según su forma lógica, independientemente del contenido; la validez solo garantiza una conclusión verdadera con premisas verdaderas.",
    'ar': "تقييم الاستدلال بحسب صورته المنطقية، بمعزل عن محتواه؛ الصورة الصحيحة لا تضمن استنتاجًا صحيحًا إلا إذا كانت المقدمات صحيحة.",
    'fa': "ارزیابی استدلال بر پایهٔ صورت منطقی آن، مستقل از محتوا؛ صورت معتبر تنها در صورتی نتیجهٔ صادق را تضمین می‌کند که مقدماتش صادق باشند.",
    'zh': "依据推理的逻辑形式对其有效性进行评估，而不考虑其内容；有效的形式唯有在前提为真时才保证结论为真。",
}
EXPECTED_V1_FR = "Évaluer un raisonnement selon sa forme logique, indépendamment du contenu ; une conclusion n’est vraie que si les prémisses le sont."
EXPECTED_REMARK_FR = "Même avec des termes absurdes, la forme « Si A alors B ; A ; donc B » reste valide."
CEILING = 158


def rows_from_file():
    p = os.path.join(BASE, CSV_PATH)
    if not os.path.isfile(p):
        sys.exit('illisible: %s' % p)
    with open(p, encoding='utf-8-sig', newline='') as f:
        return list(csv.DictReader(f))


def check(data):
    """Retourne (ok, lignes de rapport). data = liste de dicts."""
    out = []
    ok = True

    headers = set(data[0].keys()) if data else set()
    missing = sorted(REQUIRED - headers)
    a = not missing
    out.append('(a) en-têtes requis présents -> %s%s'
               % ('PASS' if a else 'FAIL : %s' % missing, '' if a else ''))
    ok &= a

    pk94 = [r for r in data if (r.get('pk') or '').strip() == '94']
    b = len(pk94) == 1
    out.append('(b) PK 94 unique -> %s (%d occurrence(s))' % ('PASS' if b else 'FAIL', len(pk94)))
    ok &= b
    if not b:
        return ok, out
    r = pk94[0]

    for l in LANGS:
        got = (r.get('description_' + l) or '').strip()
        c = got == EXPECTED_DESC[l]
        out.append('(c) desc_%-2s (%3d car.) égalité exacte -> %s'
                   % (l, len(got), 'PASS' if c else 'FAIL'))
        if not c:
            out.append('      attendu : %r' % EXPECTED_DESC[l][:80])
            out.append('      recu   : %r' % got[:80])
        ok &= c

    d = (r.get('remark_fr') or '').strip() == EXPECTED_REMARK_FR
    out.append('(d) remark_fr inchangée -> %s' % ('PASS' if d else 'FAIL (la remark remplit '
               + 'l\'emplacement exemple — Q-13 la laisse en place)'))
    ok &= d

    over = [l for l in LANGS if len((r.get('description_' + l) or '').strip()) > CEILING]
    e = not over
    out.append('(e) longueur ≤ %d -> %s' % (CEILING, 'PASS' if e else 'FAIL : %s' % over))
    ok &= e
    return ok, out


def main():
    argv = sys.argv[1:]
    if '--self-test' in argv:
        return run_self_test()
    data = rows_from_file()
    ok, out = check(data)
    print('\n'.join(out))
    print('[G4-C-W PK 94] %s' % ('PASS' if ok else 'FAIL'))
    return 0 if ok else 2


def run_self_test():
    """4 mutations in-memory, rc attendu [0, 2, 2, 2] — prouve que la garde
    réagit dans les deux sens sans dépendre du CSV courant."""
    src = rows_from_file()
    hdr = list(src[0].keys())

    def clone():
        return [dict(r) for r in src]

    cases = []
    cases.append(('sain', clone(), 0))

    rows2 = clone()
    rows2[0].pop('description_fr')
    cases.append(('desc_fr colonne absente', rows2, 2))

    rows3 = clone()
    for r in rows3:
        if (r.get('pk') or '').strip() == '94':
            r['description_fr'] = "Évaluer un raisonnement selon sa forme logique, indépendamment de son contenu."
    cases.append(('desc_fr revertee 2024', rows3, 2))

    rows4 = clone()
    for r in rows4:
        if (r.get('pk') or '').strip() == '94':
            r['remark_fr'] = 'MUTÉ'
    cases.append(('remark_fr mutee', rows4, 2))

    rows5 = clone()
    for r in rows5:
        if (r.get('pk') or '').strip() == '94':
            r['description_fr'] = EXPECTED_V1_FR
    cases.append(('desc_fr v1 (fausse)', rows5, 2))

    failures = []
    for label, data, expected in cases:
        ok, out = check(data)
        rc = 0 if ok else 2
        good = rc == expected
        print('  [%s] %-24s attendu rc=%d, recu rc=%d' % ('PASS' if good else 'FAIL', label, expected, rc))
        if not good:
            failures.append(label)
    if failures:
        print('[SELF-TEST] FAIL (%s)' % ', '.join(failures))
        return 2
    print('[SELF-TEST] PASS (%d/%d) — la garde parle dans les deux sens' % (len(cases), len(cases)))
    return 0


if __name__ == '__main__':
    sys.exit(main())
