using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BluetoothMessengerLibTest {
	/// <summary>
	/// Summary description for ContactTest
	/// </summary>
	[TestClass]
	public class ContactTest {
		public ContactTest() {
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
		public void ContactConstructorTest0() {
			Contact contact = new Contact();
			Assert.AreEqual(contact.Name, "");
			Assert.AreEqual(contact.Numbers.Count, 0);
		}

		[TestMethod]
		public void ContactConstructorTest1() {
			List<PhoneNumber> numbers = new List<PhoneNumber>();
			numbers.Add(new PhoneNumber("1234567890"));
			Contact contact = new Contact("John", numbers);
			Assert.AreEqual(contact.Name, "John");
			Assert.AreEqual(contact.Numbers[0].Number, (new PhoneNumber("1234567890")).Number);
		}

		[TestMethod]
		public void ContactConstructorTest2() {
			List<PhoneNumber> numbers = null;
			Contact contact = new Contact("John", numbers);
			Assert.AreEqual(contact.Name, "John");
			Assert.IsNull(contact.Numbers);
		}

		[TestMethod]
		public void ContactConstructorTest3() {
			List<PhoneNumber> numbers = new List<PhoneNumber>();
			numbers.Add(new PhoneNumber("1234567890"));
			Contact contact = new Contact(null, numbers);
			Assert.IsNull(contact.Name);
			Assert.AreEqual(contact.Numbers[0].Number, (new PhoneNumber("1234567890")).Number);
		}

		[TestMethod]
		public void ContactConstructorTest4() {
			List<PhoneNumber> numbers = null;
			Contact contact = new Contact(null, numbers);
			Assert.IsNull(contact.Name);
			Assert.IsNull(contact.Numbers);
		}

		[TestMethod]
		public void ContactNameTest0() {
			Contact contact = new Contact();
			contact.Name = "Robert";
			Assert.AreEqual(contact.Name, "Robert");
		}

		[TestMethod]
		public void ContactNameTest1() {
			Contact contact = new Contact();
			contact.Name = null;
			Assert.IsNull(contact.Name);
		}

		[TestMethod]
		public void ContactNumbersTest0() {
			Contact contact = new Contact();
			List<PhoneNumber> phone = new List<PhoneNumber>();
			phone.Add(new PhoneNumber("1234567890"));
			contact.Numbers = phone;
			Assert.AreEqual(contact.Numbers[0].Number, (new PhoneNumber("1234567890")).Number);
		}

		[TestMethod]
		public void ContactNumbersTest1() {
			Contact contact = new Contact();
			contact.Numbers.Add(new PhoneNumber("1234567890"));
			Assert.AreEqual(contact.Numbers[0].Number, (new PhoneNumber("1234567890")).Number);
		}
	}
}
