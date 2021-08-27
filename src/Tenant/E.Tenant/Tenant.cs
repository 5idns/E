using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace E.Tenant
{
    public class Tenant<T> where T:ITenantContext
    {
        private readonly ConcurrentDictionary<string, T> _contexts;

        private Tenant()
        {
            _contexts = new ConcurrentDictionary<string, T>();
        }

        public T this[string key]
        {
            get => _contexts.TryGetValue(key, out var context) ? context : default;
            set => _contexts.AddOrUpdate(key, value, (k, v) => value);
        }

        public bool IdentityContains(string identity)
        {
            return _contexts.Any(f =>
            {
                if (f.Value != null)
                {
                    return f.Value.IdentityContains(identity);
                }

                return false;
            });
        }

        public T FirstOrDefault(string identity)
        {
            var context = _contexts.FirstOrDefault(f =>
            {
                if (f.Value != null)
                {
                    return f.Value.IdentityContains(identity);
                }

                return false;
            });
            return context.Value;
        }

        // 定义一个静态变量来保存类的实例
        private static Tenant<T> _context;

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static Tenant<T> Context
        {
            get
            {
                if (_context == null)
                {
                    var tenant = new Tenant<T>();
                    Interlocked.CompareExchange(ref _context, tenant, null);
                }

                return _context;
            }
        }

        /// <summary>
        /// 异步本地租户ID
        /// </summary>
        private static readonly AsyncLocal<string> AsyncLocalTenantId;


        static Tenant()
        {
            AsyncLocalTenantId = new AsyncLocal<string>();
        }

        public static T ChangeTenant(string tenantId)
        {
            AsyncLocalTenantId.Value = tenantId;
            var tenantContext = Context[tenantId];
            return tenantContext;
        }

        public static T ChangeTenant(string tenantId, T context)
        {
            AsyncLocalTenantId.Value = tenantId;
            Context[tenantId] = context;
            return context;
        }

        public static bool Contains(string identity)
        {
            return Context.IdentityContains(identity);
        }

        public static T Get(string identity)
        {
            return Context.FirstOrDefault(identity);
        }

        public static T Current
        {
            get
            {
                var tenantId = AsyncLocalTenantId.Value ?? "Default";
                var tenantContext = Context[tenantId];
                return tenantContext;
            }
            set
            {
                var tenantId = AsyncLocalTenantId.Value ?? "Default";
                Context[tenantId] = value;
            }
        }
    }
}
