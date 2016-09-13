using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TNT.Models
{
    public class Nuevo_Usuario
    {
        [Required(ErrorMessage = "El email es requerido")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required(ErrorMessage = "El password es requerido")]
        [StringLength(30, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string password { get; set; }

        [DisplayName("Confirmacion de Contraseña")]
        [Compare("password", ErrorMessage="La confirmacion de password no coincide con el password")]
        public string password_confimacion { get; set; }

        [Required(ErrorMessage = "El nit es requerido")]
        [Display(Name = "NIT")]
        public string nit { get; set; }
        [Required(ErrorMessage = "El numero de celular es requerido")]
        [Display(Name = "Celular")]
        public string numero_celular { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El/los apellido(s) es/son requerido(s)")]
        [DisplayName("Apellidos")]
        public string apellidos { get; set; }

        [Required(ErrorMessage = "El carnet de identidad es requerido")]
        [DisplayName("Carnet de identidad")]
        public string carnet_identidad { get; set; }

        [Required(ErrorMessage = "La direccion es requerida")]
        [DisplayName("Direccion")]
        public string direccion { get; set; }

        [Required(ErrorMessage = "El departamento es requerido")]
        [DisplayName("Departamento")]
        public string departamento { get; set; }
    }
}