using GYMS_TR.Datos;
using GYMS_TR.Models;
using Microsoft.AspNetCore.Mvc;

namespace GYMS_TR.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ApplicationDbContext _context; //el Dbcontext es el que se va a en cargar de traerme la tabla //

        public CategoriaController(ApplicationDbContext context) // que estomo dicendo que que ven con la base detos //
        {
            _context = context;
        }

        public IActionResult CategoriaIndex()
        {
            IEnumerable<Categoria> lista = _context.Categorias;
            return View(lista);
        }

        [HttpGet]
        public IActionResult Crearcategoria() 
        { 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crearcategoria(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Categorias.Add(categoria);
                _context.SaveChanges();
                return RedirectToAction("CategoriaIndex");
            }
            return View(categoria);
        }

        [HttpGet]
        public IActionResult EditarCategoria(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var cg = _context.Categorias.Find(Id);

            if (cg ==null )
            {
                NotFound();
            }
            return View(cg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarCategoria(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Update(categoria);
                _context.SaveChanges();
                return RedirectToAction("CategoriaIndex");
            }
            return View(categoria);
        }

        [HttpGet]
        public IActionResult EliminarCategoria(int? Id)
        {
            if(Id == null || Id == 0) 
            {
                return NotFound(); 
            }

            var cg = _context.Categorias.Find(Id);

            if (cg == null)
            {
                return NotFound();
            }

            return View(cg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarCategoria(Categoria categoria)
        {
            if (categoria == null)
            {
                return NotFound();
            }
            _context.Remove(categoria);
            _context.SaveChanges();
            return RedirectToAction("CategoriaIndex");
        }

    }
}
