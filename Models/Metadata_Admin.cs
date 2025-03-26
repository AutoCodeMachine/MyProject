using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
   public class AdministratorData
    {
        [Display(Name = "管理者帳號")]
        [StringLength(20, MinimumLength = 1)]
        [Required(ErrorMessage = "請輸入管理者帳號")]
        public string AdminID { get; set; } = null!;

        [Display(Name = "管理者密碼")]
        [StringLength(20, MinimumLength = 1)]
        [Required(ErrorMessage = "請輸入管理者密碼")]
        public string AdminPassword { get; set; } = null!;

    }

    [ModelMetadataType(typeof(AdministratorData))]

    public partial class Administrator
    { 
    }

}
