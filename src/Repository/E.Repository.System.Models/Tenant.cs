using System;
using FreeSql.DataAnnotations;

namespace E.Repository.System.Models
{
    /// <summary>
    /// 租户信息
    /// </summary>
    [Table(Name = "Tenant")]
    [Index("uk_Name", "Name", true)]
    public class Tenant
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// 租户名称
        /// </summary>
        [Column(StringLength = 50, IsNullable = false)]
        public string Name { get; set; }

        /// <summary>
        /// 租户标识
        /// </summary>
        [Column(IsNullable = false)]
        public Guid Identity { get; set; }

        /// <summary>
        /// 主机标识
        /// </summary>
        [Column(StringLength = 512)]
        public string HostIdentity { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Column(IsNullable = false)]
        public bool IsEnable { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(ServerTime = DateTimeKind.Utc, CanUpdate = false)]
        public DateTime CreateTime { get; set; }

        [Column(IsVersion = true)]
        public int Version { get; set; }
    }
}
