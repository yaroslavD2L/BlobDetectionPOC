using System.Collections.Generic;

namespace BlobDetection.Domain {
	public interface IImageEncoder {
		int GetImageWidth();
		int GetImageHight();
		int GetValue( Point point );
		void SetValue( Point point, int value );
	}
}