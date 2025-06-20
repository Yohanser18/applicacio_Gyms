﻿using GYMS_TR.Datos;
using GYMS_TR.Models;
using GYMS_TR.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GYMS_TR.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;//Esto es necesario para poder recibir ;a imagenes desde la vista hacia el controlador//

        public ProductoController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult ProductoIndex()
        {
            IEnumerable<Producto> lista = _context.Producto.Include(c => c.Categoria).
                                                             Include(t => t.TipoAplicacion);
            return View(lista);
        }

        [HttpGet]
        public IActionResult UpsertProducto(int? Id) 
        {
            /*Con es es una lista de la tabla caterio que le vamos enviar a la vista producto con el selectListItems.select
             esto quiere decir que va a enviar tanto texto como valor a la vista //
            IEnumerable<SelectListItem> categoriaLista = _context.Categorias.Select(c => new SelectListItem
            {
                Text = c.NombreCategoria,
                Value = c.Id.ToString()
            });
            ViewBag.categoriaLista = categoriaLista;

            Producto producto = new Producto();*/

            /*Aqui estamos inicializando nuestro ViewModle de producto y 
             estamos llamando todo lo que esta a dentro de ViewModel productop*/

            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),// Aqui estamos inicializando todo lo que tiene que lo que tiene a dentro//

                CategoriaLista = _context.Categorias.Select(c =>  new SelectListItem
                {
                    Text = c.NombreCategoria,
                    Value = c.Id.ToString()
                }),

                TipoAplicacionLista = _context.TipoAplicacion.Select(t => new SelectListItem
                {
                    Text = t.Nombre,
                    Value = t.Id.ToString()
                })

            };

            if (Id == null)// estamos dicendo con el Id que si llamos de el bonto crear //
            {
                //` cree un nuevo pruducto //
                return View(productoVM);
            }
            else
            {
                //Aqui esatmos diciendo qued si llamos el Id desde el bonto editar eso
                //quiere dicer que ahi producto en la lista.
                productoVM.Producto = _context.Producto.Find(Id);

                if (productoVM.Producto == null)// estamos que si no encontro ese registro o culumna de producta nos de una excepcion//
                {
                    return NotFound();
                }
            }

            return View(productoVM);//la vista tiene producto que haga la accion de editar//
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertProducto(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;//este metodo es que va a resibir la imgende desde la vista//
                string wbRootPath = _webHostEnvironment.WebRootPath;//Aqui estamos diciendo que nos guarde la imagen en esta ruta//

                if (productoVM.Producto.Id == 0)//Aqui estamos deciendo que si el id del producto es igual a cero que nos cree un nuevo producto//
                {
                    //crear producto nuevo//
                    string upload = wbRootPath + WC.ImagenRuta;//Aqui esta guardando la imagen en la ruta que creamos en la carpeta wwwroot//
                    string fileName = Guid.NewGuid().ToString();// esta es para que le a signe un id a nuestra imagenes//
                    string extension = Path.GetExtension(files[0].FileName);//Aqui estamos recibiendo la imagen que agregamos//

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName+extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productoVM.Producto.ImageneUrl = fileName + extension;
                    _context.Producto.Add(productoVM.Producto);
                }
                else // Aqui estamos diceindo que el id es distinto o igual que  cero que Actualize el producto o editar//
                { 
                    //actualizar//
                    var objProducto = _context.Producto.AsNoTracking().FirstOrDefault(p => p.Id == productoVM.Producto.Id);//aqui lo que vamos hacer es que nos busque la imagen que vamos editar por el Id//

                    if (files.Count() > 0)//Aqui es que se va a cargar la imagen para crearla de nuevo//
                    {
                        string upload = wbRootPath + WC.ImagenRuta;//Aqui esta guardando la imagen en la ruta que creamos en la carpeta wwwroot//
                        string fileName = Guid.NewGuid().ToString();// esta es para que le a signe un id a nuestra imagenes//
                        string extension = Path.GetExtension(files[0].FileName);//Aqui estamos recibiendo la imagen que agregamos//

                        //Aqui se va a borrar la imagen que ya esta cargada //
                        var anteriorFile = Path.Combine(upload, objProducto.ImageneUrl);

                        if (System.IO.File.Exists(anteriorFile))//Aqui estamos diciendo que si la imagen existe borramela//
                        {
                            System.IO.File.Delete(anteriorFile);
                        }//fin de la imagen barrada anterior //

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productoVM.Producto.ImageneUrl = fileName+extension;//Ya aqui estamos cargando la imagen nueva//
                    }//En caso contrario  no cargando una  nueva imagen//
                    else
                    {
                        productoVM.Producto.ImageneUrl = objProducto.ImageneUrl;
                    }
                    _context.Producto.Update(productoVM.Producto);

                }
                _context.SaveChanges();
                return RedirectToAction("ProductoIndex");
            }// este es la llame de ModelState.Isvalid//
            // esto es por si algo falla, que se llene las listas de categoria, tipoAplicacion
            productoVM.CategoriaLista = _context.Categorias.Select(c => new SelectListItem
            {
                Text = c.NombreCategoria,
                Value = c.Id.ToString()
            });

            productoVM.TipoAplicacionLista = _context.TipoAplicacion.Select(t => new SelectListItem
            {
                Text = t.Nombre,
                Value = t.Id.ToString()
            });
            return View(productoVM);
        }

        [HttpGet]
        public IActionResult EliminarProducto(int? Id)
        {
            if (Id == null || Id == 0)//Aqui estamos diciendo que si el Id es null o cero que devuelva no encontrado//
            {
                return NotFound();
            }

            Producto producto = _context.Producto.Include(c => c.Categoria)   //Aqui estamos diceindo que nos traiga ese registra por el Id seleciondo//
                                                    .Include(t => t.TipoAplicacion)
                                                    .FirstOrDefault(p => p.Id == Id);
            if (producto == null) //Aqui estamos diciendo que si no encontro el registro por el Id que nos diga no encontrado//
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarProducto(Producto producto)
        {
            if (producto == null) 
            {
                return NotFound();
            }
            //Aui vamos a eliminar la imagen que esta agregadar//
            string upload =  _webHostEnvironment.WebRootPath + WC.ImagenRuta;
            
            //Aqui se va a borrar la imagen que ya esta cargada //
            var anteriorFile = Path.Combine(upload, producto.ImageneUrl);

            if (System.IO.File.Exists(anteriorFile))//Aqui estamos diciendo que si la imagen existe borramela//
            {
                System.IO.File.Delete(anteriorFile);
            }//fin de la imagen barrada anterior //

            _context.Producto.Remove(producto);
            _context.SaveChanges();
            return RedirectToAction("ProductoIndex");
        }
    }
}
