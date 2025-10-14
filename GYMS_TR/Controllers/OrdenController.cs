using GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio;
using GYMS_TR_Modelos;
using GYMS_TR_Modelos.ViewModels;
using GYMS_TR_Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GYMS_TR.Controllers
{
    // Solo los usuarios con el rol de administrador pueden acceder a este controlador//
    [Authorize(Roles = WC.AdminRole)] 
    public class OrdenController : Controller
    {
        /// <summary>
        ///  Aqui estemos utilizando las interfaces y repositorios creados para manejar las órdenes y sus detalles.
        /// </summary>
        /// <returns></returns>
        private readonly IOrdenRepositorio _ordenRepositorio;
        private readonly IOrdenDetalleRepositorio _ordenDetalleRepositorio;
        //Aqui vamos a crear una variable de ordenVM//
        [BindProperty]
        public OrdenVM? OrdenVM { get; set; }
        // Constructor que recibe las interfaces de los repositorios a través de inyección de dependencias//
        public OrdenController(IOrdenRepositorio ordenRepositorio, IOrdenDetalleRepositorio ordenDetalleRepositorio)
        {
            _ordenRepositorio = ordenRepositorio;
            _ordenDetalleRepositorio = ordenDetalleRepositorio;
        }
        public IActionResult OrdenIndex()
        {
            return View();
        }
        // Este método muestra los detalles de una orden específica//
        [HttpGet]
        public IActionResult Detalle(int id)
        {
            OrdenVM = new OrdenVM()
            {
                Orden = _ordenRepositorio.ObtenerPrimero(o => o.Id == id),
                OrdenDetalle = _ordenDetalleRepositorio.ObtenerTodos(d => d.OrdenId == id, incluirPropiedades: "Producto")
            };
            return View(OrdenVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Aqui metodo post para el de detalle de la orden que nos va a de volvel al carro de compras otra ves//
        public IActionResult Detalle()
        {
            //Aqui estamos creando una lista de carro de compras//
            List<CarroCompra> carroCompras = new List<CarroCompra>();
            // Aqui estamos obteniendo todos los detalles de la orden que coinciden con el Id de la orden en el modelo de vista OrdenVM//
            OrdenVM.OrdenDetalle = _ordenDetalleRepositorio.ObtenerTodos(d =>d.OrdenId == OrdenVM.Orden.Id);
            // Aqui estamos recorriendo cada item en los detalles de la orden//
            foreach (var item in OrdenVM.OrdenDetalle) 
            {
                // Aqui estamos creando una nueva instancia de CarroCompra para cada item en los detalles de la orden//
                CarroCompra carroCompra = new CarroCompra()
                {
                    ProductoId = item.ProductoId
                };
                carroCompras.Add(carroCompra);
            }
            HttpContext.Session.Clear();// Limpiamos la sesión actual//
            HttpContext.Session.Set( WC.SessionCarroCompras, carroCompras);// Establecemos la nueva lista de carro de compras en la sesión//
            HttpContext.Session.Set(WC.SessionOrdenId, OrdenVM.Orden.Id);// Establecemos el Id de la orden en la sesión//
            return RedirectToAction("CarroIndex", "Carro");// Redirigimos al usuario a la acción Index del controlador CarroCompras//

        }
        //este es metodo para eliminar la orden//
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar() 
        {
            // Primero, obtenemos todos los detalles de la orden que coinciden con el Id de la orden en el modelo de vista OrdenVM//
            Orden orden = _ordenRepositorio.ObtenerPrimero(o => o.Id == OrdenVM.Orden.Id);
            // Aqui estamos obteniendo todos los detalles de la orden que coinciden con el Id de la orden en el modelo de vista OrdenVM//
            IEnumerable<OrdenDetalle> ordenDetalles = _ordenDetalleRepositorio.ObtenerTodos(d => d.OrdenId == OrdenVM.Orden.Id);
            _ordenDetalleRepositorio.RemoverRango(ordenDetalles);
            _ordenRepositorio.Remover(orden);
            _ordenRepositorio.Grabar();

            return RedirectToAction(nameof(OrdenIndex));
        }
        

        #region // APIs Methods//
        [HttpGet]
        //Este es un metodo de apis donde vamos a obtener la lista de las ordenes//
        public IActionResult ObtenerListaOrdenes() 
        {
            return Json(new {data = _ordenRepositorio.ObtenerTodos()});
        }
        #endregion
    }
}
