using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Garde item 4 (dispatch ai-01 18/10 23:11, po-2024.4) : le paquet fr-FR du
    /// dépôt = officiel + 94 extras + 16 MANQUANTES (mesure reproduite : clés du
    /// resx neutre absentes du fr-FR — 11 SharedResources + 3 GlobalResources +
    /// 2 Exceptions). Cette PR comble les 8 VISIBLES ; les 8 restantes (admin/host :
    /// CLEANUP_ProcessError, CreatePortalHomeFolderExists.Error,
    /// EXCEPTION_GlobDotDotNotSupportedInCleanup, ErrorDecryptingSetting,
    /// OAuthConfigBase_HostConfig.Text+Help, SQL_Manifest_BadFile+Error) restent
    /// classées manquantes — une future passe les comblera sans rougir cette
    /// garde (elle épingle les présentes, pas les absentes).
    /// Visible comblé : bandeau cookies <c>cc_*</c> ×3 (sans la clé, le bandeau
    /// sert le fallback EN sous /fr), validation prénom/nom (inscription, profil),
    /// confirmation du courriel de vérification, erreurs de nom/chemin de fichier
    /// (upload membre). Guillemets français alignés sur le voisin immédiat
    /// AddFileExtensionNotAllowed (« {0} »), insertion à l'ordre du neutre.
    /// </summary>
    public class DnnFrFrVisibleKeysGuardTests
    {
        private const string GlobalResx = "DNNPlatform/App_GlobalResources/GlobalResources.fr-FR.resx";
        private const string SharedResx = "DNNPlatform/App_GlobalResources/SharedResources.fr-FR.resx";
        private const string ExceptionsResx = "DNNPlatform/App_GlobalResources/Exceptions.fr-FR.resx";

        private static string ResxValue(string resxRelativePath, string key)
        {
            var path = Path.Combine(TestRepoRoot.Find(), resxRelativePath.Replace('/', Path.DirectorySeparatorChar));
            File.Exists(path).Should().BeTrue($"le resx doit exister ({resxRelativePath})");
            var entry = XDocument.Load(path).Root!.Elements("data")
                .FirstOrDefault(d => string.Equals(d.Attribute("name")?.Value, key, StringComparison.Ordinal));
            entry.Should().NotBeNull($"la clé {key} doit exister dans {resxRelativePath}");
            return entry!.Element("value")!.Value;
        }

        [Fact]
        public void CookieBanner_Keys_ExistInFrench()
        {
            ResxValue(GlobalResx, "cc_message.Text").Should().Be(
                "Ce site utilise des cookies pour vous garantir la meilleure expérience sur notre site.",
                "bandeau cookies visible sous /fr — sans la clé, fallback EN.");
            ResxValue(GlobalResx, "cc_link.Text").Should().Be("En savoir plus", "lien du bandeau cookies.");
            ResxValue(GlobalResx, "cc_dismiss.Text").Should().Be("Compris !", "bouton du bandeau cookies.");
        }

        [Fact]
        public void RegistrationAndFileMessages_Keys_ExistInFrench()
        {
            ResxValue(SharedResx, "InvalidFirstName.Text").Should().Be("Le prénom indiqué est invalide.",
                "validation inscription/édition de profil.");
            ResxValue(SharedResx, "InvalidLastName.Text").Should().Be("Le nom indiqué est invalide.",
                "validation inscription/édition de profil.");
            ResxValue(SharedResx, "VerificationMailSendSuccessful.Text").Should().Be(
                "Le courriel de vérification a été envoyé.", "confirmation affichée après l'inscription.");
            ResxValue(ExceptionsResx, "AddFilenameNotAllowed.Text").Should().Be(
                "Le nom de fichier « {0} » n'est pas autorisé. Le fichier n'a pas été ajouté.",
                "erreur d'upload membre — guillemets français du voisin AddFileExtensionNotAllowed.");
            ResxValue(ExceptionsResx, "AddFolderNotAllowed.Text").Should().Be(
                "Le chemin de dossier « {0} » n'est pas autorisé. Le dossier n'a pas été ajouté.",
                "erreur d'upload membre — guillemets français du voisin AddFileExtensionNotAllowed.");
        }
    }
}
