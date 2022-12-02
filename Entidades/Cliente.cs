using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PIA___Back___End___1986172.Entidades
{
    public class Cliente 
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requierido")]
        [StringLength(maximumLength: 15, ErrorMessage = "El campo {0} solo puede tener hasta 15 caracteres")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "El campo {0} es requierido")]
        [StringLength(maximumLength: 15, ErrorMessage = "El campo {0} solo puede tener hasta 15 caracteres")]
        public string ApellidoCliente { get; set; }

        public int NumeroCliente { get; set; }
        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }
    }
}