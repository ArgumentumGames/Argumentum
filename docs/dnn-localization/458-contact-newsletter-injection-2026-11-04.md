# #458 — Contact + newsletter : injection EAV et preuve servie (2026-11-04)

Dispatch : `ai01-dispatch-po2023-20261104a` + #458 c.6618545936. Périmètre : 25 valeurs
par culture × 7 cultures = 175 cellules — 24 libellés `App-Resources` de l'app 50 (entité
E9169, GUID `2231FDC4-3838-4F38-90B2-B29F2054B892`, la seule qui alimente le formulaire
rendu) + 1 `Html` de l'app 48 (E10181). Méthode RuleMemo/E1 (26/10). Le pivot
(`eav-content.csv`) n'est pas modifié. ⛔ Aucune autre entité ; aucune table membres,
commandes ou comptes.

## 1. Newsletter (E10181, app 48) — déjà injectée, constat

En arrivant sur le grain, E10181 portait **déjà** ses 7 rangées dimensionnées
(`ValueId` 29533–29539, geste ~27/10, **non attribué** — aucun rapport #458 ne le
revendique). Vérifié registre↔DB 8/8 byte-identical (le prétendu « drift pt » était un
faux écart instrument : hash recopié à la main — re-dump direct `-y 0` a prouvé l'égalité).

Conséquence de sondage : une sonde **au niveau page** mélange les magasins — l'iframe
Brevo (E10169) est française **par conception** dans toutes les cultures et n'est pas
alimentée par E10181. Toute contre-preuve doit borner son extraction aux blocs
`data-accordion-child="10172"` (newsletter) et `data-accordion-child="10182"` (contact).

## 2. Contact (E9169, app 50) — pré-état et gardes

- 41 valeurs de base (dim-0), **0** rangée dimensionnée (anti-rejeu).
- Garde miroir base↔registre fr, mesurée avec comparaison **byte-faithful** (`-ceq`,
  pas `-eq` — PowerShell `-eq` est insensible à la casse et lisait 16/1/7 en 16/3/5) :
  **fr = 16 / en = 1 / aucun = 7**. Les 7 « aucun » sont le texte de démo anglais Mobius
  (« occured », majuscules `Warning:`…) ; la base `en = 1` est le seul libellé dont la
  base reflète le registre `en`.
- **Les bases ne sont pas touchées** (DoD : « français inchangé » — la base dim-0 est ce
  que sert la culture fr). Voir §5 pour le résidu que cela laisse.

## 3. Injection — Rehearse puis Apply (méthode E1)

Backup pré-geste : 41 valeurs E9169 (XML, scratchpad session). Plan : 24 champs × 7
cultures = **168 cellules** (dims : en=7, ru=8, pt=9, es=10, ar=11, fa=12, zh=13), 0
cible dupliquée, anti-rejeu en transaction (`UPDLOCK, HOLDLOCK`).

> **Sauvegardes hors scratchpad** (le scratchpad de session est nettoyé ; copiées le
> 04/11 à `G:\Mon Drive\MyIA\Argumentum\backups-eav-2026-11-04\`, textes de démonstration,
> pas de secrets — emplacement demandé par ai-01 c.6629943461) :
> `contact-e9169-backup.xml` (41 valeurs, sha16 `9F41B95E5204A66B`) ·
> `contact-e9169-applied.json` (post-geste, `D06B503499855F17`) ·
> `e9169-base-backup-20261104-pre8fr.json` (pré-#2142, `D04F742BD812C70C`) ·
> `e9169-fr8-payload.json` (`57BF308E81AB9DDA`).

- **Rehearse** : transaction unique, 168 insert+lien+relecture in-tx byte-identical,
  ROLLBACK → 41 valeurs / 0 dims / 0 fantômes.
- **Apply** : COMMIT. Seed `IDENT_CURRENT('TsDynDataValue')` : **30314** — lu après le
  Rehearse, juste avant l'Apply (`$seedBefore`, script d'injection ; la rédaction initiale
  de cette note le disait lu avant : erreur de datation, corrigée le 04/11 sur requête,
  cf. §6), **30482** relu **dans la transaction de l'Apply** après les 168 inserts
  (Δ = 168, garde du script). `ValueId` réels des 168 rangées, **mesurés par requête le
  04/11** : min = **30315**, max = **30482**, contigus. **Le Rehearse annulé a consommé
  ses 168 identités** (plage 30147-30314) : requête du 04/11,
  `COUNT(*) … BETWEEN 30147 AND 30314` → **0 rangée** — comportement normal de SQL Server,
  qui ne rend jamais des identités consommées, même par un ROLLBACK ; l'Apply a pris la
  plage suivante, contiguë. 209 valeurs
  sur E9169 (41 + 168), relecture in-tx **168/168 identiques**, contrôle inverse du
  comparateur vert.
- **Recycle app domain** : mtime `web.config` 2026-11-04 01:40:09Z, sha256 inchangé
  (jamais `iisreset`).

**Rollback** (si nécessaire) — les 168 rangées sont les seules dimensionnées de E9169.
Capturer d'abord leurs `ValueId`, puis délier et supprimer **sur cette capture** (une
table temporaire, pas une sous-requête sur une table déjà modifiée) :

```sql
SELECT v.ValueId INTO #rb
FROM TsDynDataValue v
WHERE v.EntityId = 9169
  AND EXISTS (SELECT 1 FROM TsDynDataValueDimension d WHERE d.ValueId = v.ValueId);
-- attendu : 168
DELETE d FROM TsDynDataValueDimension d JOIN #rb r ON r.ValueId = d.ValueId;
-- attendu : 168
DELETE v FROM TsDynDataValue v JOIN #rb r ON r.ValueId = v.ValueId;
-- attendu : 168
```

Relecture attendue après : **41 valeurs / 0 dimensionnée** — les 41 bases dim-0 restent,
aucune n'a de rangée dimension. **Recette rejouée le 04/11 en transaction annulée** :
sélection = 168, suppressions = 168/168, relecture in-tx 41/0, et après ROLLBACK la
base était intacte (209 valeurs / 168 dims, `IDENT_CURRENT` inchangé) — rien de durable
n'a été écrit.

## 4. Preuves

**Servie (DoD)** — sonde bornée au bloc `data-accordion-child="10182"`, témoins
calibrés **sur le registre** (source de l'injection), HTML décodé :
**8 cultures × 4 témoins = 32/32 vrais** (`ButtonSend`, `LabelSubject`,
`MessageFormIncomplete`, `TermsLabel`), **0 résidu FR hors fr** (`Envoyer`/`>Sujet<`),
témoins fr = valeurs registre fr → français inchangé.

**Tierce (l'outil qui ne connaît pas le script d'injection)** —
`tools/458-eav-content-export.py --config <web.config> verify --classify` (`--config`
est un argument **de premier niveau**, avant le sous-programme) :

- `registre_avance: 0` — les 168 cellules écrites sont vues présentes et identiques.
- L'état « avant » à 176 écarts est **dérivé** de la garde pré-geste (0 rangée
  dimensionnée mesurée), pas d'une exécution préalable de l'outil — même forme de preuve
  que le précédent RuleMemo 51→29.

## 5. Résidu connu — 8 conflits `[fr]` HORS périmètre du dispatch

L'outil classe 8 cellules en `conflit`, toutes sur la colonne `[fr]` : ce sont les 8
bases non-mirroir du §2 (7 démo anglais + 1 base = registre `en`). Le registre y décrit
un français que la base ne porte pas ; la page fr sert donc ces 8 messages (messages
d'erreur/chemin exception — `MessageSendError`, `MessageRecaptchaMissing`, avertissements
Mailchimp/reCAPTCHA…) **en anglais, comme avant le geste**. Non corrigé ici :
le dispatch exclut la 8ᵉ culture (« 25 par culture × **7** cultures ») et exige
« français inchangé ». Correction éventuelle = grain séparé nécessitant dispatch
explicite (elle change le fr servi pour ces 8 messages).

## 6. Instruments — pièges rencontrés cette session

- `-eq` insensible à la casse → la garde miroir exige `-ceq` (sinon lecture 16/3/5 au
  lieu de 16/1/7).
- `Scalar "...", @{...}` lie **un seul argument tableau** (la virgule lie plus fort) →
  appeler à espace : `Scalar $sql @{...}`.
- Hash recopié à la main = faux écart ; comparer dans la machine (`-y 0`).
- `-not $a -ceq $b` : précédence → parenthéser `(-not ($a -ceq $b))`.
- Identités : ne rien présupposer d'un ROLLBACK → mesurer (`IDENT_CURRENT` avant/après
  et `MIN/MAX(ValueId)` réels par requête). **Corrigé le 04/11 (requête c.6629943461,
  `BETWEEN 30147 AND 30314` → 0 rangée)** : le Rehearse annulé **a consommé** ses 168
  identités (30147-30314), conformément à la leçon E1 — SQL Server ne les rend jamais.
  La « contiguïté 30315-30482 » de la rédaction initiale ne prouvait rien : elle ne
  portait que sur les rangées de l'Apply, pas sur la séquence globale, et la datation du
  seed (« lu avant le Rehearse ») était fausse — sinon l'Apply aurait commencé à 30483.
- Sonde page-level mélange les magasins (iframe Brevo) → borner au bloc accordion.
