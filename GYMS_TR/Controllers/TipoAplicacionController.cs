using GYMS_TR.Datos;
using GYMS_TR.Models;
using Microsoft.AspNetCore.Mvc;

namespace GYMS_TR.Controllers
{
    public class TipoAplicacionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipoAplicacionController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult TipoAplicacionIndex()
        {
            IEnumerable<TipoAplicacion> lista = _context.TipoAplicacion;
            return View(lista);
        }

        [HttpGet]
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
                _context.TipoAplicacion.Add(tipoAplicacion);
                _context.SaveChanges();

                return RedirectToAction("TipoAplicacionIndex");
            }
            return View(tipoAplicacion);
            
        }

        [HttpGet]
        public IActionResult EditarTipoAplicacion(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var tp = _context.TipoAplicacion.Find(Id);

            if (tp == null) 
            {
                return NotFound();
            }
            return View(tp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarTipoAplicacion(TipoAplicacion tipoAplicacion)
        {
            if (ModelState.IsValid)
            {
                _context.TipoAplicacion.Update(tipoAplicacion);
                _context.SaveChanges();
                return RedirectToAction("TipoAplicacionIndex");
            }

            return View(tipoAplicacion);
        }

        [HttpGet]
        public IActionResult EliminarTipoAplicacion(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var tp = _context.TipoAplicacion.Find(Id);

            if (tp == null)
            {
                return NotFound();
            }
            return View(tp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarTipoAplicacion(TipoAplicacion tipoAplicacion)
        {
            if (tipoAplicacion == null)
            {
                return NotFound();
            }

            _context.TipoAplicacion.Remove(tipoAplicacion);
            _context.SaveChanges();
            return RedirectToAction("TipoAplicacionIndex");
        }


    }
}
