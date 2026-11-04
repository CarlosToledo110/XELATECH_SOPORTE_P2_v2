using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XELATECH_SOPORTE_P2
{
    public class GestorRegistros
    {
        private SistemaXT sistema;

        public GestorRegistros(SistemaXT sistema) { this.sistema = sistema; }

        public void RegistrarCliente(Cliente cliente)
        {
            if (BuscarCliente(cliente.Codigo) != null)
                throw new ValidacionException("Ya existe un cliente con ese código.");
            sistema.Clientes.Add(cliente);
        }

        public Cliente BuscarCliente(string codigo)
        {
            codigo = Validador.Normalizar(codigo);
            for (int i = 0; i < sistema.Clientes.Count; i++)
                if (sistema.Clientes[i].Codigo == codigo) return sistema.Clientes[i];
            return null;
        }

        public void ActualizarCliente(string codigo, string telefono, string correo)
        {
            Cliente cliente = BuscarCliente(codigo);
            if (cliente == null) throw new ValidacionException("No se encontró un cliente con ese código.");
            cliente.ActualizarContacto(telefono, correo);
        }

        public void RegistrarTecnico(Tecnico tecnico) { sistema.AgregarTecnico(tecnico); }

        public Tecnico BuscarTecnico(string codigo)
        {
            codigo = Validador.Normalizar(codigo);
            for (int i = 0; i < sistema.Tecnicos.Count; i++)
                if (sistema.Tecnicos[i].Codigo == codigo) return sistema.Tecnicos[i];
            return null;
        }

        public Tecnico BuscarApto(Especialidad especialidad)
        {
            Tecnico mejor = null;
            for (int i = 0; i < sistema.Tecnicos.Count; i++)
            {
                Tecnico actual = sistema.Tecnicos[i];
                if (actual.PuedeRecibir(especialidad) &&
                    (mejor == null || actual.TrabajosActivos < mejor.TrabajosActivos))
                    mejor = actual;
            }
            return mejor;
        }

        public bool SerieExiste(string serie)
        {
            serie = Validador.Normalizar(serie);
            for (int i = 0; i < sistema.Equipos.Count; i++)
                if (sistema.Equipos[i].NumeroSerie == serie) return true;
            return false;
        }

        public void RegistrarEquipo(Equipo equipo)
        {
            if (SerieExiste(equipo.NumeroSerie))
                throw new ValidacionException("El número de serie ya está registrado.");
            if (BuscarEquipo(equipo.CodigoInterno) != null)
                throw new ValidacionException("El código interno ya está registrado.");
            sistema.Equipos.Add(equipo);
        }

        public Equipo BuscarEquipo(string codigo)
        {
            codigo = Validador.Normalizar(codigo);
            for (int i = 0; i < sistema.Equipos.Count; i++)
                if (sistema.Equipos[i].CodigoInterno == codigo) return sistema.Equipos[i];
            return null;
        }

        public void RegistrarRepuesto(Repuesto repuesto)
        {
            if (BuscarRepuesto(repuesto.Codigo) != null)
                throw new ValidacionException("Ya existe un repuesto con ese código.");
            sistema.Repuestos.Add(repuesto);
        }

        public Repuesto BuscarRepuesto(string codigo)
        {
            codigo = Validador.Normalizar(codigo);
            for (int i = 0; i < sistema.Repuestos.Count; i++)
                if (sistema.Repuestos[i].Codigo == codigo) return sistema.Repuestos[i];
            return null;
        }
    }
}
