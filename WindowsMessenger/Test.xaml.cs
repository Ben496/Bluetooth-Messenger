using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

using InTheHand.Net;
using InTheHand.Net.Sockets;

namespace WindowsMessenger {
	/// <summary>
	/// Interaction logic for Test.xaml
	/// </summary>
	public partial class Test : Window {
		PCBluetooth _connection;
		List<BluetoothDeviceInfo> _devices;
		public Test() {
			InitializeComponent();
			_connection = new PCBluetooth();
		}

		private void button_Click(object sender, RoutedEventArgs e) {
			_devices = _connection.GetDeviceNames();
			foreach (BluetoothDeviceInfo i in _devices) {
				label.Content = i.DeviceName;
			}
		}

		private void sendButton_Click(object sender, RoutedEventArgs e) {
			// Generating message
			string messageContent;
			string messageNumber;
			try {
				messageContent = text.Text;
				messageNumber = phoneNumber.Text;
			}
			catch {
				messageNumber = "1234567890";
				messageContent = "Hello World!";
			}
			Message testMessage = new Message(messageContent, messageNumber);

			// determine device (ASUS Z00D)
			if (_devices != null) {
				foreach (var i in _devices) {
					if (string.Equals(i.DeviceName, "ASUS_Z00AD") || string.Equals(i.DeviceName, "ASUSZ00AD")) {
						Stream buffer = _connection.Connect(i);
						_connection.SendObject(buffer, testMessage);
						status.Content = "Status: sending complete";
					}
				}
			}
		}
	}
}
