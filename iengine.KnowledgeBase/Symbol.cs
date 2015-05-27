using System;
using System.Collections.Generic;

namespace iengine.Utils
{
	public class Symbol : ISolvable
	{
		/// <summary>
		/// The identifier for the symbol
		/// </summary>
		/// <value>The identifier.</value>
		public string Identifier { get; private set; }

		/// <summary>
		/// Is this symbol negated?
		/// </summary>
		private readonly bool _negation;

		/// <summary>
		/// Initializes a new instance of the <see cref="iengine.Utils.Symbol"/> class.
		/// </summary>
		/// <param name="identifier">the string represenation of the symbol.</param>
		/// <param name = "negated">whether this symbol was negated</param>
		public Symbol(string identifier, bool negated)
		{
			_negation = negated;
			Identifier = identifier;
		}

		/// <summary>
		/// Adds the current symbol to the list of existing symbols.
		/// </summary>
		/// <param name="existing">the symbols that exist</param>
		public void FillSymbols(ISet<string> existing) {
			// add the identifier for this symbol to the symbols set
			existing.Add(Identifier);
		}
			
		/// <summary>
		/// Return if the symbol exists in the knowledgebase, and the value of the objects.
		/// </summary>
		/// <param name="inputs">The object to solve against</param>
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
			return (_negation ? "~" : "") + Identifier;
		}
	}
}

