using System;
using E.Common.Enumeration;

namespace E.Exceptions
{
    public class SysException : Exception
    {
        public Status Status { get; set; }

        public ulong StatusCode => (ulong)Status;

        public SysException(ulong status = 0) : this(status, default)
        {
        }
        public SysException(ulong status = 0, string message = default) : this(status, message, null)
        {
        }

        public SysException(ulong status = 0, string message = default, Exception innerException = null) : base(message, innerException)
        {
            Status = (Status)status;
        }

        public SysException(Status status = Status.Unknown) : this(status, default)
        {
        }
        public SysException(Status status = Status.Unknown, string message = default) : this(status, message, null)
        {
        }

        public SysException(Status status = Status.Unknown, string message = default, Exception innerException = null) : base(message, innerException)
        {
            Status = status;
        }
    }
}
