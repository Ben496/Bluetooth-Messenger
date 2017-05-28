using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BluetoothMessengerLibTest {
	/// <summary>
	/// Summary description for ConversationList
	/// </summary>
	[TestClass]
	public class ConversationListTest {
		private readonly List<Conversation> _conversationList;

		public ConversationListTest() {
			_conversationList = new List<Conversation>();
			_conversationList.Add(new Conversation(new Message("Hello World 0", "1234567890")));
			_conversationList.Add(new Conversation(new Message("Hello World 1", "0987654321")));
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
		public void ConversationListConstructor0() {
			ConversationList cons = new ConversationList();
			List<Conversation> c = cons.Conversations;

			CollectionAssert.AreEqual(c, new List<Conversation>());
		}

		[TestMethod]
		public void ConversationListConstructor1() {
			List<Conversation> cl = new List<Conversation>();
			ConversationList cons = new ConversationList(cl);

			// This comparison should be fine since they should refer to the same address
			Assert.AreEqual(cons.Conversations, cl);
		}

		[TestMethod]
		public void ConversationListConstructor2() {
			ConversationList cons = new ConversationList(null);
			List<Conversation> c = cons.Conversations;

			// Both should be just a new List<Conversation>
			CollectionAssert.AreEqual(c, new List<Conversation>());
		}

		[TestMethod]
		public void AccessConversation0() {
			ConversationList cons = new ConversationList(_conversationList);
			Conversation c = cons.AccessConversation("1234567890");

			Assert.AreEqual(c.PhoneNumber, "+11234567890");
		}
	}
}
