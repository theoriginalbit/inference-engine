using System;

namespace iengine.Connective
{
	public class Disjunction : IConnective
	{
		public bool Solve(bool lhs, bool rhs)
		{
			return lhs || rhs;
		}

		public override string ToString()
		{
			return "|";
		}
	}
}

