using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FaceRaceApp.Models
{
    public class TurnoViewModel
    {
        public int TurnoId { get; set; }

        public int ClienteId { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public int Mes { get; set; }

        [Required]
        public int Dia { get; set; }

        [Required]
        public string Hora { get; set; }
    }


}