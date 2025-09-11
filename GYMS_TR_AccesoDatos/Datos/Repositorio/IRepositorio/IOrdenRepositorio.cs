using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYMS_TR_Modelos;

namespace GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio
{
    // Interfaz para el repositorio de Orden que hereda de la interfaz genérica IRepositorio
    public interface IOrdenRepositorio : IRepositorio<Orden>
    {
        // Método para actualizar una entidad Orden//
        void Actualizar(Orden orden);
    }
}
