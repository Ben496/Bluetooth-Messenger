using System.Collections.ObjectModel;

namespace WindowsMessenger.ViewModel {
	class ConversationViewModel {
		public ObservableCollection<Conversation> Conversations {
			get;
			set;
		}

		public ConversationViewModel() {
			ObservableCollection<Conversation> tmp = new ObservableCollection<Conversation>();


		}	

	}
}
