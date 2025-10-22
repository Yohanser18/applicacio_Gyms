using AspNetCoreGeneratedDocument;
using GYMS_TR_AccesoDatos.Datos;
using GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio;

//Esta es la capa de modelos//
using GYMS_TR_Modelos;
using GYMS_TR_Modelos.ViewModels;
using GYMS_TR_Utilidades;
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
        // Aqui estamos creando una variable de tipo ApplicationDbContext para poder acceder a la base de datos.//
        private readonly IProductoRepositorio _IcontextProd;
        private readonly IUsuarioAplicacionRepositorio _IcontextUser;
        private readonly IOrdenRepositorio _Icontextord;
        private readonly IOrdenDetalleRepositorio _Icontextorddet;

        // Aqui estamos creando una variable de tipo IWebHostEnvironment para poder acceder a la carpeta wwwroot donde esta le carpeta de templetes que donde vamos accedr para utilizar el template del correo.//
        private readonly IWebHostEnvironment _webHostEnvironment;
        // Aqui estamos creando una variable de tipo IEmailSender para poder enviar correos electronicos.//
        private readonly IEmailSender _emailSender;
        // Aqui estamos diciendo que vamos a usar el modelo ProductoUsuarioVM para poder mostrar los productos y el usuario que esta logueado en la aplicacion y en el boton continuar//
        [BindProperty] 
        public ProductoUsuarioVM productoUsuarioVM { get; set; }
        // Aqui estamos inyectando el ApplicationDbContext en el controlador CarroController para poder acceder a la base de datos.//
        public CarroController(IProductoRepositorio contextprod,
            IUsuarioAplicacionRepositorio contextuser, 
            IWebHostEnvironment webHostEnvironment, 
            IEmailSender emailSender, 
            IOrdenRepositorio icontextord, 
            IOrdenDetalleRepositorio icontextorddet
         )
        {
            _IcontextProd = contextprod;
            _IcontextUser = contextuser;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _Icontextord = icontextord;
            _Icontextorddet = icontextorddet;
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
            #region // este es proceso logico que utilizamas la primera vez
            /*IEnumerable<Producto> ProdLista = _context.Producto.Where(p => ProdEnCarro.Contains(p.Id));*/
            #endregion
            IEnumerable<Producto> ProdLista = _IcontextProd.ObtenerTodos(p => ProdEnCarro.Contains(p.Id));
            // Aqui estamos pasando todos los productos en una lista//
            List<Producto> prodListFinal = new List<Producto>();
            //Aqui esteremos recorriendo todos los productos que tenemos en el carro de compra//
            foreach (var carroObj in carroCompraLista)
            {
                // Aqui estamos buscando el producto que esta en el carro de compra por el Id//
                Producto productoTemp = ProdLista.FirstOrDefault(p => p.Id == carroObj.ProductoId);
                productoTemp.TempMetrocuadrado = carroObj.MetroCuadrado; //Aqui estamos pasando los metrocuadrado que el usuario selecciono en el carro de compra//
                prodListFinal.Add(productoTemp); //Aqui estamos agregando el producto a la lista final//
                TempData[WC.Exitosa] = "Exitosamente el agregar al carro";
            }
            //Aqui estamos retornando la vista con los productos que estan en el carro de compra//
            return View(ProdLista); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Aqui estamos diciendo que vamos a validar el token de seguridad para evitar ataques CSRF//
        [ActionName("CarroIndex")]//Aqui estamos diciendo que vamos a usar el metodo CarroIndex para poder agregar los productos al carro de compra//
        public IActionResult IndexCarroPost(IEnumerable<Producto> productosLista)
        {
            //Aqui le estamos diciendo a la lista que se llene con todos los productos//
            List<CarroCompra> carroCompraLista = new List<CarroCompra>();
            foreach (Producto prod in productosLista)
            {
                carroCompraLista.Add(new CarroCompra
                {
                    ProductoId = prod.Id,
                    MetroCuadrado = prod.TempMetrocuadrado
                });
            }
            // Aqui estamos actualizando la session con los nuevos valores del carro de compra//
            HttpContext.Session.Set(WC.SessionCarroCompras, carroCompraLista);
            return RedirectToAction("Resumen");
        }

        public IActionResult Resumen()
        {
            //Aqui vamos a identificar el tipo de usuario que este realizando la compra//
            UsuarioAplicacion usuarioAplicacion;

            if (User.IsInRole(WC.AdminRole))
            {
                // si el usuario es administrador//
                // si el carro de compra este ligado a una orden de compra esto quiere dicir que es una orden//
                if (HttpContext.Session.Get<int>(WC.SessionOrdenId) != 0)
                {
                    // Aqui vamos a traer la orden que esta en la session//
                    Orden orden = _Icontextord.ObtenerPrimero(o => o.Id == HttpContext.Session.Get<int>(WC.SessionOrdenId));
                    //Aqui vamos a traer el usuario que realizo la orden//
                    usuarioAplicacion = new UsuarioAplicacion()
                    {
                        NombreCompleto = orden.NombreCompleto,
                        Email = orden.Email,
                        PhoneNumber = orden.Telefono
                    };
                }
                else // si no pertenece a una orden//
                {
                    // Aqui lo dejamos vacio//
                    usuarioAplicacion = new UsuarioAplicacion();
                }
            }
            else // si no es un usuario administrador//
            {
                //Aqui estamos obteniendo la identidad del usuario que esta logueado en la aplicacion o conectado//
                var ClaimsIdentity = (ClaimsIdentity)User.Identity;
                //Aqui estamos obteniendo el Id del usuario que esta logueado en la aplicacion o conectado//
                var Cliam = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                //Aqui estamos obteniendo el usuario que esta logueado en la aplicacion o conectado//
                usuarioAplicacion = _IcontextUser.ObtenerPrimero(u => u.Id == Cliam.Value);
            }


            //Aqui vamos a traer el usuario que esta logueado en la aplicacion o conectado//
            //Aqui le estamos diciendo a la lista que se llene con todos los productos//
            List<CarroCompra> carrocompraLista = new List<CarroCompra>();
            //Aqui estamos diciendo que si la session esta llena que pase a entrar al carrito de compra//
            if (HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras) != null && 
                HttpContext.Session.Get<IEnumerable<CarroCompra>>(WC.SessionCarroCompras).Count() > 0) 
            {
                //Aqui le estamos diciendo que se llene la lista con los productos que estan en el carro de compra//
                carrocompraLista = HttpContext.Session.Get<List<CarroCompra>>(WC.SessionCarroCompras); 
            }
            //Aqui estamos obteniendo los Id de los productos que estan en el carro de compra//
            List<int> ProdEnCarro = carrocompraLista.Select(i => i.ProductoId).ToList();
            //Aqui estamos obteniendo los productos que estan en el carro de compra por el Id//
            #region //esta es proceso logico que utilizamas la primera vez
            /*IEnumerable<Producto> ProdLista = _context.Producto.Where(p => ProdEnCarro.Contains(p.Id));*/
            #endregion
            IEnumerable<Producto> ProdLista = _IcontextProd.ObtenerTodos(p => ProdEnCarro.Contains(p.Id));
            productoUsuarioVM = new ProductoUsuarioVM()
            {
                //Aqui estamos obteniendo el usuario que esta logueado en la aplicacion o conectado//
                #region //esta es proceso logico que utilizamas la primera vez
                /*UsuarioAplicacion = _context.UsuarioAplicacion.FirstOrDefault(u => u.Id == Cliam.Value),*/
                #endregion
                UsuarioAplicacion = usuarioAplicacion,// Aqui estamos obteniendo el usuario que esta logueado en la aplicacion o conectado//
                ProductoLista = ProdLista.ToList() //Aqui estamos obteniendo los productos que estan en el carro de compra por el Id//
            };
            return View(productoUsuarioVM); //Aqui estamos retornando la vista con el modelo ProductoUsuarioVM que contiene el usuario y los productos que estan en el carro de compra//
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Resumen")]
        public async Task <IActionResult> ResumenPost(ProductoUsuarioVM productoUsuarioVM) //Aqui estamos recibiendo el modelo ProductoUsuarioVM que contiene el usuario y los productos que estan en el carro de compra//
        {
            //Aqui vamos a capturar el usuario que esta logueado en la aplicacion o conectado//
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var Cliam = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

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
            //Aqui vamos a Grabar la orden y detalle en la DB//
            Orden orden = new Orden() 
            {
                UsuarioapliacacioId = Cliam.Value,
                NombreCompleto = productoUsuarioVM.UsuarioAplicacion.NombreCompleto,
                Email = productoUsuarioVM.UsuarioAplicacion.Email,
                Telefono = productoUsuarioVM.UsuarioAplicacion.PhoneNumber,
                FechaOrden = DateTime.Now
            };
            _Icontextord.Agregar(orden);
            _Icontextord.Grabar();
            //Aqui va ordenDetalle//
            foreach (var prod in productoUsuarioVM.ProductoLista)
            {
                OrdenDetalle ordenDetalle = new OrdenDetalle()
                {
                    OrdenId = orden.Id,
                    ProductoId = prod.Id,
                };
                _Icontextorddet.Agregar(ordenDetalle);
            }
            _Icontextorddet.Grabar();


            // Aqui estamos redirigiendo a la vista Confirmacion despues de enviar el correo al usuario que esta logueado en la aplicacion o conectado//
            return RedirectToAction(nameof(Confirmacion)); 
        }
        // Este metodo es para limpiar la session despues de hacer el envio//
        public IActionResult Confirmacion()
        {
            //Aqui vamos a limpiar la session cuando agomaos el envio//
            HttpContext.Session.Clear();
            return View();
        }
        // Este es el metodo para actualizar el corro de compra //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarCarro(IEnumerable<Producto> productosLista)
        {
            //Aqui le estamos diciendo a la lista que se llene con todos los productos//
            List<CarroCompra> carroCompraLista = new List<CarroCompra>();
            foreach (Producto prod in productosLista)
            {
                carroCompraLista.Add(new CarroCompra
                {
                    ProductoId = prod.Id,
                    MetroCuadrado = prod.TempMetrocuadrado
                });
            }
            // Aqui estamos actualizando la session con los nuevos valores del carro de compra//
            HttpContext.Session.Set(WC.SessionCarroCompras, carroCompraLista);
            TempData[WC.Exitosa] = "Carro de compras actualizado exitosamente";
            return RedirectToAction("CarroIndex");
        }
        //Este metodo es para remover un producto del carro de compra//
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
            TempData[WC.Exitosa] = "Producto removido exitosamente del carro de compras";
            return RedirectToAction("CarroIndex");

        }
    }
}
