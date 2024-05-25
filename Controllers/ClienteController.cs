using FaceRaceApp.DatasDB;
using FaceRaceApp.Models;
using System.Collections.Generic;
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
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Cliente/ListClient
        [HttpGet]
        public ActionResult ListClient()
        {
            ClienteData data = new ClienteData();
            List<ClienteModel> list = data.GetAllClientes();
            return View(list);
        }

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

            return RedirectToAction("ListClient"); ;
        }




    }
}
