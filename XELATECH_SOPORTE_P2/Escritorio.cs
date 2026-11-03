using System;
using System.Collections.Generic;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class Escritorio : Equipo
    {
        protected string tipoTorre;
        protected string fuentePoder;

        public string TipoTorre
        {
            get { return tipoTorre; }
            set { tipoTorre = Validador.TextoObligatorio(value, "tipo de torre"); }
        }

        public string FuentePoder
        {
            get { return fuentePoder; }
            set { fuentePoder = Validador.TextoObligatorio(value, "fuente de poder"); }
        }

        public Escritorio(string codigo, string serie, string marca, string modelo, Cliente propietario,
        string estadoFisico, string accesorios, string torre, string fuente)
        : base(codigo, serie, marca, modelo, propietario, estadoFisico, accesorios)
        {
            TipoTorre = torre;
            FuentePoder = fuente;
        }

        public override decimal CalcularCostoRevision()
        {
            return 90;
        }

        public override string ObtenerTipo()
        {
            return "Escritorio";
        }

        public override Especialidad ObtenerEspecialidadRequerida()
        {
            return Especialidad.Computadoras;
        }

        public override string DiagnosticoInicialSugerido()
        {
            return "Revisar fuente de poder, memoria, almacenamiento y placa madre.";
        }
    }
}
