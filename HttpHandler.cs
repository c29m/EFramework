using System;
using System.Web;
using System.Collections.Generic;
namespace Earlz.EFramework
{
	/**The base class used to handle HTTP requests.
	 * This class should be derived from for every different handler for HTTP requests.
	 */
	public abstract class HttpHandler
	{
		public HttpHandler ()
		{
		}
		/**Handles the HTTP GET method**/
		public virtual void Get(){
			throw new NotImplementedException();
		}
		/**Handles the HTTP POST method**/
		public virtual void Post(){
			throw new NotImplementedException();
		}
		/**Handles the HTTP PUT method**/
		public virtual void Put(){
			throw new NotImplementedException();
		}
		/**Handles the HTTP DELETE method**/
		public virtual void Delete(){
			throw new NotImplementedException();
		}
		/**Writes to the output stream**/
		public void Write(string s){
			Response.Write(s);
		}
		/**The current HttpContext**/
		public HttpContext Context{get;set;}
		/**The route that handled the request.**/
		public Route RouteRequest{get;set;}
		/**A shortcut for Context.Request**/
		public HttpRequest Request{
			get{
				return Context.Request;
			}
		}
		/**A shortcut for Context.Response**/
		public HttpResponse Response{
			get{
				return Context.Response;
			}
		}
		/**The current HttpMethod for the request**/
		public HttpMethod Method{get;set;}
		public System.Collections.Specialized.NameValueCollection Form{
			get{
				return Request.Form;
			}
		}
		ParameterDictionary routeparams=null;
		/**When using SimplePattern, this is populated with the route parameters.**/
		public ParameterDictionary RouteParams{
			get{
				return routeparams;
			}
			set{
				if(routeparams!=null){
					throw new ArgumentException("RouteParams is already set.","value");
				}
				routeparams=value;
			}
		}
		/**The route's id that handled the current request**/
		public string RouteID{get;set;}
	}
}