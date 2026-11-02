# #458 — Resynchronisation es-ES / ru-RU de `AdminResetHelp` (`Password.ascx`)

**Date :** 2026-11-03 · **Lane :** po-2024 · **Master au moment de la mesure :** `a3ab8720`
**Fichier :** `DNNPlatform/DesktopModules/Admin/Security/App_LocalResources/Password.ascx.*.resx`
**PR :** #2129

---

## 1. Pourquoi ce grain est réellement ouvert

Le pool nomme « resynchronisation es-ES/ru-RU de `AdminResetHelp`/`QAHelp` ». L'écart est
déjà **déclaré dans le code** — la garde pt-BR du fichier
(`DnnPasswordPtBrGuardTests.cs`, commentaire de classe) dit :

> AdminResetHelp et QAHelp sont traduites du NEUTRE (bouton + lien valable 24 h ; saisir
> mot de passe + question), corroboré par le fr-FR — l'es-ES (et le ru-RU pour la
> première) porte un sens périmé (« mot de passe auto-généré »). L'écart es/ru est
> déclaré dans la PR, hors périmètre de ce grain (un fichier par PR).

Le grain nomme donc un écart connu, documenté, et reporté — pas une découverte.

---

## 2. Mesure (les 8 locales + le neutre)

Le neutre (`Password.ascx.resx`, **figé depuis l'upload initial** du 2020-08-25) décrit un
**bouton + e-mail + lien valable 24 h** :

> Press the Reset Password button to send a password reset mail to the user. The reset
> link will be valid for 24hrs.

| Clé | es-ES (avant) | ru-RU (avant) | Verdict |
|---|---|---|---|
| `AdminResetHelp.Text` | *La nueva contraseña se creará automáticamente.* | *Вы можете сбросить пароль для этого пользователя. Пароль будет сгенерирован случайным образом.* | **désynchronisé** (sens périmé : génération automatique) |
| `QAHelp.Text` | *Indique su nueva pregunta/respuesta secretas* | *Вы можете обновить ваш вопрос и ответ о пароле. Введите ваш текущий пароль…* | **bon sens** (télégraphique côté es, mais pas faux) |

**Périmètre réel : 2 cellules.** `QAHelp.Text` ru-RU portait déjà le bon sens — mesuré,
pas déduit ; le grain aurait pu être lu « les deux clés des deux côtés », la mesure réduit
le périmètre à `AdminResetHelp` seul.

---

## 3. Chirurgie

Span de valeur brut (1 cellule par fichier), delta contrôlé **avant** écriture :

| Fichier | Avant | Après | Delta |
|---|---|---|---:|
| `Password.ascx.es-ES.resx` | *La nueva contraseña se creará automáticamente.* | *Pulse el botón de restablecimiento de contraseña para enviar al usuario un correo con un enlace para restablecer la contraseña. El enlace será válido durante 24 horas.* | **+123 octets** |
| `Password.ascx.ru-RU.resx` | *Вы можете сбросить пароль для этого пользователя. Пароль будет сгенерирован случайным образом.* | *Нажмите кнопку сброса пароля, чтобы отправить пользователю письмо со ссылкой для сброса пароля. Ссылка будет действительна в течение 24 часов.* | **+85 octets** |

BOM es-ES conservée (ru-RU n'en porte pas — préservé tel quel), LF seul, XML revalidé.

---

## 4. Effet de bord cross-fichier — attrapé par la garde pt-BR elle-même

La correction es-ES a retiré « automáticamente » et « creara », qui étaient les **2 seules
cellules es** justifiant ces marqueurs dans la grille espagnole de
`DnnPasswordPtBrGuardTests` (fichier 2/5 du sweep pt-BR). L'auto-contrôle de cette grille
(`Marker_Grid_Is_Non_Vacuous_On_Es`) exige que chaque marqueur touche au moins une cellule
de l'es-ES — sinon le marqueur est aveugle.

Résultat : le retrait des 2 cellules a rendu **2 marqueurs aveugles**, et l'auto-contrôle
a tiré. Correction : les 2 marqueurs sont **sortis de la grille** (ils n'ont plus de
témoin es) et le commentaire de grille a été ajusté pour consigner la raison.

> ⭐ **La garde a attrapé son propre effet de bord.** C'est exactement le rôle d'un
> auto-contrôle : il ne prouve pas que la grille est bonne, il prouve qu'elle ne s'est pas
> vidée.

---

## 5. Garde `DnnAdminResetHelpResyncGuardTests` — 5 tests

| Test | Ce qu'il tient |
|---|---|
| `Neutral_Still_Describes_The_Reset_Button_And_24h_Link` | le neutre est le référent (bouton + mail + lien 24 h) — épinglé |
| `Es_AdminResetHelp_Is_Resynced` | es-ES resynchronisée, `automáticamente` retiré |
| `Ru_AdminResetHelp_Is_Resynced` | ru-RU resynchronisée, `сгенерирован` retiré |
| `Ru_QaHelp_Was_Already_In_Sync_And_Stays_So` | QAHelp ru-RU était déjà au bon sens — épinglée |
| `Es_QaHelp_Stays_So` | QAHelp es-ES épinglée à sa valeur courante (télégraphique mais bon sens) |

**Banc** : garde lancée sur l'arbre **non corrigé** → 2 FAIL exacts (`Es_AdminResetHelp`,
`Ru_AdminResetHelp`), 3 PASS. Après chirurgie → 5/5 verts. Le rouge-avant est la sortie
de l'instrument, pas une affirmation de cette note.

---

## 6. Validation

Suite complète `Argumentum.AssetConverter.Tests` : **4 341 total (4 335 pass / 0 fail / 6
skip)** = master `a3ab8720` 4 336 + 5 (cette garde). Arithmétique exacte. Le retrait des
2 marqueurs aveugles de la grille pt-BR fait passer `Marker_Grid_Is_Non_Vacuous_On_Es`
de 1 FAIL à 0 — la grille est à nouveau non-aveugle.
