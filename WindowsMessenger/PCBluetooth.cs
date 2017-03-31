using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InTheHand.Net;
using InTheHand.Net.Sockets;
using Newtonsoft.Json;

using BluetoothMessengerLib;

namespace WindowsMessenger {
	class PCBluetooth : Bluetooth {
		private static readonly Guid _uuid = new Guid(UuidString);
		private BluetoothClient _bluetoothConnection;
		private static List<BluetoothDeviceInfo> _pairedDevices;

		public static Guid Uuid {
			get { return _uuid; }
		}

		public BluetoothClient BluetoothConnection {
			get { return _bluetoothConnection; }
		}

		public List<BluetoothDeviceInfo> PairedDevices {
			get { return _pairedDevices; }
		}
		
		public PCBluetooth() {
			_bluetoothConnection = new BluetoothClient();
		}
	}
}
