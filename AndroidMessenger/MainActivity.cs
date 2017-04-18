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
		Button _getConversations;
		TextView _conversationOutput;

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Launch the test activity
			//StartActivity(typeof(TestActivity));


			// WE NEED TO SEND OVER CONVERSATIONS FROM THIS POINT

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			_getConversations = FindViewById<Button>(Resource.Id.GetConversations);
			_conversationOutput = FindViewById<TextView>(Resource.Id.ConversationOutput);

			_getConversations.Click += (object sender, EventArgs e) => {
				buttonClick();
			};
		}

		private void buttonClick() {
			_conversationOutput.Text = "";
				_conversations = generateConversations();
				int num = _conversations.Size();
				for (int i = 0; i < num; i++) {
					Conversation con = _conversations.AccessConversation(i);
					_conversationOutput.Text += con.PhoneNumber + "\n";
				}
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
				}
			}
			return conversations;

		}
	}
}

