using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionColorApp.Models
{
    // Representa les classes DTO
    public class Color
    {
        public string Id { get; set; }
        public string Rgb { get; set; }
        public Traduccio Traduccio { get; set;  }
    }

    public class Traduccio
    {
        public string Idioma { get; set;  }
        public string Paraula { get; set; }
    }
}
