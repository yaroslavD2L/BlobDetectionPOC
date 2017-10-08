namespace BlobDetection.Domain.Default {
	internal sealed class RecursiveBasedDetector : IBlobDetector {
		private readonly IImageEncoder m_imageEncoder;
		private readonly IBlobSearcher m_blobSearcher;

		public RecursiveBasedDetector(
			IImageEncoder imageEncoder,
			IBlobSearcher blobSearcher
		) {
			m_imageEncoder = imageEncoder;
			m_blobSearcher = blobSearcher;
		}

		public Location Detect() {

			Point blobPoint = m_blobSearcher.Search();

			Location location = GetBlobEdgeLocation( blobPoint );

			return location;
		}

		private Location GetBlobEdgeLocation( Point point ) {

			bool isBlob = CanRead( point ) && Read( point ) == 1;

			if( !isBlob ) {
				return default( Location );
			}

			Location location = new Location {
				Top = point.X,
				Left = point.Y,
				Bottom = point.X,
				Right = point.Y
			};

			Location upBlobLocation = GetBlobEdgeLocation( new Point( point.X - 1, point.Y ) );
			BlobDetectionResult.Merge( ref location, ref upBlobLocation );

			Location rightBlobLocation = GetBlobEdgeLocation( new Point( point.X, point.Y + 1 ) );
			BlobDetectionResult.Merge( ref location, ref rightBlobLocation );

			Location downBlobLocation = GetBlobEdgeLocation( new Point( point.X + 1, point.Y ) );
			BlobDetectionResult.Merge( ref location, ref downBlobLocation );

			Location leftBlobLocation = GetBlobEdgeLocation( new Point( point.X, point.Y - 1 ) );
			BlobDetectionResult.Merge( ref location, ref leftBlobLocation );

			return location;
		}

		private bool CanRead( Point point ) {
			if( point.X < 0 || point.Y < 0 ||
				point.X >= m_imageEncoder.GetImageHight() ||
				point.Y >= m_imageEncoder.GetImageWidth()
			) {
				return false;
			}

			return true;
		}

		private int Read( Point point ) {

			if( m_imageEncoder.GetValue( point ) != 1 ) {
				return 0;
			}

			m_imageEncoder.SetValue( point, value: 0 );
			return 1;
		}
	}
}