using GYMS_TR_AccesoDatos.Datos;
using GYMS_TR_Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//Esta es la biblioteca //
using GYMS_TR_Utilidades;
using GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio;

namespace GYMS_TR.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class TipoAplicacionController : Controller
    {
        // Inyectamos el contexto de la base de datos del repositorio//
        private readonly ITipoAplicacionRepositorio _Icontext;
        /// <summary>
        /// Constructor del controlador de TipoAplicacion
        /// </summary>
        /// <param name="Icontext"></param>
        public TipoAplicacionController(ITipoAplicacionRepositorio Icontext)
        {
            _Icontext = Icontext;
        }
        public IActionResult TipoAplicacionIndex()
        {
            // Aqui vamos a traer todos los registros de la tabla TipoAplicacion de la Interfaces de repositario//
            IEnumerable<TipoAplicacion> lista = _Icontext.ObtenerTodos();
            return View(lista);
        }

        [HttpGet]
        // Metodo para crear un nuevo tipo de aplicacion//
        public IActionResult CrearTipoAplicacion() 
        { 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearTipoAplicacion(TipoAplicacion tipoAplicacion)
        {
            if (ModelState.IsValid)
            {
                // Aqui vamos a agregar el nuevo tipo de aplicacion  des la interfaces de repostorio//
                _Icontext.Agregar(tipoAplicacion);
                _Icontext.Grabar();

                return RedirectToAction("TipoAplicacionIndex");
            }
            return View(tipoAplicacion);
            
        }

        [HttpGet]
        // Metodo para editar un tipo de aplicacion//
        public IActionResult EditarTipoAplicacion(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var tp = _Icontext.Obtener(Id.GetValueOrDefault());

            if (tp == null) 
            {
                return NotFound();
            }
            return View(tp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Metodo para editar un tipo de aplicacion//
        public IActionResult EditarTipoAplicacion(TipoAplicacion tipoAplicacion)
        {
            if (ModelState.IsValid)
            {
                // Aqui vamos a actualizar el tipo de aplicacion  des la interfaces de repostorio//
                _Icontext.Actualizar(tipoAplicacion);
                _Icontext.Grabar();
                return RedirectToAction("TipoAplicacionIndex");
            }

            return View(tipoAplicacion);
        }

        [HttpGet]
        // Metodo para eliminar un tipo de aplicacion//
        public IActionResult EliminarTipoAplicacion(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var tp = _Icontext.Obtener(Id.GetValueOrDefault());

            if (tp == null)
            {
                return NotFound();
            }
            return View(tp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Metodo para eliminar un tipo de aplicacion//
        public IActionResult EliminarTipoAplicacion(TipoAplicacion tipoAplicacion)
        {
            if (tipoAplicacion == null)
            {
                return NotFound();
            }
            // Aqui vamos a eliminar el tipo de aplicacion  des la interfaces de repostorio//
            _Icontext.Remover(tipoAplicacion);
            _Icontext.Grabar();
            return RedirectToAction("TipoAplicacionIndex");
        }


    }
}
