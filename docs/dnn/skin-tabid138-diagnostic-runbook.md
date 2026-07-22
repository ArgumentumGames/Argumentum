# Skin `tabid=138` — Diagnostic Runbook (HTTP 500, gated v0.9.1)

> **Scope** : diagnostic + pistes de résolution à exécuter au **GO jsboige avec accès serveur VPS**.
> **Statut** : **REPORTED, non reproduit**. Ce document pré-arme le diagnostic ; il n'est PAS un diff fix copy-paste garanti (voir §4).
> **Auteur** : po-2023 (dispatch ai-01 `msg-20260722T144728-h7msce`, secondaire « runbooks gated-ops staged »).
> **Non-bloquant v0.9.0** (reco Opt 1 : fix reporté v0.9.1, tag non bloqué).

---

## 1. Symptôme rapporté

| Champ | Valeur |
|-------|--------|
| URL | `dnn.argumentum.myia.io` page `tabid=138` |
| HTTP | **500** |
| Exception | `HttpParseException` sur le contrôle **`lblBreadCrumb`** |
| Description reçue | « Type `Literal` incompatible avec `Label` (.NET 9 BCL). Skin non re-compilée post-migration IIS 2026-07-01. » |
| Source | status dashboard workspace-Argumentum (reported, pas vérifié par reproduction locale) |

**Runtime DNN = .NET Framework 4.8** (vérifié `DotNetNuke.dll` FileVersion `10.3.2+...`, repo-runtime sandbox `dnn/sandbox-runtime-1032`). La mention « .NET 9 BCL » dans le symptôme reçu est **à qualifier** au diagnostic (le runtime n'est pas .NET 9 — possiblement confusion BCL des redirects 9.0.0.0 vs runtime net48, cf `[[reference-dnn-2sxc-net48-bcl-stack]]`).

---

## 2. Constat dans le repo (code = truth)

Le contrôle `lblBreadCrumb` vit dans le **skin object DNN core** :

```
DNNPlatform/admin/Skins/breadcrumb.ascx
```

```aspx
<%@ Control Language="C#" AutoEventWireup="false" Inherits="DotNetNuke.UI.Skins.Controls.BreadCrumb" Codebehind="BreadCrumb.ascx.cs" %>
<asp:label id="lblBreadCrumb" runat="server" EnableViewState="False" itemprop="breadcrumb" itemscope itemtype="https://schema.org/breadcrumb"/>
```

**Le markup utilise déjà `<asp:label>`** (pas `Literal`). Donc l'exception « Literal incompatible avec Label » ne provient **pas** du markup tel que commité. Le code-behind `BreadCrumb.ascx.cs` **n'est pas dans le repo** (compilé dans `DotNetNuke.dll`).

Skins présents dans le repo (`DNNPlatform/Portals/_default/Skins/`) : `2shineBS5`, `Bootstrap 4 Instant`, `DnnBootsterV2`, `DnnContra`, `OpenStoreBO`, `Xcillion`, `_default`, `nvQuickTheme`. Le skin appliqué à `tabid=138` est à confirmer côté serveur (DB DNN `Tabs.SkinSrc` pour le tab 138).

---

## 3. Pourquoi pas de fix copy-paste maintenant

Un diff « Literal → Label » ne peut pas être préparé en staged sans risque de **fabriquer un fix non vérifié** car :

1. **Pas de stack trace live** → le contrôle `.ascx` exact qui plante n'est pas localisé (le message pointe `lblBreadCrumb`, mais le markup core est déjà `Label`).
2. **Code-behind absent du repo** → le mismatch `Literal`/`Label` est probablement dans `BreadCrumb.ascx.cs` (compilé) ou dans un binding d'un custom skin, non inspectable depuis le repo seul.
3. **Symptôme reçu ambigu** (`.NET 9 BCL` vs runtime net48 réel).

Conformément à la discipline **Code = Truth** (VERIFIE / RAPPORTE / SUPPOSE), ce runbook reste **diagnostic** tant que le stack trace live n'a pas été capturé.

---

## 4. Pistes de diagnostic à exécuter au GO (avec accès serveur VPS)

Ordre recommandé :

1. **Capturer le stack trace `HttpParseException` exact** :
   - Event Viewer Windows → Journaux Windows → Application (filtrer source *ASP.NET* sur l'heure du 500).
   - Ou DNN event log : `DNNPlatform/Portals/_default/Logs/` + table `EventLog`.
   - Objectif : identifier le **chemin `.ascx` exact** + la ligne + le type fautif (`Literal` vs `Label`).

2. **Confirmer le skin appliqué au tab 138** (DB DNN) :
   ```sql
   SELECT TabID, TabName, SkinSrc, ContainerSrc FROM Tabs WHERE TabID = 138;
   ```
   + `PortalSettings.SkinSrc` par défaut si `SkinSrc` est NULL.

3. **Selon le contrôle fautif** :
   - **Si skin object core `breadcrumb.ascx`** → le mismatch est dans `BreadCrumb.ascx.cs` compilé (déclare `protected Literal lblBreadCrumb` alors que le markup dit `Label`). Fix : aligner la déclaration code-behind → recompiler `DotNetNuke.dll` (territoire core DNN, pas custom). **Alternative plus rapide** : surcharger le skin object dans le skin custom pour éviter le core fautif.
   - **Si custom skin (`2shineBS5` / `Bootstrap 4 Instant`)** → corriger le `.ascx` du custom skin (diff trivial une fois le fichier localisé), re-publier le skin.
   - **Si post-migration (skin non recompilée 2026-07-01)** → recompiler / re-publier le skin concerné (le `bin/` runtime sandbox `4b0297ee` est la référence net48 propre, cf `[[reference-dnn-bin-restore-surgical]]`).

4. **Smoke post-fix** : recharger `tabid=138` → HTTP 200, breadcrumb rendu visible. Puis `docs/dnn/go-live-smoke-test.md`.

---

## 5. Recommandation produit

- **Opt 1 (reco ai-01 + jsboige)** : le fix skin **ne bloque pas le tag v0.9.0**. Gater le tag, traiter le skin en **v0.9.1**. Le rendu de `tabid=138` est une page secondaire ; la homepage et `/Argumentum` + `/Règles` rendent (2sxc content OK, cf B2.5 smoke).
- **Opt 2 (écarté)** : scope creep skin maintenant (retarde le tag pour un fix non-bloquant).

---

## 6. Cross-refs

- `[[feedback-dnn-webconfig-secret-tracked]]` — web.config runtime-expanded (gated ops).
- `[[reference-dnn-2sxc-net48-bcl-stack]]` — runtime net48 vs redirects 9.0.0.0 (contexte « .NET 9 BCL »).
- `docs/dnn/go-live-turnkey-checklist.md` — Manifests Δ #490/#682 (couvert).
- `docs/dnn/dnn-hang-option-c-connection-string-prep.md` — Option C connection-string (couvert).
- `docs/dnn/UPGRADE-ASSESSMENT.md` — cible DNN 10.3.2 + 2sxc 21.
