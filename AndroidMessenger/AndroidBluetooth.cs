using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Java.Util;

using Newtonsoft.Json;

using BluetoothMessengerLib;

namespace AndroidMessenger {
	// TO DO: add server socket functionality
	class AndroidBluetooth : Bluetooth {
		private static readonly UUID _uuid = UUID.FromString(UuidString);
		private BluetoothAdapter _adapter;
		private List<BluetoothDevice> _pairedDevices;
		private BluetoothSocket _inputSocket;	// add functionality for this field
		private BluetoothSocket _outputSocket;	// add functionality for this field

		public static UUID Uuid {
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
			if (_adapter.IsEnabled) {
				List<BluetoothDevice> devices = new List<BluetoothDevice>();
				devices = (List<BluetoothDevice>)_adapter.BondedDevices;
				_pairedDevices = devices;
				return devices;
			}
			else {
				return null;
			}
		}

		// Gets a connection socket from a device address.
		// Returns null if could not connect or device is not found.
		public BluetoothSocket Connect(string address) {
			foreach (BluetoothDevice i in _pairedDevices) {
				if (string.Equals(i.Address, address)) {
					BluetoothSocket sock = i.CreateRfcommSocketToServiceRecord(_uuid);
					try {
						sock.Connect();
						return sock;
					}
					catch {
						return null;
					}
				}
			}
			return null;
		}

		// Closes socket connection. Returns true if it succeeds.
		// Returns false if cannot Close the connection.
		public bool Disconnect(BluetoothSocket socket) {
			try {
				socket.Close();
				return true;
			}
			catch {
				return false;
			}
		}

		// Sends an object. Serialized the object into a JSON string.
		// Then sends the object 
		public bool SendObject(BluetoothSocket socket, Object obj) {
			if (socket.IsConnected) {
				var outStream = socket.OutputStream;
				Send(outStream, obj);
				return true;
			}
			return false;
		}

		// This method is so terrible that i know it can be done better.
		// Receives an object from a designated socket.
		public object ReceiveObject(BluetoothSocket socket) {
			Stream inStream = socket.InputStream;
			return Get(inStream);
		}
	}
}