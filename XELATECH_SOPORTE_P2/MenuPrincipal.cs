using System;
using System.Collections.Generic;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class MenuPrincipal
    {
        private SistemaXT sistema;
        private GestorRegistros registros;

        private GestorCarga carga;
        private GestorSolicitudes solicitudes;
        private Reportes reportes;
        private GeneradorDatos generador;

        public MenuPrincipal()
        {
            sistema = new SistemaXT();
            registros = new GestorRegistros(sistema);

            carga = new GestorCarga(sistema);
            solicitudes = new GestorSolicitudes(sistema, registros, carga);
            reportes = new Reportes(sistema, solicitudes, carga);
            generador = new GeneradorDatos(sistema, registros, solicitudes);
        }

        public void Ejecutar()
        {
            int opcion = 0;
            while (opcion != 11)
            {
                Console.WriteLine("\n============================================");
                Console.WriteLine(" XELATECH SOPORTE");
                Console.WriteLine("============================================");
                Console.WriteLine("1. Gestión de clientes");
                Console.WriteLine("2. Gestión de técnicos");
                Console.WriteLine("3. Gestión de equipos");
                Console.WriteLine("4. Gestión de solicitudes");
                Console.WriteLine("5. Gestión de repuestos");
                Console.WriteLine("6. Atención de colas");
                Console.WriteLine("7. Ordenamientos");
                Console.WriteLine("8. Búsquedas");
                Console.WriteLine("9. Reportes");
                Console.WriteLine("10. Generar datos de prueba");
                Console.WriteLine("11. Salir");
                try
                {
                    opcion = Entrada.LeerEntero("Seleccione una opción: ", 1, 11);
                    if (opcion == 1)
                    {
                        MenuClientes();
                    }
                    else if (opcion == 2)
                    {
                        MenuTecnicos();
                    }
                    else if (opcion == 3)
                    {
                        MenuEquipos();
                    }
                    else if (opcion == 4)
                    {
                        MenuSolicitudes();
                    }
                    else if (opcion == 5)
                    {
                        MenuRepuestos();
                    }
                    else if (opcion == 6)
                    {
                        MenuColas();
                    }
                    else if (opcion == 7)
                    {
                        MenuOrdenamientos();
                    }
                    else if (opcion == 8)
                    {
                        MenuBusquedas();
                    }
                    else if (opcion == 9)
                    {
                        reportes.MostrarMenu();
                    }
                    else if (opcion == 10)
                    {
                        int cantidad = Entrada.LeerEntero("Cantidad de datos de prueba por categoría (1 a 20): ", 1, 20);
                        generador.Generar(cantidad);
                    }
                    else
                    {
                        Console.WriteLine("Programa finalizado.");
                    }
                }
                catch (ValidacionException ex)
                {
                    Console.WriteLine("NO SE PUDO COMPLETAR: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocurrió un error inesperado: " + ex.Message);
                }
            }
        }


        private TipoCliente LeerTipoCliente()
        {
            Console.WriteLine("Tipo de cliente:\n1. Estudiante\n2. Profesional\n3. Negocio\n4. Institución");
            int opcion = Entrada.LeerEntero("Opción: ", 1, 4);
            if (opcion == 1)
            {
                return TipoCliente.Estudiante;
            }
            if (opcion == 2)
            {
                return TipoCliente.Profesional;
            }
            if (opcion == 3)
            {
                return TipoCliente.Negocio;
            }
            return TipoCliente.Institucion;
        }

        private Especialidad LeerEspecialidad()
        {
            Console.WriteLine("Especialidad:\n1. Computadoras\n2. Impresoras");
            int opcion = Entrada.LeerEntero("Opción: ", 1, 2);
            if (opcion == 1)
            {
                return Especialidad.Computadoras;
            }
            return Especialidad.Impresoras;
        }

        private Prioridad LeerPrioridad()
        {
            Console.WriteLine("Prioridad:\n1. Normal\n2. Urgente");
            int opcion = Entrada.LeerEntero("Opción: ", 1, 2);
            if (opcion == 1)
            {
                return Prioridad.Normal;
            }
            return Prioridad.Urgente;
        }


        private void MenuClientes()
        {
            Console.WriteLine("\n--- GESTIÓN DE CLIENTES ---");
            Console.WriteLine("1. Registrar cliente\n2. Consultar clientes\n3. Actualizar contacto\n4. Volver");
            int opcion = Entrada.LeerEntero("Opción: ", 1, 4);
            if (opcion == 1)
            {
                string codigo = Entrada.LeerTexto("Código (CLI-001): ");
                string nombre = Entrada.LeerTexto("Nombre completo: ");
                string telefono = Entrada.LeerTexto("Teléfono (8 dígitos): ");
                string correo = Entrada.LeerTextoOpcional("Correo (Enter si no tiene): ");
                TipoCliente tipo = LeerTipoCliente();
                Cliente cliente;
                if (correo == "")
                {
                    cliente = new Cliente(codigo, nombre, telefono, tipo);
                }
                else
                {
                    cliente = new Cliente(codigo, nombre, telefono, correo, tipo);
                }
                registros.RegistrarCliente(cliente);
                Console.WriteLine("Cliente registrado.");
            }
            else if (opcion == 2)
            {
                if (sistema.Clientes.Count == 0)
                {
                    Console.WriteLine("No hay clientes registrados.");
                }
                for (int i = 0; i < sistema.Clientes.Count; i++)
                {
                    Console.WriteLine(sistema.Clientes[i].ObtenerResumen());
                }
            }
            else if (opcion == 3)
            {
                string codigo = Entrada.LeerTexto("Código del cliente: ");
                string telefono = Entrada.LeerTexto("Nuevo teléfono (8 dígitos): ");
                string correo = Entrada.LeerTextoOpcional("Nuevo correo (Enter para dejar sin correo): ");
                registros.ActualizarCliente(codigo, telefono, correo);
                Console.WriteLine("Contacto actualizado.");
            }
        }

        private void MenuTecnicos()
        {
            Console.WriteLine("\n--- GESTIÓN DE TÉCNICOS ---");
            Console.WriteLine("1. Registrar técnico\n2. Consultar técnicos y especialidades\n3. Volver");
            int opcion = Entrada.LeerEntero("Opción: ", 1, 3);
            if (opcion == 1)
            {
                string codigo = Entrada.LeerTexto("Código (TEC-001): ");
                string nombre = Entrada.LeerTexto("Nombre completo: ");
                Especialidad especialidad = LeerEspecialidad();
                registros.RegistrarTecnico(new Tecnico(codigo, nombre, especialidad));
                Console.WriteLine("Técnico registrado.");
            }
            else if (opcion == 2)
            {
                if (sistema.Tecnicos.Count == 0)
                {
                    Console.WriteLine("No hay técnicos registrados.");
                }
                for (int i = 0; i < sistema.Tecnicos.Count; i++)
                {
                    Console.WriteLine(sistema.Tecnicos[i].ObtenerResumen());
                }
            }
        }

        private void MenuEquipos()
        {
            Console.WriteLine("\n--- GESTIÓN DE EQUIPOS ---");
            Console.WriteLine("1. Registrar laptop\n2. Registrar computadora de escritorio\n3. Registrar impresora\n4. Consultar equipos\n5. Volver");
            int opcion = Entrada.LeerEntero("Opción: ", 1, 5);
            if (opcion == 4)
            {
                if (sistema.Equipos.Count == 0)
                {
                    Console.WriteLine("No hay equipos registrados.");
                }
                for (int i = 0; i < sistema.Equipos.Count; i++)
                {
                    Console.WriteLine(sistema.Equipos[i].ObtenerResumen());
                }
            }
            else if (opcion >= 1 && opcion <= 3)
            {
                string codigo = Entrada.LeerTexto("Código interno (EQ-001): ");
                string serie = Entrada.LeerTexto("Número de serie: ");
                string marca = Entrada.LeerTexto("Marca: ");
                string modelo = Entrada.LeerTexto("Modelo: ");
                Cliente propietario = registros.BuscarCliente(Entrada.LeerTexto("Código del propietario (CLI-001): "));
                if (propietario == null)
                {
                    throw new ValidacionException("El propietario debe ser un cliente registrado.");
                }
                string estadoFisico = Entrada.LeerTexto("Estado físico con el que se recibe: ");
                string accesorios = Entrada.LeerTextoOpcional("Accesorios entregados (Enter si ninguno): ");

                Equipo equipo;
                if (opcion == 1)
                {
                    decimal pulgadas = Entrada.LeerDecimal("Pulgadas de pantalla: ");
                    bool bateria = Entrada.LeerSiNo("¿Tiene batería? (S/N): ");
                    equipo = new Laptop(codigo, serie, marca, modelo, propietario, estadoFisico, accesorios, pulgadas, bateria);
                }
                else if (opcion == 2)
                {
                    string torre = Entrada.LeerTexto("Tipo de torre: ");
                    string fuente = Entrada.LeerTexto("Fuente de poder: ");
                    equipo = new Escritorio(codigo, serie, marca, modelo, propietario, estadoFisico, accesorios, torre, fuente);
                }
                else
                {
                    string tecnologia = Entrada.LeerTexto("Tecnología (tinta o láser): ");
                    int paginas = Entrada.LeerEntero("Contador de páginas: ");
                    equipo = new Impresora(codigo, serie, marca, modelo, propietario, estadoFisico, accesorios, tecnologia, paginas);
                }
                registros.RegistrarEquipo(equipo);
                Console.WriteLine("Equipo registrado. Sugerencia de diagnóstico: " + equipo.DiagnosticoInicialSugerido());
            }
        }

        private void MenuSolicitudes()
        {
            Console.WriteLine("\n--- GESTIÓN DE SOLICITUDES ---");
            Console.WriteLine("1. Crear solicitud");
            Console.WriteLine("2. Consultar solicitudes");
            Console.WriteLine("3. Registrar diagnóstico");
            Console.WriteLine("4. Agregar repuesto a una solicitud");
            Console.WriteLine("5. Actualizar estado");
            Console.WriteLine("6. Consultar historial de estados");
            Console.WriteLine("7. Deshacer último cambio de estado");
            Console.WriteLine("8. Entregar equipo y calcular total");
            Console.WriteLine("9. Calificar servicio");
            Console.WriteLine("10. Volver");
            int opcion = Entrada.LeerEntero("Opción: ", 1, 10);
            if (opcion == 1)
            {
                string codigo = Entrada.LeerTexto("Código de solicitud (SOL-001): ");
                Cliente cliente = registros.BuscarCliente(Entrada.LeerTexto("Código del cliente: "));
                Equipo equipo = registros.BuscarEquipo(Entrada.LeerTexto("Código interno del equipo: "));
                string descripcion = Entrada.LeerTexto("Descripción del problema: ");
                Prioridad prioridad = LeerPrioridad();
                solicitudes.Crear(new Solicitud(codigo, cliente, equipo, descripcion, prioridad));
                Console.WriteLine("Solicitud creada y colocada en la cola " + prioridad + ".");
            }
            else if (opcion == 2)
            {
                if (sistema.Solicitudes.Count == 0)
                {
                    Console.WriteLine("No hay solicitudes registradas.");
                }
                for (int i = 0; i < sistema.Solicitudes.Count; i++)
                {
                    Console.WriteLine(sistema.Solicitudes[i].ObtenerResumen());
                }
            }
            else if (opcion == 3)
            {
                string codigo = Entrada.LeerTexto("Código de solicitud: ");
                string texto = Entrada.LeerTexto("Diagnóstico: ");
                decimal horas = Entrada.LeerDecimal("Horas de mano de obra: ");
                solicitudes.RegistrarDiagnostico(codigo, texto, horas);
                Console.WriteLine("Diagnóstico registrado. Mano de obra: " + Reportes.Dinero(horas * Solicitud.TARIFA_HORA));
            }
            else if (opcion == 4)
            {
                string codigo = Entrada.LeerTexto("Código de solicitud: ");
                Repuesto repuesto = registros.BuscarRepuesto(Entrada.LeerTexto("Código del repuesto (REP-001): "));
                int cantidad = Entrada.LeerEntero("Cantidad: ");
                solicitudes.AgregarRepuesto(codigo, repuesto, cantidad);
                Console.WriteLine("Repuesto agregado a la solicitud.");
                if (repuesto.EstaAgotado())
                {
                    Console.WriteLine("ALERTA: el repuesto " + repuesto.Nombre + " quedó AGOTADO.");
                }
                else if (repuesto.EstaEnMinimo())
                {
                    Console.WriteLine("ALERTA: el repuesto " + repuesto.Nombre + " llegó a su existencia mínima.");
                }
            }
            else if (opcion == 5)
            {
                string codigo = Entrada.LeerTexto("Código de solicitud: ");
                Console.WriteLine("Nuevo estado:\n1. En diagnóstico\n2. En reparación\n3. Lista para entregar\n4. Cancelada");
                Console.WriteLine("(Recibida, En espera y Asignada se asignan solas al crear y atender; Entregada se hace con la opción 8)");
                int estado = Entrada.LeerEntero("Opción: ", 1, 4);
                if (estado == 1)
                {
                    solicitudes.CambiarEstado(codigo, EstadoSolicitud.EnDiagnostico);
                }
                else if (estado == 2)
                {
                    solicitudes.CambiarEstado(codigo, EstadoSolicitud.EnReparacion);
                }
                else if (estado == 3)
                {
                    solicitudes.CambiarEstado(codigo, EstadoSolicitud.ListaParaEntregar);
                }
                else
                {
                    solicitudes.CambiarEstado(codigo, EstadoSolicitud.Cancelada);
                }
                Console.WriteLine("Estado actualizado.");
            }
            else if (opcion == 6)
            {
                Console.WriteLine(solicitudes.ConsultarHistorial(Entrada.LeerTexto("Código de solicitud: ")));
            }
            else if (opcion == 7)
            {
                solicitudes.Deshacer(Entrada.LeerTexto("Código de solicitud: "));
                Console.WriteLine("Se deshizo el último cambio de estado.");
            }
            else if (opcion == 8)
            {
                decimal total = solicitudes.Entregar(Entrada.LeerTexto("Código de solicitud: "));
                Console.WriteLine("Equipo entregado. Total a pagar: " + Reportes.Dinero(total));
            }
            else if (opcion == 9)
            {
                string codigo = Entrada.LeerTexto("Código de solicitud: ");
                int nota = Entrada.LeerEntero("Calificación (1 a 5): ");
                solicitudes.Calificar(codigo, nota);
                Console.WriteLine("Calificación registrada.");
            }
        }

        private void MenuRepuestos()
        {
            Console.WriteLine("\n--- GESTIÓN DE REPUESTOS ---");
            Console.WriteLine("1. Registrar repuesto\n2. Consultar inventario\n3. Ver alertas de existencia\n4. Volver");
            int opcion = Entrada.LeerEntero("Opción: ", 1, 4);
            if (opcion == 1)
            {
                string codigo = Entrada.LeerTexto("Código (REP-001): ");
                string nombre = Entrada.LeerTexto("Nombre: ");
                string categoria = Entrada.LeerTexto("Categoría: ");
                int existencia = Entrada.LeerEntero("Existencia inicial: ");
                int minimo = Entrada.LeerEntero("Existencia mínima: ");
                decimal costo = Entrada.LeerDecimal("Costo unitario: ");
                decimal venta = Entrada.LeerDecimal("Precio de venta: ");
                registros.RegistrarRepuesto(new Repuesto(codigo, nombre, categoria, existencia, minimo, costo, venta));
                Console.WriteLine("Repuesto registrado.");
            }
            else if (opcion == 2)
            {
                if (sistema.Repuestos.Count == 0)
                {
                    Console.WriteLine("No hay repuestos registrados.");
                }
                for (int i = 0; i < sistema.Repuestos.Count; i++)
                {
                    Console.WriteLine(sistema.Repuestos[i].ObtenerResumen());
                }
            }
            else if (opcion == 3)
            {
                reportes.RepuestosCriticos();
            }
        }

        private void MenuColas()
        {
            Console.WriteLine("\n--- ATENCIÓN DE COLAS ---");
            Console.WriteLine("1. Ver colas\n2. Atender siguiente solicitud\n3. Volver");
            int opcion = Entrada.LeerEntero("Opción: ", 1, 3);
            if (opcion == 1)
            {
                solicitudes.MostrarColas();
            }
            else if (opcion == 2)
            {
                Solicitud atendida = solicitudes.AtenderSiguiente();
                Console.WriteLine("Solicitud " + atendida.Codigo + " asignada al técnico " + atendida.Tecnico.NombreCompleto + ".");
            }
        }

        private void MenuOrdenamientos()
        {
            Console.WriteLine("\n--- ORDENAMIENTOS ---");
            Console.WriteLine("1. Solicitudes por costo total (Bubble Sort)");
            Console.WriteLine("2. Técnicos por servicios finalizados (Selection Sort)");
            Console.WriteLine("3. Repuestos por existencia (Shell Sort)");
            Console.WriteLine("4. Solicitudes por fecha de recepción (Quick Sort)");
            Console.WriteLine("5. Volver");
            int opcion = Entrada.LeerEntero("Opción: ", 1, 5);
            if (opcion == 5)
            {
                return;
            }
            bool ascendente = Entrada.LeerSentido();
            if (opcion == 1)
            {
                reportes.SolicitudesPorCosto(ascendente);
            }
            else if (opcion == 2)
            {
                reportes.TecnicosPorServicios(ascendente);
            }
            else if (opcion == 3)
            {
                reportes.RepuestosPorExistencia(ascendente);
            }
            else
            {
                reportes.SolicitudesPorFecha(ascendente);
            }
        }

        private void MenuBusquedas()
        {
            Console.WriteLine("\n--- BÚSQUEDAS ---");
            Console.WriteLine("1. Clientes por nombre parcial (secuencial)");
            Console.WriteLine("2. Solicitudes por estado (secuencial)");
            Console.WriteLine("3. Equipos por marca (secuencial)");
            Console.WriteLine("4. Solicitud por código (binaria)");
            Console.WriteLine("5. Volver");
            int opcion = Entrada.LeerEntero("Opción: ", 1, 5);
            if (opcion == 1)
            {
                reportes.BuscarClientesPorNombre();
            }
            else if (opcion == 2)
            {
                reportes.BuscarSolicitudesPorEstado();
            }
            else if (opcion == 3)
            {
                reportes.BuscarEquiposPorMarca();
            }
            else if (opcion == 4)
            {
                reportes.BuscarSolicitudPorCodigo();
            }
        }
    }
}
