using System;
using System.Collections.Generic;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class Laptop : Equipo
    {
        protected decimal pulgadasPantalla;
        protected bool tieneBateria;

        public decimal PulgadasPantalla
        {
            get { return pulgadasPantalla; }
            set { pulgadasPantalla = Validador.NoNegativo(value, "pulgadas de pantalla"); }
        }

        public bool TieneBateria
        {
            get { return tieneBateria; }
            set { tieneBateria = value; }
        }

        public Laptop(string codigo, string serie, string marca, string modelo, Cliente propietario,
        string estadoFisico, string accesorios, decimal pulgadas, bool bateria)
        : base(codigo, serie, marca, modelo, propietario, estadoFisico, accesorios)
        {
            PulgadasPantalla = pulgadas;
            TieneBateria = bateria;
        }

        public override decimal CalcularCostoRevision()
        {
            return 100;
        }

        public override string ObtenerTipo()
        {
            return "Laptop";
        }

        public override Especialidad ObtenerEspecialidadRequerida()
        {
            return Especialidad.Computadoras;
        }

        public override string DiagnosticoInicialSugerido()
        {
            return "Revisar batería, cargador, pantalla y temperatura.";
        }
    }
}
