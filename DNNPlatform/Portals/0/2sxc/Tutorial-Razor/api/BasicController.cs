using System.Web.Http;		// this enables [HttpGet] and [AllowAnonymous]

[AllowAnonymous]			// define that all commands can be accessed without a login
// Inherit from ToSic...ApiController to get features like App, Data or Dnn - see https://r.2sxc.org/CustomWebApi
public class BasicController : ToSic.Sxc.Dnn.ApiController
{

	[HttpGet]				// [HttpGet] says we're listening to GET requests
	public string Hello()
	{
		return "Hello from the basic controller in /api";
	}


	[HttpGet]				// [HttpGet] says we're listening to GET requests
	public int Square(int number)
	{
		return number * number;
	}


}
