namespace GYMS_TR.Models.ViewModels
{
    public class HomeVM //Aqui en este VM estaremos ulizando la entidades de categoria y producto//
    {
        public IEnumerable<Producto> Productos { get; set; }
        public IEnumerable<Categoria> Categorias { get; set; }
    }
}
