# #458 — La part VISIBLE du résidu des langpacks, mesurée servie (2026-11-05)

> Pool #458 c.6657549140, grain po-2023 « Langpacks : le résidu visible ».
> Fait suite à [`458-langpack-residu-2026-11-05.md`](458-langpack-residu-2026-11-05.md) (résidu du
> dépôt : 37–105 familles sans jumeau selon la culture), qui **classait par nom de fichier** —
> « console admin pas visible par les visiteurs » y était une assertion. Ici, c'est **mesuré** :
> les valeurs anglaises des familles résiduelles ont été cherchées dans les pages **réellement
> servies** par la préprod.

## Protocole

- **Corpus** : familles résiduelles par culture (neutre sans jumeau `<culture>` dans le dépôt),
  valeurs neutres ≥ 15 caractères avec espace (~60/famille, parseur resx, corps dé-échappé).
- **Surfaces visiteur** : accueil, `terms`, `privacy`, `Règles`, `Login`, `Register`, page 404
  (URL inexistante) — × 7 cultures de pack, préprod `dnn.argumentum.myia.io`, 05/11 ~15:20Z.
- **Hit** = la valeur anglaise exacte d'une famille résiduelle présente dans le corps servi.
- Boutique : `Shop`/`Boutique` = **404** (« Buy the game » suspendu, #1188) — pas de surface.

## Résultat — 222 hits, mais seulement **trois surfaces visibles**

Toutes cultures confondues, le résidu visible se réduit à trois familles de surfaces :

| # | Surface | Chaînes servies en anglais | Où | Qui le voit |
|---|---|---|---|---|
| 1 | **Skip-link du skin** | « Skip to main content » (`SkipLink.MainContent`) | **toutes les pages, les 8 langues y compris fr** — `<a class="…hidden-focusable" href="#to-shine-page-main">` | clavier / lecteurs d'écran (focus) — famille portée par **3 fichiers skin** (`default.ascx.resx`, `2shine.resx`, `landingpage.ascx.resx`) |
| 2 | **Page 404** | « Privacy Statement » (×2 clés) · « See More Results », « No Results Found », « Clear search text » (attributs `data-moreresults`/`data-noresult`/`title` du champ de recherche) | page 404, toutes cultures | tout visiteur qui tombe sur une URL inexistante |
| 3 | **Boutons sociaux Login/Register** | « Login with Facebook », « Register with Facebook », « Sign with your Google Account », « Register with Google+ », « Sign In with Windows Live »… (texte plein) | `/Login` et `/Register` — **pt-BR et ru-RU seulement** | visiteurs pt/ru de ces deux pages — `Login.ascx.resx` est **suivie** dans les 5 autres cultures |

Comptes par culture : **30 hits** (fr, es, zh, ar, fa) · **36 hits** (pt, ru — + les boutons sociaux).

## Ce que la mesure dément ou confirme

- **Console admin** (EditBar, PersonaBar, ResourceManager, CKE) : **0 hit** sur les 7 surfaces —
  l'assertion « invisible aux visiteurs » est maintenant **mesurée**, pas déduite du nom de fichier.
- **NBrightBuy (×25 fichiers, la plus grosse masse du résidu)** : **aucune surface exposée**
  (`Shop`/`Boutique` 404) — ne peut pas fuir aujourd'hui. Le chantier « localiser la boutique ? »
  reste une décision ouverte, mais **rien ne la rend visible avant une page boutique**.
- **pt-BR / ru-RU** confirment leur retard au compteur de familles (105 / 59) : ce sont les deux
  seules cultures où le résidu touche une page de **parcours d'inscription**.

## Lecture pour la mise en ligne

La partie visible du résidu est **petite et énumérable** — environ **une douzaine de chaînes
distinctes** sur trois surfaces — alors que le résidu total compte ~8–9 000 entrées dont
**99 % n'atteignent jamais un visiteur**. Si ai-01 veut un préprod/prod sans fuite anglaise
visible au lancement, le périmètre est : 1 chaîne de skip-link (×3 fichiers skin), ~5 chaînes
de page 404, et ~6-8 chaînes Login/Register pour pt+ru. Décision = ai-01 (ce document mesure,
il ne corrige pas).

## Limites

- Mesuré sur la préprod **au 05/11** (dépôt + packs amont installés le 12/10 — cf. note résidu,
  §3) : ce qui est servi est le mash des deux sources ; re-mesurer après toute installation.
- Instrument : recherche exacte (corps dé-échappé) — une chaîne servie **découpée** par le rendu
  (interpolation, concaténation JS) échapperait ; les trois surfaces trouvées sont des valeurs
  rendues telles quelles, le biais ne peut que **sous**-compter.
- Contrôle inverse : les libellés neutres du Login sont **absents** de `/Login` fr (0) — un zéro
  interprétable (la famille est suivie et servie localisée en fr) ; la capacité de l'instrument à
  voir un 1 est établie par les 222 hits positifs et l'échantillon ciblé 2shine/landingpage.
- ⚠️ Instrument (à retenir) : `curl` **sans `--compressed`** peut recevoir un corps gzippé et
  rendre un faux « absent » — la sonde ciblée a d'abord raté le skip-link pour cette seule raison.

*po-2023*
