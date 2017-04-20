using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;

namespace AndroidMessenger {
	class AndroidBluetoothController {
		event Action _incommingConnectionSuccess;
		event Action _updateMessageList;
		event Action _messageDisconnect;
		Thread _incommingConnection;
		Thread _listenForNewMessage;
		AndroidBluetooth _connection;
		Message _message = null;
		object _messageLock = new object();

		public event Action MessageDisconnect {
			add { _messageDisconnect += value; }
			remove { _messageDisconnect += value; }
		}

		public event Action IncommingConnectionSuccess {
			add { _incommingConnectionSuccess += value; }
			remove { _incommingConnectionSuccess -= value; }
		}

		public event Action UpdateMessageList {
			add { _updateMessageList += value; }
			remove { _updateMessageList -= value; }
		}

		public Message NewMessage {
			get {
				lock (_messageLock) {
					Message msg = _message;
					_message = null;	// Sets _message back to null (probably don't have to do this)
					return msg;
				}
			}
		}

		public AndroidBluetoothController() {
			_connection = new AndroidBluetooth();
			_incommingConnection = new Thread(incommingConnectionListener);
			_listenForNewMessage = new Thread(listenForNewMessage);
			_incommingConnectionSuccess += startListeningForMessages; // <=== TODO: should do something else for this later
			_incommingConnection.Start();
		}

		public bool ConnectToPC() {
			List<BluetoothDevice> devices = _connection.GetPairedDevices();
			foreach (BluetoothDevice i in devices) {
				if (i.Name.Equals("TESLA-WIN")) { // "KEPLER-WIN" or "TESLA-WIN"
					try {
						_connection.Connect(i);
						Parallel.Invoke(_incommingConnectionSuccess);
						return true;
					}
					catch {
						return false;
					}
				}
			}
			return false;
		}

		public bool sendMessage(Message msg) {
			return _connection.SendObject<Message>(msg);
		}

		// The thread that is running this locks until an incomming connection is received.
		private void incommingConnectionListener() {
			_connection.GetIncommingConnection();
			if (_incommingConnectionSuccess != null)
				Parallel.Invoke(_incommingConnectionSuccess);
		}

		// The thread that is runing this will loop until terminated.
		// Waiting for a new message to be received and then passes the message on.
		private void listenForNewMessage() {
			// Improve this so that the thread running this can be aborted/stopped.
			while (true) {
				try {
					lock (_messageLock) {
						Message receivedMessage = _connection.ReceiveObject<Message>();
						_message = receivedMessage;
					}
					Parallel.Invoke(_updateMessageList);
				}
				catch {
					Parallel.Invoke(_messageDisconnect);
				}
			}
		}

		// This method is used to start a message listening thread.
		public void startListeningForMessages() {
			try {
				_listenForNewMessage.Start();
			}
			catch {
				throw;
			}
		}

		public void stopListentingForMessages() {
			_listenForNewMessage.Abort();
		}
	}
}