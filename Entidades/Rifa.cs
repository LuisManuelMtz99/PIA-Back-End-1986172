using System.ComponentModel.DataAnnotations;

namespace PIA___Back___End___1986172.Entidades
{
    public class Rifa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requierido")]
        public int NumeroRifa { get; set; }

    }
}
