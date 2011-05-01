using System;
using System.Collections.Generic;
using System.Collections;
namespace Earlz.EFramework
{
	public class ParameterDictionary 
	{
		//TODO fix so that it's not stupid at trying to implement an interface. 
		public ParameterDictionary ()
		{
		}
		Dictionary<string,string> d=new Dictionary<string, string>();
		public string this[string key] {
			get {
				if(d.ContainsKey(key)){
					return d[key];
				}else{
					return null;
				}
			}
			set {
				if(d.ContainsKey(key)){
					d[key]=value;
				}else{
					d.Add(key,value);
				}
			}
		}
	
		public void Add (string key, string value)
		{
			d.Add(key,value);
		}

		public void Clear ()
		{
			d.Clear();
		}

		public bool Contains (string key)
		{
			return d.ContainsKey(key);
		}
		
		public IDictionaryEnumerator GetEnumerator ()
		{
			return d.GetEnumerator();
		}

		public void Remove (string key)
		{
			d.Remove(key);
		}

		public bool IsFixedSize {
			get {
				return false;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		public ICollection Keys {
			get {
				return d.Keys;
			}
		}

		public ICollection Values {
			get {
				return d.Values;
			}
		}
	}
}