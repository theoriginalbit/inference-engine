using System;
using System.IO;
using iengine.Utils;
using iengine.Algorithms;
using System.Collections.Generic;

namespace iengine
{
	class MainClass
	{
		private static KnowledgeBase _kb;
		private static Query _query;

		public static void Main(string[] args)
		{
			if (args.Length != 2)
				throw new ArgumentException("Not enough arguments to execute. \n Usage: iengine [method] [filename]");
				
			try
			{
				string[] lines = File.ReadAllLines(args[1]);
				ParseFile(lines);

				switch (args[0])
				{
					case "TT" :
						int count = TruthTableAlgorithm.Ask(_kb, _query);
						Console.WriteLine("{0}\n\n{1}\n\n{2}: {3}", _kb, _query, count > 0 ? "YES" : "NO", count);
						break;
					case "FC" :
						List<string> r1 = ForwardChainingAlgorithm.Ask(_kb, _query);
						Console.WriteLine("{0}\n\n{1}\n\n{2}{3}", _kb, _query, r1.Count > 0 ? "YES: " : "NO", string.Join(", ", r1));
						break;
					case "BC" :
						List<string> r2 = BackwardChainingAlgorithm.Ask(_kb, _query);
						Console.WriteLine("{0}\n\n{1}\n\n{2}{3}", _kb, _query, r2.Count > 0 ? "YES: " : "NO", string.Join(", ", r2));
						break;
					default:
						throw new Exception("Unknown method " + args[0]);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception occured. Message: {0} \n Program terminated.", e.Message);
				Environment.Exit(1);
			}
		}
			

		public static void ParseFile(string[] fileContents)
		{
			string tell = "", ask = "";

			for(int i = 0; i < fileContents.Length; i++)
			{
				if (fileContents[i] == "TELL")
					tell = fileContents[i + 1];
				else if (fileContents[i] == "ASK")
					ask = fileContents[i + 1];
			}

			_kb = new KnowledgeBase(tell);
			_query = new Query(ask);
		}
	}
}
