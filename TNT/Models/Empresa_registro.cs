using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace TNT.Models
{
    public class Empresa_registro
    {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string password { get; set; }
        [Required]
        [Display(Name = "Nombre Empresa")]
        public string nombre_empresa { get; set; }
        [Required]
        [Display(Name = "NIT")]
        public string nit { get; set; }
        [Required]
        [Display(Name = "Direccion")]
        public string direccion { get; set; }
        [Required]
        [Display(Name = "Representante Legal")]
        public string representante_legal { get; set; }
        [Required]
        [Display(Name = "Telefono")]
        public string telefono { get; set; }
        [Required]
        [Display(Name = "Dosificacion - Codigo Autorizacion")]
        public string dosificacion_codigo_autorizacion { get; set; }
        [Required]
        [Display(Name = "Dosificacion - Actividad Comercial")]
        public string dosificacion_actividad_comercial { get; set; }
        [Required]
        [Display(Name = "Departamento - Ciudad")]
        public string departamento { get; set; }


    }
}
