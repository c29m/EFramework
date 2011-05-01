using System;
using System.Web;
namespace Earlz.EFramework
{
	/**A static helper class for use inside of Global.asax(usually)**/
	public static class Routing
	{
		public static Router Router{get{return Router;}}
		static Router router;
		/**Handles the current request and calls the appropriate HttpHandler
		 * @param c The current HttpContext. From Global.asax, just use `Context`
		 * @param app The current HttpApplication. From Global.asax, just use `this`
		 * **/
		static public void DoRequest(HttpContext c,HttpApplication app){
			c.Response.ContentType="text/html"; //default
			if(c.Request.Url.AbsolutePath.Substring(0,Math.Min(c.Request.Url.AbsolutePath.Length,8))=="/static/"){
				return; //let it just serve the static files
			}
			if(router.DoRoute(c)){
				app.CompleteRequest();
			}
		}
		/**Add a route to the list of routes.
		 * @param id The RouteID
		 * @param type The pattern type to use
		 * @param pattern The route pattern to match to
		 * @param handler A lambda(or similar) to invoke a new HttpHandler, such as ()=>{return new MyHandler;}
		 */
		static public void AddRoute(string id,PatternTypes type,string pattern,HandlerInvoker handler){
			/**TODO: This needs to be smart enough so that routes can not be added while routes are being parsed, else get a 
			 * "collection modified" exception from .Net. **/
			if(router==null){
				router=new Router();
			}
			router.AddRoute(id,type,pattern,handler);
		}
	}
}

