using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GYMS_TR_Modelos
{
    //Aqui estamos heredando todo lo que tiene que ver con el scaffoid  para agreagera este campo a la tabla AspNetUser//
    public class UsuarioAplicacion : IdentityUser 
    {
        public string NombreCompleto { get; set; }
        //Aqui vamos agregar nuevas propiededes//
        [NotMapped]
        public string  Direccion { get; set; }
        [NotMapped]
        public string Ciudad { get; set; }
    }
}
