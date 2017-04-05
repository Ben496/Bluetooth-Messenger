using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConversationList
{
	private List<Conversation> _conversations;

	public virtual Conversation AccessConversation(int location)
	{
		throw new System.NotImplementedException();
	}

	public void AddNewConversation(Message sms)
	{
		Conversation newConvo = new Conversation(sms);
		_conversations.Add(newConvo);
	}

	public int hasConversation(String phoneNumber) {
		if (_conversations.Count == 0) return -1;
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

