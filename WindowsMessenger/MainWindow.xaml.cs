using System.Windows;
using WindowsMessenger.ViewModel;

namespace WindowsMessenger {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();

			ConversationViewModel cvm = new ConversationViewModel();
			DataContext = cvm;

			// Show test window.
			Window win = new Test();
			win.Show();

		}
	}
}
