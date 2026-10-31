namespace Argumentum.AssetConverter.Tests
{
	/// <summary>
	/// #458 grain module-level — pack <c>controls</c> **zh-CN** (lot 1, PR #2087).
	/// Les règles du contrat vivent dans <see cref="DnnModuleLevelControlsResxGuardBase"/> ;
	/// cette classe ne déclare que sa culture, pour que le lot ar-SA (et fa-IR
	/// ensuite) les partage au lieu de les recopier.
	/// <para>
	/// Détail du lot, contrôles indépendants et contrôles inverses :
	/// <c>docs/dnn-localization/458-item9bis-controls-zh-2026-10-30.md</c>.
	/// </para>
	/// </summary>
	public class DnnModuleLevelControlsZhResxGuardTests : DnnModuleLevelControlsResxGuardBase
	{
		protected override string Culture => "zh-CN";
	}
}
