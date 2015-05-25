using System;
using System.Text;
using System.Collections.Generic;

namespace iengine.Utils
{
	/// <summary>
	/// A knowledge base; holds the list of solvable objects that were provided in the file.
	/// </summary>
	public class KnowledgeBase
	{
		/// <summary>
		/// The solvable objects in the knowledgebase.
		/// </summary>
		public List<ISolvable> Solvables { get; private set; }

		/// <summary>
		/// Creates a new instance of the knowledgebase.
		/// </summary>
		/// <param name="tell">The values to populate the knowledgebase</param>
		public KnowledgeBase(string tell)
		{
			Solvables = new List<ISolvable>();

			// remove spaces, spaces are annoying
			tell = tell.Replace(" ", "");

			// split the knowledge base by semi-colons and create an ISolvable for each sentence
			foreach (string s in tell.Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries))
				Solvables.Add(SolvableFactory.Create(s));
		}

		/// <summary>
		/// Gets the distinct symbols from each solvable.
		/// </summary>
		/// <returns>The distinct symbols.</returns>
		public ISet<string> GetDistinctSymbols()
		{
			// a set is used so that the symbols are unique
			HashSet<string> symbols = new HashSet<string>();

			// tell each ISolvable to add its symbols to the set
			foreach (ISolvable solvable in Solvables)
				solvable.FillSymbols(symbols);

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

