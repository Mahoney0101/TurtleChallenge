namespace TurtleChallenge.UT;

[TestFixture]
public class Game
{
	private string settingsFilePath;
	private TurtleChallenge.Game game;

	[SetUp]
	public void Setup()
	{
		settingsFilePath = Path.GetTempFileName();
		File.WriteAllText(settingsFilePath, """
			5x4
			x=0, y=1, dir=North
			x=4, y=2
			mines=1,1;1,3;3,3
		""".Trim());

		game = new TurtleChallenge.Game(settingsFilePath);
	}

	[Test]
	public void TurtleChallenge_ctor_Success()
	{
		Assert.DoesNotThrow(() => new TurtleChallenge.Game(settingsFilePath));
	}

	[Test]
	public void TurtleChallenge_ThrowsFileNotFoundException_WhenPassedInvalidFilePath()
	{
		Assert.Throws<FileNotFoundException>(() => new TurtleChallenge.Game("nonexistent.txt"));
	}

	[Test]
	public void TurtleChallenge_ThrowsArgumentException_WhenPassedEmptyFilePath()
	{
		var exception = Assert.Throws<ArgumentException>(() => new TurtleChallenge.Game(""));
		Assert.That(exception.Message, Is.EqualTo("Settings file path cannot be null or empty."));
	}

	[Test]
	public void TurtleChallenge_ThrowsFormatException_SettingsFileContainsInvalidContent()
	{
		File.WriteAllText(settingsFilePath, "Invalid Content");
		Assert.Throws<FormatException>(() => new TurtleChallenge.Game(settingsFilePath));
	}

	[Test]
	public void TurtleChallenge_ThrowsFormatException_FileContainsIncorrectLines()
	{
		settingsFilePath = Path.GetTempFileName();
		File.WriteAllText(settingsFilePath, """
			5x4
			x=0, y=1, dir=North
			x=4, y=2
		""".Trim());

		var exception = Assert.Throws<FormatException>(() => new TurtleChallenge.Game(settingsFilePath));
		Assert.That(exception.Message, Is.EqualTo("Game settings file must contain at least 4 lines."));
	}

	[Test]
	public void TurtleChallenge_ThrowsFormatException_WhenBoardSizeInvalid()
	{
		settingsFilePath = Path.GetTempFileName();
		File.WriteAllText(settingsFilePath, """
			xx4
			x=0, y=1, dir=North
			x=4, y=2
			mines=1,1;1,3;3,3
		""".Trim());

		var exception = Assert.Throws<FormatException>(() => new TurtleChallenge.Game(settingsFilePath));
		Assert.That(exception.Message, Is.EqualTo("Invalid board size format. Expected 'NxM'."));
	}

	[Test]
	public void TurtleChallenge_ThrowsFormatException_WhenStartingPositionInvalid()
	{
		settingsFilePath = Path.GetTempFileName();
		File.WriteAllText(settingsFilePath, """
			2x4
			x=0, y dir=North
			x=4, y=2
			mines=1,1;1,3;3,3
		""".Trim());

		var exception = Assert.Throws<FormatException>(() => new TurtleChallenge.Game(settingsFilePath));
		Assert.That(exception.Message, Is.EqualTo("Invalid starting position format. Expected 'x=X, y=Y, dir=Direction'."));
	}

	[Test]
	public void TurtleChallenge_ThrowsFormatException_WhenStartingCoOrdsInvalid()
	{
		settingsFilePath = Path.GetTempFileName();
		File.WriteAllText(settingsFilePath, """
			2x4
			x=0, y=P, dir=North
			x=4, y=2
			mines=1,1;1,3;3,3
		""".Trim());

		var exception = Assert.Throws<FormatException>(() => new TurtleChallenge.Game(settingsFilePath));
		Assert.That(exception.Message, Is.EqualTo("Invalid starting position coordinates."));
	}

	[Test]
	public void TurtleChallenge_ThrowsFormatException_WhenExitPointFormatInvalid()
	{
		settingsFilePath = Path.GetTempFileName();
		File.WriteAllText(settingsFilePath, """
			2x4
			x=0, y=1, dir=North
			x=4 y=2
			mines=1,1;1,3;3,3
		""".Trim());

		var exception = Assert.Throws<FormatException>(() => new TurtleChallenge.Game(settingsFilePath));
		Assert.That(exception.Message, Is.EqualTo("Invalid exit point format. Expected 'x=X, y=Y'."));
	}

	[Test]
	public void TurtleChallenge_ThrowsFormatException_WhenMineFormatInvalid()
	{
		settingsFilePath = Path.GetTempFileName();
		File.WriteAllText(settingsFilePath, """
			2x4
			x=0, y=1, dir=North
			x=4, y=2
			mines=1,11,3;3,3
		""".Trim());

		var exception = Assert.Throws<FormatException>(() => new TurtleChallenge.Game(settingsFilePath));
		Assert.That(exception.Message, Is.EqualTo("Invalid mine format. Expected 'X,Y'."));
	}

	[TestCase("North", Direction.North)]
	[TestCase("East", Direction.East)]
	[TestCase("South", Direction.South)]
	[TestCase("West", Direction.West)]
	public void ParseDirection_ReturnsCorrectDirection_WhenValidDirectionsSupplied
		(string input, Direction expectedDirection)
	{
		var result = game.ParseDirection(input);

		Assert.That(result, Is.EqualTo(expectedDirection));
	}

	[Test]
	public void ParseDirection_ThrowsFormatException_WhenInvalidDirectionSupplied()
	{
		var exception = Assert.Throws<FormatException>(() => game.ParseDirection("InvalidDirection"));

		Assert.That(exception.Message, Is.EqualTo("Invalid direction. Valid values are North, East, South, or West."));
	}

	[Test]
	public void EvaluateMoves_ReturnsSuccess_WhenValidMovesSupplied()
	{
		var result = game.EvaluateMoves("mrmmmmrmm");

		Assert.That(result, Is.EqualTo("Success!"));
	}

	[Test]
	public void EvaluateMoves_ReturnsInvalidMove_WhenInvalidMoveSupplied()
	{
		var result = game.EvaluateMoves("mxm");

		Assert.That(result, Is.EqualTo("Invalid Move!"));
	}

	[Test]
	public void EvaluateMoves_ReturnsOutOfBounds_WhenOutOfBoundsMoveSuppled()
	{
		var result = game.EvaluateMoves("mmmmmm");

		Assert.That(result, Is.EqualTo("Out of Bounds!"));
	}

	[Test]
	public void EvaluateMoves_ReturnsMineHit_WhenMineHit()
	{
		var result = game.EvaluateMoves("rmmmrmmmrm");

		Assert.That(result, Is.EqualTo("Mine Hit!"));
	}

	[Test]
	public void EvaluateMoves_ThrowsArgumentException_WhenEmptyMovesSupplied()
	{
		Assert.That(() => game.EvaluateMoves(""), Throws.ArgumentException);
	}

	[Test]
	public void EvaluateMoves_ThrowArgumentException_WhenEmptyStringSupplied()
	{
		Assert.Throws<ArgumentException>(() => game.EvaluateMoves(""));
	}

	[TestCase(Direction.North, Direction.East)]
	[TestCase(Direction.East, Direction.South)]
	[TestCase(Direction.South, Direction.West)]
	[TestCase(Direction.West, Direction.North)]
	public void RotateRight_ReturnsNextDirection_WhenValidDirectionSupplied
		(Direction currentDirection,
		Direction expectedDirection)
	{
		var newDirection = game.RotateRight(currentDirection);

		Assert.That(newDirection, Is.EqualTo(expectedDirection));
	}

	[Test]
	public void RotateRight_ThrowsInvalidOperationException_WhenInvalidDirectionSupplied()
	{
		var invalidDirection = (Direction)999;

		Assert.That(() => game.RotateRight(invalidDirection),
					Throws.InvalidOperationException
						  .With.Message.EqualTo($"Invalid direction: {invalidDirection}"));
	}

	[TearDown]
	public void Teardown()
	{
		if (File.Exists(settingsFilePath)) File.Delete(settingsFilePath);
	}
}
