using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control_de_Inventario.Entindades
{
    public class Inventarioo
    {
        public int Id { get; set; }
        public string Producto { get; set; }
        public string Categoria { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioCompra { get; set; }

        // Propiedad calculada: valor total por producto
        public decimal ValorTotal => Cantidad * PrecioCompra;
    }
}
