using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace E.Tenant
{
    /// <summary>
    /// 租户上下文
    /// </summary>
    public class TenantContext : ITenantContext
    {
        private readonly ConcurrentDictionary<object, object> _resources;

        public TenantContext()
        {
            TenantId = "Default";
            _resources = new ConcurrentDictionary<object, object>();
        }

        public TenantContext(string tenantId)
        {
            TenantId = tenantId;
            _resources = new ConcurrentDictionary<object, object>();
        }

        public TenantContext(string tenantId, string[] hostIdentity)
        {
            TenantId = tenantId;
            HostIdentity = hostIdentity;
            _resources = new ConcurrentDictionary<object, object>();
        }

        /// <summary>
        /// 租户ID
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// 租户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 租户标识
        /// </summary>
        public Guid Identity { get; set; }

        /// <summary>
        /// 主机标识
        /// </summary>
        public string[] HostIdentity { get; set; }

        /// <summary>
        /// 租户资源
        /// </summary>
        public IDictionary<object, object> Resources => _resources;

        /// <summary>
        /// 检查标识是否匹配
        /// </summary>
        /// <param name="identity">标识</param>
        /// <returns></returns>
        public bool IdentityContains(string identity)
        {
            if (Guid.TryParse(identity,out var guidIdentity))
            {
                if (guidIdentity == Identity)
                {
                    return true;
                }
            }
            return HostIdentity.Contains(identity);
        }

        /// <summary>
        /// 设置资源
        /// </summary>
        /// <param name="key">资源键</param>
        /// <param name="value">资源</param>
        public void SetResource(object key, object value)
        {
            _resources.AddOrUpdate(key, value, (k, v) => value);
        }

        /// <summary>
        /// 设置资源
        /// </summary>
        /// <typeparam name="TKey">资源键类型</typeparam>
        /// <typeparam name="TValue">资源键值类型</typeparam>
        /// <param name="key">资源键</param>
        /// <param name="value">资源</param>
        public void SetResource<TKey, TValue>(TKey key, TValue value)
        {
            _resources.AddOrUpdate(key, value, (k, v) => value);
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="key">资源键</param>
        /// <returns>资源</returns>
        public object GetResource(object key)
        {
            if (_resources.TryGetValue(key, out var value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <typeparam name="TValue">资源类型</typeparam>
        /// <param name="key">资源键</param>
        /// <returns>资源</returns>
        public TValue GetResource<TValue>(object key)
        {
            if (_resources.TryGetValue(key, out var value))
            {
                if (value is TValue o)
                {
                    return o;
                }

                return default;
            }

            return default;
        }

        /// <summary>
        /// 资源索引器
        /// </summary>
        /// <param name="key">资源键</param>
        /// <returns>资源</returns>
        public object this[object key]
        {
            get => GetResource(key);
            set => SetResource(key, value);
        }
    }
}
