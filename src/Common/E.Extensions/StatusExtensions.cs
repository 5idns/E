using System;
using System.Collections.Concurrent;
using System.Globalization;
using E.Common.Extensions;
using E.Entities;

namespace E.Extensions
{
    public static class StatusExtensions
    {
        private static ConcurrentDictionary<long, StatusInfo> _status = new ConcurrentDictionary<long, StatusInfo>();

        private static Func<long, ConcurrentDictionary<CultureInfo, string>> _lazyLoading;
        public static Func<long, ConcurrentDictionary<CultureInfo, string>> LazyLoading
        {
            get; set;
        }

        public static StatusInfo GetStatusInfo(this int status)
        {
            if (_status == null)
            {
                _status = new ConcurrentDictionary<long, StatusInfo>();
            }
            return _status.GetOrAdd(status, new StatusInfo(status));
        }

        public static StatusInfo GetStatusInfo(this long status)
        {
            if (_status == null)
            {
                _status = new ConcurrentDictionary<long, StatusInfo>();
            }
            return _status.GetOrAdd(status, new StatusInfo(status));
        }

        public static string GetDescription(this StatusInfo statusInfo, CultureInfo cultureInfo = default)
        {
            if (cultureInfo == null || cultureInfo.Equals(default))
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }
            if (statusInfo.Description == null)
            {
                statusInfo.Description = new ConcurrentDictionary<CultureInfo, string>();
            }
            if (_lazyLoading != null)
            {
                var descriptions = _lazyLoading(statusInfo.StatusCode);
                if (descriptions != null && descriptions.Count > 0)
                {
                    statusInfo.Description = descriptions;
                }
            }
            var description = statusInfo.Description.GetOrAdd(cultureInfo, key =>
            {
                try
                {
                    var statusDescription = statusInfo.Status.ToDescription();
                    return statusDescription;
                }
                catch
                {
                    // ignored
                }

                return string.Empty;
            });
            return description;
        }

        public static string SetDescription(this StatusInfo statusInfo, CultureInfo cultureInfo, string description)
        {
            if (cultureInfo == null || cultureInfo.Equals(default))
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }
            if (statusInfo.Description == null)
            {
                statusInfo.Description = new ConcurrentDictionary<CultureInfo, string>();
            }
            statusInfo.Description.AddOrUpdate(cultureInfo, description, (key, value) => description);
            return description;
        }
    }
}
