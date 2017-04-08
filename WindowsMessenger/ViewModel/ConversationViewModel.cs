using System.Collections.ObjectModel;

namespace WindowsMessenger.ViewModel {
	class ConversationViewModel {
		public ObservableCollection<Conversation> Conversations {
			get;
			set;
		}

		public ConversationViewModel() {
			ObservableCollection<Conversation> tmp = new ObservableCollection<Conversation>();
			Conversation convo = new Conversation(new Message("HEY FRIEND", "6156300003"));
			convo.AddMessage(new Message("WADDUP", "6156300003"));
			tmp.Add(convo);

			Conversations = tmp;
			


		}	

	}
}
