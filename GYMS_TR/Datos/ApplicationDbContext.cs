using GYMS_TR.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GYMS_TR.Datos
{
    public class ApplicationDbContext : IdentityDbContext //Es aqui que hacemos que nuestros modelos se combiertan en 
    {                                         //una tabla de la base de datos

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<TipoAplicacion> TipoAplicacion {  get; set; }
        public DbSet<Producto> Producto { get; set; }
    }
}
