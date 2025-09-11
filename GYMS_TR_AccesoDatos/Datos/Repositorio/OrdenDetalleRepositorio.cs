using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio;
using GYMS_TR_Modelos;

namespace GYMS_TR_AccesoDatos.Datos.Repositorio
{
    public class OrdenDetalleRepositorio : Repositorio<OrdenDetalle>, IOrdenDetalleRepositorio
    {
        // Inyección de dependencia del contexto de la base de datos //
        private readonly ApplicationDbContext _context;
        // Constructor que inicializa el contexto //
        public OrdenDetalleRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        // Implementación del método Actualizar de la interfaz IOrdenDetalleRepositorio //
        public void Actualizar(OrdenDetalle ordenDetalle)
        {
            _context.OrdenDetalle.Update(ordenDetalle);
        }

    }
}
