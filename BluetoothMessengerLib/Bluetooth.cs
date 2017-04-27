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
			var buffer = System.Text.Encoding.UTF8.GetBytes(output);
			int length = buffer.Length;
			BinaryWriter writeLength = new BinaryWriter(connectionStream);
			writeLength.Write(length);
			connectionStream.Write(buffer, 0, buffer.Length);
			connectionStream.Flush();
			return true;
		}
		
		// The first 4 bytes received indicate the size of the object.
		protected T Get<T>(Stream connectionStream) {
			if (connectionStream != null) {
				int length;
				string input;
				int i;
				try {
					BinaryReader readLength = new BinaryReader(connectionStream, System.Text.Encoding.UTF8);
					length = readLength.ReadInt32();
					byte[] buffer = new byte[length];
					for (i = 0; i < length; i++) {
						buffer[i] = (byte)connectionStream.ReadByte();
						//if (length - i > 1000)
						//	connectionStream.Read(buffer, i, i + 1000);
						//else
						//	connectionStream.Read(buffer, i, length);
					}
					input = Encoding.UTF8.GetString(buffer, 0, length);
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
