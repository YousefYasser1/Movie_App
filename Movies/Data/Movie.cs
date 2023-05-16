using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Movies.Data
{
    public class Movie
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [MaxLength(1200)]
        public string StoryLine { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public double Rate { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [System.ComponentModel.DataAnnotations.Required]
        public byte[] Poster { get; set; }

        public byte GenraId { get; set; }
        public Genra Genra { get; set; }
    }
}
