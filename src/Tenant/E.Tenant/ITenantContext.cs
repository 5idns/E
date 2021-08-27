using System;
using System.Collections.Generic;

namespace E.Tenant
{
    public interface ITenantContext
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        string TenantId { get; set; }

        /// <summary>
        /// 租户名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 租户标识
        /// </summary>
        Guid Identity { get; set; }

        /// <summary>
        /// 主机标识
        /// </summary>
        string[] HostIdentity { get; set; }

        /// <summary>
        /// 租户资源
        /// </summary>
        IDictionary<object, object> Resources { get; }

        /// <summary>
        /// 检查标识是否匹配
        /// </summary>
        /// <param name="identity">标识</param>
        /// <returns></returns>
        bool IdentityContains(string identity);

        /// <summary>
        /// 设置资源
        /// </summary>
        /// <param name="key">资源键</param>
        /// <param name="value">资源</param>
        void SetResource(object key, object value);

        /// <summary>
        /// 设置资源
        /// </summary>
        /// <typeparam name="TKey">资源键类型</typeparam>
        /// <typeparam name="TValue">资源键值类型</typeparam>
        /// <param name="key">资源键</param>
        /// <param name="value">资源</param>
        void SetResource<TKey,TValue>(TKey key, TValue value);

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="key">资源键</param>
        /// <returns>资源</returns>
        object GetResource(object key);

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <typeparam name="TValue">资源类型</typeparam>
        /// <param name="key">资源键</param>
        /// <returns>资源</returns>
        TValue GetResource<TValue>(object key);

        /// <summary>
        /// 资源索引器
        /// </summary>
        /// <param name="key">资源键</param>
        /// <returns>资源</returns>
        object this[object key] { get; set; }
    }
}
