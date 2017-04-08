using System;
using System.Collections.Generic;
using System.IO;

using InTheHand.Net;
using InTheHand.Net.Sockets;

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
		
		// Sends an object. Serialized the object into a JSON string.
		// Then sends the object 
		public bool SendObject<T>(Stream connectionStream, T data) {
			bool succeed = false;
			if (_bluetoothConnection.Connected) {
				succeed = Send<T>(connectionStream, data);
				if (succeed)
					Disconnect();
			}
			return succeed;
		}

		// Receives an object from a designated socket and returns it.
		public T ReceiveObject<T>(Stream connectionStream) {
			return Get<T>(connectionStream);
		}
	}
}
