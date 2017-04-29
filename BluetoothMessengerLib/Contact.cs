using System.IO;
using System.Collections.Generic;

public class Contact
{
	private string _name;
	private List<string> _numbers;

	public string Name {
		get { return _name; }
		set { _name = value; }
	}

	public List<string> Numbers {
		get { return _numbers; }
		set { _numbers = value; }
	}

	public Contact() {
		_name = "";
		_numbers = new List<string>();
	}

	public Contact(string name, List<string> numbers) {
		_name = name;
		_numbers = numbers;
	}

}

