using System;
using System.Text;
using System.Collections.Generic;

namespace iengine.Utils
{
	public class KnowledgeBase
	{
		public List<ISolvable> Solvables { get; private set; }

		public KnowledgeBase(string tell)
		{
			Solvables = new List<ISolvable>();

			// remove spaces, spaces are annoying
			tell = tell.Replace(" ", "");

			// split the knowledge base by semi-colons and create an ISolvable for each sentence
			foreach (string s in tell.Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries))
				Solvables.Add(SolvableFactory.Create(s));
		}

		public ISet<string> GetDistinctSymbols()
		{
			// a set is used so that the symbols are unique
			HashSet<string> symbols = new HashSet<string>();

			// tell each ISolvable to add its symbols to the set
			foreach (ISolvable solvable in Solvables)
				solvable.AddSymbols(symbols);

			return symbols;
		}

		public override string ToString() {
			StringBuilder sb = new StringBuilder("TELL\n");

			// append the string for each ISolvable
			foreach (ISolvable solvable in Solvables)
				sb.AppendFormat("{0}; ", solvable);

			return sb.ToString();
		}
	}
}

