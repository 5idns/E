namespace E.Tenant
{
    /// <summary>
    /// 租户上下文
    /// </summary>
    public class TenantContext: ITenantContext
    {

        public TenantContext(string tenantId)
        {
            TenantId = tenantId;
        }

        public string TenantId { get; set; }
    }
}
