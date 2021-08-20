using System.Collections.Concurrent;
using System.Threading;

namespace E.Tenant
{
    public class Tenant
    {
        private readonly ConcurrentDictionary<string, ITenantContext> _contexts;

        private Tenant()
        {
            _contexts = new ConcurrentDictionary<string, ITenantContext>();
        }

        public ITenantContext this[string key]
        {
            get
            {
                var context = _contexts.GetOrAdd(key, k => new TenantContext(key));
                return context;
            }
            set { _contexts.AddOrUpdate(key, value, (k, v) => value); }
        }

        // 定义一个静态变量来保存类的实例
        private static Tenant _context;

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static Tenant Context
        {
            get
            {
                if (_context == null)
                {
                    var tenant = new Tenant();
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

        public static ITenantContext ChangeTenant(string tenantId)
        {
            AsyncLocalTenantId.Value = tenantId;
            var tenantContext = Context[tenantId];
            return tenantContext;
        }

        public static ITenantContext Current
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
