# #445 §3 — E-shop Stripe Native : capability scoping

**Author**: po-2024 (worker) · **Date**: 2026-07-01 · **Base**: master `d0856aa4`
**Status**: **READ-ONLY SCOPING** — research/capability mapping, no decision, no code.
**Scope**: extends [`UPGRADE-ASSESSMENT.md`](UPGRADE-ASSESSMENT.md) §5 "Eshop Strategy" with a
detailed capability map for the 5 sub-questions of [#445](https://github.com/ArgumentumGames/Argumentum/issues/445) §3.

## Context — what is already decided (do not re-litigate)

`UPGRADE-ASSESSMENT.md` §5 records a **jsboige decision (2026-06-07): Option 2 — Stripe Native**,
replacing OpenStore/NBrightBuy. The driver is **not** a .NET-8 blocker (retracted — DNN 10.x runs on
.NET Framework 4.8, so NBrightBuy *could* stay), but two independent merits:

1. **CVE elimination** — `RazorEngine 3.10.0` carries **CVE-2021-46703** (unfixable sandbox escape),
   used *only* by NBrightBuy. Removing NBrightBuy removes the CVE. (Verified NVD/GHSA, see §3/§4 of
   the assessment.)
2. **Modernization** — Stripe.net `41.8.0.0` is already present in `bin/` as the payment integration;
   the e-shop views are the remaining NBrightBuy footprint.

This document scopes **how** to implement §3's five functional requirements on Stripe native. It does
**not** revisit the Stripe-vs-OpenStore decision and does **not** pick the marketplace architecture
(a jsboige call). Each capability is marked **NATIVE / PARTIAL / NOT-NATIVE** with the integration
pattern that delivers it.

## §3.1 — Compte revendeur (reseller account) → **NATIVE via Stripe Connect**

The OpenStore "compte revendeur" notion maps to **Stripe Connect** (Stripe's multi-party /
marketplace product). Connect orchestrates money movement across multiple parties: it onboards
sellers/vendors (KYC), links their bank accounts, and routes payouts separately per seller.

This fits [#445](https://github.com/ArgumentumGames/Argumentum/issues/445)'s distribution model
("un partenaire par langue/pays qui stocke, vend et expédie localement") if the platform is to
handle money routing between buyer → Argumentum → local reseller.

- **Account types**: `Standard` (reseller gets a full Stripe Dashboard), `Express` (Stripe-hosted
  onboarding, simplified dashboard), `Custom` (fully embedded, most control / most build effort).
- **Trade-off**: Connect adds onboarding + KYC per reseller and per-country compliance. For a small
  number of partners (one per language), `Standard` or `Express` is low-friction.

**If Connect is judged over-engineered**, the simpler alternative is the **direct + offline model**:
Argumentum runs one Stripe account (direct Checkout), sells wholesale to each reseller, and the
reseller runs their *own* local storefront/payment. Money does not flow through Argumentum for the
reseller's local sales. This drops Connect entirely at the cost of central price/stock control.

> **Decision for jsboige**: Connect (platform routes money) **vs** direct+offline (reseller owns
> their local sale). The §5 note already flags this as the key design fork.

## §3.2 — Frais de port variables par zone → **NATIVE**

Stripe Checkout supports **shipping rates** (standard / express / overnight) with delivery
estimates, and **dynamic shipping** that:

- displays only the methods available for the customer's address (e.g. overnight in-country only), and
- calculates the fee from the delivery address.

For Argumentum's 3 zones (France / UE / international), this is covered directly by dynamic
shipping options. For carrier-grade rate calculation (weight, dimensions, source→destination),
Stripe integrates **EasyPost** and **Shippo** as shipping providers — the rate is then computed from
shipment data, not a static table.

- **Simple path**: static shipping rates per zone (3 rates) — minutes to configure.
- **Accurate path**: EasyPost/Shippo relay for real carrier rates — recommended once box/weight is fixed.

## §3.3 — Multi-devises (EUR, GBP, BRL, USD…) → **NATIVE**

Stripe processes in **135+ currencies** with local presentment (price shown in the buyer's native
currency), which Stripe documents as improving conversion and authorization rates and avoiding
customer-side conversion cost. `Adaptive Pricing` adds dynamic currency adjustment per buyer
location. Settlement currency vs presentment currency is a standard configuration.

For the 8 v2 languages, EUR (FR/PT/ES), GBP (EN-UK), BRL (PT-BR), USD (EN-US) and others are all
presentment options. No build work beyond per-currency Price objects on each Product.

## §3.4 — Gestion de stock → **PARTIAL — not a WMS**

This is Stripe's **weakest** of the five. Stripe Products/SKUs carry inventory *attributes* but
Stripe is **not a warehouse-management system**; real-time inventory sync is something Stripe's own
guidance builds externally (event-driven, keyed off payment events). For a physical product where
stock v1 is already exhausted and v2 doubles volume, expect to either:

- **track stock outside Stripe** (a simple DB/table, or the reseller's own system in the Connect /
  offline model), decremented from Stripe `checkout.session.completed` / `payment_intent.succeeded`
  webhooks; or
- keep **SKU inventory fields** on Stripe Products as a coarse truth and reconcile periodically.

The cleanest fit with [#445](https://github.com/ArgumentumGames/Argumentum/issues/445)'s
partner-per-zone model is: **each reseller owns their local stock** (Connect/offline), so global
"stock" is the sum of partner stocks + Argumentum's direct stock — none of which needs Stripe to be
the system of record.

## §3.5 — Fulfillment → **NATIVE (orchestration) / NOT a 3PL**

Stripe is **not** a 3PL — it does not pick, pack, or ship. What it provides is the **order event
stream** (`checkout.session.completed`, the Orders API) that a fulfillment step consumes. Stripe's
own fulfillment doc offers **manual** (Dashboard / emails / reports) or **automated** (webhook-driven)
handling.

For [#445](https://github.com/ArgumentumGames/Argumentum/issues/445) ("lien avec le partenaire local
pour l'expédition"), the pattern is: Stripe payment event → webhook → notify the local partner
(email/API) → partner ships → (optional) tracking back into Stripe. This is build work (a small
webhook receiver + partner dispatch), not a Stripe feature toggle. It is *independent* of the
Connect-vs-offline decision in §3.1.

## Capability matrix

| §3 requirement | Stripe native? | Pattern | Build effort |
|---|---|---|---|
| 3.1 Compte revendeur | **NATIVE** (Connect) | Stripe Connect (Standard/Express) *or* direct+offline | Low (Standard) – High (Custom) |
| 3.2 Port variable par zone | **NATIVE** | Checkout dynamic shipping / EasyPost / Shippo | Low (static rates) – Med (carrier) |
| 3.3 Multi-devises | **NATIVE** | 135+ presentment currencies, Adaptive Pricing | Low (per-currency Prices) |
| 3.4 Gestion de stock | **PARTIAL** | External stock + webhook decrement; or reseller-owned | Med |
| 3.5 Fulfillment | **NATIVE** (events) | Webhook → partner dispatch; Stripe is not a 3PL | Med |

## What this scoping concludes (no business decision made)

1. **Four of five requirements are native** to Stripe (reseller/Connect, zonal shipping,
   multi-currency, fulfillment-as-events). The genuine build work is concentrated in **stock (3.4)**
   and the **fulfillment webhook/partner dispatch (3.5)** — both modest and well-trodden.
2. **The one open architecture fork** is §3.1: **Stripe Connect (platform routes money to resellers)**
   vs **direct + offline (each reseller owns their local sale)**. Everything downstream (stock
   ownership, fulfillment routing) flows from this. It is a jsboige call, surfaced here, not decided.
3. **The CVE rationale stands on its own** (RazorEngine 3.10.0 → CVE-2021-46703, used only by
   NBrightBuy) — confirmed in `UPGRADE-ASSESSMENT.md` §3/§4. Stripe-native is a security win
   regardless of the Connect-vs-offline choice.
4. **Out of scope here** (require human outreach, not code scoping): §1 fabrication cost/quotes,
   §2 distribution-partner mapping — these are BD tasks tracked elsewhere in the Epic.

## Sources

- [Stripe — Power Payments for Marketplaces (Connect)](https://stripe.com/use-cases/marketplaces)
- [Stripe — How Multivendor Marketplace Payment Gateways Work](https://stripe.com/fr/resources/more/multivendor-marketplace-payments)
- [Stripe — Charge for shipping (Checkout shipping rates)](https://docs.stripe.com/payments/during-payment/charge-shipping)
- [Stripe — Dynamically customize shipping options](https://docs.stripe.com/payments/advanced/shipping)
- [Stripe — Work with multiple currencies (135+)](https://docs.stripe.com/connect/currencies)
- [Stripe — Fulfill orders (Checkout fulfillment)](https://docs.stripe.com/checkout/fulfillment)
- [Stripe — How do I store inventory data in my application](https://stripe.dev/blog/how-do-i-store-inventory-data-in-my-stripe-application)

Relates to #445, #131, #444, #458. Honors pre-tag freeze (docs-only, 0 `Cards/` write, 0 AssetConverter code change).
