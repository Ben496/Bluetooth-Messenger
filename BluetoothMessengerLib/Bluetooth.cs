using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace BluetoothMessengerLib {
	public class Bluetooth {
		// This string is used to generate the UUID for the bluetooth connection
		// Obtained from https://www.uuidgenerator.net/
		private static readonly string _uuidString = "ede10139-f5e2-433e-a48c-ffde355480ea";

		public static string UuidString {
			get { return _uuidString; }
		}
		
		// The first 4 bytes sent indicate the size of the object.
		protected bool Send<T>(Stream connectionStream, T data) {
			string output = JsonConvert.SerializeObject(data);
			int length = output.Length;
			var buffer = System.Text.Encoding.UTF8.GetBytes(output);
			BinaryWriter writeLength = new BinaryWriter(connectionStream);
			writeLength.Write(length);
			connectionStream.Write(buffer, 0, buffer.Length);
			connectionStream.Flush();
			return true;
		}
		
		// The first 4 bytes received indicate the size of the object.
		protected T Get<T>(Stream connectionStream) {
			if (connectionStream != null) {
				try {
					BinaryReader readLength = new BinaryReader(connectionStream, System.Text.Encoding.UTF8);
					int length = readLength.ReadInt32();
					byte[] buffer = new byte[length];
					connectionStream.Read(buffer, 0, length);
					string input = Encoding.UTF8.GetString(buffer, 0, length);
					T dat = JsonConvert.DeserializeObject<T>(input);
					return dat;
				}
				catch {
					return default(T);
				}
			}
			return default(T);
		}
	}
}
