using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WindowsMessenger {
	class BluetoothController {
		event Action _incommingConnectionSuccess;
		event Action<Message> _updateMessageList;
		Thread _incommingConnection;
		Thread _listenForNewMessage;
		PCBluetooth _connection;

		public event Action IncommingConnectionSuccess {
			add { _incommingConnectionSuccess += value; }
			remove { _incommingConnectionSuccess -= value; }
		}

		public event Action<Message> UpdateMessageList {
			add { _updateMessageList += value; }
			remove { _updateMessageList -= value; }
		}

		public BluetoothController() {
			_connection = new PCBluetooth();
			_incommingConnection = new Thread(incommingConnectionListener);
			_listenForNewMessage = new Thread(listenForNewMessage);
			_incommingConnectionSuccess += startListeningForMessages; // <=== TODO: should do something else for this later
			_incommingConnection.Start();
		}

		private void incommingConnectionListener() {
			_connection.GetIncommingConnection();
			if(_incommingConnectionSuccess != null)
				Application.Current.Dispatcher.Invoke(_incommingConnectionSuccess);
		}

		private void listenForNewMessage() {
			while (true) {
				Message receivedMessage = _connection.ReceiveObject<Message>();
				Application.Current.Dispatcher.Invoke(_updateMessageList, receivedMessage);
			}
		}

		private void startListeningForMessages() {
			try {
				_listenForNewMessage.Start();
			}
			catch {
				throw;
			}
		}
	}
}
