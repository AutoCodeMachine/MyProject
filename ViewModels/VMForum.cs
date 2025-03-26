using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyProject.ViewModels
{
    public class VMForum
    {
        [StringLength(12, MinimumLength = 12)]  //發布日期+0001，Ex：20250305+0001
        [Key]
        [HiddenInput]
        public string PostID { get; set; } = null!;

        [HiddenInput]
        public string ForumID { get; set; } = null!;

        [HiddenInput]
        public string? ForumName1 { get; set; }

        [HiddenInput]
        public string MemberID { get; set; } = null!;

        [HiddenInput]
        public string? AccountName { get; set; }

        [Display(Name = "發文時間")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        [HiddenInput]
        public DateTime PostTime { get; set; }

        [Display(Name = "主題")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "主題1~30個字")]
        [Required(ErrorMessage = "必填")]
        public string PostTitle { get; set; } = null!;

        [Display(Name = "內容")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "必填")]
        public string PostContents { get; set; } = null!;

        [Display(Name = "圖片")]
        [StringLength(18)]  //文章編號+01，Ex：20250305+0001+01.jpg/.png/.gif
        public List<string>? Image { get; set; }

        [HiddenInput]
        public string? ImageType { get; set; }

    }
}
