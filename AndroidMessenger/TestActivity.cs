using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
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
				_sentOutput.Text = "Connected\n";
				Message msg = (Message)_connection.ReceiveObject(input);
			};

			// Create your application here
		}
	}
}