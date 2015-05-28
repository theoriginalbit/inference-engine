using System;
using System.Collections.Generic;
using iengine.Connective;

namespace iengine.Utils
{
	/// <summary>
	/// Processes all of the solvables to create solvable objects.
	/// </summary>
	public class SolvableFactory
	{
		private static readonly List<string> _connectives = new List<string>{"<=>", "=>", "&", "|"};

		private SolvableFactory() {}

		/// <summary>
		/// Creates an ISolvable object based off of the provided clause.
		/// </summary>
		/// <param name="clause">the clause/symbol to process</param>
		public static ISolvable Create(string clause)
		{
			// remove all the spaces from the string because with them building the expression tree is impossibly complex
			clause = clause.Replace(" ", "");

			// whether the clause is negated
			bool negated = false;

			// we need to check if there are brackets (potentially negated) surrounding the entire clause if there
			// are brackets around the clause we must remove them so that we can get the containing parts
			// Note: negated clauses only occur with brackets.
			if (IsClauseBracketed(clause))
			{
				// since we're removing the brackets we must remember if this clause was negated.
				negated = clause.StartsWith("~");
				// remove the outer brackets, the outer brackets make the parsing difficult, we also strip the negation here
				clause = PrepareClause(clause);
			}

			// check if the clause contains a connective and if it does create a clause
			foreach (string connective in _connectives)
			{
				// check where the connective lies
				int index = clause.IndexOf(connective);

				// there was no connective
				if (index == -1) continue;

				// check if each side of the clause is perfectly bracketed. if they're perfectly bracketed we can split by
				// this connective, otherwise if they're not perfectly bracketed we cannot split by this connective yet
				if (CanProcess(clause, index) && CanProcess(clause, index + connective.Length))
				{
					// "split" out the left side of the clause
					string leftSide = clause.Substring(0, index);
					// "split" out the right side of the clause
					string rightSide = clause.Substring(index + connective.Length);
					// return the clause after having recursively processed the clause on each side
					return new Clause(Create(leftSide), Create(rightSide), CreateConnective(connective), negated);
				}
			}

			// the clause did not have any connectives, clearly it is a symbol (fact)
			return new Symbol(clause.Replace("~", ""), clause.StartsWith("~"));
		}

		/// <summary>
		/// Creates the connective.
		/// </summary>
		/// <returns>The connective object</returns>
		/// <param name="connective">The string representation of a connective</param>
		private static IConnective CreateConnective(string connective) 
		{
			switch (connective) {
				case "=>":
					return new Implication();
				case "<=>":
					return new Biconditional();
				case "&":
					return new Conjunction();
				case "|":
					return new Disjunction();
				default :
					throw new ArgumentException("Not a valid connective.");
			}
		}
		
		/// <summary>
		/// Checks if the clause up to the specified index is perfectly bracketed (or not bracketed at all) and can be further processed.
		/// </summary>
		/// <returns><c>true</c> the part of the clause can be processed; otherwise, <c>false</c>.</returns>
		/// <param name="clause">The clause to check.</param>
		/// <param name="endIndex">Where to stop checking.</param>
		private static bool CanProcess(string clause, int endIndex) {
			int count = 0;
			
			for (int i = 0; i < endIndex; ++i) {
				string ch = clause.Substring(i, 1);
				// increment the counter if we find a (
				if (ch == "(") ++count;
				// increment the counter if we find a )
				if (ch == ")") --count;
			}
			
			// return whether all ( had a matching ) if there were any
			return count == 0;
		}

		/// <summary>
		/// Determines if the clause is surrounded by brackets.
		/// </summary>
		/// <returns><c>true</c> if the clause is surrounded by brackets; otherwise, <c>false</c>.</returns>
		/// <param name="clause">The clause to check.</param>
		private static bool IsClauseBracketed(string clause) {
			int count = 0;
			bool foundOneSet = false;
			int endIndex = clause.Length;

			for (int i = 0; i < endIndex; ++i) {
				String ch = clause.Substring(i, 1);
				// increment the counter if we find a (
				if (ch == "(") ++count;

				// increment the counter if we find a )
				if (ch == ")") {
					--count;
					foundOneSet = true; // we've found at least one set
				}

				// if we have found at least one ( and each has a matching ) and we're not at the end of the string, clearly this clause isn't surrounded by brackets
				if (foundOneSet && count == 0 && i < endIndex - 1) {
					return false;
				}
			}

			// return whether we found any brackets and whether they were all paired
			return count == 0 && foundOneSet;
		}

		/// <summary>
		/// Removes any brackets surrounding the clause as well as any negation for the clause. By removing these we make it easier to process the clause.
		/// </summary>
		/// <returns>The prepared clause.</returns>
		/// <param name="clause">The clause to prepare.</param>
		private static String PrepareClause(string clause) {
			// if the clause wasn't bracketed we don't need to do anything to the clause
			if (!IsClauseBracketed(clause)) return clause;

			// determine the starting index to trim brackets and negation
			int startIndex = 0;
			if (clause.StartsWith("(")) startIndex = 1;
			else if (clause.StartsWith("~(")) startIndex = 2;

			// return the bracket-less clause
			return clause.Substring(startIndex, clause.Length - startIndex - 1);
		}
	}
}

