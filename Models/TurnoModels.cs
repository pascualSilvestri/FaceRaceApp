using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceRaceApp.Models
{
    public class TurnoViewModel
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; }
        public string Hora { get; set; }
    }


}