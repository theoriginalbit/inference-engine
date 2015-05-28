using System;
using System.Linq;
using iengine.Utils;
using System.Collections.Generic;


namespace iengine.Algorithms
{
	/// <summary>
	/// The forward chaining algorithm class. This class is static, as it is used specifically as a processor, rather than a data object.
	/// </summary>
	public static class ForwardChainingAlgorithm
	{
		/// <summary> 
		/// Ask the provided knowledgebase if it satisifies the query using the forward chaining algorithm.
		/// </summary>
		/// <param name="kb">A knowledgebase to query.</param>
		/// <param name="query">The query to ask</param>
		public static List<string> Ask(KnowledgeBase kb, Query query)
		{
			List<string> known = new List<string>();
			List<ISolvable> unsolved = kb.Solvables.ToList();

			// expand the knowledge base until it is fully expanded or the query is solved
			while (ExpandKnowledgeBase(unsolved, known, query))
				if (known.Contains(query.Identifier))
					break; // the query has been found

			// can't be solved
			if (!known.Contains(query.Identifier))
				return new List<string>();

			// return the solution
			return known;
		}

		/// <summary>
		/// Expands the knowledge base to try find a solution.
		/// </summary>
		/// <returns><c>true</c>, if the knowledgebase has been completely expanded, <c>false</c> otherwise.</returns>
		/// <param name="unsolved">The clauses yet to be processed</param>
		/// <param name="known">The clauses already processed</param>
		/// <param name="query">The query to ask.</param>
		private static bool ExpandKnowledgeBase(List<ISolvable> unsolved, List<string> known, Query query)
		{
			List<ISolvable> processed = new List<ISolvable>();
			ISet<string> discovered = new HashSet<string>();

			foreach (ISolvable solvable in unsolved)
			{
				ISolvable input = null, output = null;

				if (solvable is Symbol)
				{
					// this is a symbol (fact), input and output are the same
					input = output = solvable;
				}
				else if (solvable is Clause)
				{
					// the left-hand side of a clause is the input
					input = (solvable as Clause).LeftSolvable;
					// the right-hand side of a clause is the output
					output = (solvable as Clause).RightSolvable;
				}

				ISet<string> inputSymbols = new HashSet<string>();
				input.FillSymbols(inputSymbols);

				// if this is a symbol (fact) we can add it, however if this is a clause we only add the output symbols if all the input symbols are known
				if (!(solvable is Clause) || inputSymbols.Intersect(known).Count() == inputSymbols.Count)
				{
					output.FillSymbols(discovered);
					processed.Add(solvable);
				}

				// the query has been solved, the solution is found we need to end here so no extra symbols are solved
				if (discovered.Contains(query.Identifier))
					break;
			}

			// remove solved clauses so they're not solved again
			foreach (ISolvable clauses in processed)
				unsolved.Remove(clauses);

			// add the discovered symbols to the known list so future clauses can be solved
			known.AddRange(discovered);

			// if the knowledge base wasn't changed in some way then we cannot solve any further
			return !(processed.Count == 0 && discovered.Count == 0);
		}
	}
}

