using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio;
using GYMS_TR_Modelos;

namespace GYMS_TR_AccesoDatos.Datos.Repositorio
{
    // Aqui vamos a implementar los metodos de la interfaz ITipoAplicacionRepositorio//
    public class TipoAplicacionRepositorio : Repositorio<TipoAplicacion>, ITipoAplicacionRepositorio
    {
        private readonly ApplicationDbContext _context;
        // Aqui vamos a crear el constructor//
        public TipoAplicacionRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        // Este es el metodo para actualizar//
        public void Actualizar(TipoAplicacion tipoAplicacion)
        {
            // Aqui vamos a buscar la Tipo Aplicacion que vamos a actualizar//
            var tipoAnterior = _context.TipoAplicacion.FirstOrDefault(t => t.Id == tipoAplicacion.Id);
            // Aqui vamos a actualizar los campos que queremos actualizar//
            if (tipoAnterior != null)
            {
                tipoAnterior.Nombre = tipoAplicacion.Nombre;
            }
        }
    }
}
