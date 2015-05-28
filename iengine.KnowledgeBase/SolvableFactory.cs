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
		/// Creates an ISolvable object based off of the provided sentence.
		/// </summary>
		/// <param name="sentence">the sentence/symbol to process</param>
		public static ISolvable Create(string sentence)
		{
			// remove all the spaces from the string because with them building the expression tree is impossibly complex
			sentence = sentence.Replace(" ", "");

			// whether the sentence is negated
			bool negated = false;

			// we need to check if there are brackets (potentially negated) surrounding the entire sentence if there
			// are brackets around the sentence we must remove them so that we can get the containing parts
			// Note: negated sentences only occur with brackets.
			if (IsSentenceBracketed(sentence))
			{
				// since we're removing the brackets we must remember if this sentence was negated.
				negated = sentence.StartsWith("~");
				// remove the outer brackets, the outer brackets make the parsing difficult, we also strip the negation here
				sentence = PrepareSentence(sentence);
			}

			// check if the sentence contains a connective and if it does create a sentence
			foreach (string connective in _connectives)
			{
				// check where the connective lies
				int index = sentence.IndexOf(connective);

				// there was no connective
				if (index == -1) continue;

				// check if each side of the sentence is perfectly bracketed. if they're perfectly bracketed we can split by
				// this connective, otherwise if they're not perfectly bracketed we cannot split by this connective yet
				if (CanProcess(sentence, index) && CanProcess(sentence, index + connective.Length))
				{
					// "split" out the left side of the sentence
					string leftSide = sentence.Substring(0, index);
					// "split" out the right side of the sentence
					string rightSide = sentence.Substring(index + connective.Length);
					// return the sentence after having recursively processed the sentence on each side
					return new Clause(Create(leftSide), Create(rightSide), CreateConnective(connective), negated);
				}
			}

			// the sentence did not have any connectives, clearly it is a symbol (fact)
			return new Symbol(sentence.Replace("~", ""), sentence.StartsWith("~"));
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
		/// Checks if the sentence up to the specified index is perfectly bracketed (or not bracketed at all) and can be further processed.
		/// </summary>
		/// <returns><c>true</c> the part of the sentence can be processed; otherwise, <c>false</c>.</returns>
		/// <param name="sentence">The sentence to check.</param>
		/// <param name="endIndex">Where to stop checking.</param>
		private static bool CanProcess(string sentence, int endIndex) {
			int count = 0;
			
			for (int i = 0; i < endIndex; ++i) {
				string ch = sentence.Substring(i, 1);
				// increment the counter if we find a (
				if (ch == "(") ++count;
				// increment the counter if we find a )
				if (ch == ")") --count;
			}
			
			// return whether all ( had a matching ) if there were any
			return count == 0;
		}

		/// <summary>
		/// Determines if the sentence is surrounded by brackets.
		/// </summary>
		/// <returns><c>true</c> if the sentence is surrounded by brackets; otherwise, <c>false</c>.</returns>
		/// <param name="sentence">The sentence to check.</param>
		private static bool IsSentenceBracketed(string sentence) {
			int count = 0;
			bool foundOneSet = false;
			int endIndex = sentence.Length;

			for (int i = 0; i < endIndex; ++i) {
				String ch = sentence.Substring(i, 1);
				// increment the counter if we find a (
				if (ch == "(") ++count;

				// increment the counter if we find a )
				if (ch == ")") {
					--count;
					foundOneSet = true; // we've found at least one set
				}

				// if we have found at least one ( and each has a matching ) and we're not at the end of the string, clearly this sentence isn't surrounded by brackets
				if (foundOneSet && count == 0 && i < endIndex - 1) {
					return false;
				}
			}

			// return whether we found any brackets and whether they were all paired
			return count == 0 && foundOneSet;
		}

		/// <summary>
		/// Removes any brackets surrounding the sentence as well as any negation for the sentence. By removing these we make it easier to process the sentence.
		/// </summary>
		/// <returns>The prepared sentence.</returns>
		/// <param name="sentence">The sentence to prepare.</param>
		private static String PrepareSentence(string sentence) {
			// if the sentence wasn't bracketed we don't need to do anything to the sentence
			if (!IsSentenceBracketed(sentence)) return sentence;

			// determine the starting index to trim brackets and negation
			int startIndex = 0;
			if (sentence.StartsWith("(")) startIndex = 1;
			else if (sentence.StartsWith("~(")) startIndex = 2;

			// return the bracket-less sentence
			return sentence.Substring(startIndex, sentence.Length - startIndex - 1);
		}
	}
}

