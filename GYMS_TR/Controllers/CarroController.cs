using AspNetCoreGeneratedDocument;
using GYMS_TR.Datos;
using GYMS_TR.Models;
using GYMS_TR.Utilidades;
using Microsoft.AspNetCore.Mvc;

namespace GYMS_TR.Controllers
{
    public class CarroController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarroController(ApplicationDbContext context)
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
            List<int> ProdEnCarro = carroCompraLista.Select(i => i.ProductoId).ToList();//ya aqui esta nuestro carro de compra lleno //
            IEnumerable<Producto> ProdLista = _context.Producto.Where(p => ProdEnCarro.Contains(p.Id));//Aqui esta remos mostrondo los producto que esten en el carro de compras por el Id//
            return View(ProdLista);
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
