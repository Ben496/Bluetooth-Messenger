using Android.App;
using Android.OS;
using Android.Net;
using Android.Content;
using Android.Database;
using Android.Telephony;

namespace AndroidMessenger {
	[Activity(Label = "Android Messenger", MainLauncher = true, Icon = "@drawable/icon")]
	[BroadcastReceiver]
	[IntentFilter(new[] { "android.provider.Telephony.SMS_RECEIVED" })]

	public class SmsReceiver : BroadcastReceiver {
		public override void OnReceive(Context context, Intent intent) {
			if (intent.HasExtra("pdus")) {
				var smsArray = (Java.Lang.Object[])intent.Extras.Get("pdus");
				Message sms;
				string address = "";
				string message = "";
				foreach (var item in smsArray) {
					var mess = SmsMessage.CreateFromPdu((byte[])item);
					message += mess.MessageBody;
					address = mess.OriginatingAddress;
					sms = new Message(message, address, true);
					// THIS IS WHERE WE NEED TO SEND THE SMS TO THE PHONE
				}
			}
		}
	}



	public class MainActivity : Activity {
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Launch the test activity
			StartActivity(typeof(TestActivity));
			// WE NEED TO SEND OVER CONVERSATIONS FROM THIS POINT

			// Set our view from the "main" layout resource
			// SetContentView (Resource.Layout.Main);
		}



		public ConversationList generateConversations() {
			Uri inboxURI = Uri.Parse("content://sms/");
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
				}
			}
			return conversations;

		}
	}
}

