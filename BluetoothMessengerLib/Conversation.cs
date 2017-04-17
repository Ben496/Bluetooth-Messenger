using System;
using System.Collections.Generic;



public class Conversation
{
	private List<Message> _messages;
	private Contact _who;
	private string _phoneNumber;
	private string _text;

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
		set { _phoneNumber = value; }
	}

	public Conversation(Message sms) {
		_who = null;
		_phoneNumber = sms.PhoneNumber;
		_messages = new List<Message>();
		_messages.Add(sms);
	}

	public void AddMessage(Message sms)
	{
		if (_messages == null) {
			_messages = new List<Message>();
		}
		_messages.Add(sms);
	}

	public void DeleteMessage(int location)
	{
		_messages.RemoveAt(location);
	}

	public override string ToString()
	{
		string result = "";
		for (int i = 0; i < _messages.Count; i++) {
			result += _messages[i].ToString();
		}
		return result;
	}

	public string Text {
		get {
			return ToString();
		}
		set {
			_text = value;
		}
	}

	public void Sort() {
		_messages.Sort((a, b) => a.Time.CompareTo(b.Time));
	}
}

