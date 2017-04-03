using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class Message
{
	private string _text;
	private Stream _multi;		// not implemented yet
	private string _phoneNumber;
	private bool _isMMS;

	public string Text {
		get { return _text; }
		set { _text = value; }
	}

	// TODO add check to make sure phone number is valid
	public string PhoneNumber {
		get { return _phoneNumber; }
		set { _phoneNumber = value; }
	}

	public bool IsMms {
		get { return _isMMS; }
		set { _isMMS = value; }
	}

	public Message() {
		_text = null;
		_multi = null;
		_phoneNumber = null;
		_isMMS = false;
	}

	public Message(string text) {
		_text = text;
		_multi = null;
		_phoneNumber = null;
		_isMMS = false;
	}

	// TODO add check to make sure phone number is valid
	public Message(string text, string phoneNumber) {
		_text = text;
		_multi = null;
		_phoneNumber = phoneNumber;
		_isMMS = false;
	}

	// TODO add check to make sure phone number is valid
	public Message(string text, string phoneNumber, bool mms) {
		_text = text;
		_multi = null;
		_phoneNumber = phoneNumber;
		_isMMS = mms;
	}

}

