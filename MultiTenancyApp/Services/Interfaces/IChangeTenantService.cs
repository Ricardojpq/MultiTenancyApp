namespace MultiTenancyApp.Services.Interfaces
{
    public interface IChangeTenantService
    {
        public Task ReplaceTenant(Guid companyId, string userId);
    }
}
