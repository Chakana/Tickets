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

        [DisplayName("Password")]
        [Required(ErrorMessage="El password es requerido")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "El password debe tener entre 8 y 30 caracteres")]
        public string password { get; set; }
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