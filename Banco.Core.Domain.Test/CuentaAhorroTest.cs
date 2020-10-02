using NUnit.Framework;
using System;

namespace Banco.Core.Domain.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        //Escenario Valor de consignación negativo o cero
        //H1: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el dinero.
        //Criterio de Aceptación:
        //1.2 El valor a abono no puede ser menor o igual a 0.
        //Ejemplo
        //Dado El cliente tiene una cuenta de ahorro                                        //A =>Arrange /Perparación
        //Número 10001, Nombre “Cuenta ejemplo”, Saldo de 0, ciudad Valledupar
        //Cuando Va a consignar un valor menor o igual a cero (0)                          //A => Act = Acción
        //Entonces El sistema presentará el mensaje. “El valor a consignar es incorrecto”   //A => Assert = Validación

        [Test]
        public void NoPuedeConsignacionValorNegativoCuentaAhorroLocalTest()
        {
            //Preparar
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            // accion
            var resultado = cuentaAhorro.Consignar(0, "Valledupar");
            //validacion o verificacion 
            Assert.AreEqual( "El valor a consignar es incorrecto", resultado);
        }


        //Escenario: Consignación Inicial Correcta
        //HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el
        //dinero.
        //Criterio de Aceptación:
        //1.1 La consignación inicial debe ser mayor o igual a 50 mil pesos
        //1.3 El valor de la consignación se le adicionará al valor del saldo aumentará
        //Dado El cliente tiene una cuenta de ahorro
        //Número 10001, Nombre “Cuenta ejemplo”, Saldo de 0
        //Cuando Va a consignar el valor inicial de 50 mil pesos
        //Entonces El sistema registrará la consignación
        //AND presentará el mensaje. “Su Nuevo Saldo es de $50.000,00 pesos m/c”. and debe tener un movimiento

        [Test]
        public void PuedeConsignaciónInicialCorrectaAhorroLocalTest()
        {
            //Preparar
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            var movimientosIniciales = cuentaAhorro.CantidadMovimientos();
            // accion
            var resultado = cuentaAhorro.Consignar(50000, "Valledupar");
            var movimientosPosteriores = cuentaAhorro.CantidadMovimientos();

            //validacion o verificacion 
            Assert.AreEqual("Su Nuevo Saldo es de $50.000,00 pesos m/c", resultado);
            Assert.AreEqual(movimientosIniciales + 1, movimientosPosteriores);
        }


        //Escenario: Consignación Inicial Incorrecta
        //HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el dinero.
        //Criterio de Aceptación:
        //1.1 La consignación inicial debe ser mayor o igual a 50 mil pesos
        //Dado El cliente tiene una cuenta de ahorro con
        //Número 10001, Nombre “Cuenta ejemplo”, Saldo de 0
        //Cuando Va a consignar el valor inicial de $49.999 pesos
        //Entonces El sistema no registrará la consignación
        //AND presentará el mensaje. “El valor mínimo de la primera consignación debe ser
        //de $50.000 mil pesos. Su nuevo saldo es $0 pesos”. 

        [Test]
        public void NoPuedeConsignarMenosDeCincualtaMilPesosLocalTest()
        {
            //Preparar
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            // accion
            var resultado = cuentaAhorro.Consignar(49999, "Valledupar");
            //validacion o verificacion 
            Assert.AreEqual("El valor mínimo de la primera consignación debe ser de $50.000 mil pesos. Su nuevo saldo es $0 pesos", resultado);
        }



        //Escenario: Consignación posterior a la inicial correcta
        //HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el dinero.
        //Criterio de Aceptación:
        //1.3 El valor de la consignación se le adicionará al valor del saldo aumentará
        //Dado El cliente tiene una cuenta de ahorro con un saldo de 30.000
        //Cuando Va a consignar el valor inicial de $49.950 pesos
        //Entonces El sistema registrará la consignación
        //AND presentará el mensaje. “Su Nuevo Saldo es de $79.950,00 pesos m/c”.

        [Test]
        public void ConsignaciónPosteriorAInicialCorrectaTest()
        {
            //Preparar
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            var movimientosIniciales = cuentaAhorro.CantidadMovimientos();
            // accion
            var resultado = cuentaAhorro.Consignar(50000, "Valledupar");
            DateTime fecha = new DateTime(2008, 6, 1);
            resultado = cuentaAhorro.Retirar(20000, fecha );
            resultado = cuentaAhorro.Consignar(49950, "Valledupar");

            //validacion o verificacion 
            Assert.AreEqual("Su Nuevo Saldo es de $79.950,00 pesos m/c", resultado);
        }


        //Escenario: Consignación posterior a la inicial correcta
        //HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el dinero.
        //Criterio de Aceptación:
        //1.4 La consignación nacional (a una cuenta de otra ciudad) tendrá un costo de $10 mil pesos.
        //Dado El cliente tiene una cuenta de ahorro con un saldo de 30.000 perteneciente a una
        //sucursal de la ciudad de Bogotá y se realizará una consignación desde una sucursal
        //de la Valledupar.
        //Cuando Va a consignar el valor inicial de $49.950 pesos.
        //Entonces El sistema registrará la consignación restando el valor a consignar los 10 mil pesos.
        //AND presentará el mensaje. “Su Nuevo Saldo es de $69.950,00 pesos m/c”.

        [Test]
        public void ConsignaciónPosteriorAInicialCorrectaNacionalTest()
        {
            //Preparar
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Bogotá");
            var movimientosIniciales = cuentaAhorro.CantidadMovimientos();
            // accion
            var resultado = cuentaAhorro.Consignar(50000, "Bogotá");
            DateTime fecha = new DateTime(2008, 6, 1);
            resultado = cuentaAhorro.Retirar(20000, fecha);
            resultado = cuentaAhorro.Consignar(49950, "Valledupar");

            //validacion o verificacion 
            Assert.AreEqual("Su Nuevo Saldo es de $69.950,00 pesos m/c", resultado);
        }
    }
}