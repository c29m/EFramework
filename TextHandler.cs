using System;
using System.Web;
namespace Earlz.EFramework
{
	public class TextHandler : HttpHandler
	{
		string Text;
		public TextHandler (string text)
		{
			Text=text;
		}
		public override void Get ()
		{
			Response.ContentType="text/plain";
			Write(Text);
		}
		
		
	}
}

