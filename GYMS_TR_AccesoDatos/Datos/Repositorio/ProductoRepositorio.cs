using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio;
using GYMS_TR_Modelos;
using GYMS_TR_Utilidades;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYMS_TR_AccesoDatos.Datos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        // Aqui vamos a llamar  lo que va hacer todo lo que contrala los datos//
        private readonly ApplicationDbContext _context;
        // Aqui vamos a crear el constructor donde vamos a ser inyeccion de dependencia//
        public ProductoRepositorio(ApplicationDbContext context) : base(context) 
        {
            _context = context; 
        }
        //Aqui estamos llamando el metodo actualizar desde la interfaces//
        public void Actualizar(Producto producto)
        {
            //Aqui vamos a buscar el producto que vamos a actualizar//
           _context.Update(producto);
        }
        // Este metodo es para obtener todas las relaciones de producto//
        public IEnumerable<SelectListItem> ObtenerTodaRelacioneList(string objeto)
        {
            //Aqui estamos obteniendo todas las categorias y las estamos convirtiendo en una lista de SelectListItem//
            if (objeto == WC.CategoriaNombre)
            {
                return _context.Categorias.Select(i => new SelectListItem
                {
                    Text = i.NombreCategoria,
                    Value = i.Id.ToString()
                });
            }
            // Aqui estamos obteniendo todos los tipos de aplicacion y las estamos convirtiendo en una lista de SelectListItem//
            if (objeto == WC.TipoAplicacionNombre)
            {
                return _context.TipoAplicacion.Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                });
            }
            return null;
        }
    }
}
