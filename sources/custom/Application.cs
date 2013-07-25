namespace Gst {
	using System;
	using System.Runtime.InteropServices;

	partial class Application 
	{
		public static void Init() {
			gst_init (0, null);
		}
	}
}