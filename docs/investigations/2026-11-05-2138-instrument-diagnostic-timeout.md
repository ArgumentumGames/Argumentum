# #2138 — Instrument de diagnostic des timeouts `git cat-file` (condition 1)

**Date :** 2026-11-05 — lane po-2024
**Issue :** #2138 — « CI: rafales de timeouts git cat-file 30 s dans DnnResxGuardSocle.CommittedBlob »
**Condition visée :** (1) « un instrument dit, au timeout, si l'échange avait démarré et où il en était »

---

## Le problème que l'instrument lève

Les huit témoins de #2138 produisaient **le même message** pour **trois causes différentes** :

| Cause réelle | Ce que le message disait avant |
|---|---|
| Pool de threads affamé — l'échange n'a jamais commencé | « git n'a pas rendu la main en 30 s : le dépôt est peut-être verrouillé… » |
| git muet — requête écrite, en-tête jamais revenu | idem |
| Tube bloqué au milieu du contenu | idem |

Le message nommait une hypothèse (« verrou, lecteur lent ») que rien ne mesurait, et chaque re-run vert se lisait comme une preuve que ce n'était « rien ». C'est ce qui a permis à la piste du pool de threads de rester SUPPOSÉE pendant huit témoins.

## Ce que l'instrument mesure

`BatchExchangeProgress` horodate chaque étape **avant** l'appel bloquant qui la suit : au timeout, la marque dit ce qui a été **atteint**, jamais ce qui allait l'être. `Snapshot()` relit jusqu'à voir un couple étape/horodatage cohérent, pour qu'un message ne mêle pas l'étape d'un instant à l'horodatage d'un autre.

| Étape marquée | Signification |
|---|---|
| `NotStarted` | aucun thread n'a exécuté le corps de l'échange |
| `Started` | le corps a commencé (il a obtenu un thread) |
| `RequestWritten` | requête écrite **et vidée** sur l'entrée du processus batch |
| `HeaderRead` | en-tête de réponse lu (taille annoncée connue) |
| `BlobRead` | contenu entièrement lu et LF final vérifié |

`DescribeTimeout` en tire **quatre messages distincts** :

1. **jamais démarré** → « POOL DE THREADS affamé, pas git — les 30 s mesurent une attente de thread » ;
2. **démarré, requête non écrite** → blocage à l'entrée du transport ;
3. **requête écrite, aucun en-tête** → « c'est GIT qui est muet » ;
4. **en-tête lu, N/M octets** → « blocage AU MILIEU DU CONTENU, le tube de sortie ne rend plus ».

Le motif « n'a pas rendu la main » est conservé en tête de chaque message : le relevé CI de la condition (3) reste mesurable.

## Contrôles inverses (5 faits, `DnnResxGuardSocleTimeoutDiagnosisTests`)

| Fait | Ce qu'il prouve |
|---|---|
| `A_TaskThatNeverStarted_IsDiagnosedAsThreadPoolStarvation` | une tâche jamais ordonnancée rend le message « pool affamé » |
| `AnExchangeThatWroteItsRequestButGotNoHeader_IsDiagnosedAsSilentGit` | un faux transport muet rend « git muet » |
| `AnExchangeBlockedInsideThePayload_IsDiagnosedAsAPipeThatStoppedMidContent` | en-tête + 10/100 octets rend « au milieu du contenu » |
| `TheThreeCausesProduceThreeDifferentMessages` | les trois textes sont **distincts** — trois cas rendant la même ligne reproduiraient le défaut |
| `ASuccessfulExchangeReachesTheLastStageAndReportsItsByteCount` | le chemin nominal atteint `BlobRead` et compte les bons octets — sans quoi l'instrument pourrait n'afficher que « pas démarré » |

⚠️ **Un banc plus une fois instable, et c'est la mesure qui l'a dit** : le cas 2 a échoué en suite complète et passé seul, parce qu'il appliquait son délai de 250 ms avant que `Task.Run` n'ait seulement démarré. C'est la famine du pool que l'instrument traque, vue à l'échelle du banc. Corrigé en attendant la marque d'étape (`WaitForStage`) avant d'appliquer le délai : un banc qui dépend de l'ordonnanceur est instable par construction.

## Recalibrage des deux motifs (mesuré, pour la condition 3)

Recomptés sur les journaux d'attempt 1 par API (`/actions/runs/<id>/attempts/1/logs`) — sans `--attempt`, `gh run view --log` lit la **dernière** tentative et rend 0 + 0 sur un témoin réel :

| Témoin | Run | Jambe | « n'a pas rendu la main » | « tubes n'ont pas été fermés » |
|---|---|---|---|---|
| 8 | `42022228252` | build (Debug) | **2** | **0** |
| 8 | `42022228252` | build (Release) | 0 | 0 |
| 6 | `41850982011` | build (Release) | **10** | **8** |
| 6 | `41850982011` | build (Debug) | 0 | 0 |

Les chiffres d'ai-01 sont reproduits exactement (2+0 et 10+8).

### Constat : le 2ᵉ motif ne peut plus se produire sur le code mergé

Les lignes du motif 2 nomment `git cat-file **-p** HEAD:…` :

```
System.InvalidOperationException : git cat-file -p HEAD:… : processus sorti, mais ses
tubes n'ont pas été fermés en 30 s (un descendant en garde-t-il l'héritage ?)
```

C'est le transport de l'**option 1** (un `cat-file -p` par fichier), que `9b5df7fc` (option 2) a supprimé :

```
$ git show 9b5df7fc^:…/DnnResxGuardSocle.cs | Select-String "n'ont pas été fermés"
        + $"fermés en {…} s (un descendant en garde-t-il l'héritage ?)");
$ grep -rn "n'ont pas été fermés" --include=*.cs .
(0 résultat)
```

⇒ **La condition (3) « 10 runs sans AUCUN des deux motifs » est, pour le motif 2, déjà satisfaite par construction** : son producteur n'existe plus. Seul le motif 1 reste mesurable. Ce n'est pas une raison d'assouplir la condition, c'est une raison de ne pas lire « 0 + 0 » comme une preuve de bonne santé — le second zéro est structurel.

## Ce que l'instrument ne fait pas

Il ne corrige rien : c'est la condition (2). Il ne décide pas non plus entre les causes — il rend la mesure qui permettra de trancher au prochain témoin. La piste du pool de threads reste **SUPPOSÉE** jusqu'à ce qu'une occurrence réelle tombe sur le message 1.
