using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYMS_TR_Modelos;

namespace GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio
{
    // Aqui vomas agreaga todo lo que tiene que ver con las entidade que tenemos como modelos de la base de datos//
    public interface ITipoAplicacionRepositorio : IRepositorio<TipoAplicacion>
    {
        // Metodo para actualizar//
        void Actualizar(TipoAplicacion tipoAplicacion);
    }
}
