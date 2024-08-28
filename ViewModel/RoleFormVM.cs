using System.ComponentModel.DataAnnotations;

namespace UserManagement.ViewModel
{
    public class RoleFormVM
    {
        [Key]
        [Required,StringLength(256)]
        public string RoleName { get; set; }
    }
}
