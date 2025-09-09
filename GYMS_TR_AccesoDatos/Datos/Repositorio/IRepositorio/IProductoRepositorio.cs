using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYMS_TR_Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio
{
    //Aqui le estamos pasando el repositorio generioco a la interfaces//
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        // Aqui estamos definiendo el metodo actualizar//
        void Actualizar(Producto producto);

        // este es el metodo que va a devolver todos las relaciones de producto//
        IEnumerable<SelectListItem> ObtenerTodaRelacioneList(string objeto);
    }
}
