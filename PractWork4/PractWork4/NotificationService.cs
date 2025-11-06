using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PractWork4
{
    public class NotificationService
    {
        public IEnumerable<Notification> FilterAndSort(
        IEnumerable<Notification> notifications,
        NotificationFilterOptions options)
        {
            // Фильтр - уведомления
            var filteredNotifications = notifications;

            if (options.IsRead.HasValue)
            {
                filteredNotifications = filteredNotifications
                    .Where(n => n.IsRead == options.IsRead.Value);
            }

            if (options.Types != null && options.Types.Length > 0)
            {
                filteredNotifications = filteredNotifications
                    .Where(n => options.Types.Contains(n.Type));
            }

            if (!string.IsNullOrWhiteSpace(options.SearchText))
            {
                filteredNotifications = filteredNotifications
                    .Where(n => n.Title.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase) ||
                                 (n.Content != null && n.Content.Contains(options.SearchText, StringComparison.OrdinalIgnoreCase)));
            }

            if (options.MinPriority.HasValue)
            {
                filteredNotifications = filteredNotifications
                    .Where(n => n.Priority >= options.MinPriority.Value);
            }

            // Сорт - уведомления
            switch (options.SortBy)
            {
                case SortNotificationBy.Date:
                    filteredNotifications = options.Descending
                        ? filteredNotifications.OrderByDescending(n => n.CreatedAt)
                        : filteredNotifications.OrderBy(n => n.CreatedAt);
                    break;

                case SortNotificationBy.Priority:
                    filteredNotifications = options.Descending
                        ? filteredNotifications.OrderByDescending(n => n.Priority)
                        : filteredNotifications.OrderBy(n => n.Priority);
                    break;

                case SortNotificationBy.Title:
                    filteredNotifications = options.Descending
                        ? filteredNotifications.OrderByDescending(n => n.Title)
                        : filteredNotifications.OrderBy(n => n.Title);
                    break;
            }

            return filteredNotifications;
        }
    }
}
