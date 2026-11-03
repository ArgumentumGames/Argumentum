# #458 — 4ᵉ phrase de `EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_BODY` en ar-SA / fa-IR / zh-CN + retrait de la déclaration de dérive (2026-11-03)

**Lane :** po-2024 · **Pool :** #458 c.6596594457, item 2 · **Base :** `a57a5ec7` (master au geste).

## 1. L'état mesuré avant le geste (blob `HEAD:`)

La montée DNN 10.4.1 (#2127) a réécrit la valeur neutre (304 → 398 caractères) en
ajoutant une phrase qui nomme le portail une **troisième** fois. Mesure par pack,
lecture `HEAD:` (la garde lit `HEAD:`, le banc aussi) :

| Pack | Jetons `[Xxx:Yyy]` | Phrase en position 3 | Couvert par la garde ? |
|---|---:|---|---|
| **neutre** | **4** (3× `[Portal:PortalName]` + 1× `[User:DisplayName]`) | « The administrator for [Portal:PortalName] has been notified for review and will issue another password reset email if your account is approved. » | — |
| es-ES | 4 | **phrase nouvelle déjà traduite** (« El administrador de [Portal:PortalName] ha recibido una notificación… ») — paquet officiel aligné par la montée | non (hors périmètre de la garde) |
| fr-FR | 3 (`[Site: PortalName]` ×2 + `[User: DisplayName]`) | ancienne phrase, mise en forme `<br />` | non |
| ru-RU | 3 | ancienne phrase | non |
| pt-BR | 3 | ancienne phrase | non |
| **ar-SA / fa-IR / zh-CN** | **3** | ancienne phrase (« le mot de passe peut être réinitialisé une fois le compte approuvé ») | **oui** — déclaration `(3, 4)` |

⇒ La déclaration de dérive `NeutralRewrittenPendingRetranslation` ne concerne que
les trois packs couverts par `DnnAgrCoreResxIntegrityGuardTests` (ar-SA, fa-IR,
zh-CN — 172 clés `GlobalResources` chacun) : fr-FR / es-ES / ru-RU / pt-BR ne
passent pas par cette garde, et es-ES porte déjà la phrase nouvelle. **Le retrait
de la déclaration ne peut donc pas rougir un pack non couvert.**

## 2. Les trois traductions — terminologie du pack, jamais inventée

Chaque ancienne phrase (unique dans son fichier, asserté `count == 1`) est
remplacée par la traduction de la phrase nouvelle, **inline** (le remplacement ne
contient aucun saut de ligne) :

| Pack | Ancienne (remplacée) | Nouvelle |
|---|---|---|
| ar-SA | يمكن إعادة تعيين كلمة المرور بعد الموافقة على الحساب. | تم إشعار مسؤول [Portal:PortalName] لمراجعة حسابك، وسيتم إرسال بريد إلكتروني آخر لإعادة تعيين كلمة المرور إليك إذا تمت الموافقة على حسابك. |
| fa-IR | گذرواژه پس از تأیید حساب قابل بازنشانی است. | مدیر [Portal:PortalName] برای بررسی حساب شما مطلع شده است و در صورت تأیید حساب شما، رایانامه دیگری برای بازنشانی گذرواژه برایتان ارسال خواهد کرد. |
| zh-CN | 帐户获批后才能重置密码。 | [Portal:PortalName] 的管理员已收到通知，将对您的帐户进行审核；如果您的帐户获批，将另行向您发送密码重置邮件。 |

Provenance de chaque terme, comptée dans le pack **avant** d'écrire :

| Terme | ar-SA | fa-IR | zh-CN |
|---|---:|---:|---:|
| « administrateur » | مسؤول 8× | مدیر 12× | 管理员 3× |
| « e-mail » | بريد 25× | رایانامه 14× (ایمیل **0×** — le pack n'emploie jamais la translittération) | 邮件 28× |
| « notification/notifié » | إشعار 14× | مطلع 3× | 通知 12× |
| « révision/examen » | مراجعة 4× | بررسی 8× | 审核 1× |
| « approbation » (déjà ligne 2 du courriel) | الموافقة | تأیید | 审批 |

Sémantique de référence : le neutre, corroboré par la traduction officielle
es-ES livrée par la montée.

## 3. Chirurgie — contrôles d'octets

Par fichier : BOM détectée et ré-écrite (`utf-8-sig` → `\xEF\xBB\xBF` ré-prépendu),
ancre unique (`count == 1`), **delta d'octets asserté égal à la différence des
longueurs encodées**, compte de `\r` inchangé (remplacement inline), XML revalidé.

| Pack | Delta octets | Jetons de la clé après |
|---|---:|---|
| ar-SA | +137 | 3× `[Portal:PortalName]` + 1× `[User:DisplayName]` = 4 |
| fa-IR | +169 | idem = 4 |
| zh-CN | +116 | idem = 4 |

## 4. Banc rouge-avant / vert-après — mesuré en nombre

La garde lit `HEAD:` : le banc exige donc le commit des packs **avant** de lire la
couleur. Séquence exécutée :

1. commit des 3 packs, **déclaration encore présente** →
   `dotnet test --filter DnnAgrCoreResxIntegrityGuardTests` :
   **3 échecs nominatifs** (`EveryDeliveredKeyIsSaneAgainstTheNeutral` ×
   {ar-SA, fa-IR, zh-CN} × GlobalResources), message exact
   « le pack a changé de nombre de jetons… (ou retirer la déclaration si la
   traduction est refaite), but found 4 » — 49 pass / 3 fail / 52.
2. retrait de l'entrée `[("GlobalResources", "EMAIL_…BODY.Text")] = (3, 4)` →
   **52/52 vert**. La table reste (mécanisme déclaratif pour la prochaine montée
   qui réécrit un neutre), **vide**, avec le cas fondateur consigné en commentaire
   comme retiré.

## 5. Hors périmètre

- fr-FR / ru-RU / pt-BR portent aussi l'ancienne phrase (et fr-FR une mise en
  forme `<br />` avec jetons `[Site:]`) — **hors garde**, non touchés par ce grain ;
  à couvrir si le pool l'arbitre.
- Les 30 clés neuves `GlobalResources` restent à couvrir dans les 5 autres packs
  (fr-FR fait par #2133).

*Instrument : `git show HEAD:<resx>` + parseur XML (jamais grep sur octets bruts),
garde `DnnAgrCoreResxIntegrityGuardTests` (socle `DnnResxGuardSocle.CommittedBlob`).*
