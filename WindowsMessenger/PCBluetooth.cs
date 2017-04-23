using System;
using System.Collections.Generic;
using System.IO;

using InTheHand.Net;
using InTheHand.Net.Sockets;

using BluetoothMessengerLib;

namespace WindowsMessenger {
	class PCBluetooth : Bluetooth {
		private readonly Guid _uuid = new Guid(UuidString);
		private BluetoothClient _bluetoothConnection = null;
		private List<BluetoothDeviceInfo> _pairedDevices = null;
		private BluetoothDeviceInfo _device = null;
		private Stream _bluetoothStream = null;
		private int _length;

		public Guid Uuid {
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

		// Connects to selected device and returns true/false based on success
		public bool Connect(BluetoothDeviceInfo device) {
			try {
				var endPoint = new BluetoothEndPoint(device.DeviceAddress, _uuid);
				_bluetoothConnection.Connect(endPoint);
				_bluetoothStream = _bluetoothConnection.GetStream();
				_device = device;
				return true;
			}
			catch {
				_device = null;
				return false;
			}
		}

		
		public bool Disconnect() {
			try {
				_bluetoothConnection.Close();
				_bluetoothStream = null;
				return true;
			}
			catch {
				return false;
			}
		}
		
		// Sends an object. Serialized the object into a JSON string.
		// Then sends the object 
		public bool SendObject<T>(T data) {
			bool succeed = false;
			// For some reason _bluetoothConnection.Connected is false even when connected
			//if (_bluetoothConnection.Connected) {
				succeed = Send<T>(_bluetoothStream, data);
				//if (succeed)
				//	Disconnect();
			//}
			return succeed;
		}

		// Receives an object from a designated socket and returns it.
		public T ReceiveObject<T>() {
			return Get<T>(_bluetoothStream);
		}

		public bool GetIncommingConnection() {
			try {
				BluetoothListener listener = new BluetoothListener(_uuid);
				listener.Start();
				var client = listener.AcceptBluetoothClient();
				_length = client.Available;
				_bluetoothStream = client.GetStream();
				return true;
			}
			catch {
				return false;
			}
		}
	}
}
