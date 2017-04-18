using System;
using System.Threading;
using System.Windows;

namespace WindowsMessenger {
	public class PCBluetoothController {
		private event Action _incommingConnectionSuccess;
		private event Action<Message> _updateMessageList;
		private Thread _incommingConnection;
		private Thread _listenForNewMessage;
		private PCBluetooth _connection;
		private bool _bluetoothDisable = false;

		// Subscirbe using this action to be called when a device is connected.
		public event Action IncommingConnectionSuccess {
			add { _incommingConnectionSuccess += value; }
			remove { _incommingConnectionSuccess -= value; }
		}

		// Subscribe using this action to be called when a new message is received.
		public event Action<Message> UpdateMessageList {
			add { _updateMessageList += value; }
			remove { _updateMessageList -= value; }
		}

		public PCBluetoothController() {
			_connection = new PCBluetooth();
			_incommingConnection = new Thread(incommingConnectionListener);
			_listenForNewMessage = new Thread(listenForNewMessage);
			_incommingConnectionSuccess += startListeningForMessages; // <=== TODO: should do something else for this later
			_incommingConnection.Start();
		}

		public bool sendMessage(Message msg) {
			return _connection.SendObject<Message>(msg);
		}

		public PCBluetoothController(bool bluetoothDisable) {
			if (bluetoothDisable) {
				_connection = null;
				_incommingConnection = null;
				_listenForNewMessage = null;
				_incommingConnectionSuccess += startListeningForMessages; // <=== TODO: should do something else for this later
				_bluetoothDisable = true;
			}
			else {
				_connection = new PCBluetooth();
				_incommingConnection = new Thread(incommingConnectionListener);
				_listenForNewMessage = new Thread(listenForNewMessage);
				_incommingConnectionSuccess += null;
				_bluetoothDisable = false;
			}
		}

		// The thread that is running this locks until an incomming connection is received.
		private void incommingConnectionListener() {
			_connection.GetIncommingConnection();
			if(_incommingConnectionSuccess != null)
				Application.Current.Dispatcher.Invoke(_incommingConnectionSuccess);
		}

		// The thread that is runing this will loop until terminated.
		// Waiting for a new message to be received and then passes the message on.
		private void listenForNewMessage() {
			// Improve this so that the thread running this can be aborted/stopped.
			while (true) {
				Message receivedMessage = _connection.ReceiveObject<Message>();
				Application.Current.Dispatcher.Invoke(_updateMessageList, receivedMessage);
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
			if(_listenForNewMessage != null)
				_listenForNewMessage.Abort();
		}

		// This method is purely for testing purposes.
		// gives a single method to send all spoofed messages through
		public void createNewMessages(Message newMessage) {
			Application.Current.Dispatcher.Invoke(_updateMessageList, newMessage);
		}
	}
}
