using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceRaceApp.Models
{
    public class ClienteModel
    {
        public string ClienteId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public bool isDelete { get; set; } 
    }
}