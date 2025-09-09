namespace GYMS_TR_Utilidades
{
    public static class WC // Aqui vamos aguadar todas la constante de nuestro proyecto //
    {
        //esta variable va a guardar en las carpeta que creamos en wwwroot que fue la imagenes\producto//
        public const string ImagenRuta = @"\imagenes\producto\";
        //Aqui vamos a ir guardando todos lo producto que valla el usuario seleccionda para comprar//
        public const string SessionCarroCompras = "SessionCarroCompras";
        // Aqui estaremos identificando el tipo de permiso de Administrador//
        public const string AdminRole = "Admin";
        // Aqui estaremos identificando el permiso de Cliente//
        public const string ClienteRole = "Cliente";
        //Aqui vamos a guardar el correo del administrador para que desde la aplicacion se envie un correo al administrador//
        public const string EmailAdmin = "yohanser.familia@famisoft.co";
        //Aqui vamos a guardar el nombre de la categoria//
        public const string CategoriaNombre = "Categoria";
        //Aqui vamos a guardar el nombre del tipo de aplicacion//
        public const string TipoAplicacionNombre = "TipoAplicacion";
    }
}
