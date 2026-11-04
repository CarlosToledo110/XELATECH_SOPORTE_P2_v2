using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class Tecnico : Persona
    {
        public const int LIMITE_TRABAJOS = 3;

        protected Especialidad especialidad;
        protected int trabajosActivos;
        protected int serviciosFinalizados;
        protected int sumaCalificaciones;
        protected int cantidadCalificaciones;

        public Especialidad Especialidad
        {
            get { return especialidad; }
        }

        public int TrabajosActivos
        {
            get { return trabajosActivos; }
        }

        public int ServiciosFinalizados
        {
            get { return serviciosFinalizados; }
        }

        public bool Disponible
        {
            get { return trabajosActivos < LIMITE_TRABAJOS; }
        }

        public double CalificacionPromedio
        {
            get
            {
                if (cantidadCalificaciones == 0)
                {
                    return 0;
                }
                double suma = sumaCalificaciones;
                return suma / cantidadCalificaciones;
            }
        }

        public Tecnico(string codigo, string nombre, Especialidad especialidad)
        : base(Validador.CodigoConPrefijo(codigo, "TEC-", "código de técnico"), nombre)
        {
            this.especialidad = especialidad;
            trabajosActivos = 0;
            serviciosFinalizados = 0;
        }

        public bool PuedeRecibir(Especialidad requerida)
        {
            if (especialidad == requerida && Disponible)
            {
                return true;
            }
            return false;
        }

        public void AsignarTrabajo()
        {
            if (!Disponible)
            {
                throw new ValidacionException("El técnico ya alcanzó el límite de trabajos activos.");
            }
            trabajosActivos++;
        }

        public void FinalizarTrabajo()
        {
            if (trabajosActivos > 0)
            {
                trabajosActivos--;
            }
            serviciosFinalizados++;
        }

        public void LiberarTrabajoCancelado()
        {
            if (trabajosActivos > 0)
            {
                trabajosActivos--;
            }
        }

        public void RegistrarCalificacion(int nota)
        {
            if (nota < 1 || nota > 5)
            {
                throw new ValidacionException("La calificación debe estar entre 1 y 5.");
            }
            sumaCalificaciones = sumaCalificaciones + nota;
            cantidadCalificaciones++;
        }

        public override string ObtenerResumen()
        {
            return base.ObtenerResumen() + " | Especialidad: " + especialidad
            + " | Activos: " + trabajosActivos + "/" + LIMITE_TRABAJOS
            + " | Finalizados: " + serviciosFinalizados
            + " | Promedio: " + Math.Round(CalificacionPromedio, 2);
        }
    }
}
