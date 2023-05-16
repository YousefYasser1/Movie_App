using Movies.Data;
using System.ComponentModel.DataAnnotations;

namespace Movies.ViewModel
{
    public class MovieViewModel
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [MaxLength(1200)]
        public string StoryLine { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [Range(0 , 10)]
        public double Rate { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public byte[] Poster { get; set; }

        public byte GenraId { get; set; }

        public IEnumerable<Genra> Genras { get; set; }
    }
}
