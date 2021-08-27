namespace E.Tenant
{
    public interface ITenantAccessor
    {
        ITenantContext Context { get; }
    }

    public interface ITenantAccessor<out T>:ITenantAccessor where T : ITenantContext
    {
        T TenantContext { get; }
    }
}
