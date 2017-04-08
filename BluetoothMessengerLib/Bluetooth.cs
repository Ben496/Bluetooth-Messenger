using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BluetoothMessengerLib {
	public class Bluetooth {
		// This string is used to generate the UUID for the bluetooth connection
		// Obtained from https://www.uuidgenerator.net/
		private static readonly string _uuidString = "ede10139-f5e2-433e-a48c-ffde355480ea";

		public static string UuidString {
			get { return _uuidString; }
		}

		// TODO: rework this so it doesn't connect / disconnect each time.
		// Writes a single object to a given stream.
		protected bool Send(Stream connectionStream, object data) {
			string output = JsonConvert.SerializeObject(data);
			var buffer = System.Text.Encoding.UTF8.GetBytes(output);
			//BinaryWriter writeLength = new BinaryWriter(connectionStream);
			//writeLength.Write(buffer.Length);
			connectionStream.Write(buffer, 0, buffer.Length);
			connectionStream.Flush();
			return true;
		}

		// This method is so terrible that i know it can be done better.
		// Extracts a single object from a given stream
		protected object Get(Stream connectionStream) {
			LinkedList<byte> bytes = new LinkedList<byte>();
			while (true) {
				if (connectionStream != null) {
					try {
						int tmp = 0;
						while (true) {
							try {
								tmp = connectionStream.ReadByte();
							}
							catch {
								break;
							}
							if (tmp != -1)
								bytes.AddLast((byte)tmp);
							else
								break;
						}
						byte[] tmpArray = new byte[bytes.Count];
						int i = 0;
						foreach (byte b in bytes) {
							tmpArray[i] = b;
							i++;
						}
						// For some reaosn I cannot use the version that only accepts (byte[])
						// instead I have to specify the start and end locations.
						string input = Encoding.UTF8.GetString(tmpArray, 0, bytes.Count);
						object item = JsonConvert.DeserializeObject<object>(input);
						return item;
					}
					catch {
						return null;
					}
				}
			}
		}
	}
}
