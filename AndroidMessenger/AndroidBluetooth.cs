using System.Collections.Generic;
using System.IO;
using System.Linq;

using Android.Bluetooth;

using Java.Util;

using BluetoothMessengerLib;

namespace AndroidMessenger {
	// TO DO: add server socket functionality
	class AndroidBluetooth : Bluetooth {
		private readonly UUID _uuid = UUID.FromString(UuidString);
		private BluetoothAdapter _adapter = null;
		private ICollection<BluetoothDevice> _pairedDevices = null;
		private BluetoothDevice _device = null;
		private BluetoothSocket _socket = null;
		private bool _isConnected = false;

		public bool IsConnected {
			get { return _isConnected; }
		}

		public UUID Uuid {
			get { return _uuid; }
		}

		public BluetoothAdapter Adapter {
			get { return _adapter; }
		}

		public AndroidBluetooth() {
			_adapter = BluetoothAdapter.DefaultAdapter;
			_pairedDevices = GetPairedDevices();
		}

		// Gets a list of paired devices.
		// Returns null if bluetooth is disabled.
		public List<BluetoothDevice> GetPairedDevices() {
			if (_adapter != null) {
				if (_adapter.IsEnabled) {
					ICollection<BluetoothDevice> devices = new List<BluetoothDevice>();
					devices = _adapter.BondedDevices;
					return devices.ToList();
				}
				else return null;
			}
			else return null;
		}

		// Gets a connection socket from a device address.
		// Returns true/false based on connection success.
		public bool Connect(BluetoothDevice device) {
			if (_device == null)
				foreach (BluetoothDevice i in _pairedDevices)
					if (string.Equals(i.Address, device.Address))
						_device = i;
			BluetoothSocket sock = _device.CreateRfcommSocketToServiceRecord(_uuid);
			try {
				sock.Connect();
				_socket = sock;
				_isConnected = true;
				return true;
			}
			catch {
				_device = null;
				return false;
			}
		}

		// Closes socket connection. Returns true if it succeeds.
		// Returns false if cannot Close the connection.
		public bool Disconnect() {
			try {
				_socket.Close();
				_isConnected = false;
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
			if (_socket.IsConnected) {
				succeed = Send<T>(_socket.OutputStream, data);
				//if (succeed)
				//	Disconnect();
			}
			return succeed;
		}
		
		// Receives an object from a designated socket and returns it.
		public T ReceiveObject<T>() {
			Stream inStream = _socket.InputStream;
			return Get<T>(inStream);
		}

		// This function is used for listening for an incomming connection
		public bool GetIncommingConnection() {
			try {
				BluetoothServerSocket serverSock = _adapter.ListenUsingRfcommWithServiceRecord("Android Bluetooth Messenger", _uuid);
				BluetoothSocket inSock = serverSock.Accept();
				serverSock.Close(); // don't need any more connections comming in
				return true;
			}
			catch {
				return false;
			}
		}
	}
}