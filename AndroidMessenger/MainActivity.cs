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
using Android.Provider;
using Newtonsoft.Json;

namespace AndroidMessenger {
	[Activity(Label = "Android Messenger", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity {
		Button _connect;
		Button _disconnect;
		TextView _status;
		ListView _deviceList;
		CheckBox _contactMatching;
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
			_contactMatching = FindViewById<CheckBox>(Resource.Id.EnableContactMatching);

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
					if (_contactMatching.Checked) {
						con = MatchContacts(con);
					}
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

		private ConversationList MatchContacts(ConversationList conList) {
			List<Conversation> cons = conList.Conversations;
			List<Contact> contacts = new List<Contact>();

			// Reads phone contacts into the list
			Android.Net.Uri contactsUri = ContactsContract.Contacts.ContentUri;
			Android.Net.Uri phoneUri = ContactsContract.CommonDataKinds.Phone.ContentUri;
			string[] projection = { ContactsContract.Contacts.InterfaceConsts.Id,
				ContactsContract.Contacts.InterfaceConsts.DisplayName, ContactsContract.Contacts.InterfaceConsts.HasPhoneNumber };
			string[] projectionPhone = { ContactsContract.Contacts.InterfaceConsts.Id,
				ContactsContract.CommonDataKinds.Phone.Number, ContactsContract.Contacts.InterfaceConsts.DisplayName };
			ContentResolver cr = this.ContentResolver;
			ICursor c = cr.Query(contactsUri, projection, null, null, projection[0]);
			ICursor cPhone = cr.Query(phoneUri, projectionPhone, null, null, projection[0]);
			
			if (c.MoveToFirst()) {
				int j = 0;
				for (int i = 0; i < c.Count; i++) {
					try {
						string id = c.GetString(c.GetColumnIndexOrThrow(projection[0]));
						string name = c.GetString(c.GetColumnIndexOrThrow(projection[1]));
						int hasPhone = int.Parse(c.GetString(c.GetColumnIndexOrThrow(projection[2])));
						string phoneId = "";
						string phoneName = "";
						List<string> phone = new List<string>();
						cPhone.MoveToFirst();
						if (hasPhone == 1) {
							// This loop is terribly inefficient but it works
							do {
								try {
									phoneName = cPhone.GetString(cPhone.GetColumnIndexOrThrow(projectionPhone[2]));
									if (phoneName.CompareTo(name) == 0) {
										phoneId = cPhone.GetString(cPhone.GetColumnIndexOrThrow(projection[0]));
										phone.Add(cPhone.GetString(cPhone.GetColumnIndexOrThrow(projectionPhone[1])));
									}
								}
								catch {
									break;
								}
							} while (cPhone.MoveToNext());
						}
						contacts.Add(new Contact(name, phone));
						c.MoveToNext();
					}
					catch {
						break;
					}
				}
			}
			c.Close();
			cPhone.Close();

			string output = JsonConvert.SerializeObject(contacts);

			// Matching name with conversation
			foreach (Conversation conv in cons) {
				string convNumber = conv.PhoneNumber;
				string person = MatchNumbers(contacts, convNumber);
				conv.Who = person;
				List<Message> msgs = conv.Messages;
				foreach (Message msg in msgs) {
					msg.Who = person;
				}
			}
			ConversationList namedConList = new ConversationList(cons);
			return namedConList;
		}

		private string MatchNumbers(List<Contact> contacts, string matchThis) {
			foreach (Contact person in contacts) {
				foreach (string number in person.Numbers) {
					string numberSteralized = sterilizePhoneNumber(number);
					if (numberSteralized.CompareTo(matchThis) == 0) {
						return person.Name;
					}
				}
			}
			return "";
		}

		private string sterilizePhoneNumber(string num) {
			string sterilized = "";
			for (int i = 0; i < num.Length; i++) {
				if (num[i] >= 48 && num[i] <= 57) {
					sterilized += num[i];
				}
			}

			if (sterilized.Length == 11)
				sterilized = '+' + sterilized;
			else if (sterilized.Length == 10)
				sterilized = "+1" + sterilized;
			//else if (sterilized.Length == 5)
			//	return sterilized;
			//else return "INVALID";
			return sterilized;
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

