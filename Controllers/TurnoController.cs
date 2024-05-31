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
        public ActionResult Index(string dni, DateTime? fecha)
        {
            List<TurnoViewModel> turnos = GetTurnos(dni, fecha);
            return View(turnos);
        }

        private List<TurnoViewModel> GetTurnosHoy()
        {
            List<TurnoViewModel> turnos = new List<TurnoViewModel>();
            DateTime fechaHoy = DateTime.Today;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT t.TurnoId, c.Nombre, c.Apellido, t.Fecha, t.Hora
                         FROM Turnos t
                         INNER JOIN Clientes c ON t.ClienteId = c.ClienteId
                         WHERE CAST(t.Fecha AS DATE) = @FechaHoy";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FechaHoy", fechaHoy);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            turnos.Add(new TurnoViewModel
                            {
                                TurnoId = reader.GetInt32(0),
                                ClienteId = GetClienteId(reader.GetString(1), reader.GetString(2)),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                Mes = reader.GetDateTime(3).Month,
                                Dia = reader.GetDateTime(3).Day,
                                Hora = reader.GetTimeSpan(4).ToString(@"hh\:mm")
                            });
                        }
                    }
                }
            }

            return turnos;
        }

        private List<TurnoViewModel> GetTurnos(string dni, DateTime? fecha)
        {
            List<TurnoViewModel> turnos = new List<TurnoViewModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT t.TurnoId, c.Nombre, c.Apellido, t.Fecha, t.Hora
                         FROM Turnos t
                         INNER JOIN Clientes c ON t.ClienteId = c.ClienteId
                         WHERE (@DNI IS NULL OR c.DNI = @DNI)
                           AND (@Fecha IS NULL OR CAST(t.Fecha AS DATE) = @Fecha)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(dni))
                        command.Parameters.AddWithValue("@DNI", dni);
                    else
                        command.Parameters.AddWithValue("@DNI", DBNull.Value);

                    if (fecha.HasValue)
                        command.Parameters.AddWithValue("@Fecha", fecha.Value.Date);
                    else
                        command.Parameters.AddWithValue("@Fecha", DBNull.Value);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            turnos.Add(new TurnoViewModel
                            {
                                TurnoId = reader.GetInt32(0),
                                ClienteId = GetClienteId(reader.GetString(1), reader.GetString(2)),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                Mes = reader.GetDateTime(3).Month,
                                Dia = reader.GetDateTime(3).Day,
                                Hora = reader.GetTimeSpan(4).ToString(@"hh\:mm")
                            });
                        }
                    }
                }
            }

            return turnos;
        }

        private int GetClienteId(string nombre, string apellido)
        {
            // Lógica para obtener el ClienteId a partir del nombre y apellido
            // Puedes simplemente retornar un valor constante por ahora
            return 1;
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
                return RedirectToAction("Index", "Cliente");
            }
        }


        // Método para obtener cliente por DNI


        // GET: Turno/TurnoToday
        public ActionResult TurnoToday()
        {
            List<TurnoViewModel> turnosHoy = GetTurnosHoy();
            return View(turnosHoy);
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

      /*  [HttpGet]
        public JsonResult ObtenerHorariosDisponibles(int mes, int dia, int turnoId)
        {
            DateTime fecha = new DateTime(DateTime.Now.Year, mes, dia);
            var horariosDisponibles = GetAvailableHours(fecha, turnoId);
            return Json(horariosDisponibles, JsonRequestBehavior.AllowGet);
        }*/

        /*private List<string> GetAvailableHours(DateTime fecha, int turnoId)
        {
            List<string> horariosDisponibles = new List<string>();
            for (int i = 9; i <= 17; i++)
            {
                horariosDisponibles.Add(i.ToString("D2") + ":00");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Hora FROM Turnos WHERE Fecha = @Fecha AND TurnoId != @TurnoId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Fecha", fecha);
                    command.Parameters.AddWithValue("@TurnoId", turnoId);
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
*/

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

        // GET: Turno/ModificarTurno/id
        public ActionResult ModificarTurno(int turnoId)
        {
            // Obtener los datos del turno por su ID
            TurnoViewModel turno = GetTurnoById(turnoId);

            if (turno != null)
            {
                return View(turno);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Turno/ModificarTurno
        [HttpPost]
        public ActionResult ModificarTurno(TurnoViewModel turno)
        {
            if (ModelState.IsValid)
            {
                // Actualizar el turno en la base de datos
                bool resultado = ActualizarTurno(turno);

                if (resultado)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Manejar el error
                    return View(turno);
                }
            }

            return View(turno);
        }


        private TurnoViewModel GetTurnoById(int id)
        {
            // Implementar la lógica para obtener los datos del turno por su ID
            // y llenar un objeto TurnoViewModel con esos datos
            return new TurnoViewModel
            {
                TurnoId = id,
                ClienteId = 1,
                Nombre = "Nombre de Prueba",
                Apellido = "Apellido de Prueba",
                Mes = 6,
                Dia = 15,
                Hora = "10:00"
            };
        }

        private bool ActualizarTurno(TurnoViewModel turno)
        {
            // Implementar la lógica para actualizar el turno en la base de datos
            return true;
        }



        [HttpPost]
        public ActionResult EliminarTurno(int turnoId)
        {
            try
            {
                Debug.WriteLine("Received TurnoId: " + turnoId);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Turnos WHERE TurnoId = @TurnoId";
                    Debug.WriteLine("Conexión a la base de datos establecida.");
                    Debug.WriteLine("Query: " + query);

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TurnoId", turnoId);
                        Debug.WriteLine("TurnoId Parameter: " + turnoId);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        Debug.WriteLine("Rows Affected: " + rowsAffected);

                        if (rowsAffected > 0)
                        {
                            return Json(new { success = true });
                        }
                        else
                        {
                            // Si no se afectaron filas, el turno no se encontró
                            Debug.WriteLine("El turno no fue encontrado.");
                            return Json(new { success = false, message = "El turno no fue encontrado." });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
