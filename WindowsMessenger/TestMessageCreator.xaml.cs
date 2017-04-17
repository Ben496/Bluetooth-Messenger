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
			// Eliminates the \r\n at the end in a quick and dirty way.
			long tmp = long.Parse(number.Text);
			string messageNumber = tmp.ToString();
			string messageContent = message.Text;
			Message newMessage = new Message(messageContent, messageNumber);
			_controller.createNewMessages(newMessage);
		}
	}
}
