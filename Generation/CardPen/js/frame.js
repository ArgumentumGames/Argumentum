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
function autoFitCardTitles(cardNode) {
    var titles = cardNode.querySelectorAll('.title');
    for (var t = 0; t < titles.length; t++) {
        autoFitTitle(titles[t]);
    }
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
