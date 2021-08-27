using System;
using System.Runtime.Serialization;
using E.Common.Enumeration;

namespace E.Common.Entities
{
    /// <summary>
    /// 通用结果
    /// </summary>
    [Serializable]
    [DataContract]
    public class CommonResult
    {
        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public Status Status { get; set; }

        /// <summary>
        /// 状态描述信息
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        public CommonResult()
        {
            Status = Status.Unknown;
        }

        public CommonResult(Status status)
        {
            Status = status;
        }

        public CommonResult(Status status, string message)
        {
            Status = status;
            Message = message;
        }
    }

    /// <summary>
    /// 通用结果
    /// </summary>
    public class CommonResult<TData> : CommonResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        [DataMember]
        public TData Data { get; set; }

        public CommonResult()
        {
            Status = Status.Unknown;
        }

        public CommonResult(Status status) : base(status)
        {
        }

        public CommonResult(Status status, string message) : base(status, message)
        {
        }

        public CommonResult(Status status, string message,TData data) : base(status, message)
        {
            Data = data;
        }
    }
}
