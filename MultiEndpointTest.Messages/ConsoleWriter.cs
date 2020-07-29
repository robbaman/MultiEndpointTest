using System;

namespace MultiEndpointTest {
	public static class ConsoleWriter {
		public static void WriteRedLine(string text) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(new string('*', text.Length));
			Console.WriteLine(text);
			Console.WriteLine(new string('*', text.Length));
			Console.ResetColor();

		}
		public static void WriteGreenLine(string text) {
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(new string('*', text.Length));
			Console.WriteLine(text);
			Console.WriteLine(new string('*', text.Length));
			Console.ResetColor();

		}
	}
}