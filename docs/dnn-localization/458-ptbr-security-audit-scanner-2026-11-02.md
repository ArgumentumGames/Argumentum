# 458 — `security.pt-BR.resx` : volet Audit/Scanner — 66 cellules réécrites + 10 clés créées

**Date** : 2026-11-02 · **Base** : master `71259ddb` · **Item** : pool po-2024 c.6576550705, item 1 (« Audit/Scanner »).

## Le défaut, mesuré

La famille des clés de vérification sécurité (`Check*`, `AuditChecks/AuditExplanation`, `ScannerChecks`, `SettingName/Value`, `Modified*`, en-têtes de grille) était un **hybride espagnol/galicien de bout en bout** — pas seulement les cellules flaggées par les marqueurs : même les « non-flaggées » lisaient `Auditoría`, `NOME PARÁMETRO`, `base de datos`. Témoins nommés par le pool, tous vérifiés dans le fichier : `CheckViewstatemacReason.Text` (« *variable oculta donde se guarda… foiya modificado* »), `foin encontrado` ×9, `contrasenha` ×6 dans ce volet.

Mesure croisée pt/neutre/es (`.tmp/ptbr-audit/volet_audit.json`, 91 clés) : **78 clés famille au pt, 86 au neutre, 88 à l'es-ES**. Répartition du traitement :

| Catégorie | Nb | Traitement |
|---|---|---|
| Cellules contaminées | **66** | réécrites depuis le neutre anglais |
| Clés vivantes au neutre, absentes du pt | **10** | créées (`CheckTelerikPresence*` ×6 — retrait Telerik DNN 9.8/10.x ; `CheckUserProfilePage*` ×4) |
| Cellules déjà correctes en pt | 8 | laissées telles quelles (`CheckBiographyName`, `CheckDebugName`, `CheckHiddenSystemFilesName`, `CheckSqlRiskName`, `CheckViewstatemacName`/`Success`, `CheckPurpose.Header`, `Filename.Header`) |
| Clés mortes `CheckTelerikVulnerability*` | 4 | **intactes** — épinglées par la garde #2118, hors traduction |
| Neutres structurellement vides (`<value />`) | 3 | documentées, **aucune** clé pt créée (source vide = rien à traduire) : `ScannerExplanation.Text`, `ModifiedFilesExplaination.Text`, `ModifiedSettingsExplaination.Text` |

## Le fix

**66 remplacements de spans de valeur + 10 blocs insérés** (ordre ASCII du fichier pt : `CheckTelerikPresence*` entre `CheckSuperuserOldPasswordSuccess` et `CheckTelerikVulnerability*` ; `CheckUserProfilePage*` entre `CheckUnexpectedExtensionsSuccess` et `CheckViewstatemacFailure`). 59 061 → 61 339 octets (**delta 2 278 = somme exacte des deltas par cellule**, contrôlée par assert avant écriture ; BOM et LF-seul inchangés ; XML revalidé bien formé après écriture). Préconditions dérivées de la mesure du jour (taille exacte, valeurs identiques au dump, clés à créer absentes) — toute dérive depuis la mesure aborte avant écriture.

Terminologie ancrée sur les volets déjà balayés du même fichier : **habilitado** pour « enabled » de fonctionnalité ×5 (Registration/Password) vs **ativado/desativado** réservé aux bascules (SSL, IP filters) ; **banco de dados**, **Verificar**, **Configurações do site**, **senha**. Échappement conforme au fichier : `<`/`>` échappés, guillemets littéraux, le `<a href>` de `CheckPasswordFormatFailure` en entités.

## La garde `DnnSecurityPtBrAuditScannerGuardTests` — 5 tests

1. **Balayage du volet** : 38 marqueurs espagnols/galiciens (jamais valides en pt-BR), sur les 84 cellules vivantes de la famille, **hors les 4 mortes de #2118**.
2. **Épingles sentinelles** ×6 : `Check.Text`, `SettingName.Text`, `ScannerChecks.Text`, `CheckViewstatemacReason.Text`, et les 2 cellules que le balayage ne peut structurellement pas voir (`FileName.Text`, `CheckSqlRiskSuccess.Text` — pt valide mais télégraphique, seule l'épingle les tient).
3. **Les 10 clés créées** épinglées + **source vivante** : le neutre doit continuer de porter les 10 clés non vides (épingle littérale sur les deux `Name`).
4. **Inventaire exact** : 88 clés famille au pt (78 + 10), les 4 mortes présentes, les 3 vides structurelles absentes du pt (avec renvoi : si le neutre les remplit un jour, créer les contreparties).
5. **Transport** (BOM, 0 CR) via `DnnResxGuardSocle`.

## Banc de mutations — 5/5

| Cas | Mesuré |
|---|---|
| R1 marqueur renaît dans une cellule **épinglée** (`ScannerChecks.Text` → `base de datos`) | rouge (balayage + épingle : 2 FAIL) |
| R2 marqueur dans une cellule **non épinglée** (`CheckBiographySuccess.Text` → `activado o está`) | **vert d'abord** — trou dans la liste (voir ci-dessous) → rouge après correction (balayage seul : 1 FAIL exactement) |
| R3 une clé créée disparaît (`CheckTelerikPresenceName.Text` supprimée) | rouge (épingles créées + inventaire : 2 FAIL) |
| R4 le neutre perd une clé créée (`CheckUserProfilePageName.Text`) | rouge (source vivante : 1 FAIL) |
| C1 contrôle inverse : CRLF dans l'arbre de travail | **vert** (la garde lit le blob HEAD) |

**Le trou que R2 a exposé avant livraison** : la première liste de 33 marqueurs ne contenait pas `activad` — le balayage serait resté vert devant 6 des cellules originales (`activado`/`desactivado`). Complétée à 38 (`activad`, ` si `, `potenciales`, `normales`, `peligros`) après vérification honnête : **0 faux positif** sur les 84 cellules scannées, et couverture complète des 66 originales sauf les 2 cas structurellement invisibles (couverts par épingles). Le banc a validé l'instrument, pas seulement le fichier.
