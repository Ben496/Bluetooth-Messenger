using System;
using System.Collections.Generic;

public class ConversationList
{
	private List<Conversation> _conversations = new List<Conversation>();

	public Conversation AccessConversation(String number)
	{
		if (_conversations == null) return null;
		for (int i = 0; i < _conversations.Count; i++) {
			if (_conversations[i].PhoneNumber == number) {
				return _conversations[i];
			}
		}
		return null;
	}

	public Conversation get(int i) {
		if (_conversations != null)
			return _conversations[i];
		else return null;
	}

		

	public int Size() {
		return _conversations.Count;
	}

	public void AddNewConversation(Message sms)
	{
		Conversation newConvo = new Conversation(sms);
		_conversations.Add(newConvo);
	}

	public int hasConversation(String phoneNumber) {
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

