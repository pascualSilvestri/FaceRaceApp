using FaceRaceApp.DatasDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
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
        public ActionResult Index(Models.ClienteModel model)
        {
            if (ModelState.IsValid)
            {
                // Guardar los datos en la base de datos
                ClienteData data = new ClienteData();
                data.InsertCliente(model);

                // Redirigir a la misma acción para limpiar el formulario
                return RedirectToAction("Index");
            }

            // Si hay errores, volver a mostrar el formulario con los datos actuales
            return View(model);
        }

        // GET: Cliente/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cliente/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
