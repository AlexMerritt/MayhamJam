public enum Direction {
	Up, Left, Down, Right
}

public static class DirectionExtensions {
	public static Direction Opposite(this Direction dir)
	{
		switch (dir) {
			case Direction.Up: return Direction.Down;
			case Direction.Down: return Direction.Up;
			case Direction.Left: return Direction.Right;
			case Direction.Right: return Direction.Left;
			default:
				return Direction.Up;
		}
	}
}
