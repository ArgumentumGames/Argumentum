using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;

namespace Argumentum.AssetConverter
{
    public class CardGridComponent : IComponent
    {
        private readonly IEnumerable<byte[]> _cardImageData;
        private readonly int _columns;
        private readonly float _padding;

        public CardGridComponent(IEnumerable<byte[]> cardImageData, int columns, float padding)
        {
            _cardImageData = cardImageData;
            _columns = columns;
            _padding = padding;
        }

        public void Compose(IContainer container)
        {
            // QuestPDF deprecated Grid in 2022.11 in favour of Table, but this project deliberately
            // pins QuestPDF at 2022.12.12 (thread-safety — see CLAUDE.md stable-versions table) and is
            // NOT upgrading it for the v0.9.0 release. Grid stays: a faithful Table migration is not
            // possible here without a layout regression. Grid(N) means "N fixed-width columns regardless
            // of item count" and stores N as a cheap layout parameter, so it tolerates the large/degenerate
            // column counts that ComputePageGeometry can yield for edge configs (e.g. tiny card sizes).
            // Materialising N explicit Table.RelativeColumn() definitions instead allocates N column
            // objects and OOMs on those same inputs (regression caught by PdfAssemblerTests). The obsolete
            // warning is therefore suppressed with justification rather than "fixed" by a risky rewrite.
#pragma warning disable CS0618 // QuestPDF Grid deprecated; pinned version + no faithful Table equivalent
            container.Grid(grid =>
            {
                grid.Columns(_columns);

                foreach (var imageData in _cardImageData)
                {
                    if (imageData != null && imageData.Length > 0)
                    {
                        grid.Item().Padding(_padding).Image(imageData);
                    }
                    else
                    {
                        // Add an empty item to respect the grid structure
                        grid.Item().Padding(_padding).Container();
                    }
                }
            });
#pragma warning restore CS0618
        }
    }
}