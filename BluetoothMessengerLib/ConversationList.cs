using System;
using System.Collections.Generic;

public class ConversationList
{
	private List<Conversation> _conversations = new List<Conversation>();

	public List<Conversation> Conversations {
		get { return _conversations; }
	}

	public ConversationList() {

	}

	public ConversationList(List<Conversation> cons) {
		if (cons != null)
			_conversations = cons;
	}

	public Conversation AccessConversation(string number)
	{
		if (number == null)
			return null;
		string snumber = PhoneNumber.sterilizePhoneNumber(number);
		if (_conversations == null) return null;
		for (int i = 0; i < _conversations.Count; i++) {
			if (_conversations[i].PhoneNumber == snumber) {
				return _conversations[i];
			}
		}
		return null;
	}

	public Conversation AccessConversation(int location) {
		if (_conversations == null) return null;
		try {
			return _conversations[location];
		}
		catch (Exception e) {
			throw e;
		}
	}

	public void SortByTime() {
		foreach (Conversation con in _conversations) {
			con.Messages.Sort();
		}
	}	

	public int Size() {
		return _conversations.Count;
	}

	public void AddNewConversation(Message sms)
	{
		Conversation newConvo = new Conversation(sms);
		_conversations.Add(newConvo);
	}

	public int hasConversation(string phoneNumber) {
		if (_conversations == null) return -1;
		for (int i = 0; i < _conversations.Count; i++) {
			if (_conversations[i].PhoneNumber == phoneNumber) return i;
		}
		return -1;
	}

	public void addMessage(Message sms) {
		int loc = hasConversation(sms.PhoneNumber);
		if (loc != -1) {
			_conversations[loc].AddMessage(sms);
		}
		else AddNewConversation(sms);
	}

}

