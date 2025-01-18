namespace TurtleChallenge;

public class Types
{
	public class Position
	{
		public int X { get; }
		public int Y { get; }

		public Position(
			int x,
			int y)
		{
			X = x;
			Y = y;
		}

		public override bool Equals(object obj)
		{
			return obj is Position other && X == other.X && Y == other.Y;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y);
		}
	}

	public enum Direction
	{
		North,
		East,
		South,
		West
	}
}
