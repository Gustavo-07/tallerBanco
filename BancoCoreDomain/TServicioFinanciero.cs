using System;

namespace BancoCoreDomain
{
    public interface TServicioFinanciero
    {
        string Numero { get; }

        string Ciudad { get; }

        decimal Saldo { get; }

        string Consignar(decimal valorConsingacion, string CiudadRemitente);

        string Retirar(decimal valorRetiro, DateTime fechaSolicitudRetiro);

    }
}
