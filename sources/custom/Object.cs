namespace Gst {
	using System;
	using System.Runtime.InteropServices;

	partial class Object 
	{
		public object this[string property] {
		  get {
		    GLib.Value v = GetProperty (property);
		    object o = v.Val;
		    v.Dispose ();
		    return o;
		  } set {
		    GLib.Value v = new GLib.Value (this, property);
		    v.Val = value;
		    SetProperty (property, v);
		    v.Dispose ();
		  }
		}

		public void Connect (string signal, SignalHandler handler) {
		  DynamicSignal.Connect (this, signal, handler);
		}

		public void Disconnect (string signal, SignalHandler handler) {
		  DynamicSignal.Disconnect (this, signal, handler);
		}

		public void Connect (string signal, Delegate handler) {
		  DynamicSignal.Connect (this, signal, handler);
		}

		public void Disconnect (string signal, Delegate handler) {
		  DynamicSignal.Disconnect (this, signal, handler);
		}

		public object Emit (string signal, params object[] parameters) {
		  return DynamicSignal.Emit (this, signal, parameters);
		}
	}
}