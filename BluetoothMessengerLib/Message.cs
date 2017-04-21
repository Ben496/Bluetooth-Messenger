using System.IO;

public class Message
{
	private string _text;
	private int _timeStamp;
	private Stream _multi;		// not implemented yet
	private string _phoneNumber;
	private bool _isMMS;
	private bool _isSent;
	public string Text {
		get { return _text; }
		set { _text = value; }
	}

	public bool isSent {
		get { return _isSent; }
		set { _isSent = value; }
	}

	// TODO add check to make sure phone number is valid
	public string PhoneNumber {
		get { return _phoneNumber; }
		set { _phoneNumber = sterilizePhoneNumber(value); }
	}

	public bool IsMms {
		get { return _isMMS; }
		set { _isMMS = value; }
	}

	public int Time {
		get { return _timeStamp; }
		set { _timeStamp = value; }
	}

	public Message() {
		_timeStamp = 0;
		_text = null;
		_multi = null;
		_phoneNumber = null;
		_isMMS = false;
		_isSent = false;
	}

	public Message(string text) {
		_timeStamp = 0;
		_text = text;
		_multi = null;
		_phoneNumber = null;
		_isMMS = false;
		_isSent = false;
	}

	public Message(string text, string phoneNumber) {
		_timeStamp = 0;
		_text = text;
		_multi = null;
		_phoneNumber = sterilizePhoneNumber(phoneNumber);
		_isMMS = false;
		_isSent = false;
	}

	public Message(string text, string phoneNumber, bool sent) {
		_timeStamp = 0;
		_text = text;
		_multi = null;
		_phoneNumber = sterilizePhoneNumber(phoneNumber);
		_isMMS = false;
		_isSent = sent;
	}

	public Message(string text, string phoneNumber, bool sent, int time) {
		_timeStamp = time;
		_text = text;
		_multi = null;
		_phoneNumber = sterilizePhoneNumber(phoneNumber);
		_isMMS = false;
		_isSent = sent;
	}

	public override string ToString() {
		if (_isSent) {
			return "Me: " + _text + "\n";
		}
		return _phoneNumber + ": " + _text + "\n";
	}

	private string sterilizePhoneNumber(string num) {
		string sterilized = num;
		if (sterilized.Length != 12 && sterilized.Length != 10) {
			return "INVALID";
		}
		else if (sterilized.Length == 10) {
			sterilized = "+1" + sterilized;
		}
		return sterilized;
	}
}

