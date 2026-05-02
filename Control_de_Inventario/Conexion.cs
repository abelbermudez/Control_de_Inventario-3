using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control_de_Inventario
{
    public class Conexion
    {
        public static readonly string ConnectionString =
             "Data Source=LAPTOP-PF0SV0IO\\SQLEXPRESS;Database= InventarioDB;Trusted_Connection=True;";


        public static SqlConnection cn = new SqlConnection(ConnectionString);

        public static SqlConnection GetOpenConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
