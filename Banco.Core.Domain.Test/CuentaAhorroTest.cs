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

        //Escenario Valor de consignaci�n negativo o cero
        //H1: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el dinero.
        //Criterio de Aceptaci�n:
        //1.2 El valor a abono no puede ser menor o igual a 0.
        //Ejemplo
        //Dado El cliente tiene una cuenta de ahorro                                        //A =>Arrange /Perparaci�n
        //N�mero 10001, Nombre �Cuenta ejemplo�, Saldo de 0, ciudad Valledupar
        //Cuando Va a consignar un valor menor o igual a cero (0)                          //A => Act = Acci�n
        //Entonces El sistema presentar� el mensaje. �El valor a consignar es incorrecto�   //A => Assert = Validaci�n

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


        //Escenario: Consignaci�n Inicial Correcta
        //HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el
        //dinero.
        //Criterio de Aceptaci�n:
        //1.1 La consignaci�n inicial debe ser mayor o igual a 50 mil pesos
        //1.3 El valor de la consignaci�n se le adicionar� al valor del saldo aumentar�
        //Dado El cliente tiene una cuenta de ahorro
        //N�mero 10001, Nombre �Cuenta ejemplo�, Saldo de 0
        //Cuando Va a consignar el valor inicial de 50 mil pesos
        //Entonces El sistema registrar� la consignaci�n
        //AND presentar� el mensaje. �Su Nuevo Saldo es de $50.000,00 pesos m/c�. and debe tener un movimiento

        [Test]
        public void PuedeConsignaci�nInicialCorrectaAhorroLocalTest()
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


        //Escenario: Consignaci�n Inicial Incorrecta
        //HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el dinero.
        //Criterio de Aceptaci�n:
        //1.1 La consignaci�n inicial debe ser mayor o igual a 50 mil pesos
        //Dado El cliente tiene una cuenta de ahorro con
        //N�mero 10001, Nombre �Cuenta ejemplo�, Saldo de 0
        //Cuando Va a consignar el valor inicial de $49.999 pesos
        //Entonces El sistema no registrar� la consignaci�n
        //AND presentar� el mensaje. �El valor m�nimo de la primera consignaci�n debe ser
        //de $50.000 mil pesos. Su nuevo saldo es $0 pesos�. 

        [Test]
        public void NoPuedeConsignarMenosDeCincualtaMilPesosLocalTest()
        {
            //Preparar
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Valledupar");
            // accion
            var resultado = cuentaAhorro.Consignar(49999, "Valledupar");
            //validacion o verificacion 
            Assert.AreEqual("El valor m�nimo de la primera consignaci�n debe ser de $50.000 mil pesos. Su nuevo saldo es $0 pesos", resultado);
        }



        //Escenario: Consignaci�n posterior a la inicial correcta
        //HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el dinero.
        //Criterio de Aceptaci�n:
        //1.3 El valor de la consignaci�n se le adicionar� al valor del saldo aumentar�
        //Dado El cliente tiene una cuenta de ahorro con un saldo de 30.000
        //Cuando Va a consignar el valor inicial de $49.950 pesos
        //Entonces El sistema registrar� la consignaci�n
        //AND presentar� el mensaje. �Su Nuevo Saldo es de $79.950,00 pesos m/c�.

        [Test]
        public void Consignaci�nPosteriorAInicialCorrectaTest()
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


        //Escenario: Consignaci�n posterior a la inicial correcta
        //HU: Como Usuario quiero realizar consignaciones a una cuenta de ahorro para salvaguardar el dinero.
        //Criterio de Aceptaci�n:
        //1.4 La consignaci�n nacional (a una cuenta de otra ciudad) tendr� un costo de $10 mil pesos.
        //Dado El cliente tiene una cuenta de ahorro con un saldo de 30.000 perteneciente a una
        //sucursal de la ciudad de Bogot� y se realizar� una consignaci�n desde una sucursal
        //de la Valledupar.
        //Cuando Va a consignar el valor inicial de $49.950 pesos.
        //Entonces El sistema registrar� la consignaci�n restando el valor a consignar los 10 mil pesos.
        //AND presentar� el mensaje. �Su Nuevo Saldo es de $69.950,00 pesos m/c�.

        [Test]
        public void Consignaci�nPosteriorAInicialCorrectaNacionalTest()
        {
            //Preparar
            var cuentaAhorro = new CuentaAhorro(numero: "10001", nombre: "Cuenta Ejemplo", ciudad: "Bogot�");
            var movimientosIniciales = cuentaAhorro.CantidadMovimientos();
            // accion
            var resultado = cuentaAhorro.Consignar(50000, "Bogot�");
            DateTime fecha = new DateTime(2008, 6, 1);
            resultado = cuentaAhorro.Retirar(20000, fecha);
            resultado = cuentaAhorro.Consignar(49950, "Valledupar");

            //validacion o verificacion 
            Assert.AreEqual("Su Nuevo Saldo es de $69.950,00 pesos m/c", resultado);
        }
    }
}