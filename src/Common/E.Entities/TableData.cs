namespace E.Entities
{
    public class TableData<T> where T : class
    {
        public TableData()
        {

        }
        public TableData(T[] data, int start = 0, int length = 20, long count = 0)
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
        public T[] Data { get; set; }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int Start { get; set; }
        /// <summary>
        /// 页尺码
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 总记录条数
        /// </summary>
        public long RecordsTotal { get; set; }

        public long RecordsFiltered { get; set; }
    }
}
