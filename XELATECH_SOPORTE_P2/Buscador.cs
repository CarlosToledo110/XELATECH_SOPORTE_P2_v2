using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class Buscador
    {
        private static bool ContieneTexto(string texto, string parte)
        {
            texto = texto.ToLower();
            parte = parte.ToLower();
            for (int i = 0; i + parte.Length <= texto.Length; i++)
            {
                bool coincide = true;
                for (int j = 0; j < parte.Length; j++)
                {
                    if (texto[i + j] != parte[j])
                    {
                        coincide = false;
                        break;
                    }
                }
                if (coincide)
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Cliente> PorNombre(List<Cliente> lista, string texto)
        {
            List<Cliente> resultados = new List<Cliente>();
            for (int i = 0; i < lista.Count; i++)
            {
                if (ContieneTexto(lista[i].NombreCompleto, texto))
                {
                    resultados.Add(lista[i]);
                }
            }
            return resultados;
        }

        public static List<Solicitud> PorEstado(List<Solicitud> lista, string texto)
        {
            List<Solicitud> resultados = new List<Solicitud>();
            for (int i = 0; i < lista.Count; i++)
            {
                if (ContieneTexto(lista[i].Estado.ToString(), texto))
                {
                    resultados.Add(lista[i]);
                }
            }
            return resultados;
        }

        public static List<Equipo> PorMarca(List<Equipo> lista, string texto)
        {
            List<Equipo> resultados = new List<Equipo>();
            for (int i = 0; i < lista.Count; i++)
            {
                if (ContieneTexto(lista[i].Marca, texto))
                {
                    resultados.Add(lista[i]);
                }
            }
            return resultados;
        }

        public static Solicitud BinariaPorCodigo(List<Solicitud> lista, string codigo)
        {
            if (lista.Count == 0)
            {
                throw new ValidacionException("No hay solicitudes registradas.");
            }
            codigo = Validador.CodigoConPrefijo(codigo, "SOL-", "código de solicitud");
            int buscado = int.Parse(codigo.Substring(4));

            for (int i = 0; i < lista.Count - 1; i++)
            {
                if (lista[i].NumeroCodigo() > lista[i + 1].NumeroCodigo())
                {
                    throw new ValidacionException("La búsqueda binaria requiere que las solicitudes estén ordenadas por código.");
                }
            }

            int inicio = 0;
            int fin = lista.Count - 1;
            int posicion = -1;
            while (inicio <= fin)
            {
                int centro = (inicio + fin) / 2;
                if (lista[centro].NumeroCodigo() == buscado)
                {
                    posicion = centro;
                    break;
                }
                else if (buscado < lista[centro].NumeroCodigo())
                {
                    fin = centro - 1;
                }
                else
                {
                    inicio = centro + 1;
                }
            }
            if (posicion == -1)
            {
                return null;
            }
            return lista[posicion];
        }
    }
}
