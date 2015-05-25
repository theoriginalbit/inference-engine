using System;
using System.Linq;
using System.Collections.Generic;

namespace iengine.Algorithms
{
	/// <summary>
	/// A wrapper class to hold values for each row of the truth table.
	/// </summary>
	public class Row 
	{
		public Dictionary<string, bool> Values { get; set; }
	}

	/// <summary>
	/// Creates a truth table object based on the provided symbols.
	/// </summary>
	public class TruthTable
	{
		public List<Row> Rows { get; } = new List<Row>();

		/// <summary>
		/// Private instanciation, as we want to instanciate the class properly via the Build method.
		/// </summary>
		private TruthTable() {}

		/// <summary>
		/// Builds a truth table object form the provided symbols.
		/// </summary>
		/// <param name="symbols">The individual symbols to use when creating the truth table</param>
		public static TruthTable Build(ISet<string> symbols)
		{
			// create the truth table
			TruthTable table = new TruthTable();

			// the number of rows needed for this truth table; 2^n
			int rowCount = (int)Math.Pow(2, symbols.Count);

			// fill the current state so that each symbol is false
			List<bool> currentState = symbols.Select(var => false).ToList();

			// fill the truth table (inspired by a binary adder)
			for (int i = 0; i < rowCount; ++i)
			{
				Dictionary<string, bool> row = new Dictionary<string, bool>();
				int f = 1;
				// for each symbol add the current state (inversely)
				foreach (string s in symbols)
					row.Add(s, currentState[symbols.Count - f++]);

				// increment (add) true to the current state
				for (int j = 0; j < currentState.Count; ++j)
				{
					// if this state is false, make it true, otherwise make all other states false
					if (!currentState[j])
					{
						currentState[j] = true;
						break;
					}
					currentState[j] = false;
				}

				// add the row to the table with the values in it
				table.Rows.Add(new Row{ Values = row });
			}

			// return the now filled truth table
			return table;
		}
	}
}

