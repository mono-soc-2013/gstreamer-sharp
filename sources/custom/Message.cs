namespace Gst {

	using System;
	using System.Runtime.InteropServices;

	partial class Message
	{
		[DllImport ("gstreamer-1.0") ]
		static extern void gst_message_parse_error (IntPtr msg, out IntPtr err, out IntPtr debug);

		public void ParseError (out GLib.GException error, out string debug) {
			if (Type != MessageType.Error)
				throw new ArgumentException ();

			IntPtr err;
			IntPtr dbg;

			gst_message_parse_error (Handle, out err, out dbg);

			if (dbg != IntPtr.Zero)
				debug = GLib.Marshaller.Utf8PtrToString (dbg);
			else
				debug = null;

			if (err == IntPtr.Zero)
				throw new Exception ();

			error = new GLib.GException (err);
		}

		[DllImport("gstreamer-1.0", CallingConvention = CallingConvention.Cdecl)]
		static extern IntPtr gst_message_get_stream_status_object(IntPtr raw);

		[DllImport("gstreamer-1.0", CallingConvention = CallingConvention.Cdecl)]
		static extern void gst_message_set_stream_status_object(IntPtr raw, IntPtr value);

		public GLib.Value StreamStatusObject { 
			get {
				if(Type != MessageType.StreamStatus)
					throw new ArgumentException ();
				IntPtr raw_ret = gst_message_get_stream_status_object(Handle);
				GLib.Value ret = (GLib.Value) Marshal.PtrToStructure (raw_ret, typeof (GLib.Value));
				return ret;
			}
			set {
				if(Type != MessageType.StreamStatus)
					throw new ArgumentException ();
				IntPtr native_value = GLib.Marshaller.StructureToPtrAlloc (value);
				gst_message_set_stream_status_object(Handle, native_value);
				value = (GLib.Value) Marshal.PtrToStructure (native_value, typeof (GLib.Value));
				Marshal.FreeHGlobal (native_value);
			}
		}
	}
}