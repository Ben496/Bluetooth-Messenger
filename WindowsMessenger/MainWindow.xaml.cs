using System.Windows;

namespace WindowsMessenger {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();

			// Show test window.
			Window win = new Test();
			win.Show();

		}
	}
}
