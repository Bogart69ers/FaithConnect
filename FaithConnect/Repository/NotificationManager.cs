using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FaithConnect.Utils;


namespace FaithConnect.Repository
{
    public class NotificationManager
    {
        private BaseRepository<Notification> _notif;

        public NotificationManager()
        {
            _notif = new BaseRepository<Notification>();
        }

        // 1. Get a single notification by its primary key
        public Notification GetNotificationById(int id)
        {
            return _notif.Get(id);
        }

        // 2. Get all notifications (or all notifications for a user)
        public List<Notification> GetAllNotifications()
        {
            return _notif.GetAll();
        }

        // If you want to filter by userId:
        public List<Notification> GetNotificationsByUserId(int userId)
        {
            // uses _table on the repository if it's exposed as _notif._table
            // or something similar
            return _notif._table
                         .Where(n => n.userId == userId)
                         .OrderByDescending(n => n.date)
                         .ToList();
        }

        // 3. Create a new notification
        public ErrorCode CreateNotification(Notification entity, out string errMsg)
        {
            // set date to now, etc.
            entity.date = DateTime.Now;
            return _notif.Create(entity, out errMsg);
        }

        // 4. Update a notification
        public ErrorCode UpdateNotification(Notification entity, out string errMsg)
        {
            // You pass in the entity's ID, etc.
            return _notif.Update(entity.id, entity, out errMsg);
        }

        // 5. Delete a notification by ID
        public ErrorCode DeleteNotification(int id, out string errMsg)
        {
            return _notif.Delete(id, out errMsg);
        }
    }
}