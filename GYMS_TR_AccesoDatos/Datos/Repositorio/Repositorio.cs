using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GYMS_TR_AccesoDatos.Datos.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace GYMS_TR_AccesoDatos.Datos.Repositorio
{
    /*Aqui es donde vamos a utilizar todo lo que tiene que ver con 
     el metodo de agregado, el mostrado,el eliminado todo lo de dato 
     del el contrato IRepositorio.
    */
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        //Aqui vamos a agregar los que tiene que ver con lo que  permite el acceso a datos //
        private readonly ApplicationDbContext _context;
        // este es la varible que va aperimitir el acceso a datos y los metosde de hacer el CRUD//
        internal DbSet<T> dbSet;
        // Aqui vamos a crear el constructor//
        public Repositorio(ApplicationDbContext context)
        {
            /*Aqui estamos haciendo inyeccion de dependiecia*/
            _context = context;
            this.dbSet = _context.Set<T>();
        }
        //Este es para agregar //
        public void Agregar(T entidad)
        {
            dbSet.Add(entidad);
        }

        //Este es para grabar//
        public void Grabar()
        {
           _context.SaveChanges();
        }

        //Este es para obtener//
        public T Obtener(int id)
        {
            return dbSet.Find(id);
        }

        // Este es para obtener el primero//
        public T ObtenerPrimero(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null, bool isTracking = true)
        {
            // este es que se encarga de hacer el seteo de las entidades de la base de datos//
            IQueryable<T> query = dbSet;
            //este es el filtrado //
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            //Este es para incluir las propiedades//
            if (incluirPropiedades != null)
            {
                foreach (var incluirPropiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirPropiedad);
                }
            }
            //Aqui vamos hacer el AaNoTracking//
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefault();

        }

        //Este es para obtener todos//
        public IEnumerable<T> ObtenerTodos(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBay = null, string incluirPropiedades = null, bool isTracking = true)
        {
            // este es que se encarga de hacer el seteo de las entidades de la base de datos//
            IQueryable<T> query = dbSet;
            //este es el filtrado //
            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            //Este es para incluir las propiedades//
            if (incluirPropiedades != null)
            {
                foreach (var incluirPropiedad in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirPropiedad);
                }
            }
            //Aqui vamos hacer que se ordena //
            if (orderBay != null)
            {
             query = orderBay(query);   
            }
            //Aqui vamos hacer el AaNoTracking//
            if (! isTracking)
            {
                query = query.AsNoTracking();
            }

            return query.ToList();
        }

        //Este es para remover//
        public void Remover(T entidad)
        {
            dbSet.Remove(entidad);
        }

        // Este es para remover un rango//
        public void RemoverRango(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad);
        }
    }
}
