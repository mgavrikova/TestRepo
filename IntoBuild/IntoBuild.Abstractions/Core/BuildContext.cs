using System.Collections.Generic;

namespace IntoBuild.Core
{
	public class BuildContext
	{
		private readonly IDictionary<string, object> _dictionary;

		public BuildContext()
		{
			_dictionary = new Dictionary<string, object>();
		}

		public object this[string name]
		{
			get
			{
				object value;
				return _dictionary.TryGetValue(name, out value) ? value : null;
			}

			set { _dictionary[name] = value; }
		}


		public bool Contains(string name) => _dictionary.ContainsKey(name);

		public bool Remove(string name) => _dictionary.Remove(name);
	}
}
