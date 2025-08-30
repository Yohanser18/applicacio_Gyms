using Microsoft.AspNetCore.Identity;

namespace GYMS_TR.Models
{
    //Aqui estamos heredando todo lo que tiene que ver con el scaffoid  para agreagera este campo a la tabla AspNetUser//
    public class UsuarioAplicacion : IdentityUser 
    {
        public string NombreCompleto { get; set; }
    }
}
