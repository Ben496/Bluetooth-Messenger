using Android.App;
using Android.OS;
using Android.Net;
using Android.Content;
using Android.Database;
using Android.Widget;
using System;
using Android.Telephony;

namespace AndroidMessenger {
	[Activity(Label = "Android Messenger", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity {
		ConversationList _conversations = null;
		Button _connect;
		Button _selectDevice;
		Button _disconnect;
		TextView _status;
		TextView _displayMessage;
		AndroidBluetoothController _controller;

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Launch the test activity
			//StartActivity(typeof(TestActivity));

			_controller = new AndroidBluetoothController();
			_controller.IncommingConnectionSuccess += () => { _status.Text = "Status: Connected"; };
			_controller.UpdateMessageList += NewReceivedMessage;
			
			SetContentView(Resource.Layout.Main);

			_connect = FindViewById<Button>(Resource.Id.Connect);
			_selectDevice = FindViewById<Button>(Resource.Id.SelectDevice);
			_disconnect = FindViewById<Button>(Resource.Id.Disconnect);
			_status = FindViewById<TextView>(Resource.Id.Status);
			_displayMessage = FindViewById<TextView>(Resource.Id.Status);

			_connect.Click += ConnectToPC;
			_selectDevice.Click += SelectConnectionDevice;
			_disconnect.Click += DisconnectFromPC;

		}

		private void ConnectToPC(object sender, EventArgs e) {
			if (_controller.ConnectToPC())
				_status.Text = "Status: Connected";
			else
				_status.Text = "Status: Connection Failed";
		}

		private void SelectConnectionDevice(object sender, EventArgs e) {

		}

		// Not really sure where to put this, but I added the function to send a text message over the network.
		private void sendMessage(Message sms) {
			SmsManager.Default.SendTextMessage(sms.PhoneNumber, null, sms.Text, null, null);
		}

		// Should fix any phone number problems. I'm not sure what other ways it enters, so Benji will need
		// to test this problem.
		private string sterilizePhoneNumber(string num) {
			string sterilized = num;
			if (num.Length == 10) {
				sterilized = "+1" + sterilized;
			}
			return sterilized;
		}

		private void DisconnectFromPC(object sender, EventArgs e) {

		}

		public void NewReceivedMessage() {
			Message msg = _controller.NewMessage;
			_displayMessage.Text = msg.ToString();
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
					sms.Time = c.GetInt(c.GetColumnIndexOrThrow("date"));
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

