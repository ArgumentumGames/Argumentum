using ToSic.Razor.Blade;
using System.Linq;
using System;

public class Helpers: Custom.Hybrid.Code12
{
  /// <summary>
  /// Generate bootstrap4 css class names for the overlay div, based on the settings of the slide
  /// </summary>
  public dynamic OverlayAlignClasses(dynamic settingsStack) {
    var pos = settingsStack.TextPosition ?? "";
    return (pos.StartsWith("c") ? "align-items-center" : "")   // center: cl, cc, cr
      + " " + (pos.StartsWith("b") ? "align-items-end" : "");  // bottom: bl, bc, br
  }

  /// <summary>
  /// Generate bootstrap4 css class names for the overlay div, based on the settings of the slide
  /// </summary>
  public dynamic OverlayTextAlignClasses(dynamic settingsStack) {
    var pageCss = GetService<Connect.Koi.ICss>();         // Service to get CSS information about the current Theme
    var pos = settingsStack.TextPosition ?? "";
    return (pos.EndsWith("c") ? "text-center" : "")    // center: tc, cc, bc
      + " " + (pos.EndsWith("r") && pageCss.Is("bs4") ? "text-right" : pos.EndsWith("r") && pageCss.Is("bs5") ? "text-end" : ""); // right:  tr, cr, br
  }

  /// <summary>
  /// Generate custom css class names for the overlay div, based on the settings of the slide
  /// This changes the effects as well as background gradients
  /// </summary>
  public dynamic SlideWrapperClasses(dynamic settingsStack) {
    return "content-position-" + (settingsStack.TextPosition ?? "none")
      + " content-effect-" + (settingsStack.OverlayEffect ?? "none")
      + (settingsStack.DarkContent ? " dark-content" : " light-content");
  }

  /// <summary>
  /// Generate a <picture> tag for responsive images, with various resolutions for each screen size
  /// </summary>
  public dynamic PictureTag(string imgUrlOrig, string title, string probablyRatio) {
    // The Default Resize-Settings used
    var resizeSettings = AsDynamic(new {
      Format = "jpg",
      Quality = 70,
      ResizeMode = "crop",
      ScaleMode = "both",
      Width = 1600,
      AspectRatio = probablyRatio // Aspect Ratio like "0.75", "16:9" or "16/9". Values like 100vh will be ignored
    });

    // All the widths we'll use for <picture><source srcset="...">
    var widths = new[] { 320, 480, 640, 800, 1000, 1600 };

    var pictureTag = Tag.Picture();
    var setJpg = string.Join(",\n", widths.Select(width => Link.Image(imgUrlOrig, resizeSettings, width: width)  + " " + width + "w"));
    pictureTag.Add(Tag.Source().Srcset(setJpg).Type("image/jpeg"));

    pictureTag.Add(
      Tag.Img().Src(Link.Image(imgUrlOrig, resizeSettings)).Alt(title)
    );
    return pictureTag;
  }

}


