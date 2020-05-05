//Script to generate CardPen images in frame using dom-to-image.
//The dimensions and sanitized projectName are loaded elsewhere on the page.

var cards = [];
var nodes = [];
var zipButton;

window.onload = function () {
    zipButton = document.getElementById('zipButton');
    zipButton.style.display = 'none';
    
}

async function generateImages() {
    var generateButton = document.getElementById('generateButton');
    generateButton.style.display = 'none';
	nodes = document.getElementsByTagName("card");
    for (var n = 0; n < nodes.length; n++) {
        //imaginer(nodes[n],n);
        await imaginerSync(nodes[n], n);
    }
    zipButton.style.display = 'block';
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
		document.getElementById("cpError").innerHTML = msg;
		if (console) 
			console.error(msg, error);
	});
}


async function imaginerSync(node, n) {
    try {
        var dataUrl = await domtoimage.toPng(node, { height: height, width: width, scale: dpi / 96 });
        cards[n] = dataUrl;
        var img = new Image();
        img.src = dataUrl;
        document.getElementById("cpImages").appendChild(img);
    } catch (error) {
        var msg = 'Something went wrong!  Your browser may not support image generation.';
        document.getElementById("cpError").innerHTML = msg;
        if (console)
            console.error(msg, error);
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
