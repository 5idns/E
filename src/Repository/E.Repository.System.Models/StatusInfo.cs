using FreeSql.DataAnnotations;

namespace E.Repository.System.Models
{
    /// <summary>
    /// 状态信息
    /// </summary>
    [Table(Name = "StatusInfo")]
    [Index("ik_Status", "Status", false)]
    public class StatusInfo
    {
        /// <summary>
        /// 信息ID
        /// </summary>
        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        [Column(IsNullable = false)]
        public long Status { get; set; }

        /// <summary>
        /// 语言区域
        /// </summary>
        [Column(StringLength = 10, IsNullable = false)]
        public string Culture { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column(StringLength = 1024, IsNullable = false)]
        public string Description { get; set; }
        [Column(IsVersion = true)]
        public int Version { get; set; }
    }
}
