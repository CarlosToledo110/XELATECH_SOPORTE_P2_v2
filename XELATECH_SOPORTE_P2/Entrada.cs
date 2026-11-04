using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class Entrada
    {
        private static string LeerLinea()
        {
            string texto = Console.ReadLine();
            if (texto == null)
            {
                throw new ValidacionException("No se recibió ninguna entrada.");
            }
            return texto;
        }

        public static string LeerTexto(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string texto = LeerLinea();
                if (texto.Trim() != "")
                {
                    return texto.Trim();
                }
                Console.WriteLine("Este campo es obligatorio.");
            }
        }

        public static string LeerTextoOpcional(string mensaje)
        {
            Console.Write(mensaje);
            return LeerLinea().Trim();
        }

        public static int LeerEntero(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string texto = LeerLinea();
                try
                {
                    return int.Parse(texto);
                }
                catch (Exception)
                {
                    Console.WriteLine("Entrada inválida. Debe ingresar un número entero.");
                }
            }
        }

        public static int LeerEntero(string mensaje, int minimo, int maximo)
        {
            while (true)
            {
                int numero = LeerEntero(mensaje);
                if (numero >= minimo && numero <= maximo)
                {
                    return numero;
                }
                Console.WriteLine("Elija un número entre " + minimo + " y " + maximo + ".");
            }
        }

        public static decimal LeerDecimal(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string texto = LeerLinea();
                try
                {
                    decimal valor = decimal.Parse(texto);
                    if (valor < 0)
                    {
                        Console.WriteLine("El valor no puede ser negativo.");
                    }
                    else
                    {
                        return valor;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Entrada inválida. Debe ingresar un número.");
                }
            }
        }

        public static bool LeerSiNo(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string texto = LeerLinea().Trim().ToLower();
                if (texto == "s" || texto == "si")
                {
                    return true;
                }
                if (texto == "n" || texto == "no")
                {
                    return false;
                }
                Console.WriteLine("Responda S o N.");
            }
        }

        public static bool LeerSentido()
        {
            return LeerSiNo("¿Orden ascendente? (S = ascendente, N = descendente): ");
        }
    }
}
