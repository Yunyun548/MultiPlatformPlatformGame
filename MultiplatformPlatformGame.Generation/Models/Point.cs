using System;

namespace MultiplatformPlatformGame.Generation.Models
{
	public struct Point
	{
		public Point (int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public int X { get; set; }
		public int Y { get; set; }

		public static bool operator ==(Point p1, Point p2) 
		{
			return p1.X == p2.X && p2.Y == p2.Y;
		}

		public static bool operator !=(Point p1, Point p2) 
		{
			return p1.X != p2.X || p2.Y != p2.Y;
		}

		public override bool Equals(object o) 
		{
			try 
			{
				return (bool) (this == (Point) o);
			}
			catch 
			{
				return false;
			}
		}

		public override int GetHashCode() 
		{
			return X.GetHashCode() + Y.GetHashCode();
		}
	}
}