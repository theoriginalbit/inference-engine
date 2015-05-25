using System;
using System.Collections.Generic;

namespace iengine.Utils
{
	public class Symbol : ISolvable
	{
		public string Identifier { get; private set; }
		private readonly bool _negation;

		public Symbol(string identifier)
		{
			// is this symbol negated
			_negation = identifier.StartsWith("~");
			// remove the negation from this symbol
			Identifier = identifier.Replace("~", "");
		}

		public void AddSymbols(ISet<string> existing) {
			// add the identifier for this symbol to the symbols set
			existing.Add(Identifier);
		}
			
		public bool Solve(IDictionary<string, bool> inputs) {
			// We achieve negation here by applying an XOR to the boolean that specifies
			// if Symbol is negated with the value of this symbol represented in inputs.
			//
			// false xor false = false
			// false xor true = true
			// true xor false = true
			// true xor true = false
			return (inputs.ContainsKey(Identifier) && (_negation ^ inputs[Identifier]));
		}

		public override string ToString() {
			return Identifier;
		}
	}
}

