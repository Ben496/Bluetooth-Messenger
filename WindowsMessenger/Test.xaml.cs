using System.Collections.Generic;
using System.Windows;
using System.Threading;
using InTheHand.Net.Sockets;
using System;

namespace WindowsMessenger {
	/// <summary>
	/// Interaction logic for Test.xaml
	/// </summary>
	public partial class Test : Window {
		PCBluetooth _connection;
		List<BluetoothDeviceInfo> _devices;
		Thread _incommingConnection;
		Thread _listenForNewMessage;
		event Action _updateIncommingConnectionStatus;

		public Test() {
			InitializeComponent();
			_connection = new PCBluetooth();

			_updateIncommingConnectionStatus += updateConnectionStatus;

			_incommingConnection = new Thread(incommingConnection);
			_incommingConnection.Start();

			//_listenForNewMessage = new Thread(listenForNewMessage);
		}

		private void button_Click(object sender, RoutedEventArgs e) {
			_devices = _connection.GetDeviceNames();
			label.Content = "";
			foreach (BluetoothDeviceInfo i in _devices) {
				label.Content += i.DeviceName + "\n";
			}
		}

		private void sendButton_Click(object sender, RoutedEventArgs e) {
			// Generating message
			string messageContent;
			string messageNumber;
			if(text.Text != "" && phoneNumber.Text != "") {
				messageContent = text.Text;
				messageNumber = phoneNumber.Text;
			}
			else {
				messageNumber = "1234567890";
				messageContent = "Hello World!";
			}
			Message testMessage = new Message(messageContent, messageNumber);

			// determine device (ASUS Z00D)
			if (_devices != null) {
				foreach (var i in _devices) {
					if (string.Equals(i.DeviceName, "ASUS_Z00AD") || string.Equals(i.DeviceName, "ASUSZ00AD")) {
						_connection.Connect(i);
						_connection.SendObject<Message>(testMessage);
						status.Content = "Status: sending complete" + testMessage.ToString();
					}
				}
			}
		}

		private void listenButton_Click(object sender, RoutedEventArgs e) {
			listenButton.Content = "This button does nothing";
		}

		private void getMessage_Click(object sender, RoutedEventArgs e) {
			getMessage.Content = "This button does nothing";
		}

		private void disconnect_Click(object sender, RoutedEventArgs e) {
			_connection.Disconnect();
			connectionStatus.Content = "Connection Status: Disconnected";
		}

		private void incommingConnection() {
			_connection.GetIncommingConnection();
			Application.Current.Dispatcher.Invoke(_updateIncommingConnectionStatus);
			//startListenForNewMessage();
		}

		public void updateConnectionStatus() {
			connectionStatus.Content = "Connection Status: Connected";
			return;
		}

		//private void startListenForNewMessage() {
		//	if (_listenForNewMessage.ThreadState == ThreadState.Unstarted)
		//		_listenForNewMessage.Start();
		//	else
		//		MessageBox.Show("Somehow listening for messages already started: "
		//			+ _listenForNewMessage.ThreadState.ToString());
		//	return;
		//}

		//private void listenForNewMessage() {
		//	Message receivedMessage = _connection.ReceiveObject<Message>();
		//	updateReceivedMessages(receivedMessage);
		//}

		//public void updateReceivedMessages(Message receivedMessage) {
		//	messageLabel.Content += receivedMessage.ToString();
		//}
	}
}
