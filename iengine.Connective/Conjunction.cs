using System;

namespace iengine.Connective
{
	public class Conjunction : IConnective
	{
		/// <summary>
		/// Solves the result of the left and right symbols, based on the objects having a conjunctive relationship.
		/// </summary>
		/// <param name="lhs">The left symbol</param>
		/// <param name="rhs">The right symbol</param>
		public bool Solve(bool lhs, bool rhs)
		{
			return lhs && rhs;
		}
			
		public override string ToString()
		{
			return "&";
		}
	}
}

