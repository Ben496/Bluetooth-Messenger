using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Windows;
using WindowsMessenger.ViewModel;

namespace WindowsMessenger {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		//	PCBluetooth _connection;
		//	List<BluetoothDeviceInfo> _devices;
		ConversationList convos = new ConversationList();
		public MainWindow() {
			InitializeComponent();
			//		_connection = new PCBluetooth();
			convos.addMessage(new Message("HEY FRIEND", "6156300003", true, 1));
			convos.addMessage(new Message("WADDUP", "6156300003", false, 2));
			convos.addMessage(new Message("NAW", "6156300003", true, 2));
			convos.addMessage(new Message("WHAT", "6156300003", false, 2));
			convos.addMessage(new Message("WADDUP", "6157146407", true, 2));
			convos.addMessage(new Message("NAWMUCH", "6157146407", false, 2));

			ConversationViewModel cvm = new ConversationViewModel(convos);
			DataContext = cvm;

			// Show test window.
			//	Window win = new Test();
			//	win.Show();

		}

		private void textBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {

		}

		private void connectionButton_Click(object sender, RoutedEventArgs e) {
		//	_devices = _connection.GetDeviceNames();
		//	foreach (BluetoothDeviceInfo i in _devices) {
		//		label.Content = i.DeviceName;
		//	}
		}

		private void sendButton_Click(object sender, RoutedEventArgs e) {
		/*	// Generating message
			string messageContent;
			string messageNumber;
			if (text.Text != "" && phoneNumber.Text != "") {
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
						Stream buffer = _connection.Connect(i);
						_connection.SendObject<Message>(buffer, testMessage);
						status.Content = "Status: sending complete" + testMessage.ToString();
				}
			}*/
		}
	}
}
}
