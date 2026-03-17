using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter
{
    public abstract class ParallelFallacyDocumentCreatorConfigBase<TDocumentType>
        : ParallelDocumentCreatorConfigBase<TDocumentType, Fallacy>
        where TDocumentType : FallacyDocumentConfigBase, new()
    {
    }
}