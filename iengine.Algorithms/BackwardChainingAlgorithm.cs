using System;
using System.Linq;
using System.Collections.Generic;
using iengine.Utils;

namespace iengine.Algorithms
{
	public static class BackwardChainingAlgorithm
	{
		public static List<string> Ask(KnowledgeBase kb, Query query)
		{
			Queue<string> agenda = new Queue<string>();
			List<string> solved = new List<string>();
			List<ISolvable> unsolved = kb.Solvables.ToList();

			// enqueue the query to solve it
			agenda.Enqueue(query.Identifier);

			// while there is a symbol in the agenda
			while (agenda.Any())
				if (!SolveAgenda(unsolved, agenda, solved))
					return new List<String>();  // cant be solved

			// reverse the order
			solved.Reverse();

			// return the solution
			return solved;
		}

		public static bool SolveAgenda(List<ISolvable> unsolvedSentences, Queue<string> agenda, List<string> solvedVariables)
		{
			// the value symbol to try solve
			string currentSymbol = agenda.Dequeue();

			// Search the KB for a sentence the assigns this variable
			foreach (ISolvable solvable in unsolvedSentences)
			{
				ISolvable input = null, output = null;

				if (solvable is Symbol)
				{
					// this is a fact, input and output are the same
					input = output = solvable;
				}
				else if (solvable is Sentence)
				{
					// the left-hand side of a sentence is the input
					input = (solvable as Sentence).LeftSolvable;
					// the right-hand side of a sentence is the output
					output = (solvable as Sentence).RightSolvable;
				}

				// get the symbols in the output of the sentence
				ISet<string> outputSymbols = new HashSet<string>();
				output.AddSymbols(outputSymbols);

				// if the symbol we're looking for appears in the output of the sentence
				if (outputSymbols.Contains(currentSymbol))
				{
					// this variable is solved
					solvedVariables.Add(currentSymbol);
					unsolvedSentences.Remove(solvable);
					// this solvable was a symbol (fact) there is no input to add to the agenda
					if (solvable is Symbol)
						return true;

					// get the symbols in the input of the sentence
					ISet<string> inputSymbols = new HashSet<string>();
					input.AddSymbols(inputSymbols);

					// add the input symbols to the agenda as long as its not already in there or solved
					foreach (string symbol in inputSymbols)
						if (!agenda.Contains(symbol) && !solvedVariables.Contains(symbol))
							agenda.Enqueue(symbol);

					return true;
				}
			}

			// we have run out of sentences in the knowledge base, we cannot solve this problem
			return false;
		}
	}
}

