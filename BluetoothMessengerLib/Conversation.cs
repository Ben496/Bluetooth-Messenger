using System;
using System.Collections.Generic;
using System.ComponentModel;

public class Conversation : INotifyPropertyChanged
{
	private List<Message> _messages;
	private Contact _who;
	private string _phoneNumber;
	private string _text;

	public event PropertyChangedEventHandler PropertyChanged;

	private void RaisePropertyChanged(string property) {
		if (PropertyChanged != null) {
			PropertyChanged(this, new PropertyChangedEventArgs(property));
		}
	}

	public Contact Who {
		get { return _who; }
		set { _who = value; }
	}

	public List<Message> Messages {
		get { return _messages; }
		set { _messages = value; }
	}

	public string PhoneNumber {
		get { return _phoneNumber; }
		set { _phoneNumber = sterilizePhoneNumber(value); }
	}

	private string sterilizePhoneNumber(string num) {
		string sterilized = num;
		if (sterilized.Length != 12 && sterilized.Length != 10) {
			return "INVALID";
		}
		else if (sterilized.Length == 10) {
			sterilized = "+1" + sterilized;
		}
		return sterilized;
	}

	public Conversation(Message sms) {
		_who = null;
		if (sms != null)
			_phoneNumber = sterilizePhoneNumber(sms.PhoneNumber);
		else
			_phoneNumber = "null";
		_messages = new List<Message>();
		_messages.Add(sms);
	}

	public void AddMessage(Message sms)
	{
		if (_messages == null) {
			_messages = new List<Message>();
		}
		_messages.Add(sms);
		RaisePropertyChanged("Text");
	}

	public void DeleteMessage(int location)
	{
		_messages.RemoveAt(location);
	}

	public override string ToString()
	{
		return _phoneNumber;
	}

	public string Text {
		get {
			string result = "";
			for (int i = 0; i < _messages.Count; i++) {
				result += _messages[i].ToString();
			}
			return result;
		}
	}

	public void Sort() {
		_messages.Sort((a, b) => a.Time.CompareTo(b.Time));
	}
}

