using System;
using System.Collections.Generic;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class Persona
    {
        protected string codigo;
        protected string nombreCompleto;

        public string Codigo
        {
            get { return codigo; }
            set { codigo = Validador.TextoObligatorio(value, "código"); }
        }

        public string NombreCompleto
        {
            get { return nombreCompleto; }
            set { nombreCompleto = Validador.TextoObligatorio(value, "nombre"); }
        }

        protected Persona(string codigo, string nombre)
        {
            Codigo = codigo;
            NombreCompleto = nombre;
        }

        public virtual string ObtenerResumen()
        {
            return codigo + " - " + nombreCompleto;
        }

    }
}
