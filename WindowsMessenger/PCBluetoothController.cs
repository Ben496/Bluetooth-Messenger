using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WindowsMessenger {
	class PCBluetoothController {
		event Action _incommingConnectionSuccess;
		event Action<Message> _updateMessageList;
		Thread _incommingConnection;
		Thread _listenForNewMessage;
		PCBluetooth _connection;

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
			_listenForNewMessage.Abort();
		}
	}
}
