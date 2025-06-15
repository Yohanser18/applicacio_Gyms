using GYMS_TR.Datos;
using GYMS_TR.Models;
using GYMS_TR.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GYMS_TR.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ProductoIndex()
        {
            IEnumerable<Producto> lista = _context.Producto.Include(c => c.Categoria).
                                                             Include(t => t.TipoAplicacion);
            return View(lista);
        }

        [HttpGet]
        public IActionResult UpsertProducto(int? Id) 
        {
            /*Con es es una lista de la tabla caterio que le vamos enviar a la vista producto con el selectListItems.select
             esto quiere decir que va a enviar tanto texto como valor a la vista //
            IEnumerable<SelectListItem> categoriaLista = _context.Categorias.Select(c => new SelectListItem
            {
                Text = c.NombreCategoria,
                Value = c.Id.ToString()
            });
            ViewBag.categoriaLista = categoriaLista;

            Producto producto = new Producto();*/

            /*Aqui estamos inicializando nuestro ViewModle de producto y 
             estamos llamando todo lo que esta a dentro de ViewModel productop*/

            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),// Aqui estamos inicializando todo lo que tiene que lo que tiene a dentro//

                CategoriaLista = _context.Categorias.Select(c =>  new SelectListItem
                {
                    Text = c.NombreCategoria,
                    Value = c.Id.ToString()
                }),

                TipoAplicacionLista = _context.TipoAplicacion.Select(t => new SelectListItem
                {
                    Text = t.Nombre,
                    Value = t.Id.ToString()
                })

            };

            if (Id == null)// estamos dicendo con el Id que si llamos de el bonto crear //
            {
                //` cree un nuevo pruducto //
                return View(productoVM);
            }
            else
            {
                //Aqui esatmos diciendo qued si llamos el Id desde el bonto editar eso
                //quiere dicer que ahi producto en la lista.
                productoVM.Producto = _context.Producto.Find(Id);

                if (productoVM.Producto == null)// estamos que si no encontro ese registro o culumna de producta nos de una excepcion//
                {
                    return NotFound();
                }
            }

            return View(productoVM.Producto);//la vista tiene producto que haga la accion de editar//
        }
    }
}
