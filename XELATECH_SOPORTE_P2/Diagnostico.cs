using System;
using System.Collections.Generic;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class Diagnostico
    {
        protected string descripcion;
        protected string accionesRealizadas;
        protected decimal tiempoEstimadoHoras;
        protected DateTime fecha;

        public string Descripcion
        {
            get { return descripcion; }
        }

        public string AccionesRealizadas
        {
            get { return accionesRealizadas; }
        }

        public decimal TiempoEstimadoHoras
        {
            get { return tiempoEstimadoHoras; }
        }

        public DateTime Fecha
        {
            get { return fecha; }
        }

        public Diagnostico(string descripcion, decimal horas)
        {
            this.descripcion = Validador.TextoObligatorio(descripcion, "diagnóstico");
            tiempoEstimadoHoras = Validador.NoNegativo(horas, "las horas estimadas");
            fecha = DateTime.Now;
            accionesRealizadas = "Sin acciones registradas";
        }

        public void ActualizarAcciones(string texto)
        {
            accionesRealizadas = Validador.TextoObligatorio(texto, "acciones realizadas");
        }
    }

    // Enumeraciones del sistema: valores fijos que usan varias clases.
    public enum Prioridad { Normal, Urgente }

    public enum EstadoSolicitud
    {
        Recibida,
        EnEspera,
        Asignada,
        EnDiagnostico,
        EnReparacion,
        ListaParaEntregar,
        Entregada,
        Cancelada
    }

    public enum TipoCliente { Estudiante, Profesional, Negocio, Institucion }

    public enum Especialidad { Computadoras, Impresoras }

}
