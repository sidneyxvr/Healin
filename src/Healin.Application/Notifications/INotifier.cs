using System.Collections.Generic;

namespace Healin.Application.Notifications
{
    public interface INotifier
    {
        void Handle(Notification notification);
        bool HasNotification();
        IReadOnlyCollection<Notification> GetAll();
    }
}
