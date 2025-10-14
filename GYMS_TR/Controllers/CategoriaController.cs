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
    public class CategoriaController : Controller
    {
        private readonly ICategoriaRepositorio _Icontext; // Aqui vamos atraer la entidades de base de datos por el repositorio//

        public CategoriaController(ICategoriaRepositorio Icontext) // Aqui vamos a crear el constructor//
        {
            _Icontext = Icontext;
        }

        public IActionResult CategoriaIndex()
        {
            // Aqui vamos a llamar el metodo de obtener todos el IRepocitorio generico//
            IEnumerable<Categoria> lista = _Icontext.ObtenerTodos();
            return View(lista);
        }

        [HttpGet]
        // Este es para crear la categoria//
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
                // Aqui vamos a llamar el metodo de agregar el IRepocitorio generico//
                _Icontext.Agregar(categoria);
                _Icontext.Grabar();
                TempData[WC.Exitosa] = "Categoria creada exitosamente";
                return RedirectToAction("CategoriaIndex");
            }
            TempData[WC.Error] = "Error al crear la categoria";
            return View(categoria);
        }

        [HttpGet]
        public IActionResult EditarCategoria(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            // Aqui vamos a llamar el metodo de obtener el IRepocitorio generico//
            var cg = _Icontext.Obtener(Id.GetValueOrDefault());

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
                // Aqui vamos a llamar el metodo de actualizar el IRepocitorio generico//
                _Icontext.Actualizar(categoria);
                _Icontext.Grabar();
                TempData[WC.Exitosa] = "Categoria actualizada exitosamente";
                return RedirectToAction("CategoriaIndex");
            }
            TempData[WC.Error] = "Error al actualizar la categoria";
            return View(categoria);
        }

        [HttpGet]
        public IActionResult EliminarCategoria(int? Id)
        {
            if(Id == null || Id == 0) 
            {
                return NotFound(); 
            }
            // Aqui vamos a llamar el metodo de obtener el IRepocitorio generico//
            var cg = _Icontext.Obtener(Id.GetValueOrDefault());

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
            // Validamos que no venga nulo//
            if (categoria == null) 
            {
                return NotFound();
            }
            // Aqui vamos a llamar el metodo de eliminar el IRepocitorio generico//
            _Icontext.Remover(categoria);
            _Icontext.Grabar();
            TempData[WC.Exitosa] = "Categoria eliminada exitosamente";
            return RedirectToAction("CategoriaIndex");
        }

    }
}
