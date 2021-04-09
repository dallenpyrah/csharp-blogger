using System.ComponentModel.DataAnnotations;

namespace blogger.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImgUrl { get; set; }
        public bool Published { get; set; }
        public string CreatorId { get; set; }

        public Profile Creator { get; set; }
    }
}