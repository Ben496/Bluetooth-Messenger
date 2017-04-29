using System;
using System.Collections.Generic;
using System.ComponentModel;

public class Conversation : INotifyPropertyChanged
{
	private List<Message> _messages;
	private string _who;
	private string _phoneNumber;
	private string _text;

	public event PropertyChangedEventHandler PropertyChanged;

	private void RaisePropertyChanged(string property) {
		if (PropertyChanged != null) {
			PropertyChanged(this, new PropertyChangedEventArgs(property));
		}
	}

	public string Who {
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

	public Conversation(Message sms) {
		
		if (sms != null) {
			_phoneNumber = sterilizePhoneNumber(sms.PhoneNumber);
			_who = sms.Who;
		}
		else {
			_phoneNumber = "null";
			_who = "";
		}
		_messages = new List<Message>();
		_messages.Add(sms);
	}

	public void AddMessage(Message sms)
	{
		if (_messages == null) {
			_messages = new List<Message>();
		}
		if (sms.Who.CompareTo("") == 0)
			sms.Who = _who;
		_messages.Add(sms);
		RaisePropertyChanged("Text");
	}

	public void DeleteMessage(int location)
	{
		_messages.RemoveAt(location);
	}

	public override string ToString()
	{
		if (_who.CompareTo("") == 0)
			return _phoneNumber;
		else
			return _who;
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

