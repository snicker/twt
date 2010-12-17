using System;
using Growl.Connector;
using Growl.CoreLibrary;
using System.Drawing;
using System.Windows.Forms;

namespace s7.twt.Notifications
{
    public static class Growler
    {
        private static Growl.Connector.Application m_App = new Growl.Connector.Application("twt");
        private static GrowlConnector m_Connector = new GrowlConnector();

        public static NotificationType SuccessNotification = new NotificationType("ADDED", "Tweeted!");
        public static NotificationType ErrorNotification = new NotificationType("ERROR", "Error");
        public static NotificationType GeneralNotification = new NotificationType("GENERAL", "General");

        public static void Initialize() {
			m_App.Icon = Icon.ExtractAssociatedIcon( System.Windows.Forms.Application.ExecutablePath ).ToBitmap();
            m_Connector.Register(m_App, new NotificationType[] { SuccessNotification, ErrorNotification, GeneralNotification });
        }

        public static bool Growl(NotificationType nt, string message)
        {
            return Growl(nt, nt.DisplayName, message);
        }

        public static bool Growl(NotificationType nt, string title, string message)
        {
            if (m_Connector == null || ( m_Connector != null && !m_Connector.IsGrowlRunning() ) )
                return false;
            Notification notification = new Notification(m_App.Name, nt.Name, Guid.NewGuid().ToString(), title, message);
            m_Connector.Notify(notification);
            return true;
        }
    }
}
