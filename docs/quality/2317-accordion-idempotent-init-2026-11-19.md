# #2317 — accordéon BS5 : initialisation idempotente

## Cause mesurée

L'accueil contient deux instances de l'app Accordion4. Chaque `_Accordion.PartAssets.cshtml`
active `window.appAccordion4.init()` via `turnOn`, alors que le bundle BS5 parcourt **tous**
les titres `[data-accordion-parent]` à chaque appel. Chaque titre recevait donc deux écouteurs :
au clic, `hide()` puis `show()` se succédaient et laissaient le panneau ouvert.

La correction porte sur la variante **BS5, celle effectivement servie**. Elle marque chaque titre
une fois branché (`data-accordion-initialized`) sans changer la mécanique `slidetoggle`, le hash
ou le défilement du lien direct. `bs5/dist/scripts.min.js` et sa map ont été régénérés par Webpack ;
les variantes BS3/BS4, non servies par la recette, ne sont pas modifiées.

## Contrat exécutable

```bash
node DNNPlatform/Portals/1/2sxc/Accordion4/test_2317-accordion-init.cjs
```

Node standard uniquement, aucun `npm install`, réseau ou déploiement. Le test exécute **le bundle
minifié commité**, appelle `init()` deux fois avec les mêmes options que les deux instances `turnOn`,
puis rejoue les `requestAnimationFrame` et `transitionend` jusqu'à stabilisation.

- Deux largeurs logiques (390 et 1280) : un seul écouteur par titre ; état initialement ouvert,
  fermeture durable, réouverture ; hash `#11889` conservé ; autre instance non animée.
- Lien direct `#11889` : ouverture et défilement une seule fois malgré les deux initialisations.
- Visibilité : `display` est lu via `getComputedStyle`, avec le défaut CSS `display:none`, car
  `slidetoggle.hide()` n'écrit pas lui-même cette propriété à la fin de l'animation.

Le contrat final a été rejoué contre `git show HEAD:…/bs5/dist/scripts.min.js` : les **5 cas sont
rouges** sur l'ancien blob (2 écouteurs, panneau toujours ouvert). Sur le bundle corrigé : **5/5
verts**, 0 écouteur `transitionend` résiduel après stabilisation. Le job CI `accordion4-bs5-contract`
exécute le même fichier, sans installation de dépendances.

## Reconstruction du bundle

Le lockfile historique contient un conflit de pairs Webpack déjà présent ; la résolution doit être
`--legacy-peer-deps`, sans mise à jour du lockfile. Webpack 5.53 nécessite le fournisseur OpenSSL
historique sous Node récent, et son mode `watch` doit être interrompu après la compilation :

```powershell
npm --prefix DNNPlatform/Portals/1/2sxc/Accordion4 ci --ignore-scripts --no-audit --no-fund --legacy-peer-deps
$env:NODE_OPTIONS = '--openssl-legacy-provider'
npm --prefix DNNPlatform/Portals/1/2sxc/Accordion4 run build-bs5
# Attendre "Compiled successfully", puis arrêter le watcher.
```

La CSS produite a le même blob git que la version d'origine ; elle n'est donc pas incluse dans le
correctif. `scripts.min.js.map` embarque le source TypeScript corrigé dans `sourcesContent`, octets
et CRLF compris.

## Non vérifié

Aucun déploiement et aucune écriture préprod/prod. Le contrat Node prouve le comportement du bundle
sur le DOM minimal de la recette ; il ne remplace pas la recette navigateur après livraison de la
cible servie. Le test de largeur change les scénarios logiques, sans prétendre émuler un moteur de
rendu mobile.

Refs #2317.
