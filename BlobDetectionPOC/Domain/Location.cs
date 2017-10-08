namespace BlobDetection.Domain {
	public struct Location {

		public int Top { get; set; }
		public int Bottom { get; set; }
		public int Left { get; set; }
		public int Right { get; set; }

		public override bool Equals( object obj ) {
			if( !( obj is Location ) ) {
				return false;
			}

			Location keyPoint = (Location)obj;

			return this == keyPoint;
		}

		public override int GetHashCode() {
			return Top ^ Left ^ Right ^ Bottom;
		}

		public static bool operator ==( Location x, Location y ) {
			return x.Top == y.Top
				&& x.Left == y.Left
				&& x.Right == y.Right
				&& x.Bottom == y.Bottom;
		}

		public static bool operator !=( Location x, Location y ) {
			return x.Top != y.Top
				|| x.Left != y.Left
				|| x.Right != y.Right
				|| x.Bottom != y.Bottom;
		}
	}
}