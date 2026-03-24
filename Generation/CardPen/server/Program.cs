using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using System.IO;

// Définition des chemins racines
var currentDir = Directory.GetCurrentDirectory();
var cardPenRoot = Path.Combine(currentDir, ".."); // Generation/CardPen/
var cardsRoot = Path.Combine(currentDir, "../../.."); // Racine projet (contient Cards/)

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// IMPORTANT: UseDefaultFiles DOIT être appelé AVANT UseStaticFiles
// Activer les fichiers par défaut (index.html, etc.)
app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = new PhysicalFileProvider(cardPenRoot)
});

// Configuration 1 : Servir Generation/CardPen/ (ressources principales)
// Usage: /js/main.js, /css/style.css, /index.html
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(cardPenRoot),
    RequestPath = "" // Pas de préfixe, comportement par défaut
});

// Configuration 2 : Servir Cards/ avec préfixe /Cards
// Usage: /Cards/Memo/Assets/bg-memo-back.png
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(cardsRoot, "Cards")
    ),
    RequestPath = "/Cards" // Préfixe explicite pour les assets
});

app.Run();
