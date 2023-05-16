using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Data
{
    public class Genra
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        public string Name { get; set; }
    }
}
