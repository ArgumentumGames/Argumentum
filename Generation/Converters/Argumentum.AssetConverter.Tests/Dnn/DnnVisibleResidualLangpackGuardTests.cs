using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// Garde P24-01 (#457) : la part VISIBLE du résidu des langpacks, mesurée
    /// servie le 05/11/2026 (#2154, <c>docs/dnn-localization/458-langpack-residu-visible-2026-11-05.md</c>
    /// — 222 hits servis, trois surfaces seulement). Ce grain ferme les deux
    /// familles qui tenaient à un fichier du dépôt, plus une coquille pt-BR :
    /// <para>
    /// 1. <b>Skip-link du skin</b> (« Skip to main content », clé
    /// <c>SkipLink.MainContent</c>) : servi sur toutes les pages des 8 langues,
    /// y compris fr, parce qu'<b>aucun</b> jumeau de culture de 2shineBS5 (le
    /// skin servi — <c>#to-shine-page-main</c>) ne portait la clé (fr-FR : 4
    /// clés, autres : 15 ; re-mesuré au parseur). DNN retombe alors sur le
    /// neutre anglais. Correct ici dans les 7 cultures non-en — en-US retombe
    /// sur le neutre <b>à raison</b>, ce grain n'écrit donc jamais en-US.
    /// </para>
    /// <para>
    /// 2. <b>Boutons sociaux Login/Register</b> (« Login with Facebook », etc.) :
    /// les jumeaux <c>Login.ascx.&lt;culture&gt;.resx</c> de Facebook/Google/Live
    /// existaient pour fr/es/ar/fa/zh mais <b>pas</b> pour pt-BR ni ru-RU — les
    /// deux seules cultures où la mesure (#2154) voit fuiter l'anglais sur les
    /// parcours d'inscription. Les 6 jumeaux manquants sont créés depuis le
    /// squelette fr-FR du fournisseur (BOM UTF-8 + LF, comme lui).
    /// </para>
    /// <para>
    /// 3. <b>Coquille pt-BR de la page 404</b> : <c>NoResult.Text</c> portait
    /// « Não <b>foin</b> encontrado resultados » — « foin » pour « foi ». La
    /// surface search de la page 404 était déjà traduite dans les 7 cultures,
    /// cette cellule seule était fautive.
    /// </para>
    /// <para>
    /// Les valeurs exactes sont épinglées ici ET dans l'instrument
    /// <c>tools/457-p24-01-langpack-residu-visible.py</c> (dry-run par défaut,
    /// <c>--self-test</c> à contrôle inverse) — deux tables, un drift impossible
    /// à manquer : la CI casse si l'une bouge sans l'autre.
    /// </para>
    /// </summary>
    public class DnnVisibleResidualLangpackGuardTests
    {
        private const string SkinDir = "DNNPlatform/Portals/_default/Skins/2shineBS5/App_LocalResources";
        private const string AuthDir = "DNNPlatform/DesktopModules/AuthenticationServices";
        private const string SearchPtBr = "DNNPlatform/admin/Skins/App_LocalResources/Search.ascx.pt-BR.resx";
        private const string SkipKey = "SkipLink.MainContent";
        private const string SkipEn = "Skip to main content";

        /// <summary>Skip-link : la valeur attendue par culture, épinglée.</summary>
        private static readonly Dictionary<string, string> SkipByCulture = new()
        {
            ["fr-FR"] = "Aller au contenu principal",
            ["ru-RU"] = "Перейти к основному содержимому",
            ["pt-BR"] = "Ir para o conteúdo principal",
            ["es-ES"] = "Ir al contenido principal",
            ["ar-SA"] = "الانتقال إلى المحتوى الرئيسي",
            ["fa-IR"] = "پرش به محتوای اصلی",
            ["zh-CN"] = "跳转到主要内容",
        };

        private static readonly string[] SocialProviders = { "Facebook", "Google", "Live" };
        private static readonly string[] SocialCultures = { "pt-BR", "ru-RU" };

        /// <summary>Les valeurs du neutre anglais : jamais admises dans un jumeau non-en.</summary>
        private static readonly HashSet<string> EnglishNeutrals = new()
        {
            "Login with Facebook", "Register with Facebook",
            "Sign with your Google Account", "Register with Google+",
            "Register with Windows Live", "Sign In with Windows Live",
            SkipEn,
        };

        private static string RepoPath(string relative) =>
            Path.Combine(TestRepoRoot.Find(), relative.Replace('/', Path.DirectorySeparatorChar));

        private static byte[] ReadBytes(string relative) => File.ReadAllBytes(RepoPath(relative));

        private static Dictionary<string, string> Values(string relative)
        {
            // BOM retiré : XDocument.Parse lève sur un BOM en tête de chaîne décodée.
            var raw = ReadBytes(relative);
            var text = raw.Length > 2 && raw[0] == 0xEF && raw[1] == 0xBB && raw[2] == 0xBF
                ? raw.Skip(3).ToArray()
                : raw;
            var doc = XDocument.Parse(Encoding.UTF8.GetString(text));
            var values = new Dictionary<string, string>();
            foreach (var data in doc.Root?.Elements("data") ?? Enumerable.Empty<XElement>())
            {
                var name = data.Attribute("name")?.Value;
                if (name == null)
                {
                    continue;
                }
                values[name] = data.Element("value")?.Value ?? "";
            }
            return values;
        }

        [Fact]
        public void SkipLink_MainContent_est_traduit_dans_les_7_jumeaux_2shine()
        {
            foreach (var (culture, expected) in SkipByCulture)
            {
                var rel = $"{SkinDir}/2shine.{culture}.resx";
                var values = Values(rel);
                values.Should().ContainKey(SkipKey,
                    "le jumeau {0} ne porte pas {1} : DNN retombe sur le neutre anglais (fuite mesurée #2154)",
                    rel, SkipKey);
                values[SkipKey].Should().Be(expected,
                    "la valeur de {0} dans {1} doit être la traduction épinglée", SkipKey, rel);
                EnglishNeutrals.Should().NotContain(values[SkipKey],
                    "{0} ne doit pas servir l'anglais neutre en {1}", rel, culture);
            }
        }

        [Fact]
        public void SkipLink_enUS_n_est_pas_ecrit_le_neutre_suffit()
        {
            // Le jumeau en-US, s'il existe, ne doit pas être créé par ce grain :
            // l'anglais du neutre est CORRECT pour en-US. Un jumeau en-US portant
            // la même valeur serait du bruit, pas une correction.
            var enUs = $"{SkinDir}/2shine.en-US.resx";
            if (!File.Exists(RepoPath(enUs)))
            {
                return; // attendu : le grain n'écrit pas en-US
            }
            Values(enUs).Should().NotContainKey(SkipKey,
                "le jumeau en-US ne doit pas porter {0} : le neutre anglais est la valeur juste", SkipKey);
        }

        [Theory]
        [InlineData("Facebook", "pt-BR")]
        [InlineData("Facebook", "ru-RU")]
        [InlineData("Google", "pt-BR")]
        [InlineData("Google", "ru-RU")]
        [InlineData("Live", "pt-BR")]
        [InlineData("Live", "ru-RU")]
        public void Boutons_sociaux_le_jumeau_manquant_est_cree_et_non_anglais(string provider, string culture)
        {
            var rel = $"{AuthDir}/{provider}/App_LocalResources/Login.ascx.{culture}.resx";
            File.Exists(RepoPath(rel)).Should().BeTrue(
                "le jumeau {0} manquait : fuite anglaise mesurée sur /Login et /Register {1} (#2154)",
                rel, culture);
            var values = Values(rel);
            foreach (var key in values.Keys)
            {
                EnglishNeutrals.Should().NotContain(values[key],
                    "{0} : {1} ne doit pas servir l'anglais neutre", rel, key);
            }
            values.Should().ContainKey("Title.Text", "{0} doit porter le titre du fournisseur", rel);
        }

        [Fact]
        public void Search_ptBR_NoResult_ne_porte_plus_la_coquille_foin()
        {
            var values = Values(SearchPtBr);
            values.Should().ContainKey("NoResult.Text");
            values["NoResult.Text"].Should().Be("Nenhum resultado encontrado",
                "l'ancienne valeur portait la coquille « Não foin encontrado resultados » (foin/foi)");
        }

        [Fact]
        public void Enveloppes_2shine_sans_BOM_Login_exactement_un()
        {
            var bom = new byte[] { 0xEF, 0xBB, 0xBF };
            foreach (var culture in SkipByCulture.Keys)
            {
                var raw = ReadBytes($"{SkinDir}/2shine.{culture}.resx");
                raw.Take(3).Should().NotEqual(bom,
                    "2shine.{0} : sans BOM, comme ses jumeaux existants", culture);
                raw.Take(4).Should().Equal(Encoding.ASCII.GetBytes("<?xm"),
                    "2shine.{0} : le fichier commence par la déclaration XML", culture);
                Encoding.UTF8.GetString(raw).Should().NotContain("\r\n",
                    "2shine.{0} : LF seuls, comme ses jumeaux existants", culture);
            }
            foreach (var provider in SocialProviders)
            {
                foreach (var culture in SocialCultures)
                {
                    var raw = ReadBytes($"{AuthDir}/{provider}/App_LocalResources/Login.ascx.{culture}.resx");
                    raw.Take(3).Should().Equal(bom,
                        "Login.ascx.{0}.resx ({1}) : exactement un BOM UTF-8, comme le jumeau fr-FR",
                        culture, provider);
                    raw.Skip(3).Take(3).Should().NotEqual(bom,
                        "Login.ascx.{0}.resx ({1}) : pas de DOUBLE BOM (réécriture naïve utf-8-sig sur texte déjà décodé)",
                        culture, provider);
                }
            }
        }
    }
}
