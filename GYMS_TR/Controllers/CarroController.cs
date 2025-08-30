using AspNetCoreGeneratedDocument;
using GYMS_TR.Datos;
using GYMS_TR.Models;
using GYMS_TR.Models.ViewModels;
using GYMS_TR.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace GYMS_TR.Controllers
{
    [Authorize] //Aqui estamos diciendo que para poder entrar a este controlador se necesita estar logueado//
    public class CarroController : Controller
    {
        // Aqui estamos creando una variable de tipo ApplicationDbContext para poder acceder a la base de datos y poder hacer las consultas que necesitemos.//
        private readonly ApplicationDbContext _context;
        // Aqui estamos creando una variable de tipo IWebHostEnvironment para poder acceder a la carpeta wwwroot donde esta le carpeta de templetes que donde vamos accedr para utilizar el template del correo.//
        private readonly IWebHostEnvironment _webHostEnvironment;
        // Aqui estamos creando una variable de tipo IEmailSender para poder enviar correos electronicos.//
        private readonly IEmailSender _emailSender;
        // Aqui estamos diciendo que vamos a usar el modelo ProductoUsuarioVM para poder mostrar los productos y el usuario que esta logueado en la aplicacion y en el boton continuar//
        [BindProperty] 
        public ProductoUsuarioVM productoUsuarioVM { get; set; }
        // Aqui estamos inyectando el ApplicationDbContext en el controlador CarroController para poder acceder a la base de datos.//
        public CarroController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        public IActionResult CarroIndex()
        {
            //Aqui le estamos diciendo a la lista que se llene con todos los productos//
            List<CarroCompra> carroCompraLista = new List<CarroCompra>();
            //Aqui estamos diciendo que si la session esta llena que pase a entrar al carrito de compra//
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null &&
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroCompraLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras);
            }
            // Aqui estamos obteniendo los Id de los productos que estan en el carro de compra//
            List<int> ProdEnCarro = carroCompraLista.Select(i => i.ProductoId).ToList();
            // Aqui estamos obteniendo los productos que estan en el carro de compra por el Id//
            IEnumerable<Producto> ProdLista = _context.Producto.Where(p => ProdEnCarro.Contains(p.Id));
            //Aqui estamos retornando la vista con los productos que estan en el carro de compra//
            return View(ProdLista); 
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
                ProductoLista = ProdLista.ToList() //Aqui estamos obteniendo los productos que estan en el carro de compra por el Id//
            };
            return View(productoUsuarioVM); //Aqui estamos retornando la vista con el modelo ProductoUsuarioVM que contiene el usuario y los productos que estan en el carro de compra//
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Resumen")]
        public async Task <IActionResult> ResumenPost(ProductoUsuarioVM productoUsuarioVM) //Aqui estamos recibiendo el modelo ProductoUsuarioVM que contiene el usuario y los productos que estan en el carro de compra//
        {
            //Aqui vamos acceder a la capeta de wwwroot donde esta le carpeta de templetes que donde vamos accedr para utilizar el templetes//
            var rutaTemplete = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "templetes" + Path.DirectorySeparatorChar.ToString() + "PlantillaOrden.html";

            //Aqui estamos definiendo el asunto del correo que vamos a enviar al usuario que esta logueado en la aplicacion o conectado//
            var subject = "Nueva Orden";
            // Aqui estamos definiendo el cuerpo del correo que vamos a enviar al usuario que esta logueado en la aplicacion o conectado//
            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(rutaTemplete))
            {
                //Aqui estamos leyendo el contenido del archivo HTML que contiene el template del correo//
                HtmlBody = sr.ReadToEnd(); 
            }
            //Aqui estamos creando un StringBuilder para poder concatenar los productos que estan en el carro de compra//
            StringBuilder productoListaSB = new StringBuilder();
            // Aqui estamos recorriendo la lista de productos que estan en el carro de compra//
            foreach (var prod in productoUsuarioVM.ProductoLista)
            {
                productoListaSB.Append($" - Nombre: {prod.NombreProducto} <span style='font-size:14px;'> (ID: {prod.Id})<span/><br>");
            }

            // Aqui estamos formateando el cuerpo del correo con los datos del usuario y los productos que estan en el carro de compra//
            string messageBody = string.Format(HtmlBody, 
                productoUsuarioVM.UsuarioAplicacion.NombreCompleto,
                productoUsuarioVM.UsuarioAplicacion.Email,
                productoUsuarioVM.UsuarioAplicacion.PhoneNumber,
                productoListaSB.ToString());

            // Aqui estamos enviando el correo al Aministrado que esta logueado en la aplicacion o conectado//
            await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);

            // Aqui estamos redirigiendo a la vista Confirmacion despues de enviar el correo al usuario que esta logueado en la aplicacion o conectado//
            return RedirectToAction(nameof(Confirmacion)); 
        }

        public IActionResult Confirmacion()
        {
            //Aqui vamos a limpiar la session cuando agomaos el envio//
            HttpContext.Session.Clear();
            return View();
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
