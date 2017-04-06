using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
