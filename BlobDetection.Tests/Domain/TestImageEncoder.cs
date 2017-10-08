using System.Collections.Generic;

namespace BlobDetection.Domain {
	internal class TestImageEncoder : IImageEncoder {
		private int[][] m_inputData;

		public TestImageEncoder( int[][] inputData ) {
			m_inputData = inputData;
			PreviousPoints = new HashSet<Point>();
		}

		public HashSet<Point> PreviousPoints { get; private set; }

		public int GetImageHight() {
			return m_inputData.Length;
		}

		public int GetImageWidth() {
			return m_inputData[0].Length;
		}

		public int GetValue( Point point ) {
			PreviousPoints.Add( point );

			return m_inputData[point.X][point.Y];
		}

		public void SetValue( Point point, int value ) {
			m_inputData[point.X][point.Y] = value;
		}
	}
}