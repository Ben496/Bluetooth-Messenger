using System.Collections.ObjectModel;

namespace WindowsMessenger.ViewModel {
	class ConversationViewModel {
		public ObservableCollection<Conversation> Conversations {
			get;
			set;
		}

		public ConversationViewModel() {
			
		}

		public ConversationViewModel(ConversationList convos) {
			ObservableCollection<Conversation> tmp = new ObservableCollection<Conversation>();
			for (int i = 0; i < convos.Size(); i++) {
				tmp.Add(convos.get(i));
			}
			Conversations = tmp;
		}

		private object _selected;
		public object Selected {
			get {
				return _selected;
			}
			set {
				_selected = value;
			}
		}
	}
}
