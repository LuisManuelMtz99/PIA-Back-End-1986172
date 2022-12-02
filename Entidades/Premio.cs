using System.ComponentModel.DataAnnotations;

namespace PIA___Back___End___1986172.Entidades
{
    public class Premio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requierido")]
        [StringLength(maximumLength: 40, ErrorMessage = "El campo {0} solo puede tener hasta 40 caracteres")]
        public string NombrePremio { get; set; }

        [Required(ErrorMessage = "El campo {0} es requierido")]
        public int RifaId { get; set; }


    }
}

