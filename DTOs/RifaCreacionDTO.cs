using System.ComponentModel.DataAnnotations;

namespace PIA___Back___End___1986172.DTOs
{
    public class RifaCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requierido")]
        [Range(1, 99, ErrorMessage = "El numero de la rifa debe de ser un valor entre 1 y 99")]
        public int NumeroRifa { get; set; }
    }
}
