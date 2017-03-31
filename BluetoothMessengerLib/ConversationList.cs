using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConversationList
{
	private List<Conversation> _conversations;
	public virtual List<Conversation> GenerateConversations()
	{
		throw new System.NotImplementedException();
	}

	public virtual Conversation AccessConversation(int location)
	{
		throw new System.NotImplementedException();
	}

	public virtual void AddNewConversation(Contact contact)
	{
		throw new System.NotImplementedException();
	}

}

