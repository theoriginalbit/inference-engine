using System;
using System.Linq;
using System.Collections.Generic;
using iengine.Utils;

namespace iengine.Algorithms
{
	public static class TruthTableAlgorithm
	{
		public static int Ask(KnowledgeBase kb, Query query)
		{
			// get the unique symbols in the knowledge base
			ISet<string> symbols = kb.GetDistinctSymbols();

			// build the truth table for the symbols
			TruthTable table = TruthTable.Build(symbols);

			// count how many rows satisfy the knowledge base
			int count = 0;
			foreach(Row row in table.Rows)
				if (Satisifiable(kb, row.Values))
					++count;

			// return how many rows satisfied the knowledge base
			return count;
		}

		private static bool Satisifiable(KnowledgeBase kb, Dictionary<string, bool> inputs)
		{
			// attempt to solve each sentence, if it cannot be solved from the truth table row then it is not satisfiable
			foreach (ISolvable solvable in kb.Solvables)
				if (!solvable.Solve(inputs))
					return false;

			return true;
		}
	}
}

