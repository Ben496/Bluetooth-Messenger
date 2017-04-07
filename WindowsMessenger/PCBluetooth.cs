using System;
using System.Collections.Generic;
using System.IO;
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
		private List<BluetoothDeviceInfo> _pairedDevices;

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

		public List<BluetoothDeviceInfo> GetDeviceNames() {

			List<BluetoothDeviceInfo> deviceNames = new List<BluetoothDeviceInfo>();
			var array = _bluetoothConnection.DiscoverDevices();
			foreach (BluetoothDeviceInfo i in array) {
				deviceNames.Add(i);
			}
			_pairedDevices = deviceNames;
			return deviceNames;
		}

		// Returns connects to selected device and returns the associated bluetooth stream
		public Stream Connect(BluetoothDeviceInfo device) {
			try {
				var endPoint = new BluetoothEndPoint(device.DeviceAddress, _uuid);
				_bluetoothConnection.Connect(endPoint);
				return _bluetoothConnection.GetStream();
			}
			catch {
				return null;
			}
		}

		
		public bool Disconnect() {
			try {
				_bluetoothConnection.Close();
				return true;
			}
			catch {
				return false;
			}
		}

		// TODO: rework this so it doesn't connect / disconnect each time.
		// Returns true when sending succeeded.
		// False when sending failed.
		public bool SendObject(Stream connectionStream, object data) {
			bool succeed = false;
			if (_bluetoothConnection.Connected) {
				succeed = Send(connectionStream, data);
				if (succeed)
					Disconnect();
			}
			return succeed;
		}

		// Returns an received object. from the phone
		//
		public object ReceiveObject(Stream connectionStream) {
			return Get(connectionStream);
		}
	}
}
