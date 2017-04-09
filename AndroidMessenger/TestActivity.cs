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
		private Button _getPairedDevices;
		private EditText _phoneNumber;
		private EditText _messageContent;
		private Button _send;
		private ListView _deviceList;
		private AndroidBluetooth _connection;
		
		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Test);

			_connection = new AndroidBluetooth();
			
			_status = FindViewById<TextView>(Resource.Id.SentObject);
			_getPairedDevices = FindViewById<Button>(Resource.Id.GetPairedDevices);
			_phoneNumber = FindViewById<EditText>(Resource.Id.PhoneNumber);
			_messageContent = FindViewById<EditText>(Resource.Id.MessageContent);
			_send = FindViewById<Button>(Resource.Id.Send);
			_deviceList = FindViewById<ListView>(Resource.Id.DeviceList);

			AndroidBluetooth connection = new AndroidBluetooth();
			List<BluetoothDevice> devices = connection.GetPairedDevices();
			//_deviceList.ItemsSource = devices.Name;
			string[] str = new string [] { "Hello", "World" };
			IListAdapter adapter = new ArrayAdapter<string>(this, Resource.Id.DeviceList, str);
			//_deviceList.ItemsSource = str;
			//_status.Text = adapter.Count.ToString() + "\n";
			_status.Text = "";
			foreach (BluetoothDevice i in devices) {
				_status.Text += i.Name + "\n";
			}

			_getPairedDevices.Click += (object sender, EventArgs e) => {
				_getPairedDevices.Text = "This button does nothing";
			};

			_send.Click += (object sender, EventArgs e) => {
				Message newMessage = new Message(_messageContent.Text, _phoneNumber.Text);
				foreach (BluetoothDevice i in devices) {
					if (i.Name.Equals("TESLA-WIN")) {
						try {
							connection.Connect(i);
							connection.SendObject<Message>(newMessage);
							_status.Text += "Sending Success";
							return;
						}
						catch {
							_status.Text += "Connecting Failed";
						}
					}
				}
				_status.Text += "Device not found";
			};

			// Create your application here
		}
	}
}