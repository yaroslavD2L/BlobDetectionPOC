namespace BlobDetection.Domain {
	internal sealed class TestCaseData {
		public int[][] ImageData { get; set; }
		public TestBlobDetectionResult ExpectedResult { get; set; }
	}

	public class TestBlobDetectionResult {
		public int ReadCount { get; set; }

		public Location EdgeLocation {
			get; set;
		}
	}
}
