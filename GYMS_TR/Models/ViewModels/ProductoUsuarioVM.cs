namespace GYMS_TR.Models.ViewModels
{
    public class ProductoUsuarioVM // Esta es la vista modelo que vamos a usar para poder mostrar los productos y el usuario que esta logueado en la aplicacion y en el boton continuar//
    {
        public ProductoUsuarioVM()
        {
            ProductoLista = new List<Producto>();
        }
        public UsuarioAplicacion UsuarioAplicacion { get; set; }
        public IEnumerable<Producto> ProductoLista { get; set; }
    }
}
