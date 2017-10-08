using System.Collections.Generic;

namespace BlobDetection.Domain {
	public sealed class BlobDetectionResult {
		public HashSet<Point> ReadPoints { get; set; }

		public Location EdgeLocation { get; set; }

		internal static void Merge( ref Location location, ref Location newLocation ) {

			if( location == default( Location ) ) {
				location = newLocation;
			}

			if( newLocation == default( Location ) ) {
				return;
			}

			if( location.Top > newLocation.Top ) {
				location.Top = newLocation.Top;
			}

			if( location.Bottom < newLocation.Bottom ) {
				location.Bottom = newLocation.Bottom;
			}

			if( location.Left > newLocation.Left ) {
				location.Left = newLocation.Left;
			}

			if( location.Right < newLocation.Right ) {
				location.Right = newLocation.Right;
			}
		}
	}
}
