using Android.App;
using Android.OS;
using Android.Net;
using Android.Content;
using Android.Database;
using Android.Widget;
using System;
using Android.Telephony;
using Android.Bluetooth;
using System.Collections.Generic;
using Android.Views;

namespace AndroidMessenger {
	[Activity(Label = "Android Messenger", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity {
		Button _connect;
		Button _disconnect;
		TextView _status;
		ListView _deviceList;
		string _deviceName = "";

		AndroidBluetoothController _controller;

		//SmsReceiver _receiveController;

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Launch the test activity
			//StartActivity(typeof(TestActivity));

			_controller = new AndroidBluetoothController(this);
			_controller.IncommingConnectionSuccess += () => { _status.Text = "Status: Connected"; };
			_controller.UpdateMessageList += sendMessage;

			//_receiveController = new SmsReceiver();
			SmsReceiver.NewMessage += _controller.sendMessage;

			SetContentView(Resource.Layout.Main);

			_connect = FindViewById<Button>(Resource.Id.Connect);
			_disconnect = FindViewById<Button>(Resource.Id.Disconnect);
			_status = FindViewById<TextView>(Resource.Id.Status);
			_deviceList = FindViewById<ListView>(Resource.Id.DeviceList);

			// Populating listview with paired devices
			// TODO: make a controller for this
			PopulateDeviceList();
			

			_connect.Click += ConnectToPC;
			_disconnect.Click += DisconnectFromPC;
			_deviceList.ItemClick += Items;
		}

		private void Items(object sender, AdapterView.ItemClickEventArgs e) {
			_deviceName = _deviceList.GetItemAtPosition(e.Position).ToString();
		}

		private void ConnectToPC(object sender, EventArgs e) {
			if (_deviceName != "") {
				if (_controller.ConnectToPC(_deviceName)) {
					_status.Text = "Status: Connected";
					ConversationList con = generateConversations();
					_controller.sendConversations(con.Conversations);
				}
				else
					_status.Text = "Status: Connection Failed";
			}
			else
				_status.Text = "Status: Please select device";
		}

		// Not really sure where to put this, but I added the function to send a text message over the network.
		private void sendMessage(Message sms) {
			SmsManager.Default.SendTextMessage(sms.PhoneNumber, null, sms.Text, null, null);
		}

		private void DisconnectFromPC(object sender, EventArgs e) {
			_controller.DisconnectFromPC();
		}

		public void NewIncommingMessage(Message msg) {
			_controller.sendMessage(msg);
		}

		private void PopulateDeviceList() {
			AndroidBluetooth connection = new AndroidBluetooth();
			List<BluetoothDevice> devices = connection.GetPairedDevices();
			if (devices == null) {
				_status.Text = "Bluetooth communication is necessary to run this software";
				return;
			}
			string[] deviceNames = new string[devices.Count];
			for (int i = 0; i < devices.Count; i++) {
				deviceNames[i] = devices[i].Name;
			}
			ArrayAdapter<string> adapter =
				new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, deviceNames);
			_deviceList.Adapter = adapter;
			_deviceList.SetSelector(Android.Resource.Drawable.AlertDarkFrame);
		}

		private ConversationList generateConversations() {
			Android.Net.Uri inboxURI = Android.Net.Uri.Parse("content://sms/");
			ConversationList conversations = new ConversationList();
			ContentResolver cr = this.ContentResolver;
			ICursor c = cr.Query(inboxURI, null, null, null, null);
			if (c.MoveToFirst()) {
				for (int i = 0; i < c.Count; i++) {
					Message sms = new Message();
					sms.Text = c.GetString(c.GetColumnIndexOrThrow("body")).ToString();
					sms.PhoneNumber = c.GetString(c.GetColumnIndexOrThrow("address")).ToString();
					string tmp = c.GetString(c.GetColumnIndexOrThrow("date"));
					sms.Time = ulong.Parse(tmp);
					//sms.Time = (uint)c.GetInt(c.GetColumnIndexOrThrow("date"));
					if (c.GetInt(c.GetColumnIndexOrThrow("type")) == 2) {
						sms.isSent = true;
					}
					else sms.isSent = false;
					conversations.addMessage(sms);
					c.MoveToNext();
				}
			}
			return conversations;

		}
	}
}

