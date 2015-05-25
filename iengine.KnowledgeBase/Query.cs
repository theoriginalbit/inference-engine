using System;

namespace iengine.Utils
{
	public class Query : Symbol
	{
		/// <summary>
		/// The item to ask for in the database
		/// </summary>
		/// <param name="identifier">The identifier of the query</param>
		public Query(string identifier) : base(identifier) {}

		public override string ToString() {
			return "ASK\n" + base.ToString();
		}
	}
}

