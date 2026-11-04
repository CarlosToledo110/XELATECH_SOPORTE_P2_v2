using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class Ordenador
    {
        private static List<Solicitud> CopiarSolicitudes(List<Solicitud> original)
        {
            List<Solicitud> copia = new List<Solicitud>();
            for (int i = 0; i < original.Count; i++)
            {
                copia.Add(original[i]);
            }
            return copia;
        }

        public static List<Solicitud> BubblePorCosto(List<Solicitud> original, bool ascendente)
        {
            if (original.Count < 2)
            {
                throw new ValidacionException("Se necesitan al menos dos solicitudes para ordenar.");
            }
            List<Solicitud> lista = CopiarSolicitudes(original);
            for (int i = 0; i < lista.Count - 1; i++)
            {
                for (int j = 0; j < lista.Count - 1 - i; j++)
                {
                    decimal actual = lista[j].ObtenerTotalActual();
                    decimal siguiente = lista[j + 1].ObtenerTotalActual();
                    bool intercambiar = false;
                    if (ascendente)
                    {
                        if (actual > siguiente)
                        {
                            intercambiar = true;
                        }
                    }
                    else
                    {
                        if (actual < siguiente)
                        {
                            intercambiar = true;
                        }
                    }
                    if (intercambiar)
                    {
                        Solicitud auxiliar = lista[j];
                        lista[j] = lista[j + 1];
                        lista[j + 1] = auxiliar;
                    }
                }
            }
            return lista;
        }

        public static List<Solicitud> BubblePorCodigo(List<Solicitud> original)
        {
            List<Solicitud> lista = CopiarSolicitudes(original);
            for (int i = 0; i < lista.Count - 1; i++)
            {
                for (int j = 0; j < lista.Count - 1 - i; j++)
                {
                    if (lista[j].NumeroCodigo() > lista[j + 1].NumeroCodigo())
                    {
                        Solicitud auxiliar = lista[j];
                        lista[j] = lista[j + 1];
                        lista[j + 1] = auxiliar;
                    }
                }
            }
            return lista;
        }

        public static List<Tecnico> SelectionPorServicios(List<Tecnico> original, bool ascendente)
        {
            if (original.Count < 2)
            {
                throw new ValidacionException("Se necesitan al menos dos técnicos para ordenar.");
            }
            List<Tecnico> lista = new List<Tecnico>();
            for (int i = 0; i < original.Count; i++)
            {
                lista.Add(original[i]);
            }
            for (int i = 0; i < lista.Count - 1; i++)
            {
                int posicion = i;
                for (int j = i + 1; j < lista.Count; j++)
                {
                    if (ascendente)
                    {
                        if (lista[j].ServiciosFinalizados < lista[posicion].ServiciosFinalizados)
                        {
                            posicion = j;
                        }
                    }
                    else
                    {
                        if (lista[j].ServiciosFinalizados > lista[posicion].ServiciosFinalizados)
                        {
                            posicion = j;
                        }
                    }
                }
                Tecnico auxiliar = lista[i];
                lista[i] = lista[posicion];
                lista[posicion] = auxiliar;
            }
            return lista;
        }

        public static List<Repuesto> ShellPorExistencia(List<Repuesto> original, bool ascendente)
        {
            if (original.Count < 2)
            {
                throw new ValidacionException("Se necesitan al menos dos repuestos para ordenar.");
            }
            List<Repuesto> lista = new List<Repuesto>();
            for (int i = 0; i < original.Count; i++)
            {
                lista.Add(original[i]);
            }
            for (int salto = lista.Count / 2; salto > 0; salto = salto / 2)
            {
                for (int i = salto; i < lista.Count; i++)
                {
                    Repuesto temporal = lista[i];
                    int j = i;
                    bool seguir = true;
                    while (j >= salto && seguir)
                    {
                        bool fueraDeOrden = false;
                        if (ascendente)
                        {
                            if (lista[j - salto].Existencia > temporal.Existencia)
                            {
                                fueraDeOrden = true;
                            }
                        }
                        else
                        {
                            if (lista[j - salto].Existencia < temporal.Existencia)
                            {
                                fueraDeOrden = true;
                            }
                        }
                        if (fueraDeOrden)
                        {
                            lista[j] = lista[j - salto];
                            j = j - salto;
                        }
                        else
                        {
                            seguir = false;
                        }
                    }
                    lista[j] = temporal;
                }
            }
            return lista;
        }

        public static List<Solicitud> QuickPorFecha(List<Solicitud> original, bool ascendente)
        {
            if (original.Count < 2)
            {
                throw new ValidacionException("Se necesitan al menos dos solicitudes para ordenar.");
            }
            List<Solicitud> lista = CopiarSolicitudes(original);
            QuickSort(lista, 0, lista.Count - 1, ascendente);
            return lista;
        }

        private static bool VaAntes(System.DateTime a, System.DateTime b, bool ascendente)
        {
            if (ascendente)
            {
                return a < b;
            }
            return a > b;
        }

        private static void QuickSort(List<Solicitud> lista, int izquierda, int derecha, bool ascendente)
        {
            int i = izquierda;
            int j = derecha;
            System.DateTime pivote = lista[(izquierda + derecha) / 2].FechaRecepcion;
            while (i <= j)
            {
                while (VaAntes(lista[i].FechaRecepcion, pivote, ascendente))
                {
                    i++;
                }
                while (VaAntes(pivote, lista[j].FechaRecepcion, ascendente))
                {
                    j--;
                }
                if (i <= j)
                {
                    Solicitud auxiliar = lista[i];
                    lista[i] = lista[j];
                    lista[j] = auxiliar;
                    i++;
                    j--;
                }
            }
            if (izquierda < j)
            {
                QuickSort(lista, izquierda, j, ascendente);
            }
            if (i < derecha)
            {
                QuickSort(lista, i, derecha, ascendente);
            }
        }
    }
}
