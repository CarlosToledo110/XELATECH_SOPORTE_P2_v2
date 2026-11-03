using System;
using System.Collections.Generic;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class DetalleRepuesto
    {
        protected Repuesto repuesto;
        protected int cantidad;

        public Repuesto Repuesto
        {
            get { return repuesto; }
        }

        public int Cantidad
        {
            get { return cantidad; }
        }

        public DetalleRepuesto(Repuesto repuesto, int cantidad)
        {
            this.repuesto = repuesto;
            this.cantidad = Validador.MayorQueCero(cantidad, "La cantidad");
        }

        public void SumarCantidad(int adicional)
        {
            cantidad = cantidad + adicional;
        }

        public decimal Subtotal()
        {
            return repuesto.PrecioVenta * cantidad;
        }
    }
}
