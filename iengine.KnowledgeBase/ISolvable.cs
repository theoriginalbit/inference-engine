using System;
using System.Collections.Generic;

namespace iengine.Utils
{
	public interface ISolvable
	{
		void AddSymbols(ISet<string> existing);

		bool Solve(IDictionary<string, bool> inputs);
	}
}

