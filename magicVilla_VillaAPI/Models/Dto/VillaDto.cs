using System.ComponentModel.DataAnnotations;

namespace magicVilla_VillaAPI.Models.Dto
{
    public class VillaDto
    {
        public int Id { get; set; }
        //This suported by Api/Controllar annotations
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
    }
}
