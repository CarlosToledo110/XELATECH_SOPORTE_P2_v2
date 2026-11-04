using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class GestorSolicitudes
    {
        private SistemaXT sistema;
        private GestorRegistros gestorRegistros;
        private GestorCarga gestorCarga;

        public GestorSolicitudes(SistemaXT sistema, GestorRegistros gestorRegistros, GestorCarga gestorCarga)
        {
            this.sistema = sistema;
            this.gestorRegistros = gestorRegistros;
            this.gestorCarga = gestorCarga;
        }

        public Solicitud BuscarCodigo(string codigo)
        {
            codigo = Validador.Normalizar(codigo);
            for (int i = 0; i < sistema.Solicitudes.Count; i++)
            {
                if (sistema.Solicitudes[i].Codigo == codigo)
                {
                    return sistema.Solicitudes[i];
                }
            }
            return null;
        }

        private Solicitud ObtenerSolicitud(string codigo)
        {
            Solicitud solicitud = BuscarCodigo(codigo);
            if (solicitud == null)
            {
                throw new ValidacionException("No se encontró una solicitud con ese código.");
            }
            return solicitud;
        }

        public void Crear(Solicitud solicitud)
        {
            if (BuscarCodigo(solicitud.Codigo) != null)
            {
                throw new ValidacionException("Ya existe una solicitud con ese código.");
            }
            if (solicitud.Equipo.Propietario != solicitud.Cliente)
            {
                throw new ValidacionException("El equipo no pertenece a ese cliente.");
            }
            for (int i = 0; i < sistema.Solicitudes.Count; i++)
            {
                Solicitud registrada = sistema.Solicitudes[i];
                if (registrada.Equipo == solicitud.Equipo && registrada.EstaActiva())
                {
                    throw new ValidacionException("El equipo ya tiene una solicitud activa (" + registrada.Codigo + ").");
                }
            }
            sistema.Solicitudes.Add(solicitud);
            solicitud.Cliente.IncrementarSolicitudes();
            Encolar(solicitud);
        }

        public void Encolar(Solicitud solicitud)
        {
            solicitud.CambiarEstado(EstadoSolicitud.EnEspera);
            if (solicitud.Prioridad == Prioridad.Urgente)
            {
                sistema.ColaUrgente.Enqueue(solicitud);
            }
            else
            {
                sistema.ColaNormal.Enqueue(solicitud);
            }
        }

        public Solicitud AtenderSiguiente()
        {
            if (sistema.ColaUrgente.Count == 0 && sistema.ColaNormal.Count == 0)
            {
                throw new ValidacionException("No hay solicitudes en espera.");
            }
            Solicitud siguiente;
            bool esUrgente;
            if (sistema.ColaUrgente.Count > 0)
            {
                siguiente = sistema.ColaUrgente.Peek();
                esUrgente = true;
            }
            else
            {
                siguiente = sistema.ColaNormal.Peek();
                esUrgente = false;
            }

            Tecnico tecnico = gestorRegistros.BuscarApto(siguiente.Equipo.ObtenerEspecialidadRequerida());
            if (tecnico == null)
            {
                throw new ValidacionException("No hay un técnico disponible con la especialidad requerida. La solicitud "
                + siguiente.Codigo + " conserva su lugar en la cola.");
            }

            if (esUrgente)
            {
                sistema.ColaUrgente.Dequeue();
            }
            else
            {
                sistema.ColaNormal.Dequeue();
            }
            siguiente.AsignarTecnico(tecnico);
            siguiente.CambiarEstado(EstadoSolicitud.Asignada);
            gestorCarga.RegistrarCarga(tecnico, DateTime.Now);
            return siguiente;
        }

        private void MostrarCola(Queue<Solicitud> cola)
        {
            if (cola.Count == 0)
            {
                Console.WriteLine(" (vacía)");
                return;
            }
            int total = cola.Count;
            for (int i = 0; i < total; i++)
            {
                Solicitud actual = cola.Dequeue();
                Console.WriteLine(" " + (i + 1) + ". " + actual.ObtenerResumen());
                cola.Enqueue(actual);
            }
        }

        public void MostrarColas()
        {
            Console.WriteLine("Cola URGENTE (" + sistema.ColaUrgente.Count + "):");
            MostrarCola(sistema.ColaUrgente);
            Console.WriteLine("Cola NORMAL (" + sistema.ColaNormal.Count + "):");
            MostrarCola(sistema.ColaNormal);
        }

        private void QuitarDeCola(Queue<Solicitud> cola, Solicitud solicitud)
        {
            int total = cola.Count;
            for (int i = 0; i < total; i++)
            {
                Solicitud actual = cola.Dequeue();
                if (actual != solicitud)
                {
                    cola.Enqueue(actual);
                }
            }
        }

        public void CambiarEstado(string codigo, EstadoSolicitud nuevo)
        {
            Solicitud solicitud = ObtenerSolicitud(codigo);
            EstadoSolicitud anterior = solicitud.Estado;
            solicitud.CambiarEstado(nuevo);

            if (nuevo == EstadoSolicitud.Cancelada)
            {
                if (anterior == EstadoSolicitud.EnEspera)
                {
                    if (solicitud.Prioridad == Prioridad.Urgente)
                    {
                        QuitarDeCola(sistema.ColaUrgente, solicitud);
                    }
                    else
                    {
                        QuitarDeCola(sistema.ColaNormal, solicitud);
                    }
                }
                if (solicitud.Tecnico != null)
                {
                    solicitud.Tecnico.LiberarTrabajoCancelado();
                }
            }
            if (nuevo == EstadoSolicitud.Entregada && solicitud.Tecnico != null)
            {
                solicitud.Tecnico.FinalizarTrabajo();
            }
        }

        public void Deshacer(string codigo)
        {
            Solicitud solicitud = ObtenerSolicitud(codigo);
            if (solicitud.Estado == EstadoSolicitud.EnEspera)
            {
                throw new ValidacionException("La solicitud está en espera: no hay un cambio manual que deshacer.");
            }
            EstadoSolicitud anterior = solicitud.Estado;
            solicitud.DeshacerUltimoCambio();

            if (anterior == EstadoSolicitud.Asignada && solicitud.Estado == EstadoSolicitud.EnEspera)
            {
                solicitud.DesasignarTecnico();
                if (solicitud.Prioridad == Prioridad.Urgente)
                {
                    sistema.ColaUrgente.Enqueue(solicitud);
                }
                else
                {
                    sistema.ColaNormal.Enqueue(solicitud);
                }
            }
        }

        public string ConsultarHistorial(string codigo)
        {
            return ObtenerSolicitud(codigo).ConsultarHistorial();
        }

        public void RegistrarDiagnostico(string codigo, string descripcion, decimal horas)
        {
            ObtenerSolicitud(codigo).RegistrarDiagnostico(descripcion, horas);
        }

        public void AgregarRepuesto(string codigoSolicitud, Repuesto repuesto, int cantidad)
        {
            ObtenerSolicitud(codigoSolicitud).AgregarRepuesto(repuesto, cantidad);
        }

        public decimal Entregar(string codigo)
        {
            Solicitud solicitud = ObtenerSolicitud(codigo);
            if (solicitud.Estado != EstadoSolicitud.ListaParaEntregar)
            {
                throw new ValidacionException("La solicitud debe estar Lista para entregar antes de entregarse.");
            }
            decimal total = solicitud.CalcularTotal();
            CambiarEstado(codigo, EstadoSolicitud.Entregada);
            return total;
        }

        public void Calificar(string codigo, int nota)
        {
            ObtenerSolicitud(codigo).Calificar(nota);
            sistema.RegistrarCalificacion(nota);
        }
    }

}
