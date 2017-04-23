using Android.App;
using Android.Content;
using Android.Telephony;
using System;
using System.Threading;

namespace AndroidMessenger {
	public class SmsReceiverController {
		SmsReceiver _sms;
		Thread _smsThread;
		AndroidBluetoothController _controller;

		public SmsReceiverController(AndroidBluetoothController cont) {
			_controller = cont;
			_smsThread = new Thread(CreateSmsReceiver);
			_smsThread.Start();
		}

		private void CreateSmsReceiver() {
			_sms = new SmsReceiver();
			_sms.NewMessage += _controller.sendMessage;
		}
	}


	[BroadcastReceiver]
	[IntentFilter(new[] { "android.provider.Telephony.SMS_RECEIVED" })]
	public class SmsReceiver : BroadcastReceiver {
		event Func<Message, bool> _newMessage;
		AndroidBluetoothController _controller;

		public event Func<Message, bool> NewMessage {
			add { _newMessage += value; }
			remove { _newMessage -= value; }
		}

		public SmsReceiver() {

		}

		public SmsReceiver(Func<Message, bool> del) {
			_newMessage += del;
		}

		public SmsReceiver(AndroidBluetoothController cont) {
			_controller = cont;
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
					_controller.sendMessage(sms);
					// or (both the above and below are broken)
					// _newMessage(sms);
				}
			}
		}
	}

}