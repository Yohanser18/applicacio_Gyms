using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio;
using GYMS_TR_Modelos;

namespace GYMS_TR_AccesoDatos.Datos.Repositorio
{
    public class OrdenRepositorio : Repositorio<Orden>, IOrdenRepositorio
    {
        // Inyectamos el ApplicationDbContext
        private readonly ApplicationDbContext _context;
        // Constructor donde inicializamos el contexto
        public OrdenRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        // Implementamos el método Actualizar de la interfaz IOrdenRepositorio//
        public void Actualizar(Orden orden)
        {
            _context.Orden.Update(orden);
        }
    }
}
