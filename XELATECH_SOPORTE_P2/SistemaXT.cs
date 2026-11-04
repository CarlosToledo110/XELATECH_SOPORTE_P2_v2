using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class SistemaXT
    {
        public const int DIAS = 7;

        private List<Cliente> clientes = new List<Cliente>();
        private List<Tecnico> tecnicos = new List<Tecnico>();
        private List<Equipo> equipos = new List<Equipo>();
        private List<Repuesto> repuestos = new List<Repuesto>();
        private List<Solicitud> solicitudes = new List<Solicitud>();

        private Queue<Solicitud> colaUrgente = new Queue<Solicitud>();
        private Queue<Solicitud> colaNormal = new Queue<Solicitud>();

        private int[,] carga = new int[0, DIAS];

        private int[] conteoCalificaciones = new int[5];

        public List<Cliente> Clientes
        {
            get { return clientes; }
        }

        public List<Tecnico> Tecnicos
        {
            get { return tecnicos; }
        }

        public List<Equipo> Equipos
        {
            get { return equipos; }
        }

        public List<Repuesto> Repuestos
        {
            get { return repuestos; }
        }

        public List<Solicitud> Solicitudes
        {
            get { return solicitudes; }
        }

        public Queue<Solicitud> ColaUrgente
        {
            get { return colaUrgente; }
        }

        public Queue<Solicitud> ColaNormal
        {
            get { return colaNormal; }
        }

        public int[,] Carga
        {
            get { return carga; }
        }

        public int[] ConteoCalificaciones
        {
            get { return conteoCalificaciones; }
        }

        public void AgregarTecnico(Tecnico tecnico)
        {
            for (int i = 0; i < tecnicos.Count; i++)
            {
                if (tecnicos[i].Codigo == tecnico.Codigo)
                {
                    throw new ValidacionException("Ya existe un técnico con ese código.");
                }
            }
            tecnicos.Add(tecnico);
            int[,] nueva = new int[tecnicos.Count, DIAS];
            for (int i = 0; i < carga.GetLength(0); i++)
            {
                for (int j = 0; j < DIAS; j++)
                {
                    nueva[i, j] = carga[i, j];
                }
            }
            carga = nueva;
        }

        public int IndiceTecnico(Tecnico tecnico)
        {
            for (int i = 0; i < tecnicos.Count; i++)
            {
                if (tecnicos[i] == tecnico)
                {
                    return i;
                }
            }
            return -1;
        }

        public void RegistrarCalificacion(int nota)
        {
            conteoCalificaciones[nota - 1]++;
        }
    }



}
