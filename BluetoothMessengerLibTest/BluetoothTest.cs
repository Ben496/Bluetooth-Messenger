using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BluetoothMessengerLib;
using System.IO;

namespace BluetoothMessengerLibTest {
	/// <summary>
	/// Summary description for BluetoothTest
	/// </summary>
	[TestClass]
	public class BluetoothTest : Bluetooth {
		public BluetoothTest() {
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion
		
		[TestMethod]
		public void BluetoothDefaultValue0() {
			Assert.AreEqual(Bluetooth.UuidString, "ede10139-f5e2-433e-a48c-ffde355480ea");
		}

		[TestMethod]
		public void BluetoothSend0() {
			Stream s = new MemoryStream();
			string str = "Hello World";

			bool res = Send<string>(s, str);

			byte[] buffer = new byte[s.Length];
			s.Position = 0;
			s.Read(buffer, 0, (int)s.Length);
			byte[] acontents = { 13, 0, 0, 0, 34, 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 34 };
			CollectionAssert.AreEqual(buffer, acontents);
			Assert.IsTrue(res);
		}

		[TestMethod]
		public void BluetoothSend1() {
			Stream s = new MemoryStream();
			string str = null;

			bool res = Send<string>(s, str);

			Assert.IsFalse(res);
		}

		[TestMethod]
		public void BluetoothSend2() {
			Stream s = null;
			string str = "Hello World";

			bool res = Send<string>(s, str);

			Assert.IsFalse(res);
		}

		[TestMethod]
		public void BluetoothSend3() {
			Stream s = null;
			string str = null;

			bool res = Send<string>(s, str);

			Assert.IsFalse(res);
		}

		[TestMethod]
		public void BluetoothGet0() {
			Stream s = new MemoryStream();
			byte[] acontents = { 13, 0, 0, 0, 34, 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 34 };
			s.Write(acontents, 0, acontents.Length);
			s.Position = 0;

			string res = Get<string>(s);

			Assert.AreEqual(res, "Hello World");
		}

		[TestMethod]
		public void BluetoothGet1() {
			Stream s = null;

			string str = default(string);
			string res = Get<string>(s);

			Assert.AreEqual(res, str);
		}
	}
}
