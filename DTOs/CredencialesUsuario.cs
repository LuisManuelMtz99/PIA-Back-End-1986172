using System.ComponentModel.DataAnnotations;

namespace PIA___Back___End___1986172.DTOs
{
    public class CredencialesUsuario
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
