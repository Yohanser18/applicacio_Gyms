using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio;
using GYMS_TR_Modelos;

namespace GYMS_TR_AccesoDatos.Datos.Repositorio
{
    // Aqui vamos a implementar los metodos de la interfaz ICategoriaRepositorio//
    public class CategoriaRepositario : Repositorio<Categoria> , ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _context;

        // Aqui vamos a crear el constructor//
        public CategoriaRepositario(ApplicationDbContext context) : base(context) 
        {
            _context = context;   
        }

        // Este es el metodo para actualizar//
        public void Actualizar(Categoria categoria)
        {
            // Aqui vamos a buscar la categoria que vamos a actualizar//
            var CategoriaAnterior = _context.Categorias.FirstOrDefault(c => c.Id == categoria.Id);
            // Aqui vamos a actualizar los campos que queremos actualizar//
            if ( CategoriaAnterior != null)
            {
                CategoriaAnterior.NombreCategoria = CategoriaAnterior.NombreCategoria;
                CategoriaAnterior.MostrasOrden = CategoriaAnterior.MostrasOrden;
            }
        }
    }
}
