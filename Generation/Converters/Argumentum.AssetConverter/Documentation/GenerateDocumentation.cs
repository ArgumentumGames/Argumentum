using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
// Commenté temporairement pour permettre la compilation
// using Markdig;
// using Markdig.Extensions.AutoIdentifiers;
// using Markdig.Extensions.Tables;
// using Markdig.Renderers;
// using Markdig.Syntax;
// using Markdig.Syntax.Inlines;

namespace Argumentum.AssetConverter.Documentation
{
    /// <summary>
    /// Classe responsable de la génération de documentation HTML à partir de fichiers Markdown.
    /// </summary>
    public partial class DocumentationGenerator
    {
        private readonly string _inputDirectory;
        private readonly string _outputDirectory;
        private readonly string _cssPath;
        private readonly string _jsPath;
        private readonly List<DocumentationFile> _documentationFiles;
        private readonly Dictionary<string, List<DocumentationHeading>> _headingsByFile;
        private readonly Dictionary<string, List<string>> _searchIndex;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="DocumentationGenerator"/>.
        /// </summary>
        /// <param name="inputDirectory">Le répertoire contenant les fichiers Markdown.</param>
        /// <param name="outputDirectory">Le répertoire de sortie pour les fichiers HTML.</param>
        /// <param name="cssPath">Le chemin du fichier CSS personnalisé (optionnel).</param>
        /// <param name="jsPath">Le chemin du fichier JavaScript personnalisé (optionnel).</param>
        public DocumentationGenerator(string inputDirectory, string outputDirectory, string cssPath = null, string jsPath = null)
        {
            _inputDirectory = inputDirectory;
            _outputDirectory = outputDirectory;
            _cssPath = cssPath;
            _jsPath = jsPath;
            _documentationFiles = new List<DocumentationFile>();
            _headingsByFile = new Dictionary<string, List<DocumentationHeading>>();
            _searchIndex = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// Génère la documentation HTML à partir des fichiers Markdown.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task GenerateAsync()
        {
            Console.WriteLine("Génération de la documentation...");

            // Créer le répertoire de sortie s'il n'existe pas
            if (!Directory.Exists(_outputDirectory))
            {
                Directory.CreateDirectory(_outputDirectory);
            }

            // Copier les fichiers CSS et JS s'ils existent
            if (!string.IsNullOrEmpty(_cssPath) && File.Exists(_cssPath))
            {
                File.Copy(_cssPath, Path.Combine(_outputDirectory, "style.css"), true);
            }
            else
            {
                await CreateDefaultCssFile();
            }

            if (!string.IsNullOrEmpty(_jsPath) && File.Exists(_jsPath))
            {
                File.Copy(_jsPath, Path.Combine(_outputDirectory, "script.js"), true);
            }
            else
            {
                await CreateDefaultJsFile();
            }

            // Analyser les fichiers Markdown
            await ParseMarkdownFiles();

            // Générer les fichiers HTML
            await GenerateHtmlFiles();

            // Générer la table des matières
            await GenerateTableOfContents();

            // Générer l'index de recherche
            await GenerateSearchIndex();

            // Générer la page d'accueil
            await GenerateHomePage();

            Console.WriteLine("Documentation générée avec succès dans le répertoire : " + _outputDirectory);
        }

        /// <summary>
        /// Analyse les fichiers Markdown dans le répertoire d'entrée.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        private async Task ParseMarkdownFiles()
        {
            var markdownFiles = Directory.GetFiles(_inputDirectory, "*.md", SearchOption.AllDirectories);

            foreach (var file in markdownFiles)
            {
                var relativePath = Path.GetRelativePath(_inputDirectory, file);
                var outputPath = Path.ChangeExtension(Path.Combine(_outputDirectory, relativePath), ".html");
                var content = await File.ReadAllTextAsync(file);

                var documentationFile = new DocumentationFile
                {
                    InputPath = file,
                    OutputPath = outputPath,
                    Content = content,
                    Title = ExtractTitle(content),
                    RelativePath = relativePath
                };

                _documentationFiles.Add(documentationFile);
                _headingsByFile[relativePath] = ExtractHeadings(content);
                _searchIndex[relativePath] = ExtractSearchTerms(content);
            }
        }

        /// <summary>
        /// Extrait le titre du fichier Markdown.
        /// </summary>
        /// <param name="content">Le contenu du fichier Markdown.</param>
        /// <returns>Le titre du fichier.</returns>
        private string ExtractTitle(string content)
        {
            var match = Regex.Match(content, @"^\s*#\s+(.+)$", RegexOptions.Multiline);
            return match.Success ? match.Groups[1].Value.Trim() : "Documentation";
        }

        /// <summary>
        /// Extrait les titres du fichier Markdown.
        /// </summary>
        /// <param name="content">Le contenu du fichier Markdown.</param>
        /// <returns>La liste des titres.</returns>
        private List<DocumentationHeading> ExtractHeadings(string content)
        {
            var headings = new List<DocumentationHeading>();
            // Commenté temporairement pour permettre la compilation
            /*
            var pipeline = new MarkdownPipelineBuilder().UseAutoIdentifiers(AutoIdentifierOptions.GitHub).Build();
            var document = Markdown.Parse(content, pipeline);

            foreach (var heading in document.Descendants<HeadingBlock>())
            {
                var title = heading.Inline?.FirstChild is LiteralInline literalInline
                    ? literalInline.Content.ToString()
                    : string.Empty;

                var id = heading.GetAttributes().Id ?? AutoIdentifierExtension.GetHeadingId(heading);

                headings.Add(new DocumentationHeading
                {
                    Level = heading.Level,
                    Title = title,
                    Id = id
                });
            }
            */

            return headings;
        }

        /// <summary>
/// <summary>
        /// Génère les fichiers HTML à partir des fichiers Markdown.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        private async Task GenerateHtmlFiles()
        {
            // Commenté temporairement pour permettre la compilation
            /*
            var pipeline = new MarkdownPipelineBuilder()
                .UseAutoIdentifiers(AutoIdentifierOptions.GitHub)
                .UseAdvancedExtensions()
                .UsePipeTables()
                .UseGridTables()
                .UseTaskLists()
                .UseDiagrams()
                .Build();
            */

            foreach (var file in _documentationFiles)
            {
                var directory = Path.GetDirectoryName(file.OutputPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Commenté temporairement pour permettre la compilation
                // var html = Markdown.ToHtml(file.Content, pipeline);
                var html = file.Content; // Utilisation temporaire du contenu brut
                var htmlContent = GenerateHtmlPage(file.Title, html, file.RelativePath);

                await File.WriteAllTextAsync(file.OutputPath, htmlContent);
            }
        }

        /// <summary>
        /// Génère une page HTML complète.
        /// </summary>
        /// <param name="title">Le titre de la page.</param>
        /// <param name="content">Le contenu HTML.</param>
        /// <param name="relativePath">Le chemin relatif du fichier.</param>
        /// <returns>Le contenu HTML complet.</returns>
        private string GenerateHtmlPage(string title, string content, string relativePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"fr\">");
            sb.AppendLine("<head>");
            sb.AppendLine("  <meta charset=\"UTF-8\">");
            sb.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            sb.AppendLine($"  <title>{title} - Documentation Argumentum</title>");
            sb.AppendLine("  <link rel=\"stylesheet\" href=\"" + GetRelativePath(relativePath, "style.css") + "\">");
            sb.AppendLine("  <script src=\"" + GetRelativePath(relativePath, "script.js") + "\"></script>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("  <div class=\"container\">");
            sb.AppendLine("    <div class=\"sidebar\">");
            sb.AppendLine("      <div class=\"sidebar-header\">");
            sb.AppendLine("        <h1>Documentation Argumentum</h1>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <div class=\"search-container\">");
            sb.AppendLine("        <input type=\"text\" id=\"search-input\" placeholder=\"Rechercher...\">");
            sb.AppendLine("        <div id=\"search-results\"></div>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <nav id=\"toc\">");
            sb.AppendLine("        <!-- Table des matières générée par JavaScript -->");
            sb.AppendLine("      </nav>");
            sb.AppendLine("    </div>");
            sb.AppendLine("    <div class=\"content\">");
            sb.AppendLine("      <div class=\"content-header\">");
            sb.AppendLine("        <button id=\"toggle-sidebar\">☰</button>");
            sb.AppendLine($"        <h1>{title}</h1>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <div class=\"content-body\">");
            sb.AppendLine(content);
            sb.AppendLine("      </div>");
            sb.AppendLine("      <div class=\"content-footer\">");
            sb.AppendLine("        <p>Documentation générée le " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
            sb.AppendLine("      </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        /// <summary>
        /// Génère la table des matières.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        private async Task GenerateTableOfContents()
        {
            var sb = new StringBuilder();
            sb.AppendLine("const tocData = {");

            // Ajouter les fichiers
            sb.AppendLine("  files: [");
            foreach (var file in _documentationFiles)
            {
                var outputRelativePath = Path.GetRelativePath(_outputDirectory, file.OutputPath);
                sb.AppendLine($"    {{");
                sb.AppendLine($"      path: \"{outputRelativePath.Replace("\\", "/")}\",");
                sb.AppendLine($"      title: \"{file.Title}\",");
                sb.AppendLine($"      headings: [");

                // Ajouter les titres
                foreach (var heading in _headingsByFile[file.RelativePath])
                {
                    sb.AppendLine($"        {{");
                    sb.AppendLine($"          level: {heading.Level},");
                    sb.AppendLine($"          title: \"{heading.Title.Replace("\"", "\\\"")}\",");
                    sb.AppendLine($"          id: \"{heading.Id}\"");
                    sb.AppendLine($"        }},");
                }

                sb.AppendLine($"      ]");
                sb.AppendLine($"    }},");
            }
            sb.AppendLine("  ]");
            sb.AppendLine("};");

            await File.WriteAllTextAsync(Path.Combine(_outputDirectory, "toc-data.js"), sb.ToString());
        }

        /// <summary>
        /// Génère l'index de recherche.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        private async Task GenerateSearchIndex()
        {
            var sb = new StringBuilder();
            sb.AppendLine("const searchIndex = {");

            // Ajouter les termes de recherche
            foreach (var file in _documentationFiles)
            {
                var outputRelativePath = Path.GetRelativePath(_outputDirectory, file.OutputPath);
                sb.AppendLine($"  \"{outputRelativePath.Replace("\\", "/")}\": [");

                foreach (var term in _searchIndex[file.RelativePath])
                {
                    sb.AppendLine($"    \"{term.Replace("\"", "\\\"")}\",");
                }

                sb.AppendLine("  ],");
            }

            sb.AppendLine("};");

            await File.WriteAllTextAsync(Path.Combine(_outputDirectory, "search-index.js"), sb.ToString());
        }

        /// <summary>
        /// Génère la page d'accueil.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        private async Task GenerateHomePage()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"fr\">");
            sb.AppendLine("<head>");
            sb.AppendLine("  <meta charset=\"UTF-8\">");
            sb.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            sb.AppendLine("  <title>Documentation Argumentum</title>");
            sb.AppendLine("  <link rel=\"stylesheet\" href=\"style.css\">");
            sb.AppendLine("  <script src=\"script.js\"></script>");
            sb.AppendLine("  <script src=\"toc-data.js\"></script>");
            sb.AppendLine("  <script src=\"search-index.js\"></script>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("  <div class=\"container\">");
            sb.AppendLine("    <div class=\"sidebar\">");
            sb.AppendLine("      <div class=\"sidebar-header\">");
            sb.AppendLine("        <h1>Documentation Argumentum</h1>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <div class=\"search-container\">");
            sb.AppendLine("        <input type=\"text\" id=\"search-input\" placeholder=\"Rechercher...\">");
            sb.AppendLine("        <div id=\"search-results\"></div>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <nav id=\"toc\">");
            sb.AppendLine("        <!-- Table des matières générée par JavaScript -->");
            sb.AppendLine("      </nav>");
            sb.AppendLine("    </div>");
            sb.AppendLine("    <div class=\"content\">");
            sb.AppendLine("      <div class=\"content-header\">");
            sb.AppendLine("        <button id=\"toggle-sidebar\">☰</button>");
            sb.AppendLine("        <h1>Documentation Argumentum</h1>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <div class=\"content-body\">");
            sb.AppendLine("        <h2>Bienvenue dans la documentation d'Argumentum</h2>");
            sb.AppendLine("        <p>Cette documentation fournit des informations détaillées sur le projet Argumentum et son fonctionnement multilingue.</p>");
            sb.AppendLine("        <h3>Documents disponibles</h3>");
            sb.AppendLine("        <ul>");

            foreach (var file in _documentationFiles)
            {
                var outputRelativePath = Path.GetRelativePath(_outputDirectory, file.OutputPath);
                sb.AppendLine($"          <li><a href=\"{outputRelativePath.Replace("\\", "/")}\">{file.Title}</a></li>");
            }

            sb.AppendLine("        </ul>");
            sb.AppendLine("      </div>");
            sb.AppendLine("      <div class=\"content-footer\">");
            sb.AppendLine("        <p>Documentation générée le " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
            sb.AppendLine("      </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");
            sb.AppendLine("  <script>");
            sb.AppendLine("    document.addEventListener('DOMContentLoaded', function() {");
            sb.AppendLine("      initToc();");
            sb.AppendLine("      initSearch();");
            sb.AppendLine("      initSidebar();");
            sb.AppendLine("    });");
            sb.AppendLine("  </script>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            await File.WriteAllTextAsync(Path.Combine(_outputDirectory, "index.html"), sb.ToString());
        }

        /// <summary>
        /// Crée un fichier CSS par défaut.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        private async Task CreateDefaultCssFile()
        {
            var css = @"
:root {
  --primary-color: #3498db;
  --secondary-color: #2c3e50;
  --background-color: #f8f9fa;
  --text-color: #333;
  --sidebar-width: 300px;
  --header-height: 60px;
  --footer-height: 40px;
}

* {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

body {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  line-height: 1.6;
  color: var(--text-color);
  background-color: var(--background-color);
}

.container {
  display: flex;
  min-height: 100vh;
}

.sidebar {
  width: var(--sidebar-width);
  background-color: var(--secondary-color);
  color: white;
  padding: 20px;
  position: fixed;
  height: 100vh;
  overflow-y: auto;
  transition: transform 0.3s ease;
}

.sidebar-header {
  margin-bottom: 20px;
}

.sidebar-header h1 {
  font-size: 1.5rem;
  margin-bottom: 10px;
}

.search-container {
  margin-bottom: 20px;
}

#search-input {
  width: 100%;
  padding: 8px;
  border: none;
  border-radius: 4px;
}

#search-results {
  margin-top: 10px;
  max-height: 300px;
  overflow-y: auto;
}

#search-results a {
  display: block;
  padding: 5px;
  color: white;
  text-decoration: none;
  border-radius: 4px;
}

#search-results a:hover {
  background-color: rgba(255, 255, 255, 0.1);
}

#toc {
  margin-top: 20px;
}

#toc ul {
  list-style-type: none;
}

#toc li {
  margin-bottom: 5px;
}

#toc a {
  color: white;
  text-decoration: none;
  display: block;
  padding: 5px;
  border-radius: 4px;
}

#toc a:hover {
  background-color: rgba(255, 255, 255, 0.1);
}

#toc a.active {
  background-color: var(--primary-color);
}

.toc-h1 { padding-left: 0; }
.toc-h2 { padding-left: 15px; }
.toc-h3 { padding-left: 30px; }
.toc-h4 { padding-left: 45px; }
.toc-h5 { padding-left: 60px; }
.toc-h6 { padding-left: 75px; }

.content {
  flex: 1;
  margin-left: var(--sidebar-width);
  padding: 20px;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.content-header {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
  padding-bottom: 10px;
  border-bottom: 1px solid #ddd;
}

#toggle-sidebar {
  display: none;
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
  margin-right: 10px;
}

.content-body {
  flex: 1;
}

.content-footer {
  margin-top: 40px;
  padding-top: 10px;
  border-top: 1px solid #ddd;
  font-size: 0.8rem;
  color: #777;
}

h1, h2, h3, h4, h5, h6 {
  margin-top: 1.5em;
  margin-bottom: 0.5em;
  color: var(--secondary-color);
}

h1 { font-size: 2rem; }
h2 { font-size: 1.75rem; }
h3 { font-size: 1.5rem; }
h4 { font-size: 1.25rem; }
h5 { font-size: 1rem; }
h6 { font-size: 0.875rem; }

p {
  margin-bottom: 1em;
}

a {
  color: var(--primary-color);
  text-decoration: none;
}

a:hover {
  text-decoration: underline;
}

code {
  font-family: 'Courier New', Courier, monospace;
  background-color: #f0f0f0;
  padding: 2px 4px;
  border-radius: 4px;
}

pre {
  background-color: #f0f0f0;
  padding: 15px;
  border-radius: 4px;
  overflow-x: auto;
  margin-bottom: 1em;
}

pre code {
  background-color: transparent;
  padding: 0;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-bottom: 1em;
}

th, td {
  padding: 8px;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

th {
  background-color: #f0f0f0;
}

blockquote {
  border-left: 4px solid var(--primary-color);
  padding-left: 15px;
  margin-left: 0;
  margin-right: 0;
  font-style: italic;
}

img {
  max-width: 100%;
  height: auto;
}

.mermaid {
  margin-bottom: 1em;
}

@media (max-width: 768px) {
  :root {
    --sidebar-width: 250px;
  }

  .sidebar {
    transform: translateX(-100%);
    z-index: 1000;
  }

  .sidebar.active {
    transform: translateX(0);
  }

  .content {
    margin-left: 0;
  }

  #toggle-sidebar {
    display: block;
  }
}
";

            await File.WriteAllTextAsync(Path.Combine(_outputDirectory, "style.css"), css);
        }

        /// <summary>
        /// Crée un fichier JavaScript par défaut.
        /// </summary>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        private async Task CreateDefaultJsFile()
        {
            var js = @"
function initToc() {
  const tocElement = document.getElementById('toc');
  if (!tocElement || !window.tocData) return;

  const currentPath = window.location.pathname.split('/').pop();
  
  // Créer la liste des fichiers
  const fileList = document.createElement('ul');
  
  tocData.files.forEach(file => {
    const fileItem = document.createElement('li');
    const fileLink = document.createElement('a');
    fileLink.href = file.path;
    fileLink.textContent = file.title;
    
    if (file.path === currentPath) {
      fileLink.classList.add('active');
      
      // Ajouter les titres du fichier actuel
      const headingsList = document.createElement('ul');
      
      file.headings.forEach(heading => {
        const headingItem = document.createElement('li');
        const headingLink = document.createElement('a');
        headingLink.href = `#${heading.id}`;
        headingLink.textContent = heading.title;
        headingLink.classList.add(`toc-h${heading.level}`);
        
        headingItem.appendChild(headingLink);
        headingsList.appendChild(headingItem);
      });
      
      fileItem.appendChild(fileLink);
      fileItem.appendChild(headingsList);
    } else {
      fileItem.appendChild(fileLink);
    }
    
    fileList.appendChild(fileItem);
  });
  
  tocElement.appendChild(fileList);
}

function initSearch() {
  const searchInput = document.getElementById('search-input');
  const searchResults = document.getElementById('search-results');
  
  if (!searchInput || !searchResults || !window.searchIndex) return;
  
  searchInput.addEventListener('input', function() {
    const query = this.value.toLowerCase();
    searchResults.innerHTML = '';
    
    if (query.length < 3) return;
    
    const results = [];
    
    for (const [path, terms] of Object.entries(searchIndex)) {
      for (const term of terms) {
        if (term.toLowerCase().includes(query)) {
          // Trouver le titre du fichier
          let title = path;
          for (const file of tocData.files) {
            if (file.path === path) {
              title = file.title;
              break;
            }
          }
          
          results.push({ path, title });
          break;
        }
      }
    }
    
    // Afficher les résultats
    if (results.length > 0) {
      results.forEach(result => {
        const link = document.createElement('a');
        link.href = result.path;
        link.textContent = result.title;
        searchResults.appendChild(link);
      });
    } else {
      searchResults.textContent = 'Aucun résultat trouvé';
    }
  });
}

function initSidebar() {
  const toggleButton = document.getElementById('toggle-sidebar');
  const sidebar = document.querySelector('.sidebar');
  
  if (!toggleButton || !sidebar) return;
  
  toggleButton.addEventListener('click', function() {
    sidebar.classList.toggle('active');
  });
}

document.addEventListener('DOMContentLoaded', function() {
  initToc();
  initSearch();
  initSidebar();
});
";

            await File.WriteAllTextAsync(Path.Combine(_outputDirectory, "script.js"), js);
        }

        /// <summary>
        /// Obtient le chemin relatif entre deux fichiers.
        /// </summary>
        /// <param name="fromPath">Le chemin du fichier source.</param>
        /// <param name="toPath">Le chemin du fichier cible.</param>
        /// <returns>Le chemin relatif.</returns>
        private string GetRelativePath(string fromPath, string toPath)
        {
            var fromDir = Path.GetDirectoryName(fromPath);
            var toDir = "";
            
            if (string.IsNullOrEmpty(fromDir))
            {
                return toPath;
            }
            
            var relativePath = "";
            var dirCount = fromDir.Split(Path.DirectorySeparatorChar).Length;
            
            for (int i = 0; i < dirCount; i++)
            {
                relativePath += "../";
            }
            
            return relativePath + toPath;
        }
    }

    /// <summary>
    /// Représente un fichier de documentation.
    /// </summary>
    public class DocumentationFile
    {
        /// <summary>
        /// Le chemin du fichier d'entrée.
        /// </summary>
        public string InputPath { get; set; }
        
        /// <summary>
        /// Le chemin du fichier de sortie.
        /// </summary>
        public string OutputPath { get; set; }
        
        /// <summary>
        /// Le contenu du fichier.
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// Le titre du fichier.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Le chemin relatif du fichier.
        /// </summary>
        public string RelativePath { get; set; }
    }

    /// <summary>
    /// Représente un titre dans un fichier de documentation.
    /// </summary>
    public class DocumentationHeading
    {
        /// <summary>
        /// Le niveau du titre (1-6).
        /// </summary>
        public int Level { get; set; }
        
        /// <summary>
        /// Le texte du titre.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// L'identifiant du titre.
        /// </summary>
        public string Id { get; set; }
    }
}

namespace Argumentum.AssetConverter.Documentation
{
    public partial class DocumentationGenerator
    {
        /// <summary>
        /// Extrait les termes de recherche du fichier Markdown.
        /// </summary>
        /// <param name="content">Le contenu du fichier Markdown.</param>
        /// <returns>La liste des termes de recherche.</returns>
        private List<string> ExtractSearchTerms(string content)
        {
            var terms = new List<string>();
            // Commenté temporairement pour permettre la compilation
            /*
            var pipeline = new MarkdownPipelineBuilder().Build();
            var document = Markdown.Parse(content, pipeline);

            // Extraire les titres
            foreach (var heading in document.Descendants<HeadingBlock>())
            {
                if (heading.Inline?.FirstChild is LiteralInline literalInline)
                {
                    terms.Add(literalInline.Content.ToString());
                }
            }

            // Extraire les paragraphes
            foreach (var paragraph in document.Descendants<ParagraphBlock>())
            {
                var text = new StringBuilder();
                foreach (var inline in paragraph.Inline)
                {
                    if (inline is LiteralInline literalInline)
                    {
                        text.Append(literalInline.Content);
                    }
                }
                terms.Add(text.ToString());
            }
            */
            
            // Implémentation temporaire simple
            var lines = content.Split('\n');
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    terms.Add(line.Trim());
                }
            }

            return terms;
        }
    }
}