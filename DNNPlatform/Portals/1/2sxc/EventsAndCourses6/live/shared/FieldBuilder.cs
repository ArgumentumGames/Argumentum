using ToSic.Razor.Blade;

public class FieldBuilder : Custom.Hybrid.Code12
{
  /* 
    this file is for creating different fields e.g. input, textarea, file, dropdown and showing them in the template

    Example: 
    Shows a required input of type text with a label infront of it

    var FieldBuilder = CreateInstance("shared/FieldBuilders.cs");
    @FieldBuilder.Text("Subject", true)

    Shows a required input of type text with a label infront of it

    var FieldBuilder = CreateInstance("shared/FieldBuilders.cs");
    FieldBuilder.LabelInPlaceholder = true
    @FieldBuilder.EMail("Email", true)

    Shows a required input of type text with a placeholder
  */

  // handles the visibility of a label or a placeholder
  public bool LabelInPlaceholder = false;

  #region Koi based class selection
  internal dynamic PageCss { get { return _pageCss ?? (_pageCss = GetService<Connect.Koi.ICss>()); } }
  private dynamic _pageCss;
  internal string FormClasses()
  {
    return "mb-3 " + (LabelInPlaceholder ? "" : "row");
  }

  // Choose CSS classes based on the framework
  // if you customize this, you probably know what css framework you want,
  // in which case you can skip framework detection and just write the classes

  // Choose CSS classes for the labels
  internal string LabelClasses(bool required)
  {
    return "control-label "
      + (required ? "app-events6-field-required " : "")
      + (PageCss.Is("bs3") ? "col col-xs-12 col-sm-3" : "col-12 col-sm-3");
  }

  #endregion

  // Add a placeholder text to the inputs
  internal string PhLabel(string key, bool required)
  {
    return LabelInPlaceholder ? Resources.Get("Label" + key) + (required ? "*" : "") : "";
  }

  // Sets a RazorBlade Input/TextArea to required and adds the message which is different for each field type
  internal void SetRequired(dynamic content, bool required, string message) {
    if (!required) return;
    content.Attr("data-pristine-required-message", message).Required();
  }

  // returns a Label
  public dynamic Label(string label, string forControl, bool required = false)
  {
    return Tag.Label(label).Class("col-sm-3" + (required ? " app-events6-field-required " : "")).Attr("for", forControl);
  }

  // returns an input with common attributes and a possible placeholder
  public dynamic Text(string idString, bool required, bool disabled = false, string value = "")
  {
    var content = Tag.Input().Type("text").Id(idString).Placeholder(PhLabel(idString, required)).Class("form-control");
    if (value != null) { content.Attr("value", value); }
    SetRequired(content, required, Resources.LabelRequired);
    if (disabled) { content = content.Disabled(); }
    return Field(idString, required, content);
  }

  // returns an input of type email with common attributes and a possible placeholder
  public dynamic EMail(string idString, bool required)
  {
    var content = Tag.Input().Type("email").Id(idString).Placeholder(PhLabel(idString, required)).Class("form-control");
    SetRequired(content, required, Resources.LabelValidEmail);
    return Field(idString, required, content);
  }

  // returns a textarea with common attributes and a possible placeholder
  public dynamic Multiline(string idString, bool required, bool disabled = false, string value = "")
  {
    var content = Tag.Textarea().Id(idString).Placeholder(PhLabel(idString, required)).Class("form-control");
    if (value != null) { content.Add(value); }
    SetRequired(content, required, Resources.LabelRequired);
    if (disabled) { content = content.Disabled(); }
    return Field(idString, required, content);
  }

  // returns a select and options with common attributes
  public dynamic DropDown(string idString, bool required, string[] values) {
    var content = Tag.Select().Id(idString).Class("form-control");
    SetRequired(content, required, Resources.LabelRequired);
    content.Add(Tag.Option("--Please Select--").Attr("value", ""));
    foreach(var value in values){
      content.Add(Tag.Option(value));
    }
    return Field(idString, required, content);
  }

  // shows a wrapping div with choosen content
  public dynamic Field(string idString, bool required, dynamic contents) {
    var inputWrapperClasses = PageCss.Is("bs3") ? "col col-xs-12 col-sm-9" : "col-12  col-sm-9";
    var labelTranslated = Resources.Get("Label" + idString);
    var field = Tag.Div().Class(FormClasses());

    // If the label is _not_ in the placeholder, add the label first
    if (!LabelInPlaceholder) {
      field = field.Add(
        Tag.Label(ToSic.Razor.Blade.Text.First(labelTranslated, idString))
          .Class(LabelClasses(required))
          .For(idString)
      );
    }
    
    return field.Add(Tag.Div(contents).Class(!LabelInPlaceholder ? inputWrapperClasses : ""));
  }
}