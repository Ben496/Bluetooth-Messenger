using System.Windows;
using System.Windows.Input;

namespace WindowsMessenger {
	/// <summary>
	/// Interaction logic for TestMessageCreator.xaml
	/// </summary>
	public partial class NewMessageWindow : Window {
		private PCBluetoothController _controller = null;

		public NewMessageWindow() {
			InitializeComponent();
		}

		public NewMessageWindow(PCBluetoothController control) {
			_controller = control;
			InitializeComponent();
		}

		// This method goes through the BluetoothController testing setup.
		private void sendNewMessage_Click(object sender, RoutedEventArgs e) {
			// Eliminates the \r\n at the end in a quick and dirty way.
			long tmp = long.Parse(number.Text);
			string messageNumber = tmp.ToString();
			string messageContent = message.Text;
			Message newMessage = new Message(messageContent, messageNumber, true);
			if (newMessage.PhoneNumber == "INVALID") {
				MessageBox.Show("Invalid Number.");
			}
			else {
				_controller.createNewMessages(newMessage);
				_controller.sendMessage(newMessage);
			}

			this.Close();
		}
		private void message_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
			if (Keyboard.IsKeyDown(Key.Enter)) {
				sendNewMessage_Click(this, new RoutedEventArgs());
			}
		}
	}
}
