namespace Argumentum.AssetConverter.Mindmapper
{
    public interface IMindMapItem
    {
        string Id { get; set; }
        string Path { get; }
        int Depth { get; }
        string Family { get; }
        string SubFamily { get; }
        string SubSubFamily { get; }
        string Title { get; }
        string Text { get; }
        string Description { get; }
        string Example { get; }
        string Link { get; }
        int? Carte { get; }
        string Pk { get; set; }
        string PK { get; set; }
        string DecimalPath { get; }
    }
}
