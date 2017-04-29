using System.IO;
using System.Collections.Generic;

public class Contact
{
	private string _name;
	private List<PhoneNumber> _numbers;

	public string Name {
		get { return _name; }
		set { _name = value; }
	}

	public List<PhoneNumber> Numbers {
		get { return _numbers; }
		set { _numbers = value; }
	}

	public Contact() {
		_name = "";
		_numbers = new List<PhoneNumber>();
	}

	public Contact(string name, List<PhoneNumber> numbers) {
		_name = name;
		_numbers = numbers;
	}

}

