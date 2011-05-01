/*
Copyright (c) 2010 - 2011 Jordan "Earlz/hckr83" Earls  <http://www.Earlz.biz.tm>
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

1. Redistributions of source code must retain the above copyright
   notice, this list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright
   notice, this list of conditions and the following disclaimer in the
   documentation and/or other materials provided with the distribution.
3. The name of the author may not be used to endorse or promote products
   derived from this software without specific prior written permission.
   
THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL
THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR
OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Earlz.EFramework
{
	public enum HttpMethod{
		Any,
		Get,
		Put,
		Post,
		Delete
	};
	public delegate HttpHandler HandlerInvoker();
	
	/**The routing engine of EFramework.
	 * This is a simple, but powerful router utilizing simple route pattern matching and lambdas for initializing the HttpHandler for a request.**/
	public class Router
	{
		List<Route> Routes=new List<Route>();
		/**Add a route to the list of routes.
		 * @param id The RouteID
		 * @param type The pattern type to use
		 * @param pattern The route pattern to match to
		 * @param handler A lambda(or similar) to invoke a new HttpHandler, such as ()=>{return new MyHandler;}
		 */
		public void AddRoute(string id,PatternTypes type,string pattern,HandlerInvoker handler)
		{
			var r=new Route{Pattern=pattern, Handler=handler, PatternType=type, ID=id};
			Routes.Add(r);
		}
		
		void DoHandler (Route r,HttpContext c,ParameterDictionary p)
		{
			HttpHandler h=r.Handler();
			h.Context=c;
			h.RouteRequest=r;
			h.Method=ConvertMethod(c.Request.HttpMethod);
			h.RouteID=r.ID;
			h.RouteParams=p;
			CallMethod(h);
		}
		/**Handles the current route and calls the appropriate HttpHandler**/
		public bool DoRoute(HttpContext c){
			foreach(var r in Routes){
				switch(r.PatternType){
				case PatternTypes.Regex:
						if(Regex.IsMatch(c.Request.Url.AbsolutePath,r.Pattern)){
							DoHandler(r,c,null);
							return true;
						}
					break;
				case PatternTypes.Plain:
					if(c.Request.Url.AbsolutePath==r.Pattern){
						DoHandler(r,c,null);
						return true;
					}
					break;
				case PatternTypes.Simple:
					var p=new SimplePattern(r.Pattern);
					if(p.DoMatch(c.Request.Url.AbsolutePath)){
						DoHandler(r,c,p.Params);
						return true;
					}
					break;
				default:
					throw new NotImplementedException();
				}
				
			}
			return false;
		}
		HttpMethod ConvertMethod(string m){
			switch(m.ToUpper()){
				case "GET":
					return HttpMethod.Get;
				case "PUT":
					return HttpMethod.Put;
				case "POST":
					return HttpMethod.Post;
				case "DELETE":
					return HttpMethod.Delete;
				default:
					throw new ApplicationException("Cannot convert method name to a method type.");
			}
		}
		void CallMethod(HttpHandler h){
			switch(h.Method){
				case HttpMethod.Get:
					h.Get();
					break;
				case HttpMethod.Delete:
					h.Delete();
					break;
				case HttpMethod.Post:
					h.Post();
					break;
				case HttpMethod.Put:
					h.Put();
					break;
				default:
					throw new ApplicationException("Cannot call appropriate method handler");
			}
		}
		
	}
	public enum PatternTypes{
		/**Use a Regular Expression for pattern matching. Allows the most expression.**/
		Regex, 
		/**Use a plain and exact match for the pattern.**/
		Plain, 
		/**Use the SimplePattern type. 
		 * This is the easiest pattern to use, but is also fairly expressive and allows simple parameter extraction
		 */
		Simple  
	};
	
	public class Route
	{
		public string Pattern;
		public HandlerInvoker Handler;  
		//public Type Handler;
		public PatternTypes PatternType;
		public string ID;
	}
	
	
	
}





