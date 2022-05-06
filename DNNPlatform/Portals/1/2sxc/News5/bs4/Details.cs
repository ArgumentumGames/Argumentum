using ToSic.Razor.Blade;
using System;

public class Details: Custom.Hybrid.Code12
{

  // Shows a back to list button
  public dynamic BackToListButton() {
    return Tag.A(App.Resources.LabelBackToList).Class("btn").Class("btn-outline-primary").Class("app-backtolist").Href(Link.To().ToString());
  }
}

