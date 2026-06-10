//Script to generate CardPen images in frame using dom-to-image.
//The dimensions and sanitized projectName are loaded elsewhere on the page.

var cards = [];
var nodes = [];


// ---------------------------------------------------------------------------
// Auto-shrink des titres (Option B — issue #316)
// Réduit dynamiquement la taille de police d'un titre qui déborde son conteneur
// AVANT la capture domtoimage. No-op si le titre tient déjà (cas Latin courant) :
// n'agit que sur les débordements horizontaux (mots longs non sécables — p.ex.
// cyrillique RU "НЕЙРОЛИНГВИСТИЧЕСКОЕ ПРОГРАММИРОВАНИЕ" — qui, avec
// overflow-wrap:normal, sont rognés au lieu de revenir à la ligne).
// Indépendant de la langue et du gabarit : ne touche que ce qui déborde.
// ---------------------------------------------------------------------------

// Vrai si le contenu de l'élément (et de ses descendants) tient sans débordement
// horizontal. Tolérance de 1px pour les arrondis sub-pixels.
function titleContentFits(el) {
    if (el.scrollWidth > el.clientWidth + 1) {
        return false;
    }
    var descendants = el.getElementsByTagName('*');
    for (var i = 0; i < descendants.length; i++) {
        if (descendants[i].scrollWidth > descendants[i].clientWidth + 1) {
            return false;
        }
    }
    return true;
}

// Recherche dichotomique de la plus grande taille de police qui tient.
function autoFitTitle(title, minFontPx) {
    minFontPx = minFontPx || 8;
    // Repartir de la taille définie par le template (idempotent si rappelé).
    title.style.fontSize = '';
    var naturalPx = parseFloat(window.getComputedStyle(title).fontSize);
    if (!naturalPx || titleContentFits(title)) {
        return; // Tient déjà : aucun changement (cas le plus fréquent).
    }
    var lo = minFontPx;
    var hi = naturalPx;
    var best = minFontPx;
    for (var iter = 0; iter < 12; iter++) {
        var mid = (lo + hi) / 2;
        title.style.fontSize = mid + 'px';
        if (titleContentFits(title)) {
            best = mid;   // tient → viser plus grand
            lo = mid;
        } else {
            hi = mid;     // déborde → réduire
        }
    }
    title.style.fontSize = best + 'px';
}

// Applique l'auto-shrink à tous les titres d'une carte avant sa capture.
// Inclut .subtitle (Mémo Back) : avec white-space:nowrap côté template, un
// sous-titre long (p.ex. RU) déborde horizontalement au lieu de wrapper hors
// bandeau — et se fait réduire ici. No-op pour les sous-titres qui tiennent.
function autoFitCardTitles(cardNode) {
    var titles = cardNode.querySelectorAll('.title, .subtitle');
    for (var t = 0; t < titles.length; t++) {
        autoFitTitle(titles[t]);
    }
}


// ---------------------------------------------------------------------------
// Auto-shrink du corps de texte (issue #190 — Virtues overflow)
// Réduit dynamiquement la taille de police des blocs de texte qui débordent
// verticalement leur conteneur. Cible le bloc .texte (overflow:hidden) et
// réduit la taille de police de ses enfants de texte (.desc_fr, .exemple_fr,
// .desc_*, .exemple_*, etc.) jusqu'à ce que scrollHeight ≤ clientHeight.
// No-op si le texte tient déjà (cas le plus fréquent).
// Indépendant de la langue et du gabarit : ne touche que ce qui déborde.
// ---------------------------------------------------------------------------

// Vrai si le conteneur .texte ne déborde pas verticalement.
// Tolérance de 2px pour les arrondis sub-pixels (aligné sur OverflowDetector.cs).
function bodyContentFits(texteEl) {
    return texteEl.scrollHeight <= texteEl.clientHeight + 2;
}

// Recherche dichotomique de la plus grande taille de police qui tient dans .texte.
// Agit sur tous les enfants de .texte qui ont une font-size héritée.
function autoFitBodyText(cardNode, minFontPx) {
    minFontPx = minFontPx || 7;
    var texteEl = cardNode.querySelector('.texte');
    if (!texteEl) return;

    // Si le texte tient déjà, ne rien faire (cas le plus fréquent).
    if (bodyContentFits(texteEl)) return;

    // Cibler les éléments de texte dans .texte (paragraphes de description + remarque)
    var textChildren = texteEl.querySelectorAll('.desc_fr, .exemple_fr, .desc_en, .exemple_en, .desc_ru, .exemple_ru, .desc_pt, .exemple_pt, .desc_es, .exemple_es, .desc_ar, .exemple_ar, .desc_fa, .exemple_fa, .desc_zh, .exemple_zh');
    if (textChildren.length === 0) return;

    // Sauvegarder les tailles naturelles pour pouvoir repartir de zéro (idempotent).
    var naturalSizes = [];
    for (var i = 0; i < textChildren.length; i++) {
        textChildren[i].style.fontSize = '';
        naturalSizes.push(parseFloat(window.getComputedStyle(textChildren[i]).fontSize));
    }

    // Vérifier si les tailles naturelles tiennent (peut arriver après le reset).
    if (bodyContentFits(texteEl)) return;

    // Binary search : réduire uniformément tous les enfants de texte.
    // On utilise un facteur multiplicatif commun appliqué aux tailles naturelles.
    var lo = 0.3;   // facteur min (30% de la taille naturelle)
    var hi = 1.0;   // facteur max (100% = taille naturelle)
    var best = lo;

    for (var iter = 0; iter < 10; iter++) {
        var mid = (lo + hi) / 2;
        for (var j = 0; j < textChildren.length; j++) {
            var targetPx = Math.max(minFontPx, naturalSizes[j] * mid);
            textChildren[j].style.fontSize = targetPx + 'px';
        }
        if (bodyContentFits(texteEl)) {
            best = mid;   // tient → viser plus grand
            lo = mid;
        } else {
            hi = mid;     // déborde → réduire
        }
    }

    // Appliquer le meilleur facteur trouvé.
    for (var k = 0; k < textChildren.length; k++) {
        var finalPx = Math.max(minFontPx, naturalSizes[k] * best);
        textChildren[k].style.fontSize = finalPx + 'px';
    }
}


// ---------------------------------------------------------------------------
// Auto-shrink de la grille taxonomique (Mémo Back — verdict QA 2026-06-10)
// Une langue aux libellés longs (p.ex. RU) fait déborder la grille
// .familyContainer verticalement : les dernières familles sortent de la carte.
// Toute la grille est dimensionnée en em relatifs à .familyContainer : réduire
// sa font-size rescale uniformément familles, sous-familles et items.
// No-op si la carte tient déjà (cas Latin courant).
// ---------------------------------------------------------------------------

// Vrai si le conteneur ne déborde pas verticalement (tolérance 2px, alignée
// sur bodyContentFits / OverflowDetector.cs).
function cardContentFits(containerEl) {
    return containerEl.scrollHeight <= containerEl.clientHeight + 2;
}

// Recherche dichotomique de la plus grande font-size de .familyContainer qui
// fait tenir la carte en hauteur.
function autoFitFamilyGrid(cardNode, minFontPx) {
    minFontPx = minFontPx || 3;
    var grid = cardNode.querySelector('.familyContainer');
    if (!grid) return;
    var container = cardNode.querySelector('.cardContainer') || cardNode;
    // Repartir de la taille définie par le template (idempotent si rappelé).
    grid.style.fontSize = '';
    var naturalPx = parseFloat(window.getComputedStyle(grid).fontSize);
    if (!naturalPx || cardContentFits(container)) {
        return; // Tient déjà : aucun changement (cas le plus fréquent).
    }
    var lo = minFontPx;
    var hi = naturalPx;
    var best = minFontPx;
    for (var iter = 0; iter < 10; iter++) {
        var mid = (lo + hi) / 2;
        grid.style.fontSize = mid + 'px';
        if (cardContentFits(container)) {
            best = mid;   // tient → viser plus grand
            lo = mid;
        } else {
            hi = mid;     // déborde → réduire
        }
    }
    grid.style.fontSize = best + 'px';
}


async function generateImages() {
    var zipButton = document.getElementById('zipButton');
    if (zipButton) {
        zipButton.style.display = 'none';
    }
    var generateButton = document.getElementById('generateButton');
    if (generateButton) {
        generateButton.style.display = 'none';
    }
    // Précharger les polices avant génération (restauré du Golden Master)
    domtoimage.getFontsBefore();
    nodes = document.getElementsByTagName("card");
       for (var n = 0; n < nodes.length; n++) {
           //imaginer(nodes[n],n);
           autoFitCardTitles(nodes[n]);   // Option B (#316) : ajuster les titres débordants avant capture
           autoFitBodyText(nodes[n]);     // Issue #190 : ajuster le corps de texte si débordement vertical
           autoFitFamilyGrid(nodes[n]);   // QA 2026-06-10 : grille Mémo Back qui déborde verticalement (RU)
           await imaginerSync(nodes[n], n);
       }
    if (zipButton) {
        zipButton.style.display = 'block';
    }
}


function imaginer(node,n) {
	//Need to pass height and width due to an issue with oversized transparent canvases (#50).
	//domtoimage.toPng(node, { height: height, width: width, dpi: dpi }).then(function (dataUrl) {
    domtoimage.toPng(node, { height: height, width: width, scale: dpi / 150 }).then(function (dataUrl) {
  cards[n] = dataUrl;
		var img = new Image();
		img.src = dataUrl;
		document.getElementById("cpImages").appendChild(img);
	}).catch(function (error) {
		var msg = 'Something went wrong!  Your browser may not support image generation.';
        if (console)
            console.error(msg, error);
		document.getElementById("cpError").innerHTML = msg;
		
	});
}


async function imaginerSync(node, n) {
    try {
        console.log(`[imaginerSync] Processing node ${n}`, node);
        // Restauré du Golden Master: scale et cachedFonts au lieu de webfont
        const options = {
            height: height,
            width: width,
            scale: dpi / 96,  // Restauré - important pour la résolution correcte
            cachedFonts: true,  // Restauré - utilise les polices préchargées
            imagePlaceholder: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/wcAAwAB/epv2AAAAABJRU5ErkJggg=="
        };
        console.log(`[imaginerSync] Options for domtoimage:`, options);
        console.log(`[imaginerSync] Node HTML content for node ${n}:`, node.outerHTML);

  var dataUrl = await domtoimage.toPng(node, options);

        console.log(`[imaginerSync] Received dataUrl for node ${n}: ${dataUrl.substring(0, 100)}...`);

        if (dataUrl === 'data:,') {
            throw new Error('domtoimage.toPng returned an empty data URL.');
        }

		cards[n] = dataUrl;
        var img = new Image();
        img.src = dataUrl;
        document.getElementById("cpImages").appendChild(img);
    } catch (error) {
        var msg = `[imaginerSync] Error processing node ${n}: ${error.toString()}`;
        console.error(msg, error);
        var errorDiv = document.createElement('div');
        errorDiv.className = 'cp-js-error';
        errorDiv.innerHTML = msg;
        document.body.appendChild(errorDiv); // Make error visible to Playwright
        document.getElementById("cpError").innerHTML = msg;
    }
}

	
function zipper() {
	var zip = new JSZip();
	var cardZip = zip.folder(projectName);
	
	for (var c = 0; c < cards.length; c++) {
		var commaIdx = cards[c].indexOf(",");
		
		//Zero fill for the file name, and add one because CardPen indexes from 1.
		var zerofillplus = ('000' + (c + 1)).slice(-3);
		var cardName = zerofillplus;
		var idNode = nodes[c].querySelector('.cardName');
		if (idNode !== null) {
            cardName = idNode.innerHTML;
        }
		cardZip.file(projectName + cardName + ".png", cards[c].slice(commaIdx + 1), {base64: true}); 
	}
  zip.generateAsync({type:"blob"}).then(function(file){
    saveAs(file, projectName + "_cards.zip");
  });
}
