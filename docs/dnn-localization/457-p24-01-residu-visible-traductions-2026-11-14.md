# #457 P24-01 — La part visible du résidu langpack, traduite (2026-11-14)

> Grain P24-01 de la file #458 : « Traduire les ~12 textes anglais visibles ».
> Source de la mesure : [`458-langpack-residu-visible-2026-11-05.md`](458-langpack-residu-visible-2026-11-05.md)
> (#2154, po-2023) — 222 hits anglais servis, **trois surfaces** seulement.
> Division actée sur #457 (état au 13/11) : **po-2024 traduit, po-2023 vérifie
> en préprod**. Ce document est le côté dépôt du geste.

## Ce que la re-mesure a changé dans le périmètre

Le « ~12 textes à traduire » était un chiffre **rapporté** ; re-mesuré au
parseur sur l'arbre `78975bee`, le périmètre se répartit en trois familles dont
une seule exigeait du nouveau texte :

| Surface (mesure #2154) | État mesuré au parseur | Gestes |
|---|---|---|
| Skip-link skin (`SkipLink.MainContent`) | les 8 jumeaux de culture de 2shineBS5 **existent** mais **aucun ne porte la clé** (fr-FR : 4 clés, autres : 15) — DNN retombe sur le neutre anglais, d'où la fuite sur **toutes** les pages, 8 langues y compris fr | **clé ajoutée aux 7 jumeaux non-en** |
| Page 404 (Privacy ×2, search ×3) | les jumeaux du dépôt portent **déjà** les traductions (Privacy.ascx ×2 fichiers ×7 cultures, Search.ascx ×7 cultures) — sauf **une coquille pt-BR** | **1 cellule corrigée** (« Não **foin** encontrado resultados » → « Nenhum resultado encontrado ») |
| Boutons sociaux Login/Register (pt-BR, ru-RU) | jumeaux `Login.ascx.<culture>.resx` des 3 fournisseurs présents pour fr/es/ar/fa/zh, **absents pour pt-BR et ru-RU** — exactement les deux cultures que la mesure voit fuir | **6 jumeaux créés** depuis le squelette fr-FR de chaque fournisseur |

**en-US n'est jamais écrit** : le neutre est anglais, sa valeur est juste pour
en-US — le repli de DNN y est correct par construction. D'où 7 cultures, pas 8.

## Les traductions écrites

Skip-link (`SkipLink.MainContent`, neutre « Skip to main content ») :

| Culture | Valeur |
|---|---|
| fr-FR | Aller au contenu principal |
| ru-RU | Перейти к основному содержимому |
| pt-BR | Ir para o conteúdo principal |
| es-ES | Ir al contenido principal |
| ar-SA | الانتقال إلى المحتوى الرئيسي |
| fa-IR | پرش به محتوای اصلی |
| zh-CN | 跳转到主要内容 |

Boutons sociaux (2 cultures × 3 fournisseurs ; `Title.Text` suit la lecture
moderne des jumeaux existants — fr-FR et zh-CN disent déjà « Microsoft » pour
Live, es-ES garde « Windows Live ») :

| Fournisseur | Clé | pt-BR | ru-RU |
|---|---|---|---|
| Facebook | `LoginFacebook.Text` | Entrar com o Facebook | Войти через Facebook |
| Facebook | `RegisterFacebook.Text` | Registrar-se com o Facebook | Регистрация через Facebook |
| Google | `LoginGoogle.Text` | Entrar com sua conta Google | Войти через аккаунт Google |
| Google | `RegisterGoogle.Text` | Registrar-se com o Google+ | Регистрация через Google+ |
| Live | `SignInLive.Text` | Entrar com a conta Microsoft | Войти через учетную запись Microsoft |
| Live | `RegisterLive.Text` | Registrar-se com a conta Microsoft | Зарегистрироваться через учетную запись Microsoft |

## Instrument et garde

- **`tools/457-p24-01-langpack-residu-visible.py`** — dry-run par défaut
  (`--check`), `--write` gardé (refus d'écraser, refus de double insertion,
  idempotent), `--self-test` à contrôle inverse sur copies : base reconstruite
  rouge (les 3 familles), geste vert, puis (a) clé retirée → échec **nommé**,
  (b) valeur anglaise substituée → fuite **vue**, (c) second `--write` → no-op.
  Sortie : une ligne par fichier, mesurée au parseur, jamais déduite.
- **`DnnVisibleResidualLangpackGuardTests`** (xUnit, 10 tests) — le contrat
  épinglé en CI : valeurs exactes des 7 skip-links, présence + non-anglais des
  6 jumeaux sociaux, coquille absente, enveloppes (2shine sans BOM/LF ; Login
  exactement **un** BOM — la réécriture naïve `utf-8-sig` sur un texte déjà
  décodé produit un **double** BOM, attrapé ici pendant le grain).
  Contrôle falsifiant exécuté : clé retirée d'un jumeau → **1 échec/10**,
  restaurée (`cp`, jamais `git checkout --`) → **10/10**.

## Ce qui reste hors de ce grain

- **La préprod ne bouge pas ici** : ces fichiers sont la source du dépôt ;
  l'application servie (resx en place, recyclage) et la vérification sur pages
  servies reviennent à po-2023, avec re-mesure de l'instrument de #2154 après
  pose (le servi reste un mash dépôt + packs amont installés le 12/10).
- Le skin « Bootstrap 4 Instant » (`Portals/1-System` et `_default`) porte
  aussi `SkipLink.MainContent` au neutre avec des jumeaux quasi inexistants
  (de-DE seul) — **non traduit ici** : ce skin n'est pas celui que la mesure
  voit servi (le skip-link mesuré pointe `#to-shine-page-main`, 2shineBS5).
  Élargir serait du périmètre fantôme ; à rouvrir seulement si une page servie
  l'utilise.
- La fiche « Buy the game » attend la boutique (#1188) — inchangé.

*po-2024*
