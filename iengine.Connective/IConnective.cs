using System;

namespace iengine.Connective
{
	/// <summary>
	/// A connective object. It will have a left and right value, which will need to be able to be solved using a provided algorithm.
	/// </summary>
	public interface IConnective
	{
		/// <summary>
		/// Solve based on the connective type.
		/// </summary>
		/// <param name="lhs">The left symbol</param>
		/// <param name="rhs">The right symbol</param>
		bool Solve(bool lhs, bool rhs);
	}
}

