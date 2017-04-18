using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;
using Android.Bluetooth;

namespace AndroidMessenger {
	[Activity(Label = "Test Activity")]
	public class TestActivity : Activity {
		private TextView _status;
		private Button _connectToDevice;
		private EditText _phoneNumber;
		private EditText _messageContent;
		private Button _send;
		private ListView _deviceList;
		private AndroidBluetooth _connection;
		private Button _disconnect;
		
		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Test);

			_connection = new AndroidBluetooth();
			
			_status = FindViewById<TextView>(Resource.Id.SentObject);
			_connectToDevice = FindViewById<Button>(Resource.Id.ConnectDevice);
			_phoneNumber = FindViewById<EditText>(Resource.Id.PhoneNumber);
			_messageContent = FindViewById<EditText>(Resource.Id.MessageContent);
			_send = FindViewById<Button>(Resource.Id.Send);
			_deviceList = FindViewById<ListView>(Resource.Id.DeviceList);
			_disconnect = FindViewById<Button>(Resource.Id.Disconnect);

			AndroidBluetooth connection = new AndroidBluetooth();
			List<BluetoothDevice> devices = connection.GetPairedDevices();
			string[] deviceNames = new string[devices.Count];
			for (int i = 0; i < devices.Count; i++) {
				deviceNames[i] = devices[i].Name;
			}
			ArrayAdapter<string> adapter =
				new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, deviceNames);
			_deviceList.Adapter = adapter;
			_deviceList.SetSelector(Android.Resource.Drawable.AlertLightFrame);
			_status.Text = "";
			//foreach (BluetoothDevice i in devices) {
			//	_status.Text += i.Name + "\n";
			//}

			_connectToDevice.Click += (object sender, EventArgs e) => {
				_status.Text = "";
				foreach (BluetoothDevice i in devices) {
					if (i.Name.Equals("KEPLER-WIN")) { // || i.Name.Equals("KEPLER-WIN")
						try {
							connection.Connect(i);
							_status.Text += "Connecting Success\n";
							return;
						}
						catch {
							_status.Text += "Connecting Failed to " + i.Name + "\n";
						}
					}
				}
				_status.Text += "Device not found";
				//_connectToDevice.Text = "This button does nothing";
			};

			_send.Click += (object sender, EventArgs e) => {
				try {
					Message newMessage = new Message(_messageContent.Text, _phoneNumber.Text);
					connection.SendObject<Message>(newMessage);
					_status.Text += "Sending Succeeded";
				}
				catch {
					_status.Text += "Sending Failed\n";
				}
			};

			_disconnect.Click += (object sender, EventArgs e) => {
				connection.Disconnect();
				_status.Text += "Disconnected\n";
			};
			

			// Create your application here
		}
	}
}