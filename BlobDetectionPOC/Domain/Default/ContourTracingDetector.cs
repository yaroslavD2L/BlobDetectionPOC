using System;
using System.Collections.Generic;

namespace BlobDetection.Domain.Default {
	internal sealed class ContourTracingDetector : IBlobDetector {
		private const int MaxNeighboringPoints = 8;
		private readonly IImageEncoder m_imageEncoder;
		private readonly List<Point> m_contourPoints = new List<Point>();
		private readonly IBlobSearcher m_blobSearcher;

		public ContourTracingDetector(
			IImageEncoder imageEncoder,
			IBlobSearcher blobSearcher
		) {
			m_imageEncoder = imageEncoder;
			m_blobSearcher = blobSearcher;
		}

		public Location Detect() {

			ResetCounters();

			Point blobPoint = m_blobSearcher.Search();

			Location location = TraceContour( blobPoint, 5 );

			return location;
		}

		private void ResetCounters() {
			m_contourPoints.Clear();
		}

		private Location TraceContour( Point currentPosition, int previousContourPoint ) {

			Location location = new Location {
				Top = currentPosition.X,
				Left = currentPosition.Y,
				Bottom = currentPosition.X,
				Right = currentPosition.Y
			};

			int startContourPoint = ( previousContourPoint + 2 ) % MaxNeighboringPoints;

			int currentContourPoint = startContourPoint;

			for( int i = 0; i < 8; i++ ) {
				currentContourPoint = ( startContourPoint + i ) % MaxNeighboringPoints;

				Point nextPosition = CalculateNextPosition( currentPosition, currentContourPoint );

				if( !CanRead( nextPosition ) ) {
					continue;
				}

				if( ContourClosed( currentPosition, nextPosition ) ) {
					break;
				}

				if( Read( nextPosition ) == 1 ) {
					int relativeContourPoint = ( currentContourPoint + ( MaxNeighboringPoints / 2 ) ) % MaxNeighboringPoints;
					Location nextBlobLocation = TraceContour(
						nextPosition,
						previousContourPoint: relativeContourPoint
					);
					BlobDetectionResult.Merge( ref location, ref nextBlobLocation );

					break;
				}
			}

			return location;
		}

		private bool ContourClosed( Point currentPosition, Point nextPosition ) {
			return m_contourPoints.Count > 1
				&& m_contourPoints[0] == currentPosition
				&& m_contourPoints[1] == nextPosition;
		}

		private void SavePosition( Point currentPosition ) {
			if( m_contourPoints.Count < 2 ) {
				m_contourPoints.Add( currentPosition );
			}
		}

		/// <summary>
		/// The neighboring points of P are indexed from 0 to 7
		/// 5 6 7
		/// 4 P 0
		/// 3 2 1
		/// </summary>
		private static Point CalculateNextPosition( Point currentPosition, int nextContourPoint ) {
			//Point nextPoint;
			switch( nextContourPoint ) {
				case 0:
					currentPosition.Y++;
					break;
				case 1:
					currentPosition.X++;
					currentPosition.Y++;
					break;
				case 2:
					currentPosition.X++;
					break;
				case 3:
					currentPosition.X++;
					currentPosition.Y--;
					break;
				case 4:
					currentPosition.Y--;
					break;
				case 5:
					currentPosition.X--;
					currentPosition.Y--;
					break;
				case 6:
					currentPosition.X--;
					break;
				case 7:
					currentPosition.X--;
					currentPosition.Y++;
					break;
				default:
					throw new Exception( $"{nameof( nextContourPoint )} should be in range from 0 to 7." );
			}

			return currentPosition;
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
			CellType value = (CellType)m_imageEncoder.GetValue( point );

			switch( value ) {
				case CellType.Contour:
				case CellType.BlackPixel:
					m_imageEncoder.SetValue( point, value: (int)CellType.Contour );

					SavePosition( point );
					return 1;
				case CellType.WhitePixel:
					m_imageEncoder.SetValue( point, value: (int)CellType.ExternalContour );
					break;
				case CellType.ExternalContour:
					break;
			}

			return 0;
		}

	}
}
