if (args.Length < 2)
{
	Console.WriteLine("Usage: ./TurtleChallenge.exe game-settings moves");
	return;
}

try
{
	string settingsFilePath = args[0];
	string movesFilePath = args[1];

	var game = new TurtleChallenge.Game(settingsFilePath);
	string[] movesSequences = File.ReadAllLines(movesFilePath);

	foreach (var moves in movesSequences)
	{
		try
		{
			string result = game.EvaluateMoves(moves);
			Console.WriteLine($"Sequence: {moves} -> Result: {result}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error processing sequence '{moves}': {ex.Message}");
		}
	}
}
catch (Exception ex)
{
	Console.WriteLine($"Error: {ex.Message}");
}