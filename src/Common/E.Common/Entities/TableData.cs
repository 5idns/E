using System;
using System.Runtime.Serialization;

namespace E.Common.Entities
{
    [Serializable]
    [DataContract]
    public class TableData<TEntity>
        where TEntity : class
    {
        public TableData()
        {

        }

        public TableData(TEntity[] data, int start = 0, int length = 20, long count = 0)
        {
            Data = data;
            Start = start;
            Length = length;
            RecordsTotal = count;
            RecordsFiltered = count;
        }

        /// <summary>
        /// 数据集
        /// </summary>
        [DataMember]
        public TEntity[] Data { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        [DataMember]
        public int Start { get; set; }

        /// <summary>
        /// 页尺码
        /// </summary>
        [DataMember]
        public int Length { get; set; }

        /// <summary>
        /// 总记录条数
        /// </summary>
        [DataMember]
        public long RecordsTotal { get; set; }

        /// <summary>
        /// 筛选后记录条数
        /// </summary>
        [DataMember]
        public long RecordsFiltered { get; set; }
    }

    public class TableData<TEntity, TSummary> : TableData<TEntity>
        where TEntity : class 
        where TSummary : class
    {

        public TableData()
        {

        }

        public TableData(TEntity[] data, int start = 0, int length = 20, long count = 0)
        {
            Data = data;
            Start = start;
            Length = length;
            RecordsTotal = count;
            RecordsFiltered = count;
        }

        public TableData(TEntity[] data, TSummary summary, int start = 0, int length = 20, long count = 0) : base(data,
            start, length, count)
        {
            Summary = summary;
        }

        /// <summary>
        /// 数据描述信息，可用于记录统计信息
        /// </summary>
        [DataMember]
        public TSummary Summary { get; set; }
    }
}
