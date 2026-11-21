# Página de Downloads — Argumentum v2.0.0 (espelho pt do rascunho canônico FR)

> **Status: RASCUNHO (prep #135 §Downloads; alinhado com #2234 e a correspondência decidida em
> [c.6650903268](https://github.com/ArgumentumGames/Argumentum/issues/135#issuecomment-6650903268),
> grão P24-03, 21/11/2026).** Versão canônica:
> [downloads-v2.0.0.fr.md](downloads-v2.0.0.fr.md) (o aparato de trabalho — checklist, anexos — está nela
> e no espelho inglês). **A publicação é GATED** em #134 (tag GitHub Release), #132 (deploy de produção)
> e #131 (DNN 10.3.2 live). Nomes de arquivos-asset não se traduzem (nomes próprios); **`Print.Play` é a
> forma publicada** (o GitHub reescreve `&` → `.`). Os links apontam **apenas** para a release final
> (#134), nunca para pré-releases (`v0.9.0-review`, `v2.1.0-review`).

## Aviso de pré-release (retirar no tag)

⚠️ **Até a tag `v2.0.0`**, não redistribuir os assets das pré-releases: os de `v0.9.0-review` datam de
24-25/08 e precedem as correções de setembro; os de `v2.1.0-review` não são o lote aprovado. A página deve
referenciar **somente** os assets novos enviados na hora da tag.

## Por idioma — o material completo, 8 vezes

Cada idioma recebe o material **completo**: **10 documentos PDF** (nomes publicados na
[tabela derivada #2234](135-downloads-assets-table.md) — 10 bases × 8 idiomas = 80 assets), além de **dois
pacotes** construídos na hora da tag por [`tools/135-build-download-zips.py`](../../tools/135-build-download-zips.py)
e anexados à release final:

- **Cartas digitais** — `Argumentum_Cards_PNG_pt.zip`: as **175** cartas em PNG, reconstruídas a partir da
  mesma árvore de geração que a release (os PNG hoje publicados existem apenas em francês e somam 157
  cartas — duplamente desatualizados);
- **Mapas mentais** — `Argumentum_MindMaps_pt.zip`: os SVG de Falácias **e** Virtudes do idioma, vindos do
  repositório (`Cards/Fallacies/Mindmaps/pt/`), mais um link para a página do site que os exibe.

| Idioma | Material completo (10 PDF) | dos quais só Print & Play | Documentos |
|---|---:|---:|---:|
| العربية (ar) | **459,9 MB** | 205,6 MB | 10 |
| English (en) | **469,0 MB** | 211,6 MB | 10 |
| Español (es) | **472,2 MB** | 212,9 MB | 10 |
| فارسی (fa) | **470,3 MB** | 211,4 MB | 10 |
| Français (fr) — idioma-fonte | **465,5 MB** | 210,2 MB | 10 |
| Português (pt) | **481,4 MB** | 217,3 MB | 10 |
| Русский (ru) | **449,3 MB** | 204,7 MB | 10 |
| 中文 (zh) | **478,0 MB** | 213,4 MB | 10 |
| **Total** | **3 745,6 MB** | 1 687,1 MB | **80** |

« Só Print & Play » = os 4 PDF para impressão caseira (tarô padrão + Light, pôquer padrão + Light).
**Não existe zip « idioma completo »**: cada PDF é servido individualmente a partir da release.

## Por formato — os 10 documentos

| Documento | Conteúdo | Págs. | Tamanho (mín..máx nos 8 idiomas) | Uso |
|---|---|---:|---|---|
| `Argumentum_TarotCards` | Baralho principal: 15 regras + memo ×7 + 175 falácias, frente-verso **CMYK** | 379 | 57,9..66,9 MB | gráfica |
| `Argumentum_TarotCards_Virtues` | Expansão Virtudes: 131 cartas, frente-verso CMYK | 262 | 83,1..88,0 MB | gráfica |
| `Argumentum_PokerCards` | Baralho de cenários: 167 cartas ×2, 7 versos, CMYK | 334 | 25,9..32,7 MB | gráfica |
| `Argumentum_TarotCards_Print.Play_A4` | Print & Play tarô (A4, frente-verso) | 105 | 136,1..147,1 MB | casa |
| `Argumentum_TarotCards_Print.Play_Light_A4` | P&P tarô **Light** (economiza tinta) | 21 | 29,7..31,4 MB | casa |
| `Argumentum_PokerCards_Print.Play_A4` | P&P cenários | 38 | 25,7..32,5 MB | casa |
| `Argumentum_PokerCards_Print.Play_Light_A4` | P&P cenários Light | 6 | 6,3..7,4 MB | casa |
| `Argumentum_Fallacies_Web_A0` | Pôster A0 das 175 falácias | 1 | 22,6..28,1 MB | cartaz |
| `Argumentum_Fallacies_Web_A4` | Pôster A4 | 15 | 22,0..27,5 MB | cartaz |
| `Argumentum_Fallacies_Web_Thumbnails_A4` | Folha de miniaturas | 9 | 27,0..29,0 MB | referência |

Cada documento se declina como `<base>_pt.pdf` (ex. `Argumentum_TarotCards_Print.Play_A4_pt.pdf`).

Volumes da caixa: **197 / 364 / 495** cartas (decisão de fabricação #1187). Números de páginas = contrato
medido 80/80 (PR #1409 §C2); sem confundir os dois baralhos: tarô **379** págs., pôquer **334**.

## Por tipo de pacote

Correspondência **decidida** ([c.6650903268](https://github.com/ArgumentumGames/Argumentum/issues/135#issuecomment-6650903268),
05/11) — o que a página oferece hoje mantém sua natureza, atualizado e declinado nos 8 idiomas:

| Pacote | Conteúdo | Tamanho | Disponibilidade |
|---|---|---|---|
| **Completo** | 80 PDF, os 8 idiomas — sem zip global | **3 745,6 MB** (medido) | links por idioma acima (cada PDF na release final) |
| **Por idioma** | 10 PDF + zip cartas + zip mapas mentais do idioma | 449,3..481,4 MB (só PDF) | `[PLACEHOLDER — URLs release final #134]` |
| **Print & Play** | 4 PDF P&P por idioma (padrão + Light) | 204,7..217,3 MB | `Argumentum_{Tarot,Poker}Cards_Print.Play[_Light]_A4_pt.pdf` |
| **Pôster A0** | pôster das 175 falácias | 22,6..28,1 MB | `Argumentum_Fallacies_Web_A0_pt.pdf` |
| **Folhas A4** | pôster A4 + folha de miniaturas | 22,0..29,0 MB | `Argumentum_Fallacies_Web_{A4,Thumbnails_A4}_pt.pdf` |
| **Cartas digitais** | 175 PNG por idioma | `[PLACEHOLDER — medir no empacotamento]` | `Argumentum_Cards_PNG_pt.zip` (construído no tag) |
| **Cartas leves** | 175 PNG de densidade leve | `[PLACEHOLDER — a medir]` | `Argumentum_Cards_PNG_light_pt.zip` **se** a árvore da release carregar essa densidade; senão o pacote desaparece, substituído pela folha `Argumentum_Fallacies_Web_Thumbnails_A4_pt.pdf` |
| **Mapas mentais** | SVG Falácias + Virtudes por idioma | `[PLACEHOLDER — medir no empacotamento]` | `Argumentum_MindMaps_pt.zip` + link para a página do site |
| **Ontologia — falácias** | OWL das 175 falácias (SKOS + AIF), **servida pelo site** | `[PLACEHOLDER — a medir]` | `/argumentum_fallacies.owl` (raiz do site) |
| **Ontologia — virtudes** | OWL das virtudes (SKOS + AIF), **servida pelo site** | `[PLACEHOLDER — a medir]` | `/argumentum_virtues.owl` (raiz do site) |

> O nome do arquivo OWL vem da **IRI** declarada no blob, não do nome de arquivo do repositório
> (`argumentum.owl` → servido como `argumentum_fallacies.owl`); versão e data ficam em branco até a tag.
>
> Os 6 antigos zips do diretório de downloads (18/08, francês) não são mais linkados; ⛔ nenhum é excluído
> sem decisão de retirada acompanhada de prova de preservação. O erro de digitação « Argmentum » desaparece
> com os novos nomes.

Os PDF de impressão profissional são convertidos em **DeviceCMYK com OutputIntent** (`/GTS_PDFX` +
`CGATS TR 001`, #1380); as cartas em 300 dpi nativos.

---

*Espelho português (P24-03, 21/11/2026). Nomes de assets idênticos nos 8 arquivos (nomes próprios).
Checklist de publicação, SEO e anexos: canônico [FR](downloads-v2.0.0.fr.md) · [EN](downloads-v2.0.0.en.md).
Preparação — nada é publicado · veredito visual: ai-01 · revisão: jsboige.*
