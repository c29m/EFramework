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
using System.Diagnostics;



namespace Earlz.EFramework.TestApp
{


	public class Global : System.Web.HttpApplication
	{

		protected virtual void Application_Start (Object sender, EventArgs e)
		{
			Routing.AddRoute("test",PatternTypes.Simple,"/test1",()=>{return new TestHandler();} );
			Routing.AddRoute("test2",PatternTypes.Simple,"/test/{id}",()=>{return new TestHandler();} );
			Routing.AddRoute("login",PatternTypes.Simple,"/login",()=>{return new TestHandler();});
			Routing.AddRoute("logout",PatternTypes.Simple,"/logout",()=>{return new TestHandler();});
			Routing.AddRoute("whoami",PatternTypes.Simple,"/whoami",()=>{return new TestHandler();});
			Routing.AddRoute("test3",PatternTypes.Simple,"/test3/{id}/{action=[view]}/{*}",()=>{return new TestHandler();});
			Routing.AddRoute("test4",PatternTypes.Simple,"/test4",()=>{return new TextHandler("I'm test 4!");});
		}

		protected virtual void Session_Start (Object sender, EventArgs e)
		{
		}
		protected virtual void Application_BeginRequest (Object sender, EventArgs e)
		{
			Routing.DoRequest(Context,this);
		}
		
		protected virtual void Application_EndRequest (Object sender, EventArgs e)
		{
		}

		protected virtual void Application_AuthenticateRequest (Object sender, EventArgs e)
		{
		}

		protected virtual void Application_Error (Object sender, EventArgs e)
		{
		}

		protected virtual void Session_End (Object sender, EventArgs e)
		{
		}

		protected virtual void Application_End (Object sender, EventArgs e)
		{
		}
	}
}

