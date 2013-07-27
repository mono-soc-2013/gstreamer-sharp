namespace Gst {
	using System;
	using System.Runtime.InteropServices;

	partial class Element 
	{
		public static bool Link (params Element [] elements) {
			for (int i = 0; i < elements.Length - 1; i++) {
				if (!elements[i].Link (elements[i+1]))
					return false;
			}
			return true;
		}

		public static void Unlink (params Element [] elements) {
			for (int i = 0; i < elements.Length - 1; i++) {
				elements[i].Unlink (elements[i+1]);
			}
		}
	}
}