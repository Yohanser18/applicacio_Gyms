using System.Diagnostics;
using GYMS_TR_AccesoDatos.Datos;
using GYMS_TR_Modelos;
using GYMS_TR_Modelos.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using GYMS_TR_Utilidades;
using GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio;

namespace GYMS_TR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //Aqui estamos inyectando el contexto de la base de datos desde el la interfaces//
        private readonly IProductoRepositorio _Icontextprod;
        private readonly ICategoriaRepositorio _IcontextCat;
        // Aqui estamos llamando al contexto de la base de datos//
        public HomeController(ILogger<HomeController> logger, IProductoRepositorio contextprod, ICategoriaRepositorio contextcat)
        {
            _logger = logger;
            _Icontextprod = contextprod;
            _IcontextCat = contextcat;
        }
        //Aqui estamos llamdo al VM homevm que es donde estamos hacediendo las entidades//
        public IActionResult Index() 
        {
            // este es el proceso para hacer el filtrado tonto de categoria como de producto//
            HomeVM homeVM  = new HomeVM() 
            {
                #region // este es proceso logico que utilizamas la primera vez
                /*Productos = _context.Producto.Include(c => c.Categoria)
                                                     .Include(t => t.TipoAplicacion),
                Categorias = _context.Categorias*/
                #endregion
                // Aqui estamos llamando a nuestro repositorio generico que es el que tiene la herncia del Interfaces repositorio//
                Productos = _Icontextprod.ObtenerTodos(incluirPropiedades: "Categoria,TipoAplicacion"),
                Categorias = _IcontextCat.ObtenerTodos()
            };

            return View(homeVM);
        }

        [HttpGet]
        public IActionResult Detalle(int Id)
        {
           //Aqui lo que estamos haciendo es dicir que en la session del carro de compra ahi o no hay producto
            List<CarroCompra> carroComprasLista = new List<CarroCompra>(); 
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            //Aqui estamos llamando a nuestro detalle VM y le estamos diciendo que le entidad producto de vuelva un unico registro por Id//
            DetalleVM detalleVM = new DetalleVM() 
            {
                #region // este es proceso logico que utilizamas la primera vez
                /*Producto = _context.Producto.Include(c => c.Categoria)
                                             .Include(t => t.TipoAplicacion)
                                             .FirstOrDefault(p => p.Id == Id),*/
                #endregion

                Producto = _Icontextprod.ObtenerPrimero(p => p.Id == Id, incluirPropiedades: "Categoria,TipoAplicacion"),

                ExisteEnCarro = false
            };
            //Aqui estamos diciendo que el producto esta en el carro de comprar, esto es para poder hacer el removido del carro//
            foreach (var item in carroComprasLista)
            {
                if (item.ProductoId == Id)
                {
                    detalleVM.ExisteEnCarro =  true;
                }
            }


            return View(detalleVM);
        }

        [HttpPost, ActionName("Detalle")]
        public IActionResult DetallePost(int Id)
        {
            //Aqui estamos agregando producto en el carro de compras y le estamos diciendo que lo agrege por el Id ese producto al carro de compras //
            List<CarroCompra> carroComprasLista = new List<CarroCompra>(); 
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null 
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                //Aqui es donde vamos mostra el corro de compro tiene producto//
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            //Aqui es donde eestamos agregando al carro de compra productos por el Id//
            carroComprasLista.Add(new CarroCompra { ProductoId = Id });
            //esto aqui es para que muestre los producto que tiene el corro de compra y lo que acabamos de agregar //
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista);

            return RedirectToAction("Index");
        }

        public IActionResult RemoverDeCarro (int Id)//Aqui estamos haciendo la accion de remover del carro//
        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null && 
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0) 
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            //Aqui estamos diciendo que si ese producto heciste en  la lista del carro de comprsa  por el Id//
            var ProductoARemover = carroComprasLista.SingleOrDefault(x  => x.ProductoId == Id);
            //Ahora dicimos que si este en la lista es diston que null que pase removerlo del carro de compras//
            if (ProductoARemover != null)
            {
                carroComprasLista.Remove(ProductoARemover);
            }
            //Aqui estamos dicendo que esta en guaedada ese secion o producto que si existe en la secion//
            HttpContext.Session.Set(WC.SessionCarroCompras,carroComprasLista);
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
