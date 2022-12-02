﻿using System.ComponentModel.DataAnnotations;

namespace PIA___Back___End___1986172.Entidades
{
    public class Boleto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requierido")]
        public int NumeroBoleto { get; set; }

        [Required(ErrorMessage = "El campo {0} es requierido")]
        public int ClienteID { get; set; }

        [Required(ErrorMessage = "El campo {0} es requierido")]
        public int RifaID { get; set; }

    }
}

