using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionColorApp.Models
{
    /// <summary>
    /// Representa les classes DTO
    /// El client sempre interactua a través d'objectes d'aquest tipus
    /// </summary>
    public class Color
    {
        public string Id { get; set; }
        public string Rgb { get; set; }
        public Traduccio Traduccio { get; set;  }
    }

    // Innecessari, ho sé, només per fer les coses una mica més complexes
    public class Traduccio
    {
        public string Idioma { get; set;  }
        public string Paraula { get; set; }
    }
}
