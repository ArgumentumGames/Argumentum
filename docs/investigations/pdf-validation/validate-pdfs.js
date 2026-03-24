const { chromium } = require('playwright');
const path = require('path');
const fs = require('fs');

// Configuration
const PDF_DIR = path.resolve(__dirname, '../../../Generation/Converters/Argumentum.AssetConverter/bin/Release/net9.0/Target/fr/Documents/density-0/');
const SCREENSHOT_DIR = path.resolve(__dirname, '../screenshots/validation-finale-' + new Date().toISOString().replace(/[:.]/g, '-'));

async function validatePdfs() {
    console.log(`Recherche des PDFs dans : ${PDF_DIR}`);
    
    if (!fs.existsSync(PDF_DIR)) {
        console.error(`Erreur : Le dossier PDF n'existe pas : ${PDF_DIR}`);
        return;
    }

    if (!fs.existsSync(SCREENSHOT_DIR)) {
        fs.mkdirSync(SCREENSHOT_DIR, { recursive: true });
        console.log(`Dossier de screenshots créé : ${SCREENSHOT_DIR}`);
    }

    const files = fs.readdirSync(PDF_DIR).filter(file => file.toLowerCase().endsWith('.pdf'));
    
    if (files.length === 0) {
        console.warn("Aucun fichier PDF trouvé.");
        return;
    }

    console.log(`${files.length} fichiers PDF trouvés.`);

    // Lancement avec les arguments pour autoriser l'accès aux fichiers locaux si nécessaire,
    // bien que l'approche base64 contourne ce problème.
    const browser = await chromium.launch();
    const page = await browser.newPage();

    // HTML template utilisant PDF.js pour le rendu
    // Nous utilisons une version CDN de PDF.js
    const htmlTemplate = `
    <!DOCTYPE html>
    <html>
    <head>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.16.105/pdf.min.js"></script>
        <style>
            body { margin: 0; overflow: hidden; }
            #the-canvas { border: 1px solid black; }
        </style>
    </head>
    <body>
        <canvas id="the-canvas"></canvas>
        <script>
            // Le worker doit être défini
            pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.16.105/pdf.worker.min.js';

            async function renderPdf(base64Data) {
                try {
                    const pdfData = atob(base64Data);
                    const loadingTask = pdfjsLib.getDocument({data: pdfData});
                    const pdf = await loadingTask.promise;
                    
                    // Récupérer la première page
                    const page = await pdf.getPage(1);
                    
                    const scale = 1.0;
                    const viewport = page.getViewport({scale: scale});

                    const canvas = document.getElementById('the-canvas');
                    const context = canvas.getContext('2d');
                    canvas.height = viewport.height;
                    canvas.width = viewport.width;

                    const renderContext = {
                        canvasContext: context,
                        viewport: viewport
                    };
                    
                    await page.render(renderContext).promise;
                    return "Rendered";
                } catch (e) {
                    console.error(e);
                    return "Error: " + e.message;
                }
            }
        </script>
    </body>
    </html>
    `;

    for (const file of files) {
        const pdfPath = path.join(PDF_DIR, file);
        const screenshotPath = path.join(SCREENSHOT_DIR, `${file}.png`);
        
        console.log(`Traitement de : ${file}`);
        
        try {
            // Charger le contenu du PDF en Base64
            const pdfData = fs.readFileSync(pdfPath).toString('base64');

            // Charger le template HTML
            await page.setContent(htmlTemplate);

            // Exécuter le rendu dans le contexte de la page
            const result = await page.evaluate(async (data) => {
                return await renderPdf(data);
            }, pdfData);

            if (result !== "Rendered") {
                console.error(`Erreur de rendu PDF.js pour ${file}: ${result}`);
                continue;
            }
            
            // Attendre un court instant pour s'assurer que le canvas est peint
            await page.waitForTimeout(500);
            
            // Screenshot du canvas uniquement
            const canvas = await page.locator('#the-canvas');
            await canvas.screenshot({ path: screenshotPath });
            
            console.log(`Screenshot sauvegardé : ${screenshotPath}`);

        } catch (error) {
            console.error(`Erreur lors du traitement de ${file}:`, error);
        }
    }

    await browser.close();
    console.log("Validation terminée.");
}

validatePdfs().catch(console.error);