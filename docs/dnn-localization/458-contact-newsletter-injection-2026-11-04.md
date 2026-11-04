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
  base reflette le registre `en`.
- **Les bases ne sont pas touchées** (DoD : « français inchangé » — la base dim-0 est ce
  que sert la culture fr). Voir §5 pour le résidu que cela laisse.

## 3. Injection — Rehearse puis Apply (méthode E1)

Backup pré-geste : 41 valeurs E9169 (XML, scratchpad session). Plan : 24 champs × 7
cultures = **168 cellules** (dims : en=7, ru=8, pt=9, es=10, ar=11, fa=12, zh=13), 0
cible dupliquée, anti-rejeu en transaction (`UPDLOCK, HOLDLOCK`).

- **Rehearse** : transaction unique, 168 insert+lien+relecture in-tx byte-identical,
  ROLLBACK → 41 valeurs / 0 dims / 0 fantômes. Les identités consommées par le ROLLBACK
  ne sont pas rendues (leçon E1) → l'arithmétique porte sur `IDENT_CURRENT`, jamais sur
  `MAX(ValueId)`.
- **Apply** : COMMIT. Seed `IDENT_CURRENT('TsDynDataValue')` **30314 → 30482** (Δ = 168),
  209 valeurs sur E9169 (41 + 168), relecture in-tx **168/168 identiques**, contrôle
  inverse du comparateur vert.
- **Recycle app domain** : mtime `web.config` 2026-11-04 01:40:09Z, sha256 inchangé
  (jamais `iisreset`).

**Rollback** (si nécessaire) — les 168 rangées sont les seules dimensionnées de E9169 :

```sql
DELETE d FROM TsDynDataValueDimension d JOIN TsDynDataValue v ON v.ValueId = d.ValueId WHERE v.EntityId = 9169;
DELETE v FROM TsDynDataValue v WHERE v.EntityId = 9169 AND EXISTS (SELECT 1 FROM TsDynDataValueDimension d WHERE d.ValueId = v.ValueId);
```

(le premier DELETE rend le second inopérant ; l'ordre est sans importance puisque les 41
bases n'ont pas de rangée dimension).

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
- Transaction annulée ≠ identités rendues → arithmétique sur `IDENT_CURRENT`.
- Sonde page-level mélange les magasins (iframe Brevo) → borner au bloc accordion.
