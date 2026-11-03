using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class GestorCarga
    {
        private SistemaXT sistema;
        private string[] dias = { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };

        public GestorCarga(SistemaXT sistema)
        {
            this.sistema = sistema;
        }

        private int ObtenerColumna(DateTime fecha)
        {
            DayOfWeek dia = fecha.DayOfWeek;
            if (dia == DayOfWeek.Monday)
            {
                return 0;
            }
            if (dia == DayOfWeek.Tuesday)
            {
                return 1;
            }
            if (dia == DayOfWeek.Wednesday)
            {
                return 2;
            }
            if (dia == DayOfWeek.Thursday)
            {
                return 3;
            }
            if (dia == DayOfWeek.Friday)
            {
                return 4;
            }
            if (dia == DayOfWeek.Saturday)
            {
                return 5;
            }
            return 6;
        }

        public void RegistrarCarga(Tecnico tecnico, DateTime fecha)
        {
            int fila = sistema.IndiceTecnico(tecnico);
            if (fila < 0)
            {
                throw new ValidacionException("El técnico no está registrado.");
            }
            int columna = ObtenerColumna(fecha);
            sistema.Carga[fila, columna]++;
        }

        public void MostrarMatriz()
        {
            if (sistema.Tecnicos.Count == 0)
            {
                Console.WriteLine("No hay técnicos registrados.");
                return;
            }
            Console.Write("Técnico\t\t");
            for (int j = 0; j < SistemaXelaTech.DIAS; j++)
            {
                Console.Write(dias[j].Substring(0, 3) + "\t");
            }
            Console.WriteLine();
            for (int i = 0; i < sistema.Tecnicos.Count; i++)
            {
                Console.Write(sistema.Tecnicos[i].Codigo + "\t");
                for (int j = 0; j < SistemaXelaTech.DIAS; j++)
                {
                    Console.Write(sistema.Carga[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public void MostrarMayorCarga()
        {
            int mayor = 0;
            int filaMayor = -1;
            int columnaMayor = -1;
            for (int i = 0; i < sistema.Tecnicos.Count; i++)
            {
                for (int j = 0; j < SistemaXT.DIAS; j++)
                {
                    if (sistema.Carga[i, j] > mayor)
                    {
                        mayor = sistema.Carga[i, j];
                        filaMayor = i;
                        columnaMayor = j;
                    }
                }
            }
            if (filaMayor == -1)
            {
                Console.WriteLine("Todavía no hay solicitudes asignadas.");
            }
            else
            {
                Console.WriteLine("Mayor carga: " + sistema.Tecnicos[filaMayor].NombreCompleto
                + " el día " + dias[columnaMayor] + " con " + mayor + " solicitud(es).");
            }
        }
    }


}
