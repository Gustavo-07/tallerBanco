using NUnit.Framework;
using System;

namespace Banco.Core.Domain.Test
{
    public class cuentaCorrienteTests
    {
        [SetUp]
        public void Setup()
        {
        }

        //Escenario Valor de consignación negativo o cero
        //H1: Como Usuario quiero realizar consignaciones a una cuenta corriente.
        //Criterio de Aceptación:
        //1.2 El valor a abono no puede ser menor o igual a 0.
        //Ejemplo
        //Dado El cliente tiene una cuenta corriente                                        //A =>Arrange /Perparación
        //Número 10002, Nombre “Cuenta ejemplo”, Saldo de 0, ciudad Valledupar
        //Cuando Va a consignar un valor menor o igual a cero (0)                          //A => Act = Acción
        //Entonces El sistema presentará el mensaje. “El valor a consignar es incorrecto”   //A => Assert = Validación

        [Test]
        public void NoPuedeConsignarValorNegativoCuentaCorrienteTest()
        {
            //Preparar
            var cuentaCorriente = new CuentaCorriente(numero: "10002", nombre: "Cuenta Ejemplo", ciudad: "Valledupar", credito: 1000000);
            // accion
            var resultado = cuentaCorriente.Consignar(0, "Valledupar");
            //validacion o verificacion 
            Assert.AreEqual("El valor a consignar es incorrecto", resultado);
        }


        //Escenario: Consignación Inicial Correcta
        //HU: Como Usuario quiero realizar consignaciones a una cuenta corriente.
        //Criterio de Aceptación:
        //1.1 La consignación inicial debe ser mayor o igual a 100 mil pesos
        //1.3 El valor de la consignación se le adicionará al valor del saldo aumentará
        //Dado El cliente tiene una cuenta corriente
        //Número 10002, Nombre “Cuenta ejemplo”, Saldo de 0
        //Cuando Va a consignar el valor inicial de 100 mil pesos
        //Entonces El sistema registrará la consignación
        //AND presentará el mensaje. “Su Nuevo Saldo es de $100.000,00 pesos m/c”. and debe tener un movimiento

        [Test]
        public void PuedeConsignaciónInicialCorrectaCuentaCorrienteTest()
        {
            //Preparar
            var cuentaCorriente = new CuentaCorriente(numero: "10002", nombre: "Cuenta Ejemplo", ciudad: "Valledupar", credito: 1000000);
            var movimientosIniciales = cuentaCorriente.CantidadMovimientos();
            // accion
            var resultado = cuentaCorriente.Consignar(100000, "Valledupar");
            var movimientosPosteriores = cuentaCorriente.CantidadMovimientos();

            //validacion o verificacion 
            Assert.AreEqual("Su Nuevo Saldo es de $100.000,00 pesos m/c", resultado);
            Assert.AreEqual(movimientosIniciales + 1, movimientosPosteriores);
        }

        //Escenario: Consignación Inicial Incorrecta
        //HU: Como Usuario quiero realizar consignaciones a una cuenta corriente.
        //Criterio de Aceptación:
        //1.1 La consignación inicial debe ser mayor o igual a 100000 mil pesos
        //Dado El cliente tiene una cuenta de corriente con
        //Número 10002, Nombre “Cuenta ejemplo”, Saldo de 0
        //Cuando Va a consignar el valor inicial de $99.999 pesos
        //Entonces El sistema no registrará la consignación
        //AND presentará el mensaje. “El valor mínimo de la primera consignación debe ser
        //de $100.000 mil pesos. Su nuevo saldo es $0 pesos”. 

        [Test]
        public void NoPuedeConsignarMenosDeCienMilPesosTest()
        {
            //Preparar
            var cuentaCorriente = new CuentaCorriente(numero: "10002", nombre: "Cuenta Ejemplo", ciudad: "Valledupar", credito: 1000000);
            // accion
            var resultado = cuentaCorriente.Consignar(99999, "Valledupar");
            //validacion o verificacion 
            Assert.AreEqual("El valor mínimo de la primera consignación debe ser de $100.000 pesos. Su nuevo saldo es $0 pesos", resultado);
        }


        //Escenario: Retiro incorrecto
        //HU: Como Usuario quiero realizar retiros de la corriente.
        //Criterio de Aceptación:
        //1.1 para poder retirar debe de haber fondos suficientes en el saldo o en el cupo o la sumatoria de ambas
        //Dado El cliente tiene una cuenta de corriente con
        //Número 10002, Nombre “Cuenta ejemplo”, Saldo de 0 y con un cupo de 500000
        //Cuando Va a retirar el valor de de $600.000 pesos
        //Entonces El sistema no registrará el retiro
        //AND presentará el mensaje. “no tiene fondos suficientes para retirar”. 

        [Test]
        public void NoPuedeRetirarMenosDelCupo()
        {
            //Preparar
            var cuentaCorriente = new CuentaCorriente(numero: "10002", nombre: "Cuenta Ejemplo", ciudad: "Valledupar", credito: 500000);
            // accion
            var resultado = cuentaCorriente.Retirar(600000);
            //validacion o verificacion 
            Assert.AreEqual("no tiene fondos suficientes para retirar", resultado);
        }


        //Escenario: Retiro Correcto
        //HU: Como Usuario quiero realizar retiros del saldo de la cuenta corriente.
        //Criterio de Aceptación:
        //1.1 para poder retirar debe de haber fondos suficientes en el saldo.
        //Dado El cliente tiene una cuenta de corriente con
        //Número 10002, Nombre “Cuenta ejemplo”, Saldo de 250000 y con un cupo de 0
        //Cuando Va a retirar el valor de de $150000 pesos
        //Entonces El sistema registrará el retiro
        //AND presentará el mensaje. “Su Nuevo Saldo es de $100.000,00 pesos m/c”. 

        [Test]
        public void PuedeRetiraDelSaldo()
        {
            //Preparar
            var cuentaCorriente = new CuentaCorriente(numero: "10002", nombre: "Cuenta Ejemplo", ciudad: "Valledupar", credito: 0);
            // accion
            var resultado = cuentaCorriente.Consignar(250000, "Valledupar");
            resultado = cuentaCorriente.Retirar(150000);
            //validacion o verificacion 
            Assert.AreEqual("Su Nuevo Saldo es de $100.000,00 pesos m/c", resultado);
        }


    }
}
