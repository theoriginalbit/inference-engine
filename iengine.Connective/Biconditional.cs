using System;

namespace iengine.Connective
{
	public class Biconditional : IConnective
	{
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

