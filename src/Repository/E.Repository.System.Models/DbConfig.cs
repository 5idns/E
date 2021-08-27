using System;
using FreeSql;
using FreeSql.DataAnnotations;

namespace E.Repository.System.Models
{
    /// <summary>
    /// 数据库设置
    /// </summary>
    [Table(Name = "DbConfig")]
    [Index("uk_Name", "Name", true)]
    public class DbConfig
    {
        /// <summary>
        /// 数据库ID
        /// </summary>
        [Column(IsPrimary = true, IsIdentity = true)]
        public long Id { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        [Column(StringLength = 50, IsNullable = false)]
        public string Name { get; set; }

        /// <summary>
        /// 数据库上下文类型
        /// </summary>
        [Column(IsNullable = false)]
        public Type DbContextType { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        [Column(IsNullable = false)]
        public DataType DataType { get; set; }
        
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [Column(IsNullable = false)]
        public string ConnectionString { get; set; }

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
