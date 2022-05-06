using System.Linq;
using ToSic.Razor.Blade;
using Dynlist = System.Collections.Generic.IEnumerable<dynamic>;

public class SortImages: Custom.Hybrid.Code12
{
  /**
  * Returns the images, sorted by the passed setting
  */
  public dynamic GetImagesSorted(dynamic album) {
    var images = AsAdam(album, "Images").Files as Dynlist;

    switch ((string)album.Presentation.SortMode) {
      case "File asc":
        images = images.OrderBy(c => c.FileName);
        break;
      case "File desc":
        images = images.OrderByDescending(c => c.FileName);
        break;
      case "Title asc":
        images = images.OrderBy(c => !c.HasMetadata).ThenBy(c => !c.HasMetadata ? "" : ((dynamic)c.Metadata).Title);
        break;
      case "Title desc":
        images = images.OrderBy(c => !c.HasMetadata).ThenByDescending(c => !c.HasMetadata ? "" : ((dynamic)c.Metadata).Title);
        break;
      case "Upload asc":
        images = images.OrderBy(c => c.Modified);
        break;
      case "Upload desc":
        images = images.OrderByDescending(c => c.Modified);
        break;
    }
    return images;
  }
}