using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class GeneradorDatos
    {
        private SistemaXT sistema;
        private GestorRegistros gestorRegistros;

        private GestorSolicitudes gestorSolicitudes;
        private Random aleatorio = new Random();

        private string[] nombres = { "Ana", "Luis", "María", "José", "Carla", "Pedro", "Sofía", "Diego", "Lucía", "Mario" };
        private string[] apellidos = { "López", "Pérez", "García", "Morales", "Ramírez", "Tello", "Mejía", "Castillo" };
        private string[] marcas = { "HP", "Dell", "Lenovo", "Asus", "Epson", "Canon" };
        private string[] categorias = { "Memoria", "Batería", "Pantalla", "Cargador", "Cabezal", "Fuente" };
        private string[] problemas = { "No enciende", "Funciona muy lento", "Se sobrecalienta", "No imprime", "Pantalla dañada" };

        public GeneradorDatos(SistemaXT sistema, GestorRegistros gestorRegistros, GestorSolicitudes gestorSolicitudes)
        {
            this.sistema = sistema;
            this.gestorRegistros = gestorRegistros;

            this.gestorSolicitudes = gestorSolicitudes;
        }

        private string TresDigitos(int numero)
        {
            if (numero < 10)
            {
                return "00" + numero;
            }
            if (numero < 100)
            {
                return "0" + numero;
            }
            return "" + numero;
        }

        private string TextoAleatorio(string[] lista)
        {
            return lista[aleatorio.Next(0, lista.Length)];
        }

        public void Generar(int cantidad)
        {
            if (cantidad < 1 || cantidad > 20)
            {
                throw new ValidacionException("La cantidad debe estar entre 1 y 20.");
            }

            int cantidadTecnicos = cantidad;
            if (cantidadTecnicos < 2)
            {
                cantidadTecnicos = 2;
            }
            for (int i = 0; i < cantidadTecnicos; i++)
            {
                int numero = sistema.Tecnicos.Count + 1;
                string codigo = "TEC-" + TresDigitos(numero);
                while (gestorRegistros.BuscarTecnico(codigo) != null)
                {
                    numero++;
                    codigo = "TEC-" + TresDigitos(numero);
                }
                Especialidad especialidad = Especialidad.Computadoras;
                if (i % 2 == 1)
                {
                    especialidad = Especialidad.Impresoras;
                }
                gestorRegistros.RegistrarTecnico(new Tecnico(codigo, TextoAleatorio(nombres) + " " + TextoAleatorio(apellidos), especialidad));
            }

            for (int i = 0; i < cantidad; i++)
            {
                int numero = sistema.Clientes.Count + 1;
                string codigo = "CLI-" + TresDigitos(numero);
                while (gestorRegistros.BuscarCliente(codigo) != null)
                {
                    numero++;
                    codigo = "CLI-" + TresDigitos(numero);
                }
                string telefono = "" + aleatorio.Next(30000000, 59999999);
                string correo = "cliente" + numero + "@correo.com";
                TipoCliente tipo = TipoCliente.Estudiante;
                int sorteoTipo = aleatorio.Next(0, 4);
                if (sorteoTipo == 1)
                {
                    tipo = TipoCliente.Profesional;
                }
                else if (sorteoTipo == 2)
                {
                    tipo = TipoCliente.Negocio;
                }
                else if (sorteoTipo == 3)
                {
                    tipo = TipoCliente.Institucion;
                }
                gestorRegistros.RegistrarCliente(new Cliente(codigo, TextoAleatorio(nombres) + " " + TextoAleatorio(apellidos), telefono, correo, tipo));
            }

            for (int i = 0; i < cantidad; i++)
            {
                int numero = sistema.Repuestos.Count + 1;
                string codigo = "REP-" + TresDigitos(numero);
                while (gestorRegistros.BuscarRepuesto(codigo) != null)
                {
                    numero++;
                    codigo = "REP-" + TresDigitos(numero);
                }
                int existencia = aleatorio.Next(3, 31);
                int minimo = aleatorio.Next(1, 4);
                decimal costo = aleatorio.Next(10, 101);
                decimal venta = costo + aleatorio.Next(5, 41);
                string categoria = TextoAleatorio(categorias);
                gestorRegistros.RegistrarRepuesto(new Repuesto(codigo, categoria + " " + numero, categoria, existencia, minimo, costo, venta));
            }

            for (int i = 0; i < cantidad; i++)
            {
                Cliente propietario = sistema.Clientes[aleatorio.Next(0, sistema.Clientes.Count)];
                int numero = sistema.Equipos.Count + 1;
                string codigoEquipo = "EQ-" + TresDigitos(numero);
                while (gestorRegistros.BuscarEquipo(codigoEquipo) != null)
                {
                    numero++;
                    codigoEquipo = "EQ-" + TresDigitos(numero);
                }
                int numeroSerie = numero;
                string serie = "SER-" + TresDigitos(numeroSerie);
                while (gestorRegistros.SerieExiste(serie))
                {
                    numeroSerie++;
                    serie = "SER-" + TresDigitos(numeroSerie);
                }

                string marca = TextoAleatorio(marcas);
                Equipo equipo;
                int tipoEquipo = aleatorio.Next(0, 3);
                if (tipoEquipo == 0)
                {
                    equipo = new Laptop(codigoEquipo, serie, marca, "Modelo " + numero, propietario, "Usado", "Cargador", 15, true);
                }
                else if (tipoEquipo == 1)
                {
                    equipo = new Escritorio(codigoEquipo, serie, marca, "Modelo " + numero, propietario, "Usado", "Cable de poder", "ATX", "500W");
                }
                else
                {
                    equipo = new Impresora(codigoEquipo, serie, marca, "Modelo " + numero, propietario, "Usado", "Cable USB", "Tinta", aleatorio.Next(100, 5000));
                }
                gestorRegistros.RegistrarEquipo(equipo);

                int numeroSolicitud = sistema.Solicitudes.Count + 1;
                string codigoSolicitud = "SOL-" + TresDigitos(numeroSolicitud);
                while (gestorSolicitudes.BuscarCodigo(codigoSolicitud) != null)
                {
                    numeroSolicitud++;
                    codigoSolicitud = "SOL-" + TresDigitos(numeroSolicitud);
                }
                Prioridad prioridad = Prioridad.Normal;
                if (aleatorio.Next(0, 100) < 30)
                {
                    prioridad = Prioridad.Urgente;
                }
                gestorSolicitudes.Crear(new Solicitud(codigoSolicitud, propietario, equipo, TextoAleatorio(problemas), prioridad));
            }

            for (int i = 0; i < cantidad; i++)
            {
                if (aleatorio.Next(0, 100) < 60)
                {
                    try
                    {
                        Solicitud atendida = gestorSolicitudes.AtenderSiguiente();
                        AvanzarSolicitud(atendida);
                    }
                    catch (ValidacionException)
                    {
                        break;
                    }
                }
            }
            Console.WriteLine("Datos de prueba generados correctamente.");
        }

        private void AvanzarSolicitud(Solicitud solicitud)
        {
            string codigo = solicitud.Codigo;
            gestorSolicitudes.CambiarEstado(codigo, EstadoSolicitud.EnDiagnostico);
            decimal horas = aleatorio.Next(1, 6);
            gestorSolicitudes.RegistrarDiagnostico(codigo, solicitud.Equipo.DiagnosticoInicialSugerido(), horas);

            if (aleatorio.Next(0, 100) < 70)
            {
                gestorSolicitudes.CambiarEstado(codigo, EstadoSolicitud.EnReparacion);
                int tiposDeRepuesto = aleatorio.Next(1, 3);
                for (int k = 0; k < tiposDeRepuesto; k++)
                {
                    Repuesto repuesto = sistema.Repuestos[aleatorio.Next(0, sistema.Repuestos.Count)];
                    int unidades = aleatorio.Next(1, 3);
                    if (repuesto.HayExistencia(unidades))
                    {
                        gestorSolicitudes.AgregarRepuesto(codigo, repuesto, unidades);
                    }
                }
                if (aleatorio.Next(0, 100) < 60)
                {
                    gestorSolicitudes.CambiarEstado(codigo, EstadoSolicitud.ListaParaEntregar);
                    if (aleatorio.Next(0, 100) < 60)
                    {
                        gestorSolicitudes.Entregar(codigo);
                        gestorSolicitudes.Calificar(codigo, aleatorio.Next(1, 6));
                    }
                }
            }
        }
    }

}
