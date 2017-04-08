using System;
using System.Collections.Generic;



public class Conversation
{
	private List<Message> _messages;
	private Contact _who;
	private String _phoneNumber;

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

	public void GetMessageString(int location)
	{
		throw new System.NotImplementedException();
	}

}

