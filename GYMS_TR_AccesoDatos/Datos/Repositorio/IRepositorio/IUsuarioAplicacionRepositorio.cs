using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYMS_TR_Modelos;

namespace GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio
{
    // Interfaz para el repositorio de UsuarioAplicacion que hereda de la interfaz genérica IRepositorio
    public interface IUsuarioAplicacionRepositorio: IRepositorio<UsuarioAplicacion>
    {
        // Método para actualizar una entidad UsuarioAplicacion//
        void Actualizar(UsuarioAplicacion usuario);
    }
}
