using System.Collections.Generic;
using System.Linq;

namespace Healin.Application.Notifications
{
    public class Notifier : INotifier
    {
        private readonly List<Notification> _notifications;

        public Notifier()
        {
            _notifications = new List<Notification>();
        }

        public void Handle(Notification notification)
        {
            _notifications.Add(notification);
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }

        public IReadOnlyCollection<Notification> GetAll()
        {
            return _notifications.AsReadOnly();
        }
    }
}
