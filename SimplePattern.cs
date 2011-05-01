
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
// needs to parse /products/{action=[view, edit, delete]}/{id} and /products/{action=[new]}
// also needs to parse /products/{id}/{*} where the * is optional for instance for /products/20/some-product-name
namespace Earlz.EFramework
{
	public class SimplePattern
	{
		private string pattern;
		private List<Group> Groups;
		
		private class Group {
			public string ParamName;
			public string Text;
			public bool IsParam=false;
			public List<string> ValidMatches=new List<string>();
			public bool MatchAll=true;
			public bool Optional=false;
			public char End;
		}
		
		public SimplePattern (string pattern)
		{
			Pattern = pattern;
		}
		
		public string Pattern {
			get { return pattern; }
			set {
				if (value == null)
					throw new ArgumentNullException ("pattern");
				pattern = value;
				
				UpdateGroups ();
			}
		}
		
		public ParameterDictionary Params{get;set;}
		/**This method returns true(and populates Params) if the input string matches the Pattern string. **/
		public bool DoMatch (string input)
		{
			Params=new ParameterDictionary();
			string s=input;
			if(Groups.Count==0){
				throw new ApplicationException("Groups.Count==0 matches all. This shouldn't happen");
			}
			foreach(var g in Groups){
				if(!g.IsParam){
					if(g.Text.Length>=s.Length){
						if(Groups[Groups.Count-1]==g){
							return g.Text==s; //to check for exact matches(but only for the last group)
						}else{
							if(g.Optional){
								return true;
							}else{
								return false;
							}
						}
					}
					string tmp=CutString(s,0,g.Text.Length);
					if(g.Text==tmp){
						s=s.Substring(g.Text.Length);
					}else{
						return false;
					}
				}else{
					int end;
					if(g.End=='\0'){
						end=s.Length;
					}else{
						end=s.IndexOf(g.End);
						if(end==-1){
							return false;
						}
					}
					if(g.MatchAll){
						if(s.Substring(0,end)==""){
							return false;
						}
						int slash=s.IndexOf('/');
						if(slash==-1 || g.Optional){
							Params.Add(g.ParamName,s.Substring(0,end));
							s=""; //doesn't matter. 
						}else{
							Params.Add(g.ParamName,s.Substring(0,slash));
							s=s.Substring(slash); //doesn't matter. 
								
						}
					}else{
						string t=s.Substring(0,end);
						bool matched=false;
						foreach(var match in g.ValidMatches){
							if(match==t){
								matched=true;
								//break;
							}
						}
						if(matched==false){
							return false;
						}
						Params.Add(g.ParamName,t);
						s=s.Substring(end);
					}
				}
				
				
			}
			if(s.Length!=0){
				return false;
			}else{
				return true;
			}
		}
		/** This will parse the Pattern string one group at a time. **/
		int ParseParam (int start, ref Group g)
		{
			start++;
			
			int end=Pattern.Substring(start).IndexOf('}')+start;
			if(end+1>=Pattern.Length-1){
				g.End='\0';
			}else{
				g.End=Pattern[end+1];
			}
			string p=CutString(Pattern,start,end);
			g.Text=p;
			int tmp=p.IndexOf('[');
			if(tmp==-1){ //not found. Just trim it up and get the paramname
				p=p.Trim();
				if(p=="*"){
					g.Optional=true; //meh. Still add it as a match-all group for the hell of it
				}
				g.MatchAll=true;
			}else{
				//return end;
				g.MatchAll=false;
				string l=CutString(p,tmp+1,p.IndexOf(']'));
				l=l.Replace(" ","");
				p=p.Substring(0,p.IndexOf("=")).Trim();
				int count=0;
				while(true){
					if(l.Length==0){
						break;
					}
					int endm=l.IndexOf(',');
					if(endm==-1){
						endm=l.Length;
						g.ValidMatches.Add(l);
						break;
					}
					g.ValidMatches.Add(l.Remove(endm));
					l=l.Substring(endm+1);
					count++;
					if(count>100){
						throw new ApplicationException("inifinite loop detected");
					}
				}
			}
			
			g.ParamName=p;
			return end;
		}
		/**Little helper method to cut a string from start to end point. Just shorter than typing .Remove(end).Substring(start) **/
		private string CutString(string s,int start,int end){
			return s.Remove(end).Substring(start);
		}
		/** This will update all of the "groups" or parameter names/values for the pattern string. 
		 * Automatically done upon the update of Pattern */
		private void UpdateGroups ()
		{
			List<Group> groups = new List<Group> ();
			Group g=new Group();
			for(int i=0;i<Pattern.Length;i++){
				if(Pattern[i]=='{'){
					if(g!=null)
						groups.Add(g);
					g=new Group();
					g.IsParam=true;
					i=ParseParam(i,ref g);
					groups.Add(g);
					g=null;
				}else if(g==null){
					g=new Group();
					g.IsParam=false;
					g.Text+=Pattern[i];
				}else{
					g.Text+=Pattern[i];
				}
			}
			if(g!=null){
				groups.Add(g);
			}
			Groups=groups;
		}
	}
}



