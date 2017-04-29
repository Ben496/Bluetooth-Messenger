using System;
using System.Collections.Generic;
using System.ComponentModel;

public class Conversation : INotifyPropertyChanged
{
	private List<Message> _messages;
	private string _who;
	private PhoneNumber _phoneNumber;
	private string _text;

	public event PropertyChangedEventHandler PropertyChanged;

	private void RaisePropertyChanged(string property) {
		if (PropertyChanged != null) {
			PropertyChanged(this, new PropertyChangedEventArgs(property));
		}
	}

	public string Who {
		get {
			if (_who.CompareTo("") == 0)
				return _phoneNumber.Number;
			else
				return _who;
		}
		set { _who = value; }
	}

	public List<Message> Messages {
		get { return _messages; }
		set { _messages = value; }
	}

	public string PhoneNumber {
		get { return _phoneNumber.Number; }
		set { _phoneNumber = new PhoneNumber(value); }
	}

	public Conversation(Message sms) {
		
		if (sms != null) {
			_phoneNumber = new PhoneNumber(sms.PhoneNumber);
			_who = sms.Who;
		}
		else {
			_phoneNumber = new global::PhoneNumber("");
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
			return _phoneNumber.Number;
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

