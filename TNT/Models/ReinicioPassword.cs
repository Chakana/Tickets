using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TNT.Models
{
    public class ReinicioPassword
    {
        [DisplayName("Email")]
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El email tiene un formato incorrecto")]
        [MaxLength(60, ErrorMessage = "El email debe tener 60 caracteres como maximo")]
        public string email {get;set;}

    }
    public class NuevoPassword
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string password { get; set; }
        [Required]
        public string token { get; set; }
    }
}