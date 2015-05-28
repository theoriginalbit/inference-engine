using System;
using System.Linq;
using System.Collections.Generic;
using iengine.Utils;

namespace iengine.Algorithms
{
	/// <summary>
	/// The backward chaining algorithm class. This class is static, as it is used specifically as a processor, rather than a data object.
	/// </summary>
	public static class BackwardChainingAlgorithm
	{
		/// <summary> 
		/// Ask the provided knowledgebase if it satisifies the query using the backward chaining algorithm.
		/// </summary>
		/// <param name="kb">A knowledgebase to query.</param>
		/// <param name="query">The query to ask</param>
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

		/// <summary>
		/// Continues to process the agenda to check if the query is met.
		/// </summary>
		/// <returns><c>true</c>, if agenda was solved, <c>false</c> otherwise.</returns>
		/// <param name="unsolvedClauses">The clauses in the knowledgebase that haven't yet been queried.</param>
		/// <param name="agenda">The clauses available.</param>
		/// <param name="solvedVariables">The clauses already processed.</param>
		private static bool SolveAgenda(List<ISolvable> unsolvedClauses, Queue<string> agenda, List<string> solvedVariables)
		{
			// the value symbol to try solve
			string currentSymbol = agenda.Dequeue();

			// Search the KB for a clause the assigns this variable
			foreach (ISolvable solvable in unsolvedClauses)
			{
				ISolvable input = null, output = null;

				if (solvable is Symbol)
				{
					// this is a fact, input and output are the same
					input = output = solvable;
				}
				else if (solvable is Clause)
				{
					// the left-hand side of a clause is the input
					input = (solvable as Clause).LeftSolvable;
					// the right-hand side of a clause is the output
					output = (solvable as Clause).RightSolvable;
				}

				// get the symbols in the output of the clause
				ISet<string> outputSymbols = new HashSet<string>();
				output.FillSymbols(outputSymbols);

				// if the symbol we're looking for appears in the output of the clause
				if (outputSymbols.Contains(currentSymbol))
				{
					// this variable is solved
					solvedVariables.Add(currentSymbol);
					unsolvedClauses.Remove(solvable);
					// this solvable was a symbol (fact) there is no input to add to the agenda
					if (solvable is Symbol)
						return true;

					// get the symbols in the input of the clause
					ISet<string> inputSymbols = new HashSet<string>();
					input.FillSymbols(inputSymbols);

					// add the input symbols to the agenda as long as its not already in there or solved
					foreach (string symbol in inputSymbols)
						if (!agenda.Contains(symbol) && !solvedVariables.Contains(symbol))
							agenda.Enqueue(symbol);

					return true;
				}
			}

			// we have run out of clauses in the knowledge base, we cannot solve this problem
			return false;
		}
	}
}

