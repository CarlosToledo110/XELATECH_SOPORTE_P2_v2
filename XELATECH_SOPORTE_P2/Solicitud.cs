using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class Solicitud
    {
        public const int TARIFA_HORA = 60;

        protected string codigo;
        protected Cliente cliente;
        protected Equipo equipo;
        protected string descripcion;
        protected DateTime fechaRecepcion;
        protected Prioridad prioridad;
        protected EstadoSolicitud estado;
        protected Tecnico tecnico;
        protected Diagnostico diagnostico;
        protected List<DetalleRepuesto> repuestos = new List<DetalleRepuesto>();
        protected decimal horasManoObra;
        protected int calificacion;

        protected Stack<EstadoSolicitud> historial = new Stack<EstadoSolicitud>();

        public string Codigo
        {
            get { return codigo; }
        }

        public Cliente Cliente
        {
            get { return cliente; }
        }

        public Equipo Equipo
        {
            get { return equipo; }
        }

        public string Descripcion
        {
            get { return descripcion; }
        }

        public DateTime FechaRecepcion
        {
            get { return fechaRecepcion; }
        }

        public Prioridad Prioridad
        {
            get { return prioridad; }
        }

        public EstadoSolicitud Estado
        {
            get { return estado; }
        }

        public Tecnico Tecnico
        {
            get { return tecnico; }
        }

        public Diagnostico Diagnostico
        {
            get { return diagnostico; }
        }

        public int Calificacion
        {
            get { return calificacion; }
        }

        public decimal CostoManoObra
        {
            get { return horasManoObra * TARIFA_HORA; }
        }

        public decimal CostoRepuestos
        {
            get
            {
                decimal total = 0;
                for (int i = 0; i < repuestos.Count; i++)
                {
                    total = total + repuestos[i].Subtotal();
                }
                return total;
            }
        }

        public decimal CostoRevision
        {
            get { return equipo.CalcularCostoRevision(); }
        }

        public Solicitud(string codigo, Cliente cliente, Equipo equipo, string descripcion, Prioridad prioridad)
        {
            this.codigo = Validador.CodigoConPrefijo(codigo, "SOL-", "código de solicitud");
            if (cliente == null)
            {
                throw new ValidacionException("No se puede crear la solicitud: el cliente no existe.");
            }
            if (equipo == null)
            {
                throw new ValidacionException("No se puede crear la solicitud: el equipo no existe.");
            }
            this.cliente = cliente;
            this.equipo = equipo;
            this.descripcion = Validador.TextoObligatorio(descripcion, "descripción del problema");
            this.prioridad = prioridad;
            fechaRecepcion = DateTime.Now;
            estado = EstadoSolicitud.Recibida;
        }

        public bool EstaActiva()
        {
            if (estado == EstadoSolicitud.Entregada || estado == EstadoSolicitud.Cancelada)
            {
                return false;
            }
            return true;
        }

        private bool CambioPermitido(EstadoSolicitud nuevo)
        {
            if (nuevo == EstadoSolicitud.Cancelada)
            {
                return true;
            }
            if (estado == EstadoSolicitud.Recibida && nuevo == EstadoSolicitud.EnEspera)
            {
                return true;
            }
            if (estado == EstadoSolicitud.EnEspera && nuevo == EstadoSolicitud.Asignada)
            {
                return true;
            }
            if (estado == EstadoSolicitud.Asignada && nuevo == EstadoSolicitud.EnDiagnostico)
            {
                return true;
            }
            if (estado == EstadoSolicitud.EnDiagnostico && nuevo == EstadoSolicitud.EnReparacion)
            {
                return true;
            }
            if (estado == EstadoSolicitud.EnReparacion && nuevo == EstadoSolicitud.ListaParaEntregar)
            {
                return true;
            }
            if (estado == EstadoSolicitud.ListaParaEntregar && nuevo == EstadoSolicitud.Entregada)
            {
                return true;
            }
            return false;
        }

        public void CambiarEstado(EstadoSolicitud nuevo)
        {
            if (!EstaActiva())
            {
                throw new ValidacionException("La solicitud ya fue entregada o cancelada y no se puede modificar.");
            }
            if (!CambioPermitido(nuevo))
            {
                throw new ValidacionException("Cambio de estado no permitido: de " + estado + " no se puede pasar a " + nuevo + ".");
            }
            historial.Push(estado);
            estado = nuevo;
        }

        public void DeshacerUltimoCambio()
        {
            if (!EstaActiva())
            {
                throw new ValidacionException("No se puede deshacer: la solicitud ya fue entregada o cancelada.");
            }
            if (historial.Count == 0)
            {
                throw new ValidacionException("No hay cambios de estado para deshacer.");
            }
            estado = historial.Pop();
        }

        public string ConsultarHistorial()
        {
            if (historial.Count == 0)
            {
                return "Sin cambios de estado registrados.";
            }
            string texto = "Estados anteriores (del más reciente al más antiguo):\n";
            Stack<EstadoSolicitud> temporal = new Stack<EstadoSolicitud>();
            while (historial.Count > 0)
            {
                EstadoSolicitud anterior = historial.Pop();
                texto = texto + "- " + anterior + "\n";
                temporal.Push(anterior);
            }
            while (temporal.Count > 0)
            {
                historial.Push(temporal.Pop());
            }
            return texto + "Estado actual: " + estado;
        }

        public void AsignarTecnico(Tecnico nuevoTecnico)
        {
            if (nuevoTecnico == null)
            {
                throw new ValidacionException("El técnico no existe.");
            }
            if (nuevoTecnico.Especialidad != equipo.ObtenerEspecialidadRequerida())
            {
                throw new ValidacionException("El técnico no tiene la especialidad requerida para este equipo.");
            }
            nuevoTecnico.AsignarTrabajo();
            tecnico = nuevoTecnico;
        }

        public void DesasignarTecnico()
        {
            if (tecnico != null)
            {
                tecnico.LiberarTrabajoCancelado();
                tecnico = null;
            }
        }

        public void RegistrarDiagnostico(string textoDiagnostico, decimal horas)
        {
            if (estado != EstadoSolicitud.EnDiagnostico)
            {
                throw new ValidacionException("La solicitud debe estar En diagnóstico para registrar el diagnóstico.");
            }
            diagnostico = new Diagnostico(textoDiagnostico, horas);
            horasManoObra = horas;
        }

        public void AgregarRepuesto(Repuesto repuesto, int cantidad)
        {
            if (estado != EstadoSolicitud.EnReparacion)
            {
                throw new ValidacionException("Solo se pueden usar repuestos cuando la solicitud está En reparación.");
            }
            if (repuesto == null)
            {
                throw new ValidacionException("El repuesto no existe.");
            }
            repuesto.Descontar(cantidad);
            DetalleRepuesto existente = null;
            for (int i = 0; i < repuestos.Count; i++)
            {
                if (repuestos[i].Repuesto.Codigo == repuesto.Codigo)
                {
                    existente = repuestos[i];
                    break;
                }
            }
            if (existente == null)
            {
                repuestos.Add(new DetalleRepuesto(repuesto, cantidad));
            }
            else
            {
                existente.SumarCantidad(cantidad);
            }
        }

        public decimal CalcularTotal()
        {
            if (diagnostico == null)
            {
                throw new ValidacionException("Primero se debe registrar el diagnóstico y las horas de trabajo.");
            }
            return CostoRevision + CostoManoObra + CostoRepuestos;
        }

        public decimal ObtenerTotalActual()
        {
            if (diagnostico == null)
            {
                return 0;
            }
            return CalcularTotal();
        }

        public int NumeroCodigo()
        {
            return int.Parse(codigo.Substring(4));
        }

        public void Calificar(int nota)
        {
            if (estado != EstadoSolicitud.Entregada)
            {
                throw new ValidacionException("Solo se puede calificar una solicitud entregada.");
            }
            if (calificacion != 0)
            {
                throw new ValidacionException("Esta solicitud ya fue calificada.");
            }
            if (nota < 1 || nota > 5)
            {
                throw new ValidacionException("La calificación debe estar entre 1 y 5.");
            }
            calificacion = nota;
            if (tecnico != null)
            {
                tecnico.RegistrarCalificacion(nota);
            }
        }

        public string ObtenerResumen()
        {
            string nombreTecnico = "Sin técnico";
            if (tecnico != null)
            {
                nombreTecnico = tecnico.NombreCompleto;
            }
            return codigo + " | " + estado + " | " + prioridad + " | " + cliente.NombreCompleto
            + " | " + equipo.ObtenerTipo() + " | Técnico: " + nombreTecnico;
        }
    }
}
