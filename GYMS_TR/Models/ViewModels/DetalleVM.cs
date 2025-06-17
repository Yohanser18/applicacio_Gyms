namespace GYMS_TR.Models.ViewModels
{
    public class DetalleVM // Aqui es que vamos autilizar la entidad de producto y vamos a crear una propiedad nueva para ver si lla existe en el carro de compra
    {
        public DetalleVM()//Aqui estamos inicializando la entidad producto para no tener que hacerlo cada ves que vallamos autilizar esta entidad//
        {
            Producto = new Producto();
        }
        public  Producto Producto { get; set; } // esta es la entidad de producto //

        public bool ExisteEnCarro { get; set; }// esta es nueva propiedad que va a verifiacr si ya esta en le carro//
    }
}
