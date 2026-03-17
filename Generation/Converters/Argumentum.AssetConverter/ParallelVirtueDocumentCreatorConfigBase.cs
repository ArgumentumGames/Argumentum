using Argumentum.AssetConverter.Entities;

namespace Argumentum.AssetConverter
{
    public abstract class ParallelVirtueDocumentCreatorConfigBase<TDocumentType>
        : ParallelDocumentCreatorConfigBase<TDocumentType, Virtue>
        where TDocumentType : DocumentConfig, IMindMapDocumentConfig, new()
    {
    }
}