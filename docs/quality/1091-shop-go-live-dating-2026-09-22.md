# #1091 — Datation de la mise en vente de la boutique (dimensionnement du risque de l'archive 2023)

**Date** : 2026-09-22 ~12:30 · **Lane** : po-2023 (worker) · **Base** : `origin/master` `32f9b826`
**Nature** : mesure **lecture seule** sur la base de préprod (`localhost\SQLEXPRESS` / `ArgumentumGames`,
NBrightBuy 4.1.11 / OS_Stripe 1.0.0.0).
**Instrument** : `sqlcmd` batches `GO` → [`tools/1091-shop-go-live-probes.sql`](../../tools/1091-shop-go-live-probes.sql).
**Interdits respectés** : ⛔ **l'archive de 2023 n'est ni ouverte, ni extraite, ni déplacée, ni testée** (une
**date** suffit) · ⛔ aucun `XMLData` lu (il porte des clés Stripe **live** en clair et des données client —
cf. WARN de grain ③) · ⛔ aucune valeur de credential, aucun hash, aucune identité client reproduits ·
⛔ aucune écriture (gel #972).

---

## Réponse

**La boutique a ouvert à la vente le 2023-02-27** — première commande le **2023-02-27 à 21:12:36**, fournisseur
de paiement **Stripe** configuré le **même jour à 15:20:47** (≈ 6 h plus tôt). Sa **préparation** (catalogue,
livraison, catégories) date du **4-6 janvier 2023**. La page DNN `Acheter le jeu` existait depuis le
**2022-04-01** mais **sans activité de vente** — elle a dormi ~10 mois.

**Conséquence pour l'archive de 2023** : le risque se lit sur **une seule date**, `2023-02-27`.

| Date de l'archive | Ce qu'elle peut contenir |
|---|---|
| **avant** 2023-02-27 | **aucune** commande, **aucun** client, **aucune** donnée de paiement : la boutique n'existait pas encore transactionnellement. Risque ≈ nul. |
| **après** 2023-02-27 | configuration Stripe (les **clés API live**, déjà signalées en clair dans `NBrightBuy.XMLData`), jusqu'à 124 commandes, 111 fiches client (**PII** : nom, courriel, adresse)… et **aucune donnée de carte** : tout paiement est passé par Stripe (hébergé/tokenisé) dès la première commande. |

Autrement dit : **le risque « données de carte » de l'archive est nul dans les deux branches** ; seule la
branche « après » porte des **clés live** et des **PII**. Le second fournisseur (`MANUALPAYMENT`) n'a que ses
**2 lignes de configuration par défaut datées 2020-07-27** (ère plugin, avant l'existence de la boutique) — il
n'a jamais encaissé de commande dans ce corpus.

---

## 1. Les trois axes de datation (mesurés)

### a. La page existe depuis avril 2022

| Élément | Créée | Dernière modification |
|---|---|---|
| `dbo.Tabs` — `Acheter le jeu` | **2022-04-01T07:08:14.787** | 2026-08-14T21:44:21.813 |

La page **existe** en 2022 — mais l'existence d'une page ne dit rien de l'ouverture des ventes (c'est
exactement le piège que ce tableau désamorce).

### b. Le magasin est constitué en janvier 2023

Plancher de chaque type d'objet de la boutique (`MIN(ModifiedDate)` par `TypeCode`) :

| `TypeCode` | Lignes | Première date |
|---|---:|---|
| `CLIENT` | 111 | **2023-01-04T23:19:37.797** |
| `SHIPPING` / `SHIPPINGLANG` | 1 / 1 | **2023-01-05T00:38:35** |
| `CATEGORY` / `CATEGORYLANG` | 4 / 4 | **2023-01-06T00:04:20.790** |
| `PRDLANG` | 3 | **2023-01-06T00:15:04.077** |
| `TAX` | 1 | 2023-03-02T14:25:10.763 |

### c. L'ouverture transactionnelle = 27 février 2023

| Élément | Date |
|---|---|
| `OSStripePAYMENTLANG` (fournisseur de paiement configuré) | **2023-02-27T15:20:47.160** |
| **Première commande** (`TypeCode='ORDER'`) | **2023-02-27T21:12:36.890** |
| `OSStripePAYMENT` (ligne de configuration) | 2023-04-23T18:41:25.033 |

Commandes par année : **2023 → 76**, **2024 → 31**, **2025 → 17**, **2026 → 0**.
Détail 2023 par mois : fév **3**, mars **27**, avr **2**, mai **18**, juin **5**, juil **2**, août **2**,
sept **3**, nov **14** (rien en janvier, octobre, décembre). Clients par année : 2023 → 64, 2024 → 28,
2025 → 18, 2026 → 1.

**MESURÉ** : toutes les dates ci-dessus. **DÉRIVÉ** : que « première commande » = ouverture à la vente
(3 objets indépendants pointent la même journée : la ligne Stripe, sa traduction, la commande).
**SUPPOSÉ (et signalé)** : la signification commerciale de l'absence de commande en 2026 (arrêt de la vente ?
canal déplacé ?) — **non mesurable ici**.

## 2. Contre-épreuves

**Témoin d'instrument (double)** — l'instrument **sait** voir avant 2023 : les mêmes requêtes rendent
`PLUGIN` 2020-07-25, `USERDATA` 2020-07-27, `GROUP`/`GROUPLANG` 2020-07-25, `MANUALPAYMENT` 2020-07-27, et la
page `Acheter le jeu` **2022-04-01**. Le plancher 2023 des objets de boutique est donc une **propriété du
magasin**, pas une cécité de l'instrument — c'est la contre-épreuve qui manquait pour lire un « rien avant
2023 » autrement que comme un artefact.

**Test falsifiant** — l'énoncé « aucune vente avant 2023 » serait réfuté par **une seule** ligne `ORDER`
antérieure. Mesuré : `MIN(YEAR(ModifiedDate))` sur `ORDER` = **2023**, et aucune ligne en 2022 dans
l'histogramme annuel. Le falsificateur passe.

**Limite de l'instrument, dite** — `NBrightBuy` **n'a pas** de colonne de création (`ItemId`, `PortalId`,
`ModuleId`, `TypeCode`, `XMLData`, `GUIDKey`, **`ModifiedDate`**, `TextData`, `XrefItemId`, `ParentItemId`,
`Lang`, `UserId`, `LegacyItemId`). `ModifiedDate` est une **modification** : `MIN(ModifiedDate)` majore donc la
date du premier évènement (la création d'une ligne est ≤ sa modification). L'énoncé rigoureux est : **la
première commande a eu lieu au plus tard le 2023-02-27 21:12:36** ; les énoncés « première commande =
27/02/2023 » et « ouverture = février 2023 » sont **DÉRIVÉS**, corroborés par trois lignes indépendantes
datées du même jour et par le plancher catalogue de janvier.

## 3. Ce que cette datation n'établit pas

- **Où est l'archive, ni ce qu'elle contient.** Elle n'a **pas** été trouvée aux emplacements sondés
  (`D:\Dev\Argumentum`, `D:\DNN`, `G:\Mon Drive\Argumentum`, `D:\` profondeur 2 — seul un `.bak` de 2026 y
  figure). ⛔ Elle n'a été ni ouverte, ni extraite, ni déplacée, ni testée ; **sa date suffit** au DoD, et cette
  date est le seul discriminant utile (tableau ci-dessus).
- **Le mode (live/test) des clés Stripe du corpus** : cela n'a **pas** été relu ici (⛔ pas de `XMLData`) — c'est
  l'objet du grain ③, qui avait mesuré un **triplet LIVE seul** sur la machine.
- **Qu'aucune donnée de carte n'existe ailleurs** que dans Stripe (tableur, courriel, sauvegarde hors Stripe) —
  hors de portée de cet instrument.
- **Le contenu des commandes** : seuls les **comptes** et les **dates** ont été lus. Aucun nom, courriel,
  adresse, montant ni identifiant client.

---

## Reproductibilité

```bash
sqlcmd -S "<serveur>" -d "<base>" -i tools/1091-shop-go-live-probes.sql -h -1 -W
```

---
*po-2023 (worker lane) · instruments : `sqlcmd` (batches `GO`), lecture seule ·
verdict/arbitrage : ai-01 & jsboige · po-2023 signale, ne déclare pas PASS.*
