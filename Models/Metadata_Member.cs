using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    public class MemberData
    {
        [Display(Name = "會員編號")]
        [StringLength(36, MinimumLength = 36)]
        [Key]
        [HiddenInput]
        public string MemberID { get; set; } = null!; //採用GUID

        [Display(Name = "身分證字號")]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression("[A-Z][1-2][0-9]{8}", ErrorMessage = "格式錯誤 (Ex：A1○○○○○○○○)")]
        [Required(ErrorMessage = "必填")]
        public string PersonalID { get; set; } = null!;

        [Display(Name = "會員暱稱")]
        [StringLength(24, MinimumLength = 1, ErrorMessage = "帳號名稱1~24個字")]
        [Required(ErrorMessage = "必填")]
        public string AccountName { get; set; } = null!;

        [Display(Name = "姓名")]
        [StringLength(24, MinimumLength = 1, ErrorMessage = "姓名1~24個字")]
        [Required(ErrorMessage = "必填")]
        public string PersonalName { get; set; } = null!;

        [Display(Name = "性別")]
        [Required(ErrorMessage = "必填")]
        public bool Gender { get; set; }

        [Display(Name = "生日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Required(ErrorMessage = "必填")]
        public DateOnly Birthday { get; set; }

        [Display(Name = "手機號碼")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "手機號碼為10碼")]
        [Required(ErrorMessage = "必填")]
        public string Mobile { get; set; } = null!;

        [Display(Name = "電子郵件信箱")]
        [StringLength(40)]
        [EmailAddress(ErrorMessage = "E-Mail格式錯誤")]
        [Required(ErrorMessage = "必填")]
        public string Email { get; set; } = null!;

    }

    [ModelMetadataType(typeof(MemberData))]
    public partial class Member
    {
    }

    public class MemberPasswordData
    {
        [Display(Name = "會員密碼")]
        [Required(ErrorMessage = "必填")]
        [StringLength(24, MinimumLength = 8, ErrorMessage = "密碼為8-24碼")]
        [DataType(DataType.Password)]
        public string AccountPassword { get; set; } = null!;

    }

    [ModelMetadataType(typeof(MemberPasswordData))]
    public partial class MemberPassword
    {
    }

}
