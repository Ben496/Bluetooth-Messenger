using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothMessengerLib {
	public class Bluetooth {
		// This string is used to generate the UUID for the bluetooth connection
		// Obtained from https://www.uuidgenerator.net/
		private static readonly string _uuidString = "ede10139-f5e2-433e-a48c-ffde355480ea";

		public static string UuidString {
			get { return _uuidString; }
		}
	}
}
