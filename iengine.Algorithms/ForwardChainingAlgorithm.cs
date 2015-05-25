using System;
using System.Linq;
using iengine.Utils;
using System.Collections.Generic;


namespace iengine.Algorithms
{
	public static class ForwardChainingAlgorithm
	{
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
				else if (solvable is Sentence)
				{
					// the left-hand side of a sentence is the input
					input = (solvable as Sentence).LeftSolvable;
					// the right-hand side of a sentence is the output
					output = (solvable as Sentence).RightSolvable;
				}

				ISet<string> inputSymbols = new HashSet<string>();
				input.AddSymbols(inputSymbols);

				// if this is a symbol (fact) we can add it, however if this is a sentence we only add the output symbols if all the input symbols are known
				if (!(solvable is Sentence) || inputSymbols.Intersect(known).Count() == inputSymbols.Count)
				{
					output.AddSymbols(discovered);
					processed.Add(solvable);
				}

				// the query has been solved, the solution is found we need to end here so no extra symbols are solved
				if (discovered.Contains(query.Identifier))
					break;
			}

			// remove solved sentences so they're not solved again
			foreach (ISolvable sentences in processed)
				unsolved.Remove(sentences);

			// add the discovered symbols to the known list so future sentences can be solved
			known.AddRange(discovered);

			// if the knowledge base wasn't changed in some way then we cannot solve any further
			return !(processed.Count == 0 && discovered.Count == 0);
		}
	}
}

