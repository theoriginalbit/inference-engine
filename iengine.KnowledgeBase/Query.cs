using System;

namespace iengine.Utils
{
	public class Query : Symbol
	{
		public Query(string identifier) : base(identifier) {}

		public override string ToString() {
			return "ASK\n" + base.ToString();
		}
	}
}

