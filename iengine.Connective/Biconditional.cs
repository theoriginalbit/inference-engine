using System;

namespace iengine.Connective
{
	public class Biconditional : IConnective
	{
		/// <summary>
		/// Solves the result of the left and right symbols, based on the objects having a biconditional relationship.
		/// </summary>
		/// <param name="lhs">The left symbol</param>
		/// <param name="rhs">The right symbol</param>
		public bool Solve(bool lhs, bool rhs)
		{
			return !(lhs ^ rhs); // <=> is the opposite of xor (^)
		}

		public override string ToString()
		{
			return "<=>";
		}
	}
}

