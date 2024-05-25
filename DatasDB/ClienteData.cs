using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FaceRaceApp.DatasDB
{
    public class ClienteData
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

        public void InsertCliente(Models.ClienteModel cliente)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Clientes (Nombre, Apellido, DNI, Telefono, Correo) VALUES (@Nombre, @Apellido, @DNI, @Telefono, @Correo)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    command.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                    command.Parameters.AddWithValue("@DNI", cliente.DNI);
                    command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                    command.Parameters.AddWithValue("@Correo", cliente.Correo);
                    command.Parameters.AddWithValue("@IsDeleted", false);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Models.ClienteModel> GetAllClientes()
        {
            List<Models.ClienteModel> clientes = new List<Models.ClienteModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Clientes WHERE IsDeleted = 0";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Models.ClienteModel cliente = new Models.ClienteModel();
                            cliente.ClienteId = reader["ClienteId"].ToString();
                            cliente.Nombre = reader["Nombre"].ToString();
                            cliente.Apellido = reader["Apellido"].ToString();
                            cliente.DNI = reader["DNI"].ToString();
                            cliente.Telefono = reader["Telefono"].ToString();
                            cliente.Correo = reader["Correo"].ToString();
                            cliente.isDelete = (bool)reader["IsDeleted"];
                            clientes.Add(cliente);
                        }
                    }
                }
            }

            return clientes;
        }



        public bool EliminarCliente(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Clientes SET IsDeleted = 1 WHERE ClienteId = @ClienteId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClienteId", id);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }

        public bool ActualizarCliente(Models.ClienteModel cliente)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Clientes SET Nombre = @Nombre, Apellido = @Apellido, DNI = @DNI, Telefono = @Telefono, Correo = @Correo WHERE ClienteId = @ClienteId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    command.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                    command.Parameters.AddWithValue("@DNI", cliente.DNI);
                    command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                    command.Parameters.AddWithValue("@Correo", cliente.Correo);
                    command.Parameters.AddWithValue("@ClienteId", cliente.ClienteId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }



    }


}
