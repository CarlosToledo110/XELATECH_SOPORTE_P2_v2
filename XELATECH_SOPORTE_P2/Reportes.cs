using System;
using System.Collections.Generic;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class Reportes
    {
        private SistemaXT sistema;
        private GestorSolicitudes gestorSolicitudes;
        private GestorCarga gestorCarga;

        public Reportes(SistemaXT sistema, GestorSolicitudes gestorSolicitudes, GestorCarga gestorCarga)
        {
            this.sistema = sistema;
            this.gestorSolicitudes = gestorSolicitudes;
            this.gestorCarga = gestorCarga;
        }

        public static string Dinero(decimal valor)
        {
            return "Q" + Math.Round(valor, 2);
        }

        private string Sentido(bool ascendente)
        {
            if (ascendente)
            {
                return "Ascendente";
            }
            return "Descendente";
        }

        public void SolicitudesEnEspera()
        {
            Console.WriteLine("--- Solicitudes en espera separadas por prioridad ---");
            gestorSolicitudes.MostrarColas();
        }

        public void SolicitudesPorEstado()
        {
            Console.WriteLine("--- Solicitudes activas, finalizadas y canceladas ---");
            Console.WriteLine("ACTIVAS:");
            int activas = 0;
            for (int i = 0; i < sistema.Solicitudes.Count; i++)
            {
                if (sistema.Solicitudes[i].EstaActiva())
                {
                    Console.WriteLine(" " + sistema.Solicitudes[i].ObtenerResumen());
                    activas++;
                }
            }
            Console.WriteLine(" Total activas: " + activas);

            Console.WriteLine("FINALIZADAS (entregadas):");
            int finalizadas = 0;
            for (int i = 0; i < sistema.Solicitudes.Count; i++)
            {
                if (sistema.Solicitudes[i].Estado == EstadoSolicitud.Entregada)
                {
                    Console.WriteLine(" " + sistema.Solicitudes[i].ObtenerResumen());
                    finalizadas++;
                }
            }
            Console.WriteLine(" Total finalizadas: " + finalizadas);

            Console.WriteLine("CANCELADAS:");
            int canceladas = 0;
            for (int i = 0; i < sistema.Solicitudes.Count; i++)
            {
                if (sistema.Solicitudes[i].Estado == EstadoSolicitud.Cancelada)
                {
                    Console.WriteLine(" " + sistema.Solicitudes[i].ObtenerResumen());
                    canceladas++;
                }
            }
            Console.WriteLine(" Total canceladas: " + canceladas);
        }

        public void TecnicosDisponibles()
        {
            Console.WriteLine("--- Técnicos disponibles y técnicos con carga completa ---");
            if (sistema.Tecnicos.Count == 0)
            {
                Console.WriteLine("No hay técnicos registrados.");
                return;
            }
            Console.WriteLine("DISPONIBLES:");
            for (int i = 0; i < sistema.Tecnicos.Count; i++)
            {
                if (sistema.Tecnicos[i].Disponible)
                {
                    Console.WriteLine(" " + sistema.Tecnicos[i].ObtenerResumen());
                }
            }
            Console.WriteLine("CON CARGA COMPLETA:");
            for (int i = 0; i < sistema.Tecnicos.Count; i++)
            {
                if (!sistema.Tecnicos[i].Disponible)
                {
                    Console.WriteLine(" " + sistema.Tecnicos[i].ObtenerResumen());
                }
            }
        }

        public void TecnicoMasServicios()
        {
            Console.WriteLine("--- Técnico con más servicios finalizados ---");
            if (sistema.Tecnicos.Count == 0)
            {
                Console.WriteLine("No hay técnicos registrados.");
                return;
            }
            int mayor = sistema.Tecnicos[0].ServiciosFinalizados;
            for (int i = 1; i < sistema.Tecnicos.Count; i++)
            {
                if (sistema.Tecnicos[i].ServiciosFinalizados > mayor)
                {
                    mayor = sistema.Tecnicos[i].ServiciosFinalizados;
                }
            }
            if (mayor == 0)
            {
                Console.WriteLine("Todavía ningún técnico ha finalizado servicios.");
                return;
            }
            for (int i = 0; i < sistema.Tecnicos.Count; i++)
            {
                if (sistema.Tecnicos[i].ServiciosFinalizados == mayor)
                {
                    Console.WriteLine(sistema.Tecnicos[i].NombreCompleto + ": " + mayor + " servicio(s) finalizado(s)");
                }
            }
        }

        public void EquiposPorTipo()
        {
            Console.WriteLine("--- Equipos atendidos por tipo ---");
            string[] tipos = { "Laptop", "Escritorio", "Impresora" };
            int[] cantidades = new int[3];
            for (int i = 0; i < sistema.Solicitudes.Count; i++)
            {
                Solicitud solicitud = sistema.Solicitudes[i];
                if (solicitud.Tecnico != null)
                {
                    for (int k = 0; k < tipos.Length; k++)
                    {
                        if (solicitud.Equipo.ObtenerTipo() == tipos[k])
                        {
                            cantidades[k];
                        }
                    }
                }
            }
            for (int k = 0; k < tipos.Length; k)
            {
                Console.WriteLine(tipos[k] + ": " + cantidades[k]);
            }
        }

        public void SolicitudesPorCosto(bool ascendente)
        {
            List<Solicitud> ordenadas = Ordenador.BubblePorCosto(sistema.Solicitudes, ascendente);
            Console.WriteLine("Algoritmo: Bubble Sort | Criterio: costo total | Sentido: " + Sentido(ascendente));
            for (int i = 0; i < ordenadas.Count; i++)
            {
                Console.WriteLine(ordenadas[i].Codigo + " | " + ordenadas[i].Estado + " | Total: "
                + Dinero(ordenadas[i].ObtenerTotalActual()));
            }
        }

        public void SolicitudesPorFecha(bool ascendente)
        {
            List<Solicitud> ordenadas = Ordenador.QuickPorFecha(sistema.Solicitudes, ascendente);
            Console.WriteLine("Algoritmo: Quick Sort | Criterio: fecha de recepción | Sentido: " + Sentido(ascendente));
            for (int i = 0; i < ordenadas.Count; i++)
            {
                Console.WriteLine(ordenadas[i].Codigo + " | " + ordenadas[i].FechaRecepcion + " | " + ordenadas[i].Prioridad);
            }
        }

        public void RepuestosPorExistencia(bool ascendente)
        {
            List<Repuesto> ordenados = Ordenador.ShellPorExistencia(sistema.Repuestos, ascendente);
            Console.WriteLine("Algoritmo: Shell Sort | Criterio: existencia disponible | Sentido: " + Sentido(ascendente));
            for (int i = 0; i < ordenados.Count; i++)
            {
                Console.WriteLine(ordenados[i].ObtenerResumen());
            }
        }

        public void TecnicosPorServicios(bool ascendente)
        {
            List<Tecnico> ordenados = Ordenador.SelectionPorServicios(sistema.Tecnicos, ascendente);
            Console.WriteLine("Algoritmo: Selection Sort | Criterio: servicios finalizados | Sentido: " + Sentido(ascendente));
            for (int i = 0; i < ordenados.Count; i++)
            {
                Console.WriteLine(ordenados[i].ObtenerResumen());
            }
        }

        public void RepuestosCriticos()
        {
            Console.WriteLine("--- Repuestos con existencia mínima o agotados ---");
            int encontrados = 0;
            for (int i = 0; i < sistema.Repuestos.Count; i++)
            {
                Repuesto repuesto = sistema.Repuestos[i];
                if (repuesto.EstaEnMinimo())
                {
                    string estado = "[MÍNIMO]";
                    if (repuesto.EstaAgotado())
                    {
                        estado = "[AGOTADO]";
                    }
                    Console.WriteLine(repuesto.ObtenerResumen() + " " + estado);
                    encontrados++;
                }
            }
            if (encontrados == 0)
            {
                Console.WriteLine("No hay repuestos en existencia mínima ni agotados.");
            }
        }

        public void IngresosFinalizados()
        {
            Console.WriteLine("--- Ingresos totales por servicios finalizados ---");
            decimal total = 0;
            int cantidad = 0;
            for (int i = 0; i < sistema.Solicitudes.Count; i++)
            {
                if (sistema.Solicitudes[i].Estado == EstadoSolicitud.Entregada)
                {
                    total = total + sistema.Solicitudes[i].CalcularTotal();
                    cantidad++;
                }
            }
            Console.WriteLine("Servicios entregados: " + cantidad);
            Console.WriteLine("Ingresos totales: " + Dinero(total));
        }

        public void BuscarClientesPorNombre()
        {
            string texto = Entrada.LeerTexto("Nombre o parte del nombre del cliente: ");
            List<Cliente> resultados = Buscador.PorNombre(sistema.Clientes, texto);
            Console.WriteLine("Búsqueda secuencial | Clientes por nombre parcial | Coincidencias: " + resultados.Count);
            for (int i = 0; i < resultados.Count; i++)
            {
                Console.WriteLine(" " + resultados[i].ObtenerResumen());
            }
        }

        public void BuscarSolicitudesPorEstado()
        {
            string texto = Entrada.LeerTexto("Estado de la solicitud (ejemplo: Entregada, EnEspera): ");
            List<Solicitud> resultados = Buscador.PorEstado(sistema.Solicitudes, texto);
            Console.WriteLine("Búsqueda secuencial | Solicitudes por estado | Coincidencias: " + resultados.Count);
            for (int i = 0; i < resultados.Count; i++)
            {
                Console.WriteLine(" " + resultados[i].ObtenerResumen());
            }
        }

        public void BuscarEquiposPorMarca()
        {
            string texto = Entrada.LeerTexto("Marca del equipo: ");
            List<Equipo> resultados = Buscador.PorMarca(sistema.Equipos, texto);
            Console.WriteLine("Búsqueda secuencial | Equipos por marca | Coincidencias: " + resultados.Count);
            for (int i = 0; i < resultados.Count; i++)
            {
                Console.WriteLine(" " + resultados[i].ObtenerResumen());
            }
        }

        public void BuscarSolicitudPorCodigo()
        {
            string codigo = Entrada.LeerTexto("Código de la solicitud (SOL-001): ");
            List<Solicitud> ordenadas = Ordenador.BubblePorCodigo(sistema.Solicitudes);
            Solicitud encontrada = Buscador.BinariaPorCodigo(ordenadas, codigo);
            Console.WriteLine("Búsqueda binaria | Solicitud por código");
            if (encontrada == null)
            {
                Console.WriteLine(" No se encontró esa solicitud.");
            }
            else
            {
                Console.WriteLine(" " + encontrada.ObtenerResumen());
            }
        }

        public void ResultadosBusquedas()
        {
            Console.WriteLine("--- Resultados de las formas de búsqueda ---");
            BuscarClientesPorNombre();
            BuscarSolicitudesPorEstado();
            BuscarEquiposPorMarca();
            BuscarSolicitudPorCodigo();
        }

        public void MatrizSemanal()
        {
            Console.WriteLine("--- Matriz semanal de carga de trabajo ---");
            gestorCarga.MostrarMatriz();
            gestorCarga.MostrarMayorCarga();
        }

        public void CalificacionesPorNota()
        {
            Console.WriteLine("--- Calificaciones recibidas ---");
            for (int i = 0; i < sistema.ConteoCalificaciones.Length; i++)
            {
                Console.WriteLine((i + 1) + " estrella(s): " + sistema.ConteoCalificaciones[i]);
            }
        }

        public void MostrarMenu()
        {
            Console.WriteLine("\n1. Solicitudes en espera por prioridad");
            Console.WriteLine("2. Solicitudes activas, finalizadas y canceladas");
            Console.WriteLine("3. Técnicos disponibles y con carga completa");
            Console.WriteLine("4. Técnico con más servicios finalizados");
            Console.WriteLine("5. Equipos atendidos por tipo");
            Console.WriteLine("6. Solicitudes ordenadas por costo total");
            Console.WriteLine("7. Solicitudes ordenadas por fecha");
            Console.WriteLine("8. Repuestos ordenados por existencia");
            Console.WriteLine("9. Repuestos con existencia mínima o agotados");
            Console.WriteLine("10. Ingresos totales por servicios finalizados");
            Console.WriteLine("11. Resultados de las búsquedas");
            Console.WriteLine("12. Matriz semanal de carga");
            Console.WriteLine("13. Calificaciones recibidas");
            int opcion = Entrada.LeerEntero("Reporte: ", 1, 13);
            if (opcion == 1)
            {
                SolicitudesEnEspera();
            }
            else if (opcion == 2)
            {
                SolicitudesPorEstado();
            }
            else if (opcion == 3)
            {
                TecnicosDisponibles();
            }
            else if (opcion == 4)
            {
                TecnicoMasServicios();
            }
            else if (opcion == 5)
            {
                EquiposPorTipo();
            }
            else if (opcion == 6)
            {
                SolicitudesPorCosto(Entrada.LeerSentido());
            }
            else if (opcion == 7)
            {
                SolicitudesPorFecha(Entrada.LeerSentido());
            }
            else if (opcion == 8)
            {
                RepuestosPorExistencia(Entrada.LeerSentido());
            }
            else if (opcion == 9)
            {
                RepuestosCriticos();
            }
            else if (opcion == 10)
            {
                IngresosFinalizados();
            }
            else if (opcion == 11)
            {
                ResultadosBusquedas();
            }
            else if (opcion == 12)
            {
                MatrizSemanal();
            }
            else
            {
                CalificacionesPorNota();
            }
        }
    }
}
