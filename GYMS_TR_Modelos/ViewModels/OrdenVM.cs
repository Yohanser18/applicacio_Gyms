using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMS_TR_Modelos.ViewModels
{
    // Aquí puedes agregar propiedades específicas para la vista de órdenes//
    public class OrdenVM
    {
        public Orden Orden  { get; set; }

        public IEnumerable<OrdenDetalle> OrdenDetalle { get; set; }
    }
}
