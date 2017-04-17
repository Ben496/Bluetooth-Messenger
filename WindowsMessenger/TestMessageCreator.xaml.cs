using System.Windows;

namespace WindowsMessenger {
	/// <summary>
	/// Interaction logic for TestMessageCreator.xaml
	/// </summary>
	public partial class TestMessageCreator : Window {
		private PCBluetoothController _controller = null;

		public TestMessageCreator() {
			InitializeComponent();
		}

		public TestMessageCreator(PCBluetoothController control) {
			_controller = control;
			InitializeComponent();
		}

		// This method goes through the BluetoothController testing setup.
		private void sendTestMessage_Click(object sender, RoutedEventArgs e) {
			string messageNumber = number.Text;
			string messageContent = message.Text;
			Message newMessage = new Message(messageNumber, messageContent);
			_controller.createNewMessages(newMessage);
		}
	}
}
