using System;

using Android.App;
using Android.OS;
using Android.Widget;
using Android.Bluetooth;

namespace AndroidMessenger {
	[Activity(Label = "TestActivity")]
	public class TestActivity : Activity {
		private Button _startServer;
		private TextView _sentOutput;
		private AndroidBluetooth _connection;
		
		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Test);

			_connection = new AndroidBluetooth();

			_startServer = FindViewById<Button>(Resource.Id.StartServer);
			_sentOutput = FindViewById<TextView>(Resource.Id.SentObject);

			_startServer.Click += (object sender, EventArgs e) => {
				BluetoothSocket input = _connection.GetConnection();
				Message msg = _connection.ReceiveObject<Message>(input);
				_sentOutput.Text += msg.ToString();
			};

			// Create your application here
		}
	}
}