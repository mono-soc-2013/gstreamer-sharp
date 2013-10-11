// Iterator.cs - Custom iterator wrapper for IEnumerable
//
// Author:  Sebastian Dröge
//			Stephan Sundermann <stephansundermann@gmail.com>
//
// Copyright (c) 2004-2009 Novell, Inc.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of version 2 of the Lesser GNU General 
// Public License as published by the Free Software Foundation.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this program; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.

namespace Gst {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	using GLib;

	public partial class Iterator : IEnumerable {

		private class Enumerator : IEnumerator {
			Iterator iterator;
			Hashtable seen = new Hashtable ();

			private object current = null;
			public object Current {
				get {
					return current;
				}
			}

			public bool MoveNext () {
				IntPtr raw_ret;
				bool retry = false;

				if (iterator.Handle == IntPtr.Zero)
					return false;

				do {
					GLib.Value value;
					IteratorResult ret = iterator.Next (out value);

					switch (ret) {
					case IteratorResult.Done:
						return false;
					case IteratorResult.Ok:
						if (seen.Contains (value)) {
							retry = true;
							break;
						}
						seen.Add (value , null);
						current = value.Val;
						return true;
					case IteratorResult.Resync:
						iterator.Resync ();
						retry = true;
						break;
						default:
					case IteratorResult.Error:
						throw new Exception ("Error while iterating pads");
					}
				} while (retry);

				return false;
			}

			public void Reset () {
				seen.Clear ();
				if (iterator.Handle != IntPtr.Zero)
					iterator.Resync ();
			}

			public Enumerator (Iterator iterator) {
				this.iterator = iterator;
			}
		}

		private Enumerator enumerator = null;

		public IEnumerator GetEnumerator () {
			if (this.enumerator == null)
				this.enumerator = new Enumerator (this);
			return this.enumerator;
		}

		~Iterator () {
			if (Raw != IntPtr.Zero)
				Free ();
		}

	}
}
