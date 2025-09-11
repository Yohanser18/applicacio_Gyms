using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYMS_TR_Modelos;

namespace GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio
{
    // Interfaz para el repositorio de OrdenDetalle que hereda de la interfaz genérica IRepositorio
    public interface IOrdenDetalleRepositorio : IRepositorio<OrdenDetalle>
    {
        // Método para actualizar una entidad OrdenDetalle//
        void Actualizar(OrdenDetalle ordenDetalle);
    }
}
