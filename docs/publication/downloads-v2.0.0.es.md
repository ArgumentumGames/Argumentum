# Página de Descargas — Argumentum v2.0.0 (espejo es del borrador canónico FR)

> **Estado: BORRADOR (prep #135 §Descargas; alineado con #2234 y la correspondencia decidida en
> [c.6650903268](https://github.com/ArgumentumGames/Argumentum/issues/135#issuecomment-6650903268),
> grano P24-03, 21/11/2026).** Versión canónica:
> [downloads-v2.0.0.fr.md](downloads-v2.0.0.fr.md) (el aparato de trabajo — checklist, anexos — vive en
> ella y en el espejo inglés). **La publicación está GATED** en #134 (tag GitHub Release), #132 (despliegue
> prod) y #131 (DNN 10.3.2 live). Los nombres de archivo-asset no se traducen (nombres propios); **`Print.Play`
> es la forma publicada** (GitHub reescribe `&` → `.`). Los enlaces apuntan **solo** a la release final
> (#134), nunca a pre-releases (`v0.9.0-review`, `v2.1.0-review`).

## Aviso de pre-release (retirar en el tag)

⚠️ **Hasta el tag `v2.0.0`**, no redistribuir los assets de las pre-releases: los de `v0.9.0-review` datan
del 24-25/08 y preceden a las correcciones de septiembre; los de `v2.1.0-review` no son el lote aprobado.
La página debe referenciar **únicamente** los assets nuevos subidos en el momento del tag.

## Por idioma — el material completo, 8 veces

Cada idioma recibe el material **completo**: **10 documentos PDF** (nombres publicados en la
[tabla derivada #2234](135-downloads-assets-table.md) — 10 bases × 8 idiomas = 80 assets), más **dos
paquetes** construidos en el tag por [`tools/135-build-download-zips.py`](../../tools/135-build-download-zips.py)
y adjuntados a la release final:

- **Cartas digitales** — `Argumentum_Cards_PNG_es.zip`: las **175** cartas en PNG, reconstruidas desde el
  mismo árbol de generación que la release (los PNG hoy servidos existen solo en francés y cuentan 157
  cartas — doblemente obsoletos);
- **Mapas mentales** — `Argumentum_MindMaps_es.zip`: los SVG de Falacias **y** Virtudes del idioma, tomados
  del repositorio (`Cards/Fallacies/Mindmaps/es/`), más un enlace a la página del sitio que los muestra.

| Idioma | Material completo (10 PDF) | de los cuales solo Print & Play | Documentos |
|---|---:|---:|---:|
| العربية (ar) | **459,9 MB** | 205,6 MB | 10 |
| English (en) | **469,0 MB** | 211,6 MB | 10 |
| Español (es) | **472,2 MB** | 212,9 MB | 10 |
| فارسی (fa) | **470,3 MB** | 211,4 MB | 10 |
| Français (fr) — idioma fuente | **465,5 MB** | 210,2 MB | 10 |
| Português (pt) | **481,4 MB** | 217,3 MB | 10 |
| Русский (ru) | **449,3 MB** | 204,7 MB | 10 |
| 中文 (zh) | **478,0 MB** | 213,4 MB | 10 |
| **Total** | **3 745,6 MB** | 1 687,1 MB | **80** |

« Solo Print & Play » = los 4 PDF de impresión casera (tarot estándar + Light, póker estándar + Light).
**No existe zip « idioma completo »**: cada PDF se sirve individualmente desde la release.

## Por formato — los 10 documentos

| Documento | Contenido | Págs. | Tamaño (mín..máx en 8 idiomas) | Uso |
|---|---|---:|---|---|
| `Argumentum_TarotCards` | Mazo principal: 15 reglas + memo ×7 + 175 falacias, doble cara **CMYK** | 379 | 57,9..66,9 MB | imprenta |
| `Argumentum_TarotCards_Virtues` | Expansión Virtudes: 131 cartas, doble cara CMYK | 262 | 83,1..88,0 MB | imprenta |
| `Argumentum_PokerCards` | Mazo de escenarios: 167 cartas ×2, 7 dorsos, CMYK | 334 | 25,9..32,7 MB | imprenta |
| `Argumentum_TarotCards_Print.Play_A4` | Print & Play tarot (A4, doble cara) | 105 | 136,1..147,1 MB | casa |
| `Argumentum_TarotCards_Print.Play_Light_A4` | P&P tarot **Light** (ahorra tinta) | 21 | 29,7..31,4 MB | casa |
| `Argumentum_PokerCards_Print.Play_A4` | P&P escenarios | 38 | 25,7..32,5 MB | casa |
| `Argumentum_PokerCards_Print.Play_Light_A4` | P&P escenarios Light | 6 | 6,3..7,4 MB | casa |
| `Argumentum_Fallacies_Web_A0` | Póster A0 de las 175 falacias | 1 | 22,6..28,1 MB | cartel |
| `Argumentum_Fallacies_Web_A4` | Póster A4 | 15 | 22,0..27,5 MB | cartel |
| `Argumentum_Fallacies_Web_Thumbnails_A4` | Hoja de miniaturas | 9 | 27,0..29,0 MB | referencia |

Cada documento se declina como `<base>_es.pdf` (ej. `Argumentum_TarotCards_Print.Play_A4_es.pdf`).

Volúmenes de la caja: **197 / 364 / 495** cartas (decisión de fabricación #1187). Números de páginas =
contrato medido 80/80 (PR #1409 §C2); sin confundir los dos mazos: tarot **379** págs., póker **334**.

## Por tipo de paquete

Correspondencia **decidida** ([c.6650903268](https://github.com/ArgumentumGames/Argumentum/issues/135#issuecomment-6650903268),
05/11) — lo que la página ofrece hoy mantiene su naturaleza, actualizado y declinado en los 8 idiomas:

| Paquete | Contenido | Tamaño | Disponibilidad |
|---|---|---|---|
| **Completo** | 80 PDF, los 8 idiomas — sin zip global | **3 745,6 MB** (medido) | enlaces por idioma arriba (cada PDF en la release final) |
| **Por idioma** | 10 PDF + zip cartas + zip mapas mentales del idioma | 449,3..481,4 MB (solo PDF) | `[PLACEHOLDER — URLs release final #134]` |
| **Print & Play** | 4 PDF P&P por idioma (estándar + Light) | 204,7..217,3 MB | `Argumentum_{Tarot,Poker}Cards_Print.Play[_Light]_A4_es.pdf` |
| **Póster A0** | póster de las 175 falacias | 22,6..28,1 MB | `Argumentum_Fallacies_Web_A0_es.pdf` |
| **Hojas A4** | póster A4 + hoja de miniaturas | 22,0..29,0 MB | `Argumentum_Fallacies_Web_{A4,Thumbnails_A4}_es.pdf` |
| **Cartas digitales** | 175 PNG por idioma | `[PLACEHOLDER — medir en el empaquetado]` | `Argumentum_Cards_PNG_es.zip` (construido en el tag) |
| **Cartas ligeras** | 175 PNG de densidad ligera | `[PLACEHOLDER — a medir]` | `Argumentum_Cards_PNG_light_es.zip` **si** el árbol de la release lleva esa densidad; si no, el paquete desaparece, reemplazado por la hoja `Argumentum_Fallacies_Web_Thumbnails_A4_es.pdf` |
| **Mapas mentales** | SVG Falacias + Virtudes por idioma | `[PLACEHOLDER — medir en el empaquetado]` | `Argumentum_MindMaps_es.zip` + enlace a la página del sitio |
| **Ontología — falacias** | OWL de las 175 falacias (SKOS + AIF), **servida por el sitio** | `[PLACEHOLDER — a medir]` | `/argumentum_fallacies.owl` (raíz del sitio) |
| **Ontología — virtudes** | OWL de las virtudes (SKOS + AIF), **servida por el sitio** | `[PLACEHOLDER — a medir]` | `/argumentum_virtues.owl` (raíz del sitio) |

> El nombre del archivo OWL lo decide la **IRI** declarada por el blob, no el nombre de archivo del
> repositorio (`argumentum.owl` → servido como `argumentum_fallacies.owl`); versión y fecha quedan en
> blanco hasta el tag.
>
> Los 6 antiguos zips del directorio de descargas (18/08, francés) ya no están enlazados; ⛔ ninguno se
> elimina sin decisión de retirada acompañada de una prueba de preservación. La errata « Argmentum »
> desaparece con los nuevos nombres.

Los PDF de impresión profesional se convierten a **DeviceCMYK con OutputIntent** (`/GTS_PDFX` +
`CGATS TR 001`, #1380); las cartas a 300 dpi nativos.

---

*Espejo español (P24-03, 21/11/2026). Nombres de assets idénticos en los 8 archivos (nombres propios).
Checklist de publicación, SEO y anexos: canónico [FR](downloads-v2.0.0.fr.md) · [EN](downloads-v2.0.0.en.md).
Preparación — no se publica nada · veredicto visual: ai-01 · revisión: jsboige.*
