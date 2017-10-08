namespace BlobDetection.Domain.Default {
	internal sealed class SimpleBlobSearcher : IBlobSearcher {
		private readonly IImageEncoder m_imageEncoder;

		public SimpleBlobSearcher( IImageEncoder encoder ) {
			m_imageEncoder = encoder;
		}

		public Point Search() {

			int imageWidth = m_imageEncoder.GetImageWidth();
			int imageHight = m_imageEncoder.GetImageHight();

			for( int i = 0; i < imageWidth; i++ ) {
				for( int j = 0; j < imageHight; j++ ) {

					Point point = new Point( i, j );

					int value = m_imageEncoder.GetValue( point ); ;

					if( value == 1 ) {
						return point;
					}
				}
			}

			return default( Point );
		}
	}
}
