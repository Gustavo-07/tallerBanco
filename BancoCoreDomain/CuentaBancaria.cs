using BancoCoreDomain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Banco.Core.Domain
{
    public abstract class CuentaBancaria : TServicioFinanciero
    {
        protected CuentaBancaria(string numero, string nombre, string ciudad)
        {
            Numero = numero;
            Nombre = nombre;
            Ciudad = ciudad;
            Saldo = 0;
            _movimientos = new List<CuentaBancariaMovimiento>();
        }

        public string Numero { get; }

        public string Nombre { get; }

        public string Ciudad { get; }

        public decimal Saldo { get; set; }

        public virtual string Consignar(decimal valorConsignacion, string CiudadRemitente)
        {
            return " ";
        }



        public virtual string Retirar(decimal valorRetiro, DateTime fechaSolicitudRetiro)
        {
            return " ";
        }

        public bool NoTieneConsignacion()
        {
            return !_movimientos.Any(t => t.Tipo == "CONSIGNACION");
        }
        public readonly List<CuentaBancariaMovimiento> _movimientos;

        public int CantidadMovimientos() => _movimientos.Count;
}

    public class CuentaBancariaMovimiento
    {
        public CuentaBancariaMovimiento(decimal saldoAnterior, decimal valorCredito, decimal valrDevito, string tipo)
        {
            SaldoAnterior = saldoAnterior;
            ValorCredito = valorCredito;
            ValrDevito = valrDevito;
            Tipo = tipo;
        }

        public decimal SaldoAnterior { get; private set; }

        public decimal ValorCredito { get; private set; }

        public decimal ValrDevito { get; private set; }

        public string Tipo { get; private set; }
    }
}
