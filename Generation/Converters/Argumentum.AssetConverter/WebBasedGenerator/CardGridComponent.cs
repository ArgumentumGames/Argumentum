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
        }
    }
}