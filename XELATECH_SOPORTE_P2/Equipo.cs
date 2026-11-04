using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public abstract class Equipo
    {
        protected string codigoInterno;
        protected string numeroSerie;
        protected string marca;
        protected string modelo;
        protected Cliente propietario;
        protected string estadoFisico;
        protected string accesorios;

        public string CodigoInterno
        {
            get { return codigoInterno; }
        }

        public string NumeroSerie
        {
            get { return numeroSerie; }
            set { numeroSerie = Validador.TextoObligatorio(value, "número de serie").ToUpper(); }
        }

        public string Marca
        {
            get { return marca; }
            set { marca = Validador.TextoObligatorio(value, "marca"); }
        }

        public string Modelo
        {
            get { return modelo; }
            set { modelo = Validador.TextoObligatorio(value, "modelo"); }
        }

        public Cliente Propietario
        {
            get { return propietario; }
        }

        public string EstadoFisico
        {
            get { return estadoFisico; }
            set { estadoFisico = Validador.TextoObligatorio(value, "estado físico"); }
        }

        public string Accesorios
        {
            get { return accesorios; }
            set
            {
                if (value == null || value.Trim() == "")
                {
                    accesorios = "Ninguno";
                }
                else
                {
                    accesorios = value.Trim();
                }
            }
        }

        protected Equipo(string codigo, string serie, string marca, string modelo, Cliente propietario, string estadoFisico, string accesorios)
        {
            codigoInterno = Validador.CodigoConPrefijo(codigo, "EQ-", "código interno");
            NumeroSerie = serie;
            Marca = marca;
            Modelo = modelo;
            if (propietario == null)
            {
                throw new ValidacionException("El propietario debe ser un cliente registrado.");
            }
            this.propietario = propietario;
            EstadoFisico = estadoFisico;
            Accesorios = accesorios;
        }

        public abstract decimal CalcularCostoRevision();
        public abstract string ObtenerTipo();
        public abstract Especialidad ObtenerEspecialidadRequerida();

        public virtual string DiagnosticoInicialSugerido()
        {
            return "Revisión general del equipo y pruebas de funcionamiento.";
        }

        public string ObtenerResumen()
        {
            return codigoInterno + " | " + ObtenerTipo() + " | " + marca + " " + modelo
            + " | Serie: " + numeroSerie + " | Propietario: " + propietario.NombreCompleto;
        }
    }
}
