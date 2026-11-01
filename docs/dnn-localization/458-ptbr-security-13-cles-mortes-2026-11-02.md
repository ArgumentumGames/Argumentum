# 458 — `security.pt-BR.resx` : les 13 clés mortes, nommées et épinglées

**Date** : 2026-11-02 · **Mesure sur** master `623f8fd8` · **Item** : pool po-2024 #458 c.6561677860, item 2 (« nommer les 13 clés mortes »).

## Définition

Morte = **présente dans `security.pt-BR.resx`**, **absente du neutre `Security.resx`** et **absente de `security.es-ES.resx`** : il n'existe aucune source anglaise à traduire, et l'es-ES ne la porte pas non plus. Mesure : 362 clés pt − 349 communes avec le neutre = **13**, exactement les mêmes que le relevé web2 du 01/11 (c.5750260550 → relevé corrigé 15:04Z).

## L'inventaire, avec provenance mesurée (`git log -S` + `git show` sur le neutre)

| Clé | Valeur pt actuelle | Retirée du neutre par | Remplacée au neutre par |
|---|---|---|---|
| `CheckTelerikVulnerabilityFailure.Text` | `A vulnerabilidade do componente Telerik não foi corregido. Vá para http://www.dnnsoftware.com/…` | `4e3f4c1f` (Upgrade DNN 9.11) | — (plus de descendant) |
| `CheckTelerikVulnerabilityName.Text` | `Comprobar se o componente Telerik tem vulnerabilidades.` | `4e3f4c1f` | — |
| `CheckTelerikVulnerabilityReason.Text` | `Componentes de terceros utilizados em DNN podem ter vulnerabilidades…` | `4e3f4c1f` | — |
| `CheckTelerikVulnerabilitySuccess.Text` | `Componente Telerik já actualizado.` | `4e3f4c1f` | — |
| `Não.Text` | `Não` | **jamais existée au neutre** | `No.Text` (`No`) |
| `Redirect_AfterLogin.Help` | `Selecione a página donde se redirigirán os usuarios después de realizar a identificação.` | `27675a95` (Update Argumentum DNN website) | `RedirectionMovedToSiteSettings.Text` |
| `Redirect_AfterLogin.Text` | `Redireção después de identificação` | `27675a95` | idem |
| `Redirect_AfterLogout.Help` | `Selecione a página donde se redirigirán os usuarios después de cerrar a sesión.` | `27675a95` | idem |
| `Redirect_AfterLogout.Text` | `Redireccionar ao cerrar a sesión` | `27675a95` | idem |
| `Redirect_AfterRegistration.Help` | `Selecione a página donde se redirigirán os usuarios después de completar corretamente o registro.` | `27675a95` | idem |
| `Redirect_AfterRegistration.Text` | `Redireccionar después do registro` | `27675a95` | idem |
| `plSSLEnabled.Help` | `Entre si foi instalador um certificado SSL para este sitio` | `4e3f4c1f` | `plSSLEnforced.Help` |
| `plSSLEnabled.Text` | `Habilitar SSL` | `4e3f4c1f` | `plSSLEnforced.Text` (`Mixed Mode`) |

La vérification Telerik a **quitté ce fichier** : aucun descendant du préfixe `CheckTelerikVulnerability` n'existe au neutre. Les 6 `Redirect_After*` sont le mécanisme de redirection d'avant la refonte « Site Settings » — le bundle actuel charge `RedirectionMovedToSiteSettings.Text` à la place (constaté dans `BasicSettingsPanelBody` et `RegistrationSettingsPanelBody`).

Leurs valeurs portent la contamination espagnole du reste du fichier (`después de`, `donde se redirigirán`, `cerrar a sesión`, `terceros`, `Comprobar`) : ce ne sont **pas** des cellules saines, ce sont des cellules **sans objet** — c'est précisément pourquoi elles ne sont pas dans le périmètre du sweep.

## Décision

**Conservées, ni supprimées ni traduites.** Une suppression serait un changement de comportement potentiel pour un runtime qui lirait encore ces clés (règle du dépôt : jamais de suppression sans preuve de préservation). Une traduction n'est pas définissable : aucune source anglaise n'existe.

**Épinglage** : `DnnSecurityDeadKeysPtBrGuardTests` (5 tests, sur `DnnResxGuardSocle`) —
1. l'inventaire mesuré (pt − neutre, en `XDocument`) doit égaler **exactement** les 13 nommées : une clé qui disparaît (suppression silencieuse) **ou** une 14ᵉ qui naît (copie silencieuse) rougit ;
2. aucune n'existe dans l'es-ES (si elle y apparaît, elle n'est plus morte : l'inventaire est à refaire) ;
3. chaque valeur est épinglée par SHA-256 — « traduire » une clé morte devient rouge ;
4. les contreparties vivantes (`No.Text`, `RedirectionMovedToSiteSettings.Text`, `plSSLEnforced.Text/.Help`) doivent exister au neutre et le préfixe retiré ne doit pas y revenir ;
5. invariant de transport sur le blob commité (BOM, 0 CR).

Banc de mutations **6/6** : 14ᵉ clé née · clé supprimée · valeur traduite · **le neutre regagne `Não.Text`** (l'autre moitié de la définition) · contrôle inverse CR (vert, la garde lit le blob `HEAD`) · inventaire nommé amputé.

## Leçon d'instrument (mesurée en mesurant cet inventaire)

Un parse resx par regex qui ignore la valeur **auto-fermée** `<value />` ne se contente pas de rater la clé vide : `<value[^>]*>(.*?)</value>` matche `<value />` puis **avale tout jusqu'au `</value>` du bloc suivant** — la clé *suivante* disparaît du parse. Le neutre porte 3 `<value />` (`ScannerExplanation.Text`, `ModifiedFilesExplaination.Text`, `ModifiedSettingsExplaination.Text`) : un tel parse compte **15** mortes au lieu de 13. `pt` et `es` n'en portent aucun — aucun livrable antérieur du sweep n'est affecté, mais tout futur instrument doit parser par bloc `<data>…</data>` (comme la garde C#) ou traiter `<value />` explicitement.

## Pour tuer une de ces clés (procédure)

1. Prouver qu'aucun runtime ne la lit (recherche de consommation dans le bundle et le code servi, pas seulement le dépôt).
2. La supprimer **et** retirer son nom de l'inventaire de la garde **dans le même commit** — le rouge de la garde est alors la signature du geste délibéré, pas un accident.
