using System.ComponentModel.DataAnnotations;

namespace PIA___Back___End___1986172.DTOs
{
    public class ClienteCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requierido")]
        [StringLength(maximumLength: 15, ErrorMessage = "El campo {0} solo puede tener hasta 15 caracteres")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "El campo {0} es requierido")]
        [StringLength(maximumLength: 15, ErrorMessage = "El campo {0} solo puede tener hasta 15 caracteres")]
        public string ApellidoCliente { get; set; }


        [Required(ErrorMessage = "El campo {0} es requierido")]
        public int NumeroCliente { get; set; }


    }
}
