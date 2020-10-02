using System;
using System.Collections.Generic;
using System.Text;

namespace Banco.Core.Domain
{
    public class CuentaCorriente : CuentaBancaria
    {
        private decimal cupoSobregiro { get; set; } = 0;
        private decimal credito { get; set; }

        public CuentaCorriente(string numero, string nombre, string ciudad, decimal credito) : base(numero, nombre, ciudad)
        {
            credito = this.credito;
        }

        public override string Consignar(decimal valorConsignacion, string ciudad)
        {
            if (valorConsignacion <= 0)
                return "El valor a consignar es incorrecto";

            if (valorConsignacion < 100000 && NoTieneConsignacion())
                return "El valor mínimo de la primera consignación debe ser de $100.000 pesos. Su nuevo saldo es $0 pesos";

            decimal sumaTotal;
            decimal valorFaltante;
            if(cupoSobregiro < credito)
            {
                sumaTotal = cupoSobregiro + valorConsignacion;
                if(sumaTotal <= credito)
                {
                    cupoSobregiro = valorConsignacion;

                    return $"Se ha consignado el monto a su cupo preaprobado Nuevo Saldo es de ${ Saldo: n2}pesos m/ c";
                }
                else
                {
                    valorFaltante = credito - cupoSobregiro;
                    valorConsignacion -= valorFaltante;
                    cupoSobregiro = valorFaltante;

                    var SaldoAnterior = Saldo;

                    Saldo += valorConsignacion;
                    _movimientos.Add(new CuentaBancariaMovimiento(SaldoAnterior, valorConsignacion, 0, "CONSIGNACION"));

                    return $"Su Nuevo Saldo es de ${Saldo:n2} pesos m/c";

                }
            }
            else
            {
                var SaldoAnterior = Saldo;
                Saldo += valorConsignacion;
                _movimientos.Add(new CuentaBancariaMovimiento(SaldoAnterior, valorConsignacion, 0, "CONSIGNACION"));

                return $"Su Nuevo Saldo es de ${Saldo:n2} pesos m/c";
            }

        }


        public string Retirar(decimal valorRetiro)
        {
            cupoSobregiro = credito;

            if (valorRetiro > Saldo + cupoSobregiro)
            {
                return "no tiene fondos suficientes para retirar";
            }

            if (valorRetiro <= Saldo)
            {
                var SaldoAnterior = Saldo;
                Saldo -= valorRetiro;
                _movimientos.Add(new CuentaBancariaMovimiento(SaldoAnterior, 0, valorRetiro, "CONSIGNACION"));

                return $"Su Nuevo Saldo es de ${Saldo:n2} pesos m/c";
            }

            if(Saldo == 0 && valorRetiro <= cupoSobregiro)
            {
                var SaldoAnterior = Saldo;
                cupoSobregiro -= valorRetiro;
                _movimientos.Add(new CuentaBancariaMovimiento(SaldoAnterior, 0, valorRetiro, "CONSIGNACION"));

                return $"Su Nuevo Saldo es de ${Saldo:n2} pesos m/c";
            }

            if (Saldo < valorRetiro && valorRetiro <= cupoSobregiro)
            {
                var SaldoAnterior = Saldo;
                cupoSobregiro -= valorRetiro;
                _movimientos.Add(new CuentaBancariaMovimiento(SaldoAnterior, 0, valorRetiro, "CONSIGNACION"));

                return $"Su Nuevo Saldo es de ${Saldo:n2} pesos m/c";
            }


            return "";

        }

    }
}
