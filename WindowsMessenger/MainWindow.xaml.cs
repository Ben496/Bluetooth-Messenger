using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WindowsMessenger.ViewModel;

namespace WindowsMessenger {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		//	PCBluetooth _connection;
		//	List<BluetoothDeviceInfo> _devices;
		ConversationList convos = new ConversationList();
		ConversationViewModel cvm;
		PCBluetoothController _bluetooth;

		public MainWindow() {
			InitializeComponent();

			try {
				_bluetooth = new PCBluetoothController();
			}
			catch (System.PlatformNotSupportedException) {
				MessageBox.Show("This pc either has bluetooth disabled or doesn't have a compatable bluetooth adapter");
				this.Close();
				return;
			}
			_bluetooth.IncommingConnectionSuccess += connectedInfo;
			_bluetooth.UpdateMessageList += addNewMessage;
			_bluetooth.Disconnected += disconnectedInfo;

			//convos.addMessage(new Message("HEY FRIEND", "(615)630-0003", true, 1));
			//convos.addMessage(new Message("WADDUP", "6156300003", false, 2));
			//convos.addMessage(new Message("NAW", "6156300003", true, 2));
			//convos.addMessage(new Message("WHAT", "6156300003", false, 2));
			//convos.addMessage(new Message("WADDUP", "6157146407", true, 2));
			//convos.addMessage(new Message("NAWMUCH", "6157146407", false, 2));

			cvm = new ConversationViewModel(convos);
			DataContext = cvm;

			// creating testing interface
			//Window messageTesting = new TestMessageCreator(_bluetooth);
			//messageTesting.Show();

			// Show test window.
			//Window win = new Test();
			//win.Show();

		}
		
		public void addNewMessage(Message newMessage) {
			cvm.addMessage(newMessage);
		}

		// Temporary function to display a message box when application is connected.
		public void connectedInfo() {
			MessageBox.Show("Connected to device");
		}

		public void disconnectedInfo() {
			MessageBox.Show("Disconnected from device");
		}

		private void textBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
			Conversations.Focus();
			Conversations.CaretIndex = Conversations.Text.Length;
			Conversations.ScrollToEnd();
		}

		private void connectionButton_Click(object sender, RoutedEventArgs e) {

			_bluetooth.connectToDevice();
			//	_devices = _connection.GetDeviceNames();
			//	foreach (BluetoothDeviceInfo i in _devices) {
			//		label.Content = i.DeviceName;
			//	}
		}

		private void sendButton_Click(object sender, RoutedEventArgs e) {
			string messageContent;
			string messageNumber;
			if (MessageText.Text != "") {
				messageContent = MessageText.Text;
				Conversation outgoing = (Conversation)cvm.Selected;
				messageNumber = outgoing.PhoneNumber;
			}
			else {
				return;
			}
			Message newMessage = new Message(messageContent, messageNumber, true);
			if (newMessage.PhoneNumber == "INVALID") {
				MessageBox.Show("Invalid Number.");
			}
			else {
				addNewMessage(newMessage);
				_bluetooth.sendMessage(newMessage);
			}
		}

		private void On_Closing(object sender, CancelEventArgs e) {
			if (_bluetooth != null)
				_bluetooth.disconnect();
		}

		private void sendNewButton_Click(object sender, RoutedEventArgs e) {
			Window newMessage = new NewMessageWindow(_bluetooth);
			newMessage.Show();
		}
	}
}
