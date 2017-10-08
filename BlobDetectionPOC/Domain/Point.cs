namespace BlobDetection.Domain {
	public struct Point {

		public Point( int x, int y ) {
			X = x;
			Y = y;
		}

		public int X { get; set; }
		public int Y { get; set; }

		public override bool Equals( object obj ) {
			if( !( obj is Point ) ) {
				return false;
			}

			Point point = (Point)obj;
			return this == point;
		}

		public override int GetHashCode() {
			return X ^ Y;
		}

		public static bool operator ==( Point left, Point right ) {
			return left.X == right.X
				&& left.Y == right.Y;
		}

		public static bool operator !=( Point left, Point right ) {
			return left.X != right.X
				|| left.Y != right.Y;
		}

		public override string ToString() {
			return $"{{{X}, {Y}}}";
		}
	}
}
