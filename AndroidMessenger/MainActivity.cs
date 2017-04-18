using Android.App;
using Android.OS;
using Android.Net;
using Android.Content;
using Android.Database;
using Android.Widget;
using System;

namespace AndroidMessenger {
	[Activity(Label = "Android Messenger", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity {
		ConversationList _conversations = null;
		Button _connect;
		Button _selectDevice;
		Button _disconnect;
		TextView _status;

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Launch the test activity
			//StartActivity(typeof(TestActivity));
			
			SetContentView(Resource.Layout.Main);

			_connect = FindViewById<Button>(Resource.Id.Connect);
			_selectDevice = FindViewById<Button>(Resource.Id.SelectDevice);
			_disconnect = FindViewById<Button>(Resource.Id.Disconnect);
			_status = FindViewById<TextView>(Resource.Id.Status);

			_connect.Click += ConnectToPC;
			_selectDevice.Click += SelectConnectionDevice;
			_disconnect.Click += DisconnectFromPC;

		}

		private void ConnectToPC(object sender, EventArgs e) {

		}

		private void SelectConnectionDevice(object sender, EventArgs e) {

		}

		private void DisconnectFromPC(object sender, EventArgs e) {

		}

		public ConversationList generateConversations() {
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

