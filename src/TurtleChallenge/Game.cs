namespace TurtleChallenge;

public class Game
{
	private int BoardWidth, BoardHeight;
	private Position StartPosition;
	private Direction StartDirection;
	private Position ExitPoint;
	private HashSet<Position> Mines;

	public Game(
		string settingsFilePath)
	{
		if (string.IsNullOrWhiteSpace(settingsFilePath))
			throw new ArgumentException("Settings file path cannot be null or empty.");

		if (!File.Exists(settingsFilePath))
			throw new FileNotFoundException("Settings file not found.");

		ParseGameSettings(settingsFilePath);
	}

	private void ParseGameSettings(
		string filePath)
	{
		var lines = File.ReadAllLines(filePath);
		if (lines.Length < 4)
			throw new FormatException("Game settings file must contain at least 4 lines.");

		var boardSize = lines[0].Split('x');
		if (boardSize.Length != 2 ||
			!int.TryParse(boardSize[0], out int boardWidth) ||
			!int.TryParse(boardSize[1], out int boardHeight))
		{
			throw new FormatException("Invalid board size format. Expected 'NxM'.");
		}

		BoardWidth = boardWidth;
		BoardHeight = boardHeight;

		var start = lines[1].Split(',');
		if (start.Length != 3)
			throw new FormatException("Invalid starting position format. Expected 'x=X, y=Y, dir=Direction'.");

		if (!int.TryParse(start[0].Split('=')[1], out var startX) ||
			!int.TryParse(start[1].Split('=')[1], out var startY))
		{
			throw new FormatException("Invalid starting position coordinates.");
		}

		StartPosition = new Position(startX, startY);
		StartDirection = ParseDirection(start[2].Split('=')[1].Trim());

		var exit = lines[2].Split(',');
		if (exit.Length != 2 ||
			!int.TryParse(exit[0].Split('=')[1], out var exitX) ||
			!int.TryParse(exit[1].Split('=')[1], out var exitY))
		{
			throw new FormatException("Invalid exit point format. Expected 'x=X, y=Y'.");
		}
		ExitPoint = new Position(exitX, exitY);

		Mines = [];
		var mineData = lines[3].Split('=')[1].Trim();
		if (!string.IsNullOrEmpty(mineData))
		{
			Mines = mineData
				.Split(';')
				.Select(mine =>
				{
					var coords = mine.Split(',');
					if (coords.Length != 2 ||
						!int.TryParse(coords[0], out var mineX) ||
						!int.TryParse(coords[1], out var mineY))
					{
						throw new FormatException("Invalid mine format. Expected 'X,Y'.");
					}
					return new Position(mineX, mineY);
				})
				.ToHashSet();
		}
	}

	public Direction ParseDirection(
		string direction)
	{
		return direction switch
		{
			"North" => Direction.North,
			"East" => Direction.East,
			"South" => Direction.South,
			"West" => Direction.West,
			_ => throw new FormatException("Invalid direction. Valid values are North, East, South, or West.")
		};
	}

	public string EvaluateMoves(
		string moves)
	{
		if (string.IsNullOrWhiteSpace(moves))
			throw new ArgumentException("Moves cannot be null or empty.");

		var currentPosition = StartPosition;
		var currentDirection = StartDirection;

		foreach (var move in moves)
		{
			switch (move)
			{
				case 'r':
					currentDirection = RotateRight(currentDirection);
					break;

				case 'm':
					currentPosition = MoveForward(currentPosition, currentDirection);
					if (IsOutOfBounds(currentPosition))
						return "Out of Bounds!";
					if (Mines.Contains(currentPosition))
						return "Mine Hit!";
					break;

				default:
					return "Invalid Move!";
			}
		}

		return currentPosition.Equals(ExitPoint) ? "Success!" : "Still in danger!";
	}

	public Direction RotateRight(
		Direction direction) => direction switch
	{
		Direction.North => Direction.East,
		Direction.East => Direction.South,
		Direction.South => Direction.West,
		Direction.West => Direction.North,
		_ => throw new InvalidOperationException($"Invalid direction: {direction}")
	};

	private Position MoveForward(
		Position position, 
		Direction direction)
	{
		return direction switch
		{
			Direction.North => new Position(position.X, position.Y - 1),
			Direction.East => new Position(position.X + 1, position.Y),
			Direction.South => new Position(position.X, position.Y + 1),
			Direction.West => new Position(position.X - 1, position.Y),
			_ => throw new InvalidOperationException($"Invalid direction: {direction}")
		};
	}

	private bool IsOutOfBounds(
		Position position)
	{
		return position.X < 0 || position.Y < 0 || position.X >= BoardWidth || position.Y >= BoardHeight;
	}
}
