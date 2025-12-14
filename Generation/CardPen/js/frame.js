//Script to generate CardPen images in frame using dom-to-image.
//The dimensions and sanitized projectName are loaded elsewhere on the page.

var cards = [];
var nodes = [];


async function generateImages() {
    var zipButton = document.getElementById('zipButton');
    if (zipButton) {
        zipButton.style.display = 'none';
    }
    var generateButton = document.getElementById('generateButton');
    if (generateButton) {
        generateButton.style.display = 'none';
    }
   nodes = document.getElementsByTagName("card");
       for (var n = 0; n < nodes.length; n++) {
           //imaginer(nodes[n],n);
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
        const options = { height: height, width: width, webfont: true, /* scale: dpi / 96, */ imagePlaceholder: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/wcAAwAB/epv2AAAAABJRU5ErkJggg==" };
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
