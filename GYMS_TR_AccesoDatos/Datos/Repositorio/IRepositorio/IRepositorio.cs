using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio
{
    /*Aqui vomas agreaga todo lo que tiene que ver con las entidade que tenemos como modelos de la base de datos*/
    public interface IRepositorio<T> where T : class
    {
        //Este es el que va a traer un solo registro de la base de datos//
        T Obtener(int id);

        /*Este es que va a traer todos los registros de dicha entidad de la base de datos*/
        IEnumerable<T> ObtenerTodos(
            Expression<Func<T, bool>> filtro = null, //este es el filtrado//
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBay = null,// este es el que se encarga de ordenarlos//
            string incluirPropiedades = null,// este es el que se encaga de icluir las propiedades //
            bool isTracking = true // este es el que se encarga de hacer el seguimiento de los cambios//
            );
         //este es el que se encarga de el primer registro de las entidades //
         T ObtenerPrimero(
            Expression<Func<T, bool>> filtro = null, //este es el filtrado//
            string incluirPropiedades = null,// este es el que se encaga de icluir las propiedades //
            bool isTracking = true // este es el que se encarga de hacer el seguimiento de los cambios//
            );

        //Este es el que se encarga de agregar un registro a la base de datos//
        void Agregar(T entidad);
        //Este es el que se encarga de eliminar un registro de la base de datos//
        void Remover(T entidad);
        //Este es el que se encarga de grabar los cambios en la base de datos//
        void Grabar();
    }
}
