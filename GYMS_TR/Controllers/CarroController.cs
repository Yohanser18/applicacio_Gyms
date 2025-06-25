using AspNetCoreGeneratedDocument;
using GYMS_TR.Datos;
using GYMS_TR.Models;
using GYMS_TR.Models.ViewModels;
using GYMS_TR.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GYMS_TR.Controllers
{
    [Authorize] //Aqui estamos diciendo que para poder entrar a este controlador se necesita estar logueado//
    public class CarroController : Controller
    {
        private readonly ApplicationDbContext _context;// Aqui estamos creando una variable de tipo ApplicationDbContext para poder acceder a la base de datos y poder hacer las consultas que necesitemos.//

        [BindProperty] // Aqui estamos diciendo que vamos a usar el modelo ProductoUsuarioVM para poder mostrar los productos y el usuario que esta logueado en la aplicacion y en el boton continuar//
        public ProductoUsuarioVM productoUsuarioVM { get; set; }

        public CarroController(ApplicationDbContext context)// Aqui estamos inyectando el ApplicationDbContext en el controlador CarroController para poder acceder a la base de datos.//
        {
            _context = context;
        }

        public IActionResult CarroIndex()
        {
            List<CarroCompra> carroCompraLista = new List<CarroCompra>();//Aqui le estamos diciendo a la lista que se llene con todos los productos//
            //Aqui estamos diciendo que si la session esta llena que pase a entrar al carrito de compra//
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroCompraLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            List<int> ProdEnCarro = carroCompraLista.Select(i => i.ProductoId).ToList();// Aqui estamos obteniendo los Id de los productos que estan en el carro de compra//
            IEnumerable<Producto> ProdLista = _context.Producto.Where(p => ProdEnCarro.Contains(p.Id));// Aqui estamos obteniendo los productos que estan en el carro de compra por el Id//
            return View(ProdLista); //Aqui estamos retornando la vista con los productos que estan en el carro de compra//
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Aqui estamos diciendo que vamos a validar el token de seguridad para evitar ataques CSRF//
        [ActionName("CarroIndex")]//Aqui estamos diciendo que vamos a usar el metodo CarroIndex para poder agregar los productos al carro de compra//
        public IActionResult IndexCarroPost()
        {
            return RedirectToAction("Resumen");
        }

        public IActionResult Resumen()
        {
            //Aqui vamos a traer el usuario que esta logueado en la aplicacion o conectado//
            var ClaimsIdentity = (ClaimsIdentity)User.Identity; //Aqui estamos obteniendo la identidad del usuario que esta logueado en la aplicacion o conectado//
            var Cliam =  ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier); //Aqui estamos obteniendo el Id del usuario que esta logueado en la aplicacion o conectado//
            
            List<CarroCompra> carrocompraLista = new List<CarroCompra>();//Aqui le estamos diciendo a la lista que se llene con todos los productos//
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null && 
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0) //Aqui estamos diciendo que si la session esta llena que pase a entrar al carrito de compra//
            {
                carrocompraLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras); //Aqui le estamos diciendo que se llene la lista con los productos que estan en el carro de compra//
            }
            List<int> ProdEnCarro = carrocompraLista.Select(i => i.ProductoId).ToList(); //Aqui estamos obteniendo los Id de los productos que estan en el carro de compra//
            IEnumerable<Producto> ProdLista = _context.Producto.Where(p => ProdEnCarro.Contains(p.Id)); //Aqui estamos obteniendo los productos que estan en el carro de compra por el Id//

            productoUsuarioVM = new ProductoUsuarioVM()
            {
                UsuarioAplicacion = _context.UsuarioAplicacion.FirstOrDefault(u => u.Id == Cliam.Value), //Aqui estamos obteniendo el usuario que esta logueado en la aplicacion o conectado//
                ProductoLista = ProdLista //Aqui estamos obteniendo los productos que estan en el carro de compra por el Id//
            };

            return View(productoUsuarioVM); //Aqui estamos retornando la vista con el modelo ProductoUsuarioVM que contiene el usuario y los productos que estan en el carro de compra//
        }

        public IActionResult RemoverCarro(int Id) 
        {
            List<CarroCompra> carroCompraLista = new List<CarroCompra>();//Aqui le estamos diciendo a la lista que se llene con todos los productos//
            //Aqui estamos diciendo que si la session esta llena que pase a entrar al carrito de compra//
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
              carroCompraLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            //Aqui le estomos diciendo que remueva ese producto por el Id del carro de compras//
            carroCompraLista.Remove(carroCompraLista.FirstOrDefault(p => p.ProductoId == Id));
            HttpContext.Session.Set(WC.SessionCarroCompras, carroCompraLista );

            return RedirectToAction("CarroIndex");

        }
    }
}
