using GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;

namespace GYMS_TR.Controllers
{
    public class OrdenController : Controller
    {
        /// <summary>
        ///  Aqui estemos utilizando las interfaces y repositorios creados para manejar las órdenes y sus detalles.
        /// </summary>
        /// <returns></returns>
        private readonly IOrdenRepositorio _ordenRepositorio;
        private readonly IOrdenDetalleRepositorio _ordenDetalleRepositorio;
        public OrdenController(IOrdenRepositorio ordenRepositorio, IOrdenDetalleRepositorio ordenDetalleRepositorio)
        {
            _ordenRepositorio = ordenRepositorio;
            _ordenDetalleRepositorio = ordenDetalleRepositorio;
        }
        public IActionResult OrdenIndex()
        {
            return View();
        }

        #region Apis
        [HttpGet]
        //Este es un metodo de apis donde vamos a obtener la lista de las ordenes//
        public IActionResult ObtenerListaOrdenes() 
        {
            return Json(new {data = _ordenRepositorio.ObtenerTodos()});
        }
        #endregion
    }
}
