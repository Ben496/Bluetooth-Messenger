using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace WindowsMessenger {
	class Notification {
		public static void createToastNotification(Message msg) {
			// Create toast xml template
			XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);

			// Assign text to to xml
			XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
			stringElements[0].AppendChild(toastXml.CreateTextNode(msg.Who));
			stringElements[1].AppendChild(toastXml.CreateTextNode(msg.Text));

			// Assign application icon
			XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
			string imagePath = "file:///" + Path.GetFullPath("Icon.png");
			imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

			// Create and show the toast
			ToastNotification toast = new ToastNotification(toastXml);
			ToastNotificationManager.CreateToastNotifier("Windows Messenger").Show(toast);
		}
	}
}
