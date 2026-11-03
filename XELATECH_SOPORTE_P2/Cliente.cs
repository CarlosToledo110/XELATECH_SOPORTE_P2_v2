using System;
using System.Collections.Generic;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
        class Cliente : Persona
        {
            protected string telefono;
            protected string correo;
            protected TipoCliente tipo;
            protected int cantidadSolicitudes;

            public string Telefono
            {
                get { return telefono; }
                set { telefono = Validador.TelefonoValido(value); }
            }

            public string Correo
            {
                get { return correo; }
                set { correo = Validador.CorreoValido(value); }
            }

            public TipoCliente Tipo
            {
                get { return tipo; }
                set { tipo = value; }
            }

            public int CantidadSolicitudes
            {
                get { return cantidadSolicitudes; }
            }

            public Cliente(string codigo, string nombre, string telefono, string correo, TipoCliente tipo)
            : base(Validador.CodigoConPrefijo(codigo, "CLI-", "código de cliente"), nombre)
            {
                Telefono = telefono;
                Correo = correo;
                Tipo = tipo;
                cantidadSolicitudes = 0;
            }

            public Cliente(string codigo, string nombre, string telefono, TipoCliente tipo)
            : base(Validador.CodigoConPrefijo(codigo, "CLI-", "código de cliente"), nombre)
            {
                Telefono = telefono;
                correo = "Sin correo";
                Tipo = tipo;
                cantidadSolicitudes = 0;
            }

            public void IncrementarSolicitudes()
            {
                cantidadSolicitudes++;
            }

            public void ActualizarContacto(string telefono, string correo)
            {
                Telefono = telefono;
                if (correo == null || correo.Trim() == "")
                {
                    this.correo = "Sin correo";
                }
                else
                {
                    Correo = correo;
                }
            }

            public override string ObtenerResumen()
            {
                return base.ObtenerResumen() + " | Tel: " + telefono + " | Correo: " + correo
                + " | Tipo: " + tipo + " | Solicitudes: " + cantidadSolicitudes;
            }
        }
}
