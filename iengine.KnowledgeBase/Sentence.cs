using System;
using System.Collections.Generic;
using iengine.Connective;

namespace iengine.Utils
{
	/// <summary>
	/// A solvable object that contains more than one symbol.
	/// </summary>
	public class Sentence : ISolvable
	{
		/// <summary>
		/// Gets the left solvable.
		/// </summary>
		/// <value>The left solvable.</value>
		public ISolvable LeftSolvable { get; private set; }
		/// <summary>
		/// Gets the right solvable.
		/// </summary>
		/// <value>The right solvable.</value>
		public ISolvable RightSolvable { get; private set; }
		//How these two solvables connect
		private readonly IConnective _connective;

		/// <summary>
		/// Initializes a new instance of the <see cref="iengine.Utils.Sentence"/> class.
		/// </summary>
		/// <param name="left">the left solvable</param>
		/// <param name="right">the right solvable</param>
		/// <param name="connective">how the solvables connect</param>
		public Sentence(ISolvable left, ISolvable right, IConnective connective)
		{
			LeftSolvable = left;
			RightSolvable = right;
			_connective = connective;
		}
			
		/// <summary>
		/// Fills the list of existing symbols with the symbols that currently exist on each side of the sentence.
		/// </summary>
		/// <param name="existing">the symbols that exist</param>
		public void FillSymbols(ISet<string> existing) {
			// recursively tell the left and right ISolvables to add their symbols
			LeftSolvable.FillSymbols(existing);
			RightSolvable.FillSymbols(existing);
		}

		/// <summary>
		/// Return how the two sides of the sentence equate to one another.
		/// </summary>
		/// <param name="inputs">the objects to solve</param>
		public bool Solve(IDictionary<string, bool> inputs) {
			// ask the connective to solve given the recursive solution for the left and right ISolvables
			return _connective.Solve(LeftSolvable.Solve(inputs), RightSolvable.Solve(inputs));
		}

		public override string ToString() {
			return string.Format("({0} {1} {2})", LeftSolvable, _connective, RightSolvable);
		}
	}
}

