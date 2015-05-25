using System;
using System.Collections.Generic;

namespace iengine.Utils
{
	public interface ISolvable
	{
		/// <summary>
		/// Adds the symbols
		/// </summary>
		/// <param name="existing">the symbols that exist</param>
		void FillSymbols(ISet<string> existing);

		/// <summary>
		/// Solve the objects
		/// </summary>
		/// <param name="inputs">the objects to solve</param>
		bool Solve(IDictionary<string, bool> inputs);
	}
}

