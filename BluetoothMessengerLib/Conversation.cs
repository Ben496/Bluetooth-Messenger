using System;
using System.Collections.Generic;



public class Conversation
{
	private List<Message> _messages;
	private Contact _who;
	private String _phoneNumber;
	private String _text;

	public Contact Who {
		get { return _who; }
		set { _who = value; }
	}

	public List<Message> Messages {
		get { return _messages; }
		set { _messages = value; }
	}

	public String PhoneNumber {
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

	public override String ToString()
	{
		String result = "";
		for (int i = 0; i < _messages.Count; i++) {
			result += _messages[i].ToString();
		}
		return result;
	}

	public String Text {
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

