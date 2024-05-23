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

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}