using FaceRaceApp.DatasDB;
using FaceRaceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FaceRaceApp.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            return View();
        }

        // POST: Cliente
        [HttpPost]
        public ActionResult Index(ClienteModel model)
        {
            if (ModelState.IsValid)
            {
                ClienteData data = new ClienteData();
                data.InsertCliente(model);

                // Almacenar el mensaje de éxito en TempData
                TempData["SuccessMessage"] = "Cliente creado correctamente.";

                // Almacenar los datos del cliente en TempData para mostrar en la vista
                TempData["ClienteData"] = $"Nombre: {model.Nombre}, Apellido: {model.Apellido}, DNI: {model.DNI}, Teléfono: {model.Telefono}, Correo: {model.Correo}";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Cliente/ListClient

        // GET: Cliente/ListClient
        [HttpGet]
        public ActionResult ListClient(string nombre = null, string dni = null)
        {
            ClienteData data = new ClienteData();
            List<ClienteModel> list = data.GetAllClientes();

            if (!string.IsNullOrEmpty(nombre))
            {
                list = list.Where(c => c.Nombre.IndexOf(nombre, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            if (!string.IsNullOrEmpty(dni))
            {
                list = list.Where(c => c.DNI.Contains(dni)).ToList();
            }

            return View(list);
        }
        /*[HttpGet]
        public ActionResult ListClient()
        {
            ClienteData data = new ClienteData();
            List<ClienteModel> list = data.GetAllClientes();
            return View(list);
        }*/

        // POST: Cliente/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            ClienteData data = new ClienteData();
            data.EliminarCliente(id);
            return RedirectToAction("ListClient");
        }

        [HttpPost]
        public ActionResult Modificar(ClienteModel model)
        {
            // Imprime el modelo en la consola de depuración
            System.Diagnostics.Debug.WriteLine(model.ClienteId);

            if (ModelState.IsValid)
            {
                ClienteData data = new ClienteData();
                data.ActualizarCliente(model);
                return RedirectToAction("ListClient");
            }

            return RedirectToAction("ListClient");
        }

        // POST: Cliente/EliminarCliente
        [HttpPost]
        public ActionResult EliminarCliente(int clienteId)
        {
            try
            {
                ClienteData data = new ClienteData();
                data.EliminarCliente(clienteId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
