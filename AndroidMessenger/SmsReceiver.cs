using Android.App;
using Android.Content;
using Android.Telephony;
using System;

namespace AndroidMessenger {
	[BroadcastReceiver]
	[IntentFilter(new[] { "android.provider.Telephony.SMS_RECEIVED" })]
	public class SmsReceiver : BroadcastReceiver {
		event Func<Message, bool> _newMessage;

		public event Func<Message, bool> NewMessage {
			add { _newMessage += value; }
			remove { _newMessage -= value; }
		}

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
					sms = new Message(message, address, false); // shouldn't this be false? (I changed it to false)
					// THIS IS WHERE WE NEED TO SEND THE SMS TO THE PHONE
					_newMessage(sms);
				}
			}
		}
	}

}