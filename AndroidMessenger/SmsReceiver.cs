
using Android.App;
using Android.Content;

namespace AndroidMessenger {
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

}