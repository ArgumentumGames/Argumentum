//Script to generate CardPen images in frame using dom-to-image.
//The dimensions and sanitized projectName are loaded elsewhere on the page.

var cards = [];
var nodes = [];



async function generateAndDownloadCard() {

    var dpi = 500;
    var height = 479.04;
    var width = 287.04;
    

    domtoimage.getFontsBefore();
    nodes = document.getElementsByTagName("card");
    var dataUrl = await domtoimage.toPng(nodes[0], { height: height, width: width, scale: dpi / 96, cachedFonts: true });
    cards[0] = dataUrl;
    //var commaIdx = cards[0].indexOf(",");

    //Zero fill for the file name, and add one because CardPen indexes from 1.
    var zerofillplus = ('000' + (1)).slice(-3);
    var cardName = zerofillplus;
    var idNode = nodes[0].querySelector('.cardName');
    if (idNode !== null) {
        cardName = idNode.innerHTML;
    }
    var fileName = cardName + ".png";
    var fileBlob = dataURItoBlob(cards[0]);
    //var fileContent = cards[0].slice(commaIdx + 1);
    saveAs(fileBlob, fileName);

}

function dataURItoBlob(dataURI) {
    // convert base64 to raw binary data held in a string
    // doesn't handle URLEncoded DataURIs - see SO answer #6850276 for code that does this
    var byteString = atob(dataURI.split(',')[1]);

    // separate out the mime component
    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0]

    // write the bytes of the string to an ArrayBuffer
    var ab = new ArrayBuffer(byteString.length);

    // create a view into the buffer
    var ia = new Uint8Array(ab);

    // set the bytes of the buffer to the correct values
    for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }

    // write the ArrayBuffer to a blob, and you're done
    var blob = new Blob([ab], { type: mimeString });
    return blob;

}


async function generateImages() {
    var zipButton = document.getElementById('zipButton');
    zipButton.style.display = 'none';
    var generateButton = document.getElementById('generateButton');
	generateButton.style.display = 'none';
    domtoimage.getFontsBefore();
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
        if (console)
            console.error(msg, error);
		document.getElementById("cpError").innerHTML = msg;
		
	});
}


async function imaginerSync(node, n) {
    try {
		var dataUrl = await domtoimage.toPng(node, { height: height, width: width, scale: dpi / 96, cachedFonts: true });
        cards[n] = dataUrl;
        var img = new Image();
        img.src = dataUrl;
        document.getElementById("cpImages").appendChild(img);
    } catch (error) {
		var msg = 'Something went wrong!  Your browser may not support image generation.';
        if (console)
            console.error(msg, error);
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
