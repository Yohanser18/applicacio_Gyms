using System.Diagnostics;
using GYMS_TR.Datos;
using GYMS_TR.Models;
using GYMS_TR.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using GYMS_TR.Utilidades;

namespace GYMS_TR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;

            _context = context;
        }

        public IActionResult Index() //Aqui estamos llamdo al VM homevm que es donde estamos hacediendo las entidades//
        {
            HomeVM homeVM  = new HomeVM() // esta es el proceso para hacer el filtrado tonto de categoria como de producto//
            {
                Productos = _context.Producto.Include(c => c.Categoria)
                                                     .Include(t => t.TipoAplicacion),
                Categorias = _context.Categorias
            };

            return View(homeVM);
        }

        [HttpGet]
        public IActionResult Detalle(int Id)
        {
            DetalleVM detalleVM = new DetalleVM() //Aqui estamos llamando a nuestro detalle VM y le estamos diciendo que le entidad producto de vuelva un unico registro por Id//
            { 
                Producto = _context.Producto.Include(c => c.Categoria)
                                             .Include(t => t.TipoAplicacion)
                                             .FirstOrDefault(p => p.Id == Id),
                ExisteEnCarro = false
            };

            return View(detalleVM);
        }

        [HttpPost, ActionName("Detalle")]
        public IActionResult DetallePost(int Id)
        {
            List<CarroCompra> carroComprasLista = new List<CarroCompra>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null 
                && HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            carroComprasLista.Add(new CarroCompra { ProductoId = Id });
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista);

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
