using System.Diagnostics;
using BlobDetection.Domain.Default;
using NUnit.Framework;

namespace BlobDetection.Domain {
	[TestFixture]
	internal sealed class BlobDetectorTests {

		private static readonly TestCaseData[] TestCaseData = new[] {
			new TestCaseData {
				ImageData = new [] {
					new [] { 0,0,0 },
					new [] { 0,0,1 },
					new [] { 0,0,0 },
				},
				ExpectedResult = new TestBlobDetectionResult {
					ReadCount = 8,//7,
					EdgeLocation =new Location {
						Top = 1,
						Left = 2,
						Bottom = 1,
						Right = 2
					}
				}
			},
			new TestCaseData {
				ImageData = new [] {
					new [] { 0,0,0 },
					new [] { 0,0,1 },
					new [] { 0,0,1 },
				},
				ExpectedResult = new TestBlobDetectionResult {
					ReadCount = 8,
					EdgeLocation = new Location{
						Top = 1,
						Left = 2,
						Bottom = 2,
						Right = 2
					}
				}
			},
			new TestCaseData {
				ImageData = new [] {
					new [] { 0,0,0,0,0,0,0,0,0,0 },
					new [] { 0,0,1,1,1,0,0,0,0,0 },
					new [] { 0,0,1,1,1,1,1,0,0,0 },
					new [] { 0,0,1,0,0,0,1,0,0,0 },
					new [] { 0,0,1,1,1,1,1,0,0,0 },
					new [] { 0,0,0,0,1,0,1,0,0,0 },
					new [] { 0,0,0,0,1,0,1,0,0,0 },
					new [] { 0,0,0,0,1,1,1,0,0,0 },
					new [] { 0,0,0,0,0,0,0,0,0,0 },
					new [] { 0,0,0,0,0,0,0,0,0,0 },
				},
				ExpectedResult = new TestBlobDetectionResult {
					ReadCount = 52,//57,
					EdgeLocation = new Location {
						Top = 1,
						Left = 2,
						Bottom = 7,
						Right = 6
					}
				}
			},
			new TestCaseData {
				ImageData = new [] {
					new [] { 1,1,1,1,1,1,1,1,1,1 },
					new [] { 1,1,1,1,1,1,1,1,1,1 },
					new [] { 1,1,1,1,1,1,1,1,1,1 },
					new [] { 1,1,1,1,1,1,1,1,1,1 },
					new [] { 1,1,1,1,1,1,1,1,1,1 },
					new [] { 1,1,1,1,1,1,1,1,1,1 },
					new [] { 1,1,1,1,1,1,1,1,1,1 },
					new [] { 1,1,1,1,1,1,1,1,1,1 },
					new [] { 1,1,1,1,1,1,1,1,1,1 },
					new [] { 1,1,1,1,1,1,1,1,1,1 },
				},
				ExpectedResult = new TestBlobDetectionResult {
					ReadCount = 36,//100,
					EdgeLocation = new Location {
						Top = 0,
						Left = 0,
						Bottom = 9,
						Right = 9
					}
				}
			},
			new TestCaseData {
				ImageData = new [] {
					new [] { 0,0,0,0,0,0,0,0,0,0},
					new [] { 1,0,0,0,0,0,0,1,1,1 },
					new [] { 1,0,0,0,0,1,1,1,0,0 },
					new [] { 1,0,1,0,0,0,1,0,0,0 },
					new [] { 1,1,1,1,1,1,1,0,0,0 },
					new [] { 0,1,0,0,1,0,1,0,0,0 },
					new [] { 0,0,0,0,1,0,1,0,0,0 },
					new [] { 0,0,0,0,1,1,1,0,0,0 },
					new [] { 0,0,0,0,0,0,0,0,0,0 },
					new [] { 0,0,0,0,0,0,0,0,0,0 },
				},
				ExpectedResult = new TestBlobDetectionResult {
					ReadCount = 69,//64,
					EdgeLocation = new Location {
						Top = 1,
						Left = 0,
						Bottom = 7,
						Right = 9
					}
				}
			}
		};

		[Test, TestCaseSource( "TestCaseData" )]
		public void Run( TestCaseData testCase ) {

			TestImageEncoder imageEncoder = new TestImageEncoder( testCase.ImageData );
			IBlobSearcher blobSearcher = new SimpleBlobSearcher( imageEncoder );

			//IBlobDetector blobDetector = new RecursiveBasedDetector( imageEncoder, blobSearcher );
			IBlobDetector blobDetector = new ContourTracingDetector( imageEncoder, blobSearcher );

			Location result = blobDetector.Detect();

			int actualReadCount = imageEncoder.PreviousPoints.Count;
			OutputSearchPath( imageEncoder );

			Assert.AreEqual( testCase.ExpectedResult.ReadCount, actualReadCount );

			Assert.AreEqual( testCase.ExpectedResult.EdgeLocation, result );
		}

		private static void OutputSearchPath( TestImageEncoder imageEncoder ) {
			int counter = 1;
			foreach( var item in imageEncoder.PreviousPoints ) {
				imageEncoder.SetValue( item, counter++ );
			}

			int imageWidth = imageEncoder.GetImageWidth();
			int imageHight = imageEncoder.GetImageHight();

			for( int i = 0; i < imageWidth; i++ ) {
				for( int j = 0; j < imageHight; j++ ) {

					Debug.Write( $@"	{ imageEncoder.GetValue( new Point( i, j ) )}," );
				}
				Debug.WriteLine( "" );
			}

			Debug.WriteLine( "" );
		}
	}
}
