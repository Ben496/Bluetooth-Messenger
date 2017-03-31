using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ContactList
{
	private List<Contact> _contacts;
	public virtual List<Contact> GenerateContacts()
	{
		throw new System.NotImplementedException();
	}

	public virtual Contact GetContact(int location)
	{
		throw new System.NotImplementedException();
	}

}

