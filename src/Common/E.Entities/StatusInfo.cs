using System.Collections.Concurrent;
using System.Globalization;
using E.Common.Enumeration;

namespace E.Entities
{
    /// <summary>
    /// 状态信息
    /// </summary>
    public class StatusInfo
    {
        public StatusInfo(long status)
        {
            StatusCode = status;
            Status = (Status)status;
        }
        /// <summary>
        /// 状态码
        /// </summary>
        public long StatusCode { get; set; }

        public Status Status { get; set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        public ConcurrentDictionary<CultureInfo, string> Description { get; set; }
    }
}
