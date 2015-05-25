using System;
using System.Collections.Generic;
using iengine.Connective;

namespace iengine.Utils
{
	// TODO: Respect brackets in sentence (a & b) => (~c => d)
	// TODO: Add support for negated sentences ~(a & b) => c
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
		public static ISolvable Create(string sentence) {
			// check if the sentence contains a connective and if it does create a sentence
			foreach (string s in _connectives)
				if (sentence.Contains(s))
					return CreateSentence(sentence, s);

			// the sentence did not have any connectives, clearly it is a symbol (fact)
			return new Symbol(sentence);
		}

		/// <summary>
		/// Creates the sentence object.
		/// </summary>
		/// <returns>The sentence object.</returns>
		/// <param name="sentence">The sentence to process</param>
		/// <param name="connective">The connective to use between the symbols.</param>
		private static Sentence CreateSentence(string sentence, string connective)
		{
			// split the sentence by the connective
			string[] values = sentence.Split(new string[]{connective}, StringSplitOptions.RemoveEmptyEntries);
			// create a sentence by recursively making an ISolvable from the sentences on either side of the conenctive
			return new Sentence(Create(values[0]), Create(values[1]), CreateConnective(connective));
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
	}
}

