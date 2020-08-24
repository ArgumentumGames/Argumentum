// Important notes: 
// - This class should have the same name as the file it's in
public class Shared {

  public void EnableEditForAnonymous(dynamic Edit) {
    // Special command to ensure that the toolbars appear, even if they are won't work.
    // This is not an official API, and may change any time.
    // This will tell the edit-UI that it's enabled (which is usually only the case if a user is an editor)
    Edit.Enabled = true;

    // This will tell the edit-UI that we need the JS features
    Edit.Enable(api: true, forms: true, context: true, styles: true);

  }
}
