using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
   
    public class ForumData
    {
        [StringLength(12, MinimumLength = 12)]  //發布日期+0001，Ex：20250305+0001
        [Key]
        [HiddenInput]
        public string PostID { get; set; } = null!;

        [HiddenInput]
        public string ForumID { get; set; } = null!;

        [HiddenInput]
        public string MemberID { get; set; } = null!;

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

        //ICollection：表示即使沒有設定值，這個屬性也永遠不會是 null，它會是一個空的集合（List<ForumComments>），這樣就避免了 null 引發的錯誤
        public virtual ICollection<ForumComments> ForumComments { get; set; } = new List<ForumComments>();
    }

    [ModelMetadataType(typeof(ForumData))]

    public partial class Forum
    { 
    }

    public class ForumCommentsData
    {
        [Display(Name = "留言編號")]
        [StringLength(12, MinimumLength = 12)]  //發布日期+0001，Ex：20250305+0001
        [Key]
        [HiddenInput]
        public string CommentID { get; set; } = null!;

        [Display(Name = "留言時間")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        public DateTime PostTime { get; set; }

        [Display(Name = "留言內容")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "留言限制1~255個字")]
        [Required(ErrorMessage = "必填")]
        public string PostContents { get; set; } = null!;
    }
    

    [ModelMetadataType(typeof(ForumCommentsData))]
    public partial class ForumComments
    {
    }

    public class ForumImageData
    {
        [Display(Name = "圖片檔案")]
        [StringLength(18)]  //PostID+01，Ex：20250305+0001+01 .jpg/.png/.gif
        [Key]
        [HiddenInput]
        public string Image { get; set; } = null!;

        [HiddenInput]
        public string ImageType { get; set; } = null!;
    }

    [ModelMetadataType(typeof(ForumImageData))]
    public partial class ForumImage
    {
    }

    public class ForumHikingGroupData
    {
        [Display(Name = "文章編號")]
        [StringLength(12, MinimumLength = 12)]  //發布日期+0001，Ex：20250305+0001
        [Key]
        public string PostID { get; set; } = null!;

        [Display(Name = "發文時間")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        public DateTime PostTime { get; set; }

        [Display(Name = "路線名稱")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "名稱1~30個字")]
        [Required(ErrorMessage = "必填")]
        public string PostTitle { get; set; } = null!;

        [Display(Name = "行程說明")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "必填")]
        public string PostContents { get; set; } = null!;

        [Display(Name = "出發時間")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        [Required(ErrorMessage = "必填")]
        public DateTime DepartureTime { get; set; }

        [Display(Name = "人數上限")]
        [Range(1, 50, ErrorMessage = "人數上限1~50人")]
        [Required(ErrorMessage = "必填")]
        public int MaxMembers { get; set; }

        [Display(Name = "報名截止時間")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        [Required(ErrorMessage = "必填")]
        public DateTime Deadline { get; set; }

        [Display(Name = "參加會員名稱")]
        [StringLength(1200, MinimumLength = 1)]
        [Required(ErrorMessage = "必填")]
        [HiddenInput]
        public string JoinMembers { get; set; } = null!;
    }

    [ModelMetadataType(typeof(ForumHikingGroupData))]
    public partial class ForumHikingGroup
    {
    }

    public class HikingRecordsData
    {
        [Display(Name = "記錄編號")]
        [StringLength(40, MinimumLength = 40)]
        [Key]
        [HiddenInput]
        public string RecordID { get; set; } = null!; //採用 MemberID(GUID)+0001

        [Display(Name = "發表時間")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd  hh:mm:ss}")]
        [HiddenInput]
        public DateTime UploadDate { get; set; }

        [Display(Name = "三角點")]
        [Required(ErrorMessage = "必填")]
        public string LandmarkID { get; set; } = null!;

        [Display(Name = "行程日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Required(ErrorMessage = "必填")]
        public DateOnly HikingDate { get; set; }

        [Display(Name = "行程總時間")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0: hh:mm}")]
        [Required(ErrorMessage = "必填")]
        public TimeOnly HikingTime { get; set; }

        [Display(Name = "gpx檔案")]
        public string? gpxFile { get; set; }

        [Display(Name = "行程筆記")]
        [DataType(DataType.MultilineText)]
        public string? HikingNote { get; set; }

        [Display(Name = "圖片")]
        [StringLength(46)]  //RecordID+01，Ex：MemberID(GUID)+0001+01 .jpg/.png/.gif
        public string? ImageName { get; set; }

        [HiddenInput]
        public string? ImageType { get; set; }

        [Display(Name = "共享或私有")]
        [Required(ErrorMessage = "必填")]
        public bool PublicPrivate { get; set; }
    }

    [ModelMetadataType(typeof(HikingRecordsData))]
    public partial class HikingRecords
    {
    }

    public class ForumNameData 
    {
        [Display(Name = "版面代號")]
        [StringLength(1, ErrorMessage = "請輸入單一字母或數字")]
        [RegularExpression(@"^[A-Za-z0-9]$", ErrorMessage = "只能輸入單一英文字母或數字")]
        [Key]
        public string ForumID { get; set; } = null!;

        [Display(Name = "版面名稱")]
        [StringLength(5, ErrorMessage = "名稱限制為五個字")]
        public string ForumName1 { get; set; } = null!;
    }

    [ModelMetadataType(typeof(ForumNameData))]
    public partial class ForumName
    {
    }
}
