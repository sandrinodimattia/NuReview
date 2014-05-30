using System.ComponentModel.DataAnnotations;

namespace NuReview.Models
{
    public class ReviewEditModel
    {
        [Display(Name = "Name")]
        [Required]
        public string Name
        {
            get;
            set;
        }

        [Display(Name = "Package Id")]
        [Required]
        public string PackageId
        {
            get;
            set;
        }

        [Display(Name = "Review")]
        [Required]
        public string Body
        {
            get;
            set;
        }

        [Required]
        [Range(0, 10)]
        public int Score
        {
            get;
            set;
        }
    }
}