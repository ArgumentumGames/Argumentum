using Xunit;

namespace Argumentum.AssetConverter.Tests
{
    /// <summary>
    /// #458 grain module-level — lot <c>admin</c> zh-CN (PR #2088, 27 fichiers,
    /// 328 clés). Toutes les règles vivent dans le socle abstrait (motif acté
    /// pour <c>controls</c>, décision du 31/10) : cette classe ne porte que
    /// l'identité culturelle du lot — un lot suivant est une sous-classe, pas
    /// une copie des règles.
    /// </summary>
    public class DnnModuleLevelAdminZhResxGuardTests : DnnModuleLevelAdminResxGuardBase
    {
        /// <inheritdoc />
        protected override string Culture => "zh-CN";
    }
}
