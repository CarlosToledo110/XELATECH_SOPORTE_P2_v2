using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class Repuesto
    {
        protected string codigo;
        protected string nombre;
        protected string categoria;
        protected int existencia;
        protected int existenciaMinima;
        protected decimal costoUnitario;
        protected decimal precioVenta;
        protected int cantidadUtilizada;

        public string Codigo
        {
            get { return codigo; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = Validador.TextoObligatorio(value, "nombre del repuesto"); }
        }

        public string Categoria
        {
            get { return categoria; }
            set { categoria = Validador.TextoObligatorio(value, "categoría"); }
        }

        public int Existencia
        {
            get { return existencia; }
        }

        public int ExistenciaMinima
        {
            get { return existenciaMinima; }
            set { existenciaMinima = Validador.NoNegativo(value, "existencia mínima"); }
        }

        public decimal CostoUnitario
        {
            get { return costoUnitario; }
            set { costoUnitario = Validador.NoNegativo(value, "costo unitario"); }
        }

        public decimal PrecioVenta
        {
            get { return precioVenta; }
            set { precioVenta = Validador.NoNegativo(value, "precio de venta"); }
        }

        public int CantidadUtilizada
        {
            get { return cantidadUtilizada; }
        }

        public Repuesto(string codigo, string nombre, string categoria, int existencia, int minimo, decimal costo, decimal venta)
        {
            this.codigo = Validador.CodigoConPrefijo(codigo, "REP-", "código de repuesto");
            Nombre = nombre;
            Categoria = categoria;
            this.existencia = Validador.NoNegativo(existencia, "existencia");
            ExistenciaMinima = minimo;
            CostoUnitario = costo;
            PrecioVenta = venta;
            cantidadUtilizada = 0;
            if (existenciaMinima > this.existencia)
            {
                throw new ValidacionException("La existencia mínima no puede ser mayor que la existencia inicial.");
            }
            if (precioVenta < costoUnitario)
            {
                throw new ValidacionException("El precio de venta no puede ser menor que el costo unitario.");
            }
        }

        public bool HayExistencia(int cantidad)
        {
            if (cantidad > 0 && existencia >= cantidad)
            {
                return true;
            }
            return false;
        }

        public void Descontar(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ValidacionException("La cantidad debe ser mayor que cero.");
            }
            if (!HayExistencia(cantidad))
            {
                throw new ValidacionException("No hay existencia suficiente de " + nombre + " (disponible: " + existencia + ").");
            }
            existencia = existencia - cantidad;
            cantidadUtilizada = cantidadUtilizada + cantidad;
        }

        public bool EstaEnMinimo()
        {
            return existencia <= existenciaMinima;
        }

        public bool EstaAgotado()
        {
            return existencia == 0;
        }

        public string ObtenerResumen()
        {
            return codigo + " | " + nombre + " | Existencia: " + existencia + " | Mínimo: " + existenciaMinima
            + " | Venta: Q" + precioVenta;
        }
    }
}
