using System;
using System.Collections.Generic;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class Impresora : Equipo
    {
        protected string tecnologia;
        protected int contadorPaginas;

        public string Tecnologia
        {
            get { return tecnologia; }
            set { tecnologia = Validador.TextoObligatorio(value, "tecnología"); }
        }

        public int ContadorPaginas
        {
            get { return contadorPaginas; }
            set { contadorPaginas = Validador.NoNegativo(value, "contador de páginas"); }
        }

        public Impresora(string codigo, string serie, string marca, string modelo, Cliente propietario,
        string estadoFisico, string accesorios, string tecnologia, int paginas)
        : base(codigo, serie, marca, modelo, propietario, estadoFisico, accesorios)
        {
            Tecnologia = tecnologia;
            ContadorPaginas = paginas;
        }

        public override decimal CalcularCostoRevision()
        {
            return 75;
        }

        public override string ObtenerTipo()
        {
            return "Impresora";
        }

        public override Especialidad ObtenerEspecialidadRequerida()
        {
            return Especialidad.Impresoras;
        }

        public override string DiagnosticoInicialSugerido()
        {
            return "Revisar cabezales, rodillos, cartuchos y conexión.";
        }
    }
}
