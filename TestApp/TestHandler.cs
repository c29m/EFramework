using System;
namespace Earlz.EFramework.TestApp
{
	public class TestHandler : HttpHandler
	{
		public override void Get ()
		{
			Response.ContentType="text/plain";
			if(RouteID=="test3"){
				Response.Write("test 3: id="+RouteParams["id"]+" slug="+RouteParams["*"]);
				return;
			}
			if(RouteID=="test2"){
				Response.Write("Arrived at test 2: id="+RouteParams["id"]);
				return;
			}
			if(RouteID=="test"){
				Response.Write("Arrived at test");
				return;
			}
		}
	}
}

