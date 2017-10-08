using System.Collections.Generic;

namespace BlobDetection.Domain {
	public interface IBlobDetector {
		Location Detect();
	}
}