using System;

namespace iengine.Connective
{
	public class Conjunction : IConnective
	{
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

