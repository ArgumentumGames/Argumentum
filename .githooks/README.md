# Git hooks (Argumentum)

This repo ships shared git hooks in `.githooks/`. To activate them once per clone:

```bash
git config core.hooksPath .githooks
```

## pre-commit — web.config secret guard (`#131` Phase D)

**Why:** `DNNPlatform/web.config` is git-tracked but the committed copy on `master`
is a **placeholder template** (`Data Source=REPLACE`, `validationKey="REPLACE"`,
`decryptionKey="REPLACE"` — keys excluded from the repo by design, per `#442`).

At **runtime**, DNN expands these placeholders to real credentials (SQL password +
machineKey) and rewrites the file in-place. The expanded file then shows as
modified (`M`) in `git status`, creating a permanent risk of accidentally
`git add`-ing and committing the real password or machineKey.

**What the hook does:** if `DNNPlatform/web.config` is staged and contains a
**real** (non-placeholder) secret pattern, the commit is **rejected**:

| Pattern | Blocks when |
|---|---|
| `Password=<value>` | value is not `REPLACE` / `<<...>>` / `${...}` |
| `validationKey="<16+ hex>"` | real machineKey validation key present |
| `decryptionKey="<16+ hex>"` | real machineKey decryption key present |

The master placeholder template passes (it only contains `REPLACE` tokens).

**This is defense-in-depth**, orthogonal to machineKey rotation (which is a
server-ops task, `jsboige`-gated). It does **not** modify the template itself.

### Bypass (dangerous)

```bash
git commit --no-verify   # bypass ONCE — review with a coordinator first
```
