// Important notes: 
// - This class should have the same name as the file it's in
// - This inherits from ToSic.Sxc.Code.WithDnnContext
//   which will automatically provide the common objects like App, Dnn etc.
//   from the current context to use in your code
public class FunctionsWithContext: ToSic.Sxc.Dnn.DynamicCode {

  public string QrPath(string link) {
        // path to qr-code generator
        var qrPath = "//api.qrserver.com/v1/create-qr-code/?color={foreground}&bgcolor={background}&qzone=0&margin=0&size={dim}x{dim}&ecc={ecc}&data={link}"
            .Replace("{foreground}", App.Settings.QrForegroundColor.Replace("#", ""))
            .Replace("{background}", App.Settings.QrBackgroundColor.Replace("#", ""))
            .Replace("{dim}", App.Settings.QrDimension.ToString())
            .Replace("{ecc}", App.Settings.QrEcc)
            .Replace("{link}", link)
            ;
        return qrPath;
    }

}
