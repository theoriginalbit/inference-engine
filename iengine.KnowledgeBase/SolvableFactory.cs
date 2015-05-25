using System;
using System.Collections.Generic;
using iengine.Connective;

namespace iengine.Utils
{
	// TODO: Support generic propositional logic.
	public class SolvableFactory
	{
		private static readonly List<string> _connectives = new List<string>{"<=>", "=>", "&", "|"};

		private SolvableFactory() {}

		public static ISolvable Create(string sentence) {
			// check if the sentence contains a connective and if it does create a sentence
			foreach (string s in _connectives)
				if (sentence.Contains(s))
					return CreateSentence(sentence, s);

			// the sentence did not have any connectives, clearly it is a symbol (fact)
			return new Symbol(sentence);
		}

		private static Sentence CreateSentence(string sentence, string connective)
		{
			// split the sentence by the connective
			string[] values = sentence.Split(new string[]{connective}, StringSplitOptions.RemoveEmptyEntries);
			// create a sentence by recursively making an ISolvable from the sentences on either side of the conenctive
			return new Sentence(Create(values[0]), Create(values[1]), CreateConnective(connective));
		}

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

