using FaceRaceApp.DatasDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using FaceRaceApp.Models;

namespace FaceRaceApp.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            ClienteData data = new ClienteData();
            var clientes = data.GetAllClientes();
            return View(clientes);
        }

        // POST: Cliente
        [HttpPost]
        public ActionResult Index(ClienteModel model)
        {
            if (ModelState.IsValid)
            {
                ClienteData data = new ClienteData();
                data.InsertCliente(model);

                TempData["ClienteData"] = "Cliente creado correctamente.";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Cliente/Details/5
        public ActionResult Details(int id)
        {
            ClienteData data = new ClienteData();
            ClienteModel cliente = data.GetClienteById(id);

            if (cliente == null)
            {
                return HttpNotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        public ActionResult Create(ClienteModel model)
        {
            if (ModelState.IsValid)
            {
                ClienteData data = new ClienteData();
                data.InsertCliente(model);

                TempData["ClienteData"] = "Cliente creado correctamente.";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int id)
        {
            ClienteData data = new ClienteData();
            ClienteModel cliente = data.GetClienteById(id);

            if (cliente == null)
            {
                return HttpNotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ClienteModel model)
        {
            if (ModelState.IsValid)
            {
                ClienteData data = new ClienteData();
                data.UpdateCliente(id, model);

                TempData["ClienteData"] = "Cliente editado correctamente.";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int id)
        {
            ClienteData data = new ClienteData();
            ClienteModel cliente = data.GetClienteById(id);

            if (cliente == null)
            {
                return HttpNotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ClienteData data = new ClienteData();
                data.DeleteCliente(id);

                TempData["ClienteData"] = "Cliente eliminado correctamente.";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

      
    }
}