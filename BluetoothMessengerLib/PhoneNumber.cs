public class PhoneNumber
{
	private string _number = "";
	//private string _type;

	public PhoneNumber(string num) {
		_number = sterilizePhoneNumber(num);
	}
	public string Number {
		get {
			return _number;
		}
		set {
			_number = sterilizePhoneNumber(value);
		}
	}

	private string sterilizePhoneNumber(string num) {
		string sterilized = "";
		for (int i = 0; i < num.Length; i++) {
			if (num[i] >= 48 && num[i] <= 57) {
				sterilized += num[i];
			}
		}

		if (sterilized.Length == 11)
			sterilized = '+' + sterilized;
		else if (sterilized.Length == 10)
			sterilized = "+1" + sterilized;
		//else if (sterilized.Length == 5)
		//	return sterilized;
		//else return "INVALID";
		return sterilized;
	}
}