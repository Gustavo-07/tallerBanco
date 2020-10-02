using System;
using System.Collections.Generic;
using System.Text;

namespace Banco.Core.Domain
{
    public class CuentaAhorro : CuentaBancaria
    {
        public CuentaAhorro(string numero, string nombre, string ciudad) : base(numero, nombre, ciudad)
        {
        }

        private decimal recaudoConsignacionNacional { get; set; }
        private decimal recaudoCostoRetiros { get; set; }
        private decimal cantidadRetirosMes { get; set; } = 0;
        private DateTime fechaRegistroMes { get; set; } 

        public override string Consignar(decimal valorConsignacion, string CiudadRemitente)
        {
            if (valorConsignacion <= 0)
                return "El valor a consignar es incorrecto";

            if (valorConsignacion < 50000 && NoTieneConsignacion())
                return "El valor mínimo de la primera consignación debe ser de $50.000 mil pesos. Su nuevo saldo es $0 pesos";


            var SaldoAnterior = Saldo;

            if (CiudadRemitente.Equals(Ciudad))
            {
                Saldo += valorConsignacion;
            }

            if(!CiudadRemitente.Equals(Ciudad) && valorConsignacion <= 10000)
            {
                return "El valor a consignar debe ser mayor a 10.000 pesos para consignaciones a otras ciudades";
            }

            if (!CiudadRemitente.Equals(Ciudad) && valorConsignacion > 10000)
            {
                valorConsignacion = cobrarCostoConsignacionNacional(valorConsignacion);
                Saldo += valorConsignacion;
            }
  
            _movimientos.Add(new CuentaBancariaMovimiento(SaldoAnterior, valorConsignacion, 0, "CONSIGNACION"));

            return $"Su Nuevo Saldo es de ${Saldo:n2} pesos m/c";
        }


        public override string Retirar(decimal valorRetiro, DateTime fechaSolicitudRetiro)
        {
            // tiene saldo insuficiente o  no tiene fondos en la cuenta?
            if (Saldo < valorRetiro || Saldo == 0)
            {
                return "No posee saldo suficiente para realizar el retiro";
            }
             
            // tiene saldo para retirar pero es menor que 20000?
            if (Saldo > valorRetiro && Saldo < 20000)
            {
                return "el saldo minimo debe ser de 20000 pesos";
            }

            // cuantos retiros ha realizado en el mes?
            if( consultarCantidadRetirosMes(fechaSolicitudRetiro) > 3)
            {
                Saldo = cobrarCostoRetiro(Saldo);
            }
            var SaldoAnterior = Saldo;
            Saldo -= valorRetiro;
            _movimientos.Add(new CuentaBancariaMovimiento(SaldoAnterior, 0, valorRetiro, "CONSIGNACION"));

            return $"Su Nuevo Saldo es de ${Saldo:n2} pesos m/c";
        }

        private decimal cobrarCostoConsignacionNacional(decimal consignacion)
        {
            consignacion -= 10000;
            recaudoConsignacionNacional += 10000;
            return consignacion;
        }

        private decimal cobrarCostoRetiro (decimal saldo)
        {
            saldo -= 5000;
            recaudoCostoRetiros += 5000;
            return saldo;
        }

        private decimal consultarCantidadRetirosMes(DateTime fechaSolicitudRetiro)
        {
            // es la primera vez que retira?
            if(cantidadRetirosMes == 0)
            {
                return cantidadRetirosMes;
            }
            else
            {
                // el mes con el que se lleva el historial de retiros ya paso?
                if( !fechaSolicitudRetiro.ToString("MM/YY").Equals(fechaRegistroMes.ToString("MM/YY")))
                {
                    cantidadRetirosMes = 0;
                    return cantidadRetirosMes;
                }

                return cantidadRetirosMes;
            }


        }

    }
}
