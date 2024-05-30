using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FaceRaceApp.DatasDB;
using FaceRaceApp.Models; // Asegúrate de incluir el espacio de nombres correcto

namespace FaceRaceApp.Controllers
{
    public class TurnoController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

        // GET: Turno
        public ActionResult Index()
        {
            return View();
        }

        // GET: Turno/CreateTurno
        public ActionResult CreateTurno()
        {
            return View();
        }

        // POST: Turno/CreateTurno
        [HttpPost]
        public ActionResult CreateTurno(string mes, string dia, string hora)
        {

            return View();
        }

        [HttpPost]
        public ActionResult GetCliente(string dni)
        {
            TurnoViewModel model = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ClienteId, Nombre, Apellido FROM Clientes WHERE DNI = @DNI";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DNI", dni);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model = new TurnoViewModel
                            {
                                ClienteId = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2)
                            };
                        }
                    }
                }
            }

            if (model != null)
            {
                return View("CreateTurno", model);
            }
            else
            {
                // Cliente no encontrado, mostrar mensaje de error
                ViewBag.ErrorMessage = "Cliente no encontrado";
                /*return View("CreateTurno", new TurnoViewModel());*/
                return RedirectToAction("Index","Cliente");
            }
        }


        // Método para obtener cliente por DNI


        // GET: Turno/TurnoToday
        public ActionResult TurnoToday()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TurnoViewModel model)
        {
            try
            {
                DateTime fecha = new DateTime(DateTime.Now.Year, model.Mes, model.Dia);
                TimeSpan horaTurno = TimeSpan.Parse(model.Hora);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Turnos (ClienteId, Fecha, Hora) VALUES (@ClienteId, @Fecha, @Hora)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClienteId", model.ClienteId);
                        command.Parameters.AddWithValue("@Fecha", fecha);
                        command.Parameters.AddWithValue("@Hora", horaTurno);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("CreateTurno");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return View("Error");
            }
        }


        [HttpGet]
        public JsonResult ObtenerHorariosDisponibles(int mes, int dia)
        {
            DateTime fecha = new DateTime(DateTime.Now.Year, mes, dia);
            var horariosDisponibles = GetAvailableHours(fecha);
            return Json(horariosDisponibles, JsonRequestBehavior.AllowGet);
        }


        private List<string> GetAvailableHours(DateTime fecha)
        {
            List<string> horariosDisponibles = new List<string>();
            for (int i = 9; i <= 17; i++)
            {
                horariosDisponibles.Add(i.ToString("D2") + ":00");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Hora FROM Turnos WHERE Fecha = @Fecha";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Fecha", fecha);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string hora = reader.GetTimeSpan(0).ToString(@"hh\:mm");
                            horariosDisponibles.Remove(hora);
                        }
                    }
                }
            }

            return horariosDisponibles;
        }



    }
}
