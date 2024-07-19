namespace Entradas.Shared
{
    public class Constants
    {

        public static class EstadosOrden
        {
            public const string REGISTRADO = "REGISTRADO";
            public const string PROCESANDO = "PROCESANDO";
            public const string RECHAZADO = "RECHAZADO";
            public const string ANULADO = "ANULADO";
            public const string APROBADO = "APROBADO";
        }

        public static class MediosPago
        {
            public const string YAPE = "YAPE";
            public const string PLIN = "PLIN";
            public const string TRANSFERENCIA = "TRANSFERENCIA";
            public const string DEPOSITO = "DEPOSITO";
        }
    }
}
