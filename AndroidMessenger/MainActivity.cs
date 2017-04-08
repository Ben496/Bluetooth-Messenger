using Android.App;
using Android.Widget;
using Android.OS;
using Android.Net;
using BluetoothMessengerLib;
using Android.Content;
using Android.Runtime;
using Android.Telephony;
using Android.Util;
using Android.Database;

namespace AndroidMessenger {
	[Activity(Label = "Android Messenger", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity {
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);
			
			

			// Set our view from the "main" layout resource
			// SetContentView (Resource.Layout.Main);
		}

		public ConversationList generateConversations() {
			Uri inboxURI = Uri.Parse("content://sms/inbox");
			ConversationList conversations = new ConversationList();
			ContentResolver cr = this.ContentResolver;
			ICursor c = cr.Query(inboxURI, null, null, null, null);
			if (c.MoveToFirst()) {
				for (int i = 0; i < c.Count; i++) {
					Message sms = new Message();
					sms.Text = c.GetString(c.GetColumnIndexOrThrow("body")).ToString();
					sms.PhoneNumber = c.GetString(c.GetColumnIndexOrThrow("address")).ToString();
					conversations.addMessage(sms);
				}
			}
			return conversations;

		}
	}
}

