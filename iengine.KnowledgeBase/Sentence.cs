using System;
using System.Collections.Generic;
using iengine.Connective;

namespace iengine.Utils
{
	public class Sentence : ISolvable
	{
		public ISolvable LeftSolvable { get; private set; }
		public ISolvable RightSolvable { get; private set; }
		private readonly IConnective _connective;

		public Sentence(ISolvable left, ISolvable right, IConnective connective)
		{
			LeftSolvable = left;
			RightSolvable = right;
			_connective = connective;
		}
			
		public void AddSymbols(ISet<string> existing) {
			// recursively tell the left and right ISolvables to add their symbols
			LeftSolvable.AddSymbols(existing);
			RightSolvable.AddSymbols(existing);
		}

		public bool Solve(IDictionary<string, bool> inputs) {
			// ask the connective to solve given the recursive solution for the left and right ISolvables
			return _connective.Solve(LeftSolvable.Solve(inputs), RightSolvable.Solve(inputs));
		}

		public override string ToString() {
			return string.Format("({0} {1} {2})", LeftSolvable, _connective, RightSolvable);
		}
	}
}

