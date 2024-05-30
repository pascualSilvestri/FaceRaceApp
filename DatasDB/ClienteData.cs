using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FaceRaceApp.Models;


namespace FaceRaceApp.DatasDB
{
    public class ClienteData
    {
        private static List<ClienteModel> clientes = new List<ClienteModel>();

        public void InsertCliente(ClienteModel model)
        {
            model.Id = clientes.Count > 0 ? clientes.Max(c => c.Id) + 1 : 1;
            clientes.Add(model);
        }

        public ClienteModel GetClienteById(int id)
        {
            return clientes.FirstOrDefault(c => c.Id == id);
        }

        public void UpdateCliente(int id, ClienteModel updatedCliente)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                cliente.Nombre = updatedCliente.Nombre;
                cliente.Apellido = updatedCliente.Apellido;
                cliente.DNI = updatedCliente.DNI;
                cliente.Telefono = updatedCliente.Telefono;
                cliente.Correo = updatedCliente.Correo;
            }
        }

        public void DeleteCliente(int id)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                clientes.Remove(cliente);
            }
        }

        public List<ClienteModel> GetAllClientes()
        {
            return clientes;
        }
    }
}