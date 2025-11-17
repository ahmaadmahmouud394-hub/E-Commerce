using E_Commerce.Domain.Enums;

namespace E_Commerce.Domain.Permessions
{
    public interface IPermessions
    {
        public EPermissions Brand { get; set; }
        public EPermissions Users { get; set; }
        public EPermissions Categories { get; set; }
        public EPermissions Products { get; set; }
        public EPermissions Roles { get; set; }
    }
}
