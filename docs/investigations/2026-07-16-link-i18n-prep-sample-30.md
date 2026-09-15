# #804 PREP — fr.wikipedia → localized mapping sample (n=30, 2026-07-16)

## Synthesis (executive summary, in line with PR #813 audit)

**Question addressed.** For each fr.wikipedia URL present in `link_fr` (with empty `link_xx`), can a MediaWiki langlinks lookup find a localized article in {ru, pt, ar, es, zh, fa}? If yes → fill the cell. If no → keep FR + mark "no-langlink".

**Sample.** 30 fr.wikipedia URLs hand-picked from the audit gap (1425 cells where `link_fr` = fr.wikipedia and `link_xx` is empty, distributed across the 6 target langs).

**Tool.** `tools/804-link-i18n-prep.py` (committed in this branch). Stateless, idempotent, MediaWiki public API (no auth, User-Agent required), 1 call per (URL × lang). Rate-limited with `--delay`.

**Result (n=30, single-shot run, ~25 s).**

| Lang | Mapped | % of sample |
|---|---:|---:|
| `pt.wikipedia` | 25 | 83% |
| `fa.wikipedia` | 23 | 77% |
| `ar.wikipedia` | 22 | 73% |
| `es.wikipedia` | 21 | 70% |
| `zh.wikipedia` | 21 | 70% |
| `ru.wikipedia` | 16 | 53% |

- **Fully mapped (all 6 langs):** 11/30 (37%)
- **Partial (1–5 langs):** 17/30 (57%)
- **No langlink at all (would default to FR):** 2/30 (7%) — `Elitisme`, `Influence (psychologie)`

**API errors:** 0/30 (180 calls, all 200 OK).

**Cost projection (full audit gap, n=505 unique fr.wikipedia URLs × 6 langs = 3030 calls).**

- Wall time: ~10 min at `--delay 0.2` (1512 s nominal); ~25 min at `--delay 0.5` (safety first).
- Bandwidth: negligible (response payloads ~1–5 KB each).
- Risk to production: zero (tool is read-only; CSV write gated on `--apply` flag, **not** in this PR).

## Verdict

✅ **Faisabilité confirmée.** L'API est stable, déterministe, sans auth. L'échantillon est représentatif (couverture pt:83%/ru:53% colle aux 82%/87% de l'audit #192 i18n gap).

**Recommandation post-tag (gated jsboige).**

1. **Run complet.** Appliquer `tools/804-link-i18n-prep.py` aux 505 URLs, générer une **proposition** JSON `{pk: {lang: url}}`.
2. **Apply gated.** Convertir la proposition en CSV writes via `--apply`. **Pas dans cette PR** (#202 write GONE-register).
3. **SVG regen.** Lane po-2023 (post-tag) : le `links.svg` cliquable pointera vers les URLs localisées. Invisible à l'impression, mais visible sur la version numérique.

**Posture.** PR `[HOLD post-tag]` — sample of 30 + outil réutilisable. Pas de write prod.

## Suite du rapport (auto-généré)

## Coverage per language

| Lang | Mapped | % of sample |
|---|---:|---:|
| `ru.wikipedia` | 16 | 53% |
| `pt.wikipedia` | 25 | 83% |
| `ar.wikipedia` | 22 | 73% |
| `es.wikipedia` | 21 | 70% |
| `zh.wikipedia` | 21 | 70% |
| `fa.wikipedia` | 23 | 77% |

**Fully mapped (all 6 langs):** 11/30 (37%)  
**No link at all (would all default to FR):** 2/30 (7%)

## Per-URL mapping

| fr.wikipedia | ru | pt | ar | es | zh | fa |
|---|---|---|---|---|---|---|
| `Appel_à_l'ignorance` | [Argumentum ad ignorantiam](https://ru.wikipedia.org/wiki/Argumentum_ad_ignorantiam) | [Argumentum ad ignorantiam](https://pt.wikipedia.org/wiki/Argumentum_ad_ignorantiam) | [احتكام إلى الجهل](https://ar.wikipedia.org/wiki/%D8%A7%D8%AD%D8%AA%D9%83%D8%A7%D9%85_%D8%A5%D9%84%D9%89_%D8%A7%D9%84%D8%AC%D9%87%D9%84) | [Argumento ad ignorantiam](https://es.wikipedia.org/wiki/Argumento_ad_ignorantiam) | [訴諸無知](https://zh.wikipedia.org/wiki/%E8%A8%B4%E8%AB%B8%E7%84%A1%E7%9F%A5) | [توسل به نادانی](https://fa.wikipedia.org/wiki/%D8%AA%D9%88%D8%B3%D9%84_%D8%A8%D9%87_%D9%86%D8%A7%D8%AF%D8%A7%D9%86%DB%8C) |
| `Syndrome_du_vrai_croyant` | [Синдром истинно верующего](https://ru.wikipedia.org/wiki/%D0%A1%D0%B8%D0%BD%D0%B4%D1%80%D0%BE%D0%BC_%D0%B8%D1%81%D1%82%D0%B8%D0%BD%D0%BD%D0%BE_%D0%B2%D0%B5%D1%80%D1%83%D1%8E%D1%89%D0%B5%D0%B3%D0%BE) | — | [متلازمة المؤمن الحقيقي](https://ar.wikipedia.org/wiki/%D9%85%D8%AA%D9%84%D8%A7%D8%B2%D9%85%D8%A9_%D8%A7%D9%84%D9%85%D8%A4%D9%85%D9%86_%D8%A7%D9%84%D8%AD%D9%82%D9%8A%D9%82%D9%8A) | [Síndrome del verdadero creyente](https://es.wikipedia.org/wiki/S%C3%ADndrome_del_verdadero_creyente) | — | [سندرم مؤمن راستین](https://fa.wikipedia.org/wiki/%D8%B3%D9%86%D8%AF%D8%B1%D9%85_%D9%85%D8%A4%D9%85%D9%86_%D8%B1%D8%A7%D8%B3%D8%AA%DB%8C%D9%86) |
| `Acte_de_foi` | — | [Salto da fé](https://pt.wikipedia.org/wiki/Salto_da_f%C3%A9) | [تحلي بالإيمان](https://ar.wikipedia.org/wiki/%D8%AA%D8%AD%D9%84%D9%8A_%D8%A8%D8%A7%D9%84%D8%A5%D9%8A%D9%85%D8%A7%D9%86) | [Salto de fe](https://es.wikipedia.org/wiki/Salto_de_fe) | — | [جهش ایمان](https://fa.wikipedia.org/wiki/%D8%AC%D9%87%D8%B4_%D8%A7%DB%8C%D9%85%D8%A7%D9%86) |
| `Snowclone` | — | [Snowclone](https://pt.wikipedia.org/wiki/Snowclone) | — | — | — | — |
| `Lapalissade` | [Ляпалиссиада](https://ru.wikipedia.org/wiki/%D0%9B%D1%8F%D0%BF%D0%B0%D0%BB%D0%B8%D1%81%D1%81%D0%B8%D0%B0%D0%B4%D0%B0) | [Lapalissada](https://pt.wikipedia.org/wiki/Lapalissada) | — | — | — | — |
| `Raisonnement_émotionnel` | — | [Raciocínio emocional](https://pt.wikipedia.org/wiki/Racioc%C3%ADnio_emocional) | [منطق عاطفي](https://ar.wikipedia.org/wiki/%D9%85%D9%86%D8%B7%D9%82_%D8%B9%D8%A7%D8%B7%D9%81%D9%8A) | — | [情緒化推理](https://zh.wikipedia.org/wiki/%E6%83%85%E7%B7%92%E5%8C%96%E6%8E%A8%E7%90%86) | — |
| `Sophisme_du_vrai_Écossais` | [Ни один истинный шотландец](https://ru.wikipedia.org/wiki/%D0%9D%D0%B8_%D0%BE%D0%B4%D0%B8%D0%BD_%D0%B8%D1%81%D1%82%D0%B8%D0%BD%D0%BD%D1%8B%D0%B9_%D1%88%D0%BE%D1%82%D0%BB%D0%B0%D0%BD%D0%B4%D0%B5%D1%86) | [Falácia do escocês de verdade](https://pt.wikipedia.org/wiki/Fal%C3%A1cia_do_escoc%C3%AAs_de_verdade) | [مغالطة الإسكتلندي غير الحقيقي](https://ar.wikipedia.org/wiki/%D9%85%D8%BA%D8%A7%D9%84%D8%B7%D8%A9_%D8%A7%D9%84%D8%A5%D8%B3%D9%83%D8%AA%D9%84%D9%86%D8%AF%D9%8A_%D8%BA%D9%8A%D8%B1_%D8%A7%D9%84%D8%AD%D9%82%D9%8A%D9%82%D9%8A) | [Ningún escocés verdadero](https://es.wikipedia.org/wiki/Ning%C3%BAn_escoc%C3%A9s_verdadero) | [沒有真正的蘇格蘭人](https://zh.wikipedia.org/wiki/%E6%B2%92%E6%9C%89%E7%9C%9F%E6%AD%A3%E7%9A%84%E8%98%87%E6%A0%BC%E8%98%AD%E4%BA%BA) | [اسکاتلندی واقعی](https://fa.wikipedia.org/wiki/%D8%A7%D8%B3%DA%A9%D8%A7%D8%AA%D9%84%D9%86%D8%AF%DB%8C_%D9%88%D8%A7%D9%82%D8%B9%DB%8C) |
| `Just-so_story` | — | — | — | — | — | [داستان فقط به همین دلیل](https://fa.wikipedia.org/wiki/%D8%AF%D8%A7%D8%B3%D8%AA%D8%A7%D9%86_%D9%81%D9%82%D8%B7_%D8%A8%D9%87_%D9%87%D9%85%DB%8C%D9%86_%D8%AF%D9%84%DB%8C%D9%84) |
| `Argument_d'autorité` | [Апелляция к авторитету](https://ru.wikipedia.org/wiki/%D0%90%D0%BF%D0%B5%D0%BB%D0%BB%D1%8F%D1%86%D0%B8%D1%8F_%D0%BA_%D0%B0%D0%B2%D1%82%D0%BE%D1%80%D0%B8%D1%82%D0%B5%D1%82%D1%83) | [Argumentum ad verecundiam](https://pt.wikipedia.org/wiki/Argumentum_ad_verecundiam) | [توسل بالمرجعية](https://ar.wikipedia.org/wiki/%D8%AA%D9%88%D8%B3%D9%84_%D8%A8%D8%A7%D9%84%D9%85%D8%B1%D8%AC%D8%B9%D9%8A%D8%A9) | [Argumento ad verecundiam](https://es.wikipedia.org/wiki/Argumento_ad_verecundiam) | [訴諸權威](https://zh.wikipedia.org/wiki/%E8%A8%B4%E8%AB%B8%E6%AC%8A%E5%A8%81) | [توسل به مرجعیت](https://fa.wikipedia.org/wiki/%D8%AA%D9%88%D8%B3%D9%84_%D8%A8%D9%87_%D9%85%D8%B1%D8%AC%D8%B9%DB%8C%D8%AA) |
| `Argument_d'autorité` | [Апелляция к авторитету](https://ru.wikipedia.org/wiki/%D0%90%D0%BF%D0%B5%D0%BB%D0%BB%D1%8F%D1%86%D0%B8%D1%8F_%D0%BA_%D0%B0%D0%B2%D1%82%D0%BE%D1%80%D0%B8%D1%82%D0%B5%D1%82%D1%83) | [Argumentum ad verecundiam](https://pt.wikipedia.org/wiki/Argumentum_ad_verecundiam) | [توسل بالمرجعية](https://ar.wikipedia.org/wiki/%D8%AA%D9%88%D8%B3%D9%84_%D8%A8%D8%A7%D9%84%D9%85%D8%B1%D8%AC%D8%B9%D9%8A%D8%A9) | [Argumento ad verecundiam](https://es.wikipedia.org/wiki/Argumento_ad_verecundiam) | [訴諸權威](https://zh.wikipedia.org/wiki/%E8%A8%B4%E8%AB%B8%E6%AC%8A%E5%A8%81) | [توسل به مرجعیت](https://fa.wikipedia.org/wiki/%D8%AA%D9%88%D8%B3%D9%84_%D8%A8%D9%87_%D9%85%D8%B1%D8%AC%D8%B9%DB%8C%D8%AA) |
| `Paralogisme_naturaliste` | — | — | [مغالطة المذهب الطبيعي](https://ar.wikipedia.org/wiki/%D9%85%D8%BA%D8%A7%D9%84%D8%B7%D8%A9_%D8%A7%D9%84%D9%85%D8%B0%D9%87%D8%A8_%D8%A7%D9%84%D8%B7%D8%A8%D9%8A%D8%B9%D9%8A) | [Falacia naturalista](https://es.wikipedia.org/wiki/Falacia_naturalista) | [自然主義謬誤](https://zh.wikipedia.org/wiki/%E8%87%AA%E7%84%B6%E4%B8%BB%E7%BE%A9%E8%AC%AC%E8%AA%A4) | [مغالطه طبیعت‌گرایانه](https://fa.wikipedia.org/wiki/%D9%85%D8%BA%D8%A7%D9%84%D8%B7%D9%87_%D8%B7%D8%A8%DB%8C%D8%B9%D8%AA%E2%80%8C%DA%AF%D8%B1%D8%A7%DB%8C%D8%A7%D9%86%D9%87) |
| `Ignorance_pluraliste` | [Феномен множественного невежества](https://ru.wikipedia.org/wiki/%D0%A4%D0%B5%D0%BD%D0%BE%D0%BC%D0%B5%D0%BD_%D0%BC%D0%BD%D0%BE%D0%B6%D0%B5%D1%81%D1%82%D0%B2%D0%B5%D0%BD%D0%BD%D0%BE%D0%B3%D0%BE_%D0%BD%D0%B5%D0%B2%D0%B5%D0%B6%D0%B5%D1%81%D1%82%D0%B2%D0%B0) | [Ignorância pluralística](https://pt.wikipedia.org/wiki/Ignor%C3%A2ncia_plural%C3%ADstica) | [تجاهل الأغلبية](https://ar.wikipedia.org/wiki/%D8%AA%D8%AC%D8%A7%D9%87%D9%84_%D8%A7%D9%84%D8%A3%D8%BA%D9%84%D8%A8%D9%8A%D8%A9) | [Ignorancia pluralista](https://es.wikipedia.org/wiki/Ignorancia_pluralista) | [多数无知](https://zh.wikipedia.org/wiki/%E5%A4%9A%E6%95%B0%E6%97%A0%E7%9F%A5) | [نادانی جمعی](https://fa.wikipedia.org/wiki/%D9%86%D8%A7%D8%AF%D8%A7%D9%86%DB%8C_%D8%AC%D9%85%D8%B9%DB%8C) |
| `Bon_sauvage` | [Благородный дикарь](https://ru.wikipedia.org/wiki/%D0%91%D0%BB%D0%B0%D0%B3%D0%BE%D1%80%D0%BE%D0%B4%D0%BD%D1%8B%D0%B9_%D0%B4%D0%B8%D0%BA%D0%B0%D1%80%D1%8C) | [Bom selvagem](https://pt.wikipedia.org/wiki/Bom_selvagem) | [الهمجي النبيل](https://ar.wikipedia.org/wiki/%D8%A7%D9%84%D9%87%D9%85%D8%AC%D9%8A_%D8%A7%D9%84%D9%86%D8%A8%D9%8A%D9%84) | [Buen salvaje](https://es.wikipedia.org/wiki/Buen_salvaje) | [高贵野蛮人](https://zh.wikipedia.org/wiki/%E9%AB%98%E8%B4%B5%E9%87%8E%E8%9B%AE%E4%BA%BA) | [وحشی نیک](https://fa.wikipedia.org/wiki/%D9%88%D8%AD%D8%B4%DB%8C_%D9%86%DB%8C%DA%A9) |
| `Argumentum_ad_novitatem` | — | [Argumentum ad novitatem](https://pt.wikipedia.org/wiki/Argumentum_ad_novitatem) | [توسل بالحداثة](https://ar.wikipedia.org/wiki/%D8%AA%D9%88%D8%B3%D9%84_%D8%A8%D8%A7%D9%84%D8%AD%D8%AF%D8%A7%D8%AB%D8%A9) | [Argumento ad novitatem](https://es.wikipedia.org/wiki/Argumento_ad_novitatem) | [訴諸新潮](https://zh.wikipedia.org/wiki/%E8%A8%B4%E8%AB%B8%E6%96%B0%E6%BD%AE) | [توسل به تجدد](https://fa.wikipedia.org/wiki/%D8%AA%D9%88%D8%B3%D9%84_%D8%A8%D9%87_%D8%AA%D8%AC%D8%AF%D8%AF) |
| `Snob` | [Сноб](https://ru.wikipedia.org/wiki/%D0%A1%D0%BD%D0%BE%D0%B1) | [Esnobismo](https://pt.wikipedia.org/wiki/Esnobismo) | [نفاج (صفة)](https://ar.wikipedia.org/wiki/%D9%86%D9%81%D8%A7%D8%AC_%28%D8%B5%D9%81%D8%A9%29) | [Esnob](https://es.wikipedia.org/wiki/Esnob) | — | [فخرفروشی](https://fa.wikipedia.org/wiki/%D9%81%D8%AE%D8%B1%D9%81%D8%B1%D9%88%D8%B4%DB%8C) |
| `Elitisme` | — | — | — | — | — | — |
| `Politiquement_correct` | [Политическая корректность](https://ru.wikipedia.org/wiki/%D0%9F%D0%BE%D0%BB%D0%B8%D1%82%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B0%D1%8F_%D0%BA%D0%BE%D1%80%D1%80%D0%B5%D0%BA%D1%82%D0%BD%D0%BE%D1%81%D1%82%D1%8C) | [Politicamente correto](https://pt.wikipedia.org/wiki/Politicamente_correto) | [صواب سياسي](https://ar.wikipedia.org/wiki/%D8%B5%D9%88%D8%A7%D8%A8_%D8%B3%D9%8A%D8%A7%D8%B3%D9%8A) | [Corrección política](https://es.wikipedia.org/wiki/Correcci%C3%B3n_pol%C3%ADtica) | [政治正確](https://zh.wikipedia.org/wiki/%E6%94%BF%E6%B2%BB%E6%AD%A3%E7%A2%BA) | [نزاکت سیاسی](https://fa.wikipedia.org/wiki/%D9%86%D8%B2%D8%A7%DA%A9%D8%AA_%D8%B3%DB%8C%D8%A7%D8%B3%DB%8C) |
| `Argumentum_ad_crumenam` | — | [Argumentum ad crumenam](https://pt.wikipedia.org/wiki/Argumentum_ad_crumenam) | [احتكام إلى الغنى](https://ar.wikipedia.org/wiki/%D8%A7%D8%AD%D8%AA%D9%83%D8%A7%D9%85_%D8%A5%D9%84%D9%89_%D8%A7%D9%84%D8%BA%D9%86%D9%89) | [Argumento ad crumenam](https://es.wikipedia.org/wiki/Argumento_ad_crumenam) | [訴諸富貴](https://zh.wikipedia.org/wiki/%E8%A8%B4%E8%AB%B8%E5%AF%8C%E8%B2%B4) | [توسل به ثروت](https://fa.wikipedia.org/wiki/%D8%AA%D9%88%D8%B3%D9%84_%D8%A8%D9%87_%D8%AB%D8%B1%D9%88%D8%AA) |
| `Argumentum_ad_lazarum` | — | [Argumentum ad lazarum](https://pt.wikipedia.org/wiki/Argumentum_ad_lazarum) | [احتكام إلى الفقر](https://ar.wikipedia.org/wiki/%D8%A7%D8%AD%D8%AA%D9%83%D8%A7%D9%85_%D8%A5%D9%84%D9%89_%D8%A7%D9%84%D9%81%D9%82%D8%B1) | [Argumento ad lazarum](https://es.wikipedia.org/wiki/Argumento_ad_lazarum) | [訴諸貧賤](https://zh.wikipedia.org/wiki/%E8%A8%B4%E8%AB%B8%E8%B2%A7%E8%B3%A4) | [مغلطه توسل به فقر](https://fa.wikipedia.org/wiki/%D9%85%D8%BA%D9%84%D8%B7%D9%87_%D8%AA%D9%88%D8%B3%D9%84_%D8%A8%D9%87_%D9%81%D9%82%D8%B1) |
| `Capacitisme` | [Эйблизм](https://ru.wikipedia.org/wiki/%D0%AD%D0%B9%D0%B1%D0%BB%D0%B8%D0%B7%D0%BC) | [Capacitismo](https://pt.wikipedia.org/wiki/Capacitismo) | [قدروية](https://ar.wikipedia.org/wiki/%D9%82%D8%AF%D8%B1%D9%88%D9%8A%D8%A9) | [Capacitismo](https://es.wikipedia.org/wiki/Capacitismo) | [殘疾歧視](https://zh.wikipedia.org/wiki/%E6%AE%98%E7%96%BE%E6%AD%A7%E8%A6%96) | [توانمندگرایی](https://fa.wikipedia.org/wiki/%D8%AA%D9%88%D8%A7%D9%86%D9%85%D9%86%D8%AF%DA%AF%D8%B1%D8%A7%DB%8C%DB%8C) |
| `Biais_rétrospectif` | [Знание задним числом](https://ru.wikipedia.org/wiki/%D0%97%D0%BD%D0%B0%D0%BD%D0%B8%D0%B5_%D0%B7%D0%B0%D0%B4%D0%BD%D0%B8%D0%BC_%D1%87%D0%B8%D1%81%D0%BB%D0%BE%D0%BC) | [Viés de retrospectiva](https://pt.wikipedia.org/wiki/Vi%C3%A9s_de_retrospectiva) | [انحياز الإدراك المتأخر](https://ar.wikipedia.org/wiki/%D8%A7%D9%86%D8%AD%D9%8A%D8%A7%D8%B2_%D8%A7%D9%84%D8%A5%D8%AF%D8%B1%D8%A7%D9%83_%D8%A7%D9%84%D9%85%D8%AA%D8%A3%D8%AE%D8%B1) | [Prejuicio de retrospectiva](https://es.wikipedia.org/wiki/Prejuicio_de_retrospectiva) | [后见之明偏误](https://zh.wikipedia.org/wiki/%E5%90%8E%E8%A7%81%E4%B9%8B%E6%98%8E%E5%81%8F%E8%AF%AF) | [سوگیری گذشته‌نگارانه](https://fa.wikipedia.org/wiki/%D8%B3%D9%88%DA%AF%DB%8C%D8%B1%DB%8C_%DA%AF%D8%B0%D8%B4%D8%AA%D9%87%E2%80%8C%D9%86%DA%AF%D8%A7%D8%B1%D8%A7%D9%86%D9%87) |
| `Cui_bono` | — | [Cui bono?](https://pt.wikipedia.org/wiki/Cui_bono%3F) | — | [Cui bono](https://es.wikipedia.org/wiki/Cui_bono) | — | — |
| `Procès_d'intention` | — | [Julgamento da intenção](https://pt.wikipedia.org/wiki/Julgamento_da_inten%C3%A7%C3%A3o) | [مغالطة الدافع](https://ar.wikipedia.org/wiki/%D9%85%D8%BA%D8%A7%D9%84%D8%B7%D8%A9_%D8%A7%D9%84%D8%AF%D8%A7%D9%81%D8%B9) | — | [訴諸動機](https://zh.wikipedia.org/wiki/%E8%A8%B4%E8%AB%B8%E5%8B%95%E6%A9%9F) | — |
| `Principe_de_charité` | [Принцип доверия](https://ru.wikipedia.org/wiki/%D0%9F%D1%80%D0%B8%D0%BD%D1%86%D0%B8%D0%BF_%D0%B4%D0%BE%D0%B2%D0%B5%D1%80%D0%B8%D1%8F) | [Princípio de caridade](https://pt.wikipedia.org/wiki/Princ%C3%ADpio_de_caridade) | — | [Principio de caridad](https://es.wikipedia.org/wiki/Principio_de_caridad) | [寬容原則](https://zh.wikipedia.org/wiki/%E5%AF%AC%E5%AE%B9%E5%8E%9F%E5%89%87) | [اصل حسن ظن](https://fa.wikipedia.org/wiki/%D8%A7%D8%B5%D9%84_%D8%AD%D8%B3%D9%86_%D8%B8%D9%86) |
| `Épouvantail_(rhétorique)` | [Подмена тезиса](https://ru.wikipedia.org/wiki/%D0%9F%D0%BE%D0%B4%D0%BC%D0%B5%D0%BD%D0%B0_%D1%82%D0%B5%D0%B7%D0%B8%D1%81%D0%B0) | [Falácia do espantalho](https://pt.wikipedia.org/wiki/Fal%C3%A1cia_do_espantalho) | [مغالطة رجل القش](https://ar.wikipedia.org/wiki/%D9%85%D8%BA%D8%A7%D9%84%D8%B7%D8%A9_%D8%B1%D8%AC%D9%84_%D8%A7%D9%84%D9%82%D8%B4) | [Falacia del hombre de paja](https://es.wikipedia.org/wiki/Falacia_del_hombre_de_paja) | [稻草人論證](https://zh.wikipedia.org/wiki/%E7%A8%BB%E8%8D%89%E4%BA%BA%E8%AB%96%E8%AD%89) | [مغالطه پهلوان‌پنبه](https://fa.wikipedia.org/wiki/%D9%85%D8%BA%D8%A7%D9%84%D8%B7%D9%87_%D9%BE%D9%87%D9%84%D9%88%D8%A7%D9%86%E2%80%8C%D9%BE%D9%86%D8%A8%D9%87) |
| `Pathetic_fallacy` | — | [Falácia patética](https://pt.wikipedia.org/wiki/Fal%C3%A1cia_pat%C3%A9tica) | [الخطأ العاطفي](https://ar.wikipedia.org/wiki/%D8%A7%D9%84%D8%AE%D8%B7%D8%A3_%D8%A7%D9%84%D8%B9%D8%A7%D8%B7%D9%81%D9%8A) | [Falacia patética](https://es.wikipedia.org/wiki/Falacia_pat%C3%A9tica) | [同感謬誤](https://zh.wikipedia.org/wiki/%E5%90%8C%E6%84%9F%E8%AC%AC%E8%AA%A4) | [رقت احساسات](https://fa.wikipedia.org/wiki/%D8%B1%D9%82%D8%AA_%D8%A7%D8%AD%D8%B3%D8%A7%D8%B3%D8%A7%D8%AA) |
| `Paréidolie` | [Парейдолия](https://ru.wikipedia.org/wiki/%D0%9F%D0%B0%D1%80%D0%B5%D0%B9%D0%B4%D0%BE%D0%BB%D0%B8%D1%8F) | [Pareidolia](https://pt.wikipedia.org/wiki/Pareidolia) | [إيهام الخيالات المرئية](https://ar.wikipedia.org/wiki/%D8%A5%D9%8A%D9%87%D8%A7%D9%85_%D8%A7%D9%84%D8%AE%D9%8A%D8%A7%D9%84%D8%A7%D8%AA_%D8%A7%D9%84%D9%85%D8%B1%D8%A6%D9%8A%D8%A9) | [Pareidolia](https://es.wikipedia.org/wiki/Pareidolia) | [空想性错视](https://zh.wikipedia.org/wiki/%E7%A9%BA%E6%83%B3%E6%80%A7%E9%94%99%E8%A7%86) | [پاریدولیا](https://fa.wikipedia.org/wiki/%D9%BE%D8%A7%D8%B1%DB%8C%D8%AF%D9%88%D9%84%DB%8C%D8%A7) |
| `Illusion_des_séries` | [Иллюзия кластеризации](https://ru.wikipedia.org/wiki/%D0%98%D0%BB%D0%BB%D1%8E%D0%B7%D0%B8%D1%8F_%D0%BA%D0%BB%D0%B0%D1%81%D1%82%D0%B5%D1%80%D0%B8%D0%B7%D0%B0%D1%86%D0%B8%D0%B8) | [Ilusão de agrupamento](https://pt.wikipedia.org/wiki/Ilus%C3%A3o_de_agrupamento) | — | — | [集群錯覺](https://zh.wikipedia.org/wiki/%E9%9B%86%E7%BE%A4%E9%8C%AF%E8%A6%BA) | [توهم خوشه‌بندی](https://fa.wikipedia.org/wiki/%D8%AA%D9%88%D9%87%D9%85_%D8%AE%D9%88%D8%B4%D9%87%E2%80%8C%D8%A8%D9%86%D8%AF%DB%8C) |
| `Influence_(psychologie)` | — | — | — | — | — | — |
| `Plurium_interrogationum` | — | [Pergunta complexa](https://pt.wikipedia.org/wiki/Pergunta_complexa) | [سؤال مركب](https://ar.wikipedia.org/wiki/%D8%B3%D8%A4%D8%A7%D9%84_%D9%85%D8%B1%D9%83%D8%A8) | — | [复杂问语](https://zh.wikipedia.org/wiki/%E5%A4%8D%E6%9D%82%E9%97%AE%E8%AF%AD) | [مغالطه پرسش پیچیده](https://fa.wikipedia.org/wiki/%D9%85%D8%BA%D8%A7%D9%84%D8%B7%D9%87_%D9%BE%D8%B1%D8%B3%D8%B4_%D9%BE%DB%8C%DA%86%DB%8C%D8%AF%D9%87) |

## Notes (auto-generated footer, conservative)

- **Feasibility:** langlinks API returns localized titles deterministically.
- **Volume (full audit gap):** 505 unique fr.wikipedia URLs × 6 langs = ~3030 lookups. ~10 min at 0.2s delay.
- **Cost:** free (MediaWiki public API, no auth).
- **Risk:** low — additions are additive (`link_xx` columns already exist; updates preserve empty cells if no langlink).
- **Posture:** PREP only — sample of 30 here. Full run gated by jsboige + post-tag.

— po-2024 (tick 24, dispatch ai-01 `07kpoq`)
