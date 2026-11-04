using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class Validador
    {
        public static string Normalizar(string codigo)
        {
            if (codigo == null)
            {
                return "";
            }
            return codigo.Trim().ToUpper();
        }

        public static string TextoObligatorio(string valor, string campo)
        {
            if (valor == null || valor.Trim() == "")
            {
                throw new ValidacionException("El campo " + campo + " es obligatorio.");
            }
            return valor.Trim();
        }

        public static int NoNegativo(int valor, string campo)
        {
            if (valor < 0)
            {
                throw new ValidacionException(campo + " no puede ser negativo.");
            }
            return valor;
        }

        public static decimal NoNegativo(decimal valor, string campo)
        {
            if (valor < 0)
            {
                throw new ValidacionException(campo + " no puede ser negativo.");
            }
            return valor;
        }

        public static int MayorQueCero(int valor, string campo)
        {
            if (valor <= 0)
            {
                throw new ValidacionException(campo + " debe ser mayor que cero.");
            }
            return valor;
        }

        public static string CorreoValido(string correo)
        {
            correo = TextoObligatorio(correo, "correo");
            int posicionArroba = -1;
            int posicionPunto = -1;
            for (int i = 0; i < correo.Length; i++)
            {
                if (correo[i] == '@' && posicionArroba == -1)
                {
                    posicionArroba = i;
                }
                if (correo[i] == '.')
                {
                    posicionPunto = i;
                }
            }
            if (posicionArroba < 1 || posicionPunto < posicionArroba + 2 || posicionPunto == correo.Length - 1)
            {
                throw new ValidacionException("El correo no tiene un formato válido (ejemplo: nombre@correo.com).");
            }
            return correo;
        }

        public static string TelefonoValido(string telefono)
        {
            telefono = TextoObligatorio(telefono, "teléfono");
            if (telefono.Length != 8)
            {
                throw new ValidacionException("El teléfono debe tener exactamente 8 dígitos.");
            }
            for (int i = 0; i < telefono.Length; i++)
            {
                if (telefono[i] < '0' || telefono[i] > '9')
                {
                    throw new ValidacionException("El teléfono solo debe contener números.");
                }
            }
            return telefono;
        }

        public static string CodigoConPrefijo(string codigo, string prefijo, string campo)
        {
            codigo = TextoObligatorio(codigo, campo).ToUpper();
            int cantidadDigitos = codigo.Length - prefijo.Length;
            if (cantidadDigitos < 1 || cantidadDigitos > 6 || codigo.Substring(0, prefijo.Length) != prefijo)
            {
                throw new ValidacionException("El " + campo + " debe tener el formato " + prefijo + "001.");
            }
            for (int i = prefijo.Length; i < codigo.Length; i++)
            {
                if (codigo[i] < '0' || codigo[i] > '9')
                {
                    throw new ValidacionException("El " + campo + " debe tener el formato " + prefijo + "001.");
                }
            }
            return codigo;
        }
    }

}
