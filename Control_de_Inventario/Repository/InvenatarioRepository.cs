using Control_de_Inventario.Entindades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control_de_Inventario.Repository
{
    public class InvenatarioRepository
    {
        public void Registrar(Inventarioo producto)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ConnectionString))
            {
                string query = @"INSERT INTO Inventario (Producto, Categoria, Cantidad, PrecioCompra) 
                                VALUES (@Producto, @Categoria, @Cantidad, @PrecioCompra)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Producto", producto.Producto);
                cmd.Parameters.AddWithValue("@Categoria", producto.Categoria);
                cmd.Parameters.AddWithValue("@Cantidad", producto.Cantidad);
                cmd.Parameters.AddWithValue("@PrecioCompra", producto.PrecioCompra);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Obtener todos los productos
        public List<Inventarioo> ObtenerTodos()
        {
            List<Inventarioo> productos = new List<Inventarioo>();

            using (SqlConnection conn = new SqlConnection(Conexion.ConnectionString))
            {
                string query = "SELECT * FROM Inventario ORDER BY Id DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productos.Add(new Inventarioo() 
                    {
                        Id = (int)reader["Id"],
                        Producto = reader["Producto"].ToString(),
                        Categoria = reader["Categoria"].ToString(),
                        Cantidad = (int)reader["Cantidad"],
                        PrecioCompra = (decimal)reader["PrecioCompra"]
                    });
                }
            }
            return productos;
        }

        // Mostrar productos con stock menor a 5
        public List<Inventarioo> ObtenerProductosStockBajo()
        {
            List<Inventarioo> productos = new List<Inventarioo>();

            using (SqlConnection conn = new SqlConnection(Conexion.ConnectionString))
            {
                string query = "SELECT * FROM InventarioDB WHERE Cantidad < 5 ORDER BY Cantidad ASC";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productos.Add(new Inventarioo
                    {
                        Id = (int)reader["Id"],
                        Producto = reader["Producto"].ToString(),
                        Categoria = reader["Categoria"].ToString(),
                        Cantidad = (int)reader["Cantidad"],
                        PrecioCompra = (decimal)reader["PrecioCompra"]
                    });
                }
            }
            return productos;
        }

        // Mostrar valor total del inventario (suma de todos los productos: Cantidad * PrecioCompra)
        public decimal ObtenerValorTotalInventario()
        {
            decimal valorTotal = 0;

            using (SqlConnection conn = new SqlConnection(Conexion.ConnectionString))
            {
                string query = "SELECT SUM(Cantidad * PrecioCompra) AS ValorTotal FROM InventarioDB";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    valorTotal = Convert.ToDecimal(result);
                }
            }
            return valorTotal;
        }

        // Buscar productos por categoría
        public List<Inventarioo> BuscarPorCategoria(string categoria)
        {
            List<Inventarioo> productos = new List<Inventarioo>();

            using (SqlConnection conn = new SqlConnection(Conexion.ConnectionString))
            {
                string query = "SELECT * FROM InventarioDB WHERE Categoria LIKE @Categoria";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Categoria", "%" + categoria + "%");
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    productos.Add(new Inventarioo()
                    {
                        Id = (int)reader["Id"],
                        Producto = reader["Producto"].ToString(),
                        Categoria = reader["Categoria"].ToString(),
                        Cantidad = (int)reader["Cantidad"],
                        PrecioCompra = (decimal)reader["PrecioCompra"]
                    });
                }
            }
            return productos;
        }

        // Actualizar producto
        public void Actualizar(Inventarioo producto)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ConnectionString))
            {
                string query = @"UPDATE InventarioDB 
                                SET Producto = @Producto, 
                                    Categoria = @Categoria, 
                                    Cantidad = @Cantidad, 
                                    PrecioCompra = @PrecioCompra 
                                WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", producto.Id);
                cmd.Parameters.AddWithValue("@Producto", producto.Producto);
                cmd.Parameters.AddWithValue("@Categoria", producto.Categoria);
                cmd.Parameters.AddWithValue("@Cantidad", producto.Cantidad);
                cmd.Parameters.AddWithValue("@PrecioCompra", producto.PrecioCompra);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Eliminar producto
        public void Eliminar(int id)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ConnectionString))
            {
                string query = "DELETE FROM Inventario WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Obtener producto por ID
        public Inventarioo ObtenerPorId(int id)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.ConnectionString))
            {
                string query = "SELECT * FROM Inventario WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Inventarioo
                    {
                        Id = (int)reader["Id"],
                        Producto = reader["Producto"].ToString(),
                        Categoria = reader["Categoria"].ToString(),
                        Cantidad = (int)reader["Cantidad"],
                        PrecioCompra = (decimal)reader["PrecioCompra"]
                    };
                }
                return null;
            }
        }
    }
}
