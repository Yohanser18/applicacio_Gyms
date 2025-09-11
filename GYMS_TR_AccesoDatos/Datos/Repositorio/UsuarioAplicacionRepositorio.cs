using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio;
using GYMS_TR_Modelos;

namespace GYMS_TR_AccesoDatos.Datos.Repositorio
{
    public class UsuarioAplicacionRepositorio: Repositorio<UsuarioAplicacion>, IUsuarioAplicacionRepositorio
    {
        private readonly ApplicationDbContext _context;
        public UsuarioAplicacionRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Actualizar(UsuarioAplicacion usuario)
        {
            _context.UsuarioAplicacion.Update(usuario);
        }
    }
}
