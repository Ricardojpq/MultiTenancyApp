using MultiTenancyApp.Services.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MultiTenancyApp.Utils
{
    public enum Permissions
    {
        [Hide]
        IsEmployee = 0,
        [Display(Description = "Este Usuario Puede Crear Productos")]
        CreateProduct,
        [Display(Description = "Este Usuario Puede Leer Productos")]
        ReadProduct,
        BindUser,
        ReadPermission,
        ModifyPermission
    }
}
