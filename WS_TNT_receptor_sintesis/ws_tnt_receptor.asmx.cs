using System.Web.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.IO;




namespace WS_TNT_receptor_sintesis
{
    /// <summary>
    /// Descripción breve de ws_tnt_receptor
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class ws_tnt_receptor : System.Web.Services.WebService
    {
        void POST(string url, string jsonContent)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                }
            }
            catch (WebException ex)
            {
                StreamWriter sw = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath("~") + "log_1", true);
                sw.WriteLine(DateTime.Now.ToString() + "request error,COD_REC:" + jsonContent + " error:" + ex.Message);
                sw.Flush();
                sw.Close();
            }
        }

        [WebMethod]
        public RespTransaccion datosTransaccion(WsTransaccion datos, string user, string password)
        {
            // datos contiene toda la información enviada de retorno 
            // aca se puede vaciar los datos de retorno a una BD por ejemplo
            // datos.Transaccion
            RespTransaccion respuestaWS = new RespTransaccion();
            //Utilizar una codificación para los errores. 
            string url_envio = System.Configuration.ConfigurationManager.AppSettings["url_envio_factura"];
            string user_sintesis = System.Configuration.ConfigurationManager.AppSettings["user_sintesis"];
            string user_password = System.Configuration.ConfigurationManager.AppSettings["password"];

            if ((user.Equals(user_sintesis)) && (password.Equals(password)))
            {
                if (datos.Transaccion == "P")
                {
                    bool exito = DA_TNT.ws_actualiza_pago(datos.CodigoRecaudacion, datos.NroRentaRecibo, datos.Fecha.ToString(), datos.Hora.ToString());
                    if (exito)
                    {
                        POST(url_envio, "{\"codigo_recaudacion\":\"" + datos.CodigoRecaudacion + "\"}");
                        //enviar correo con ticket
                        //enviar factura
                        respuestaWS.CodError = 0;
                        respuestaWS.Descripcion = "OK datos enviados:" + datos.Agencia + "|" + datos.Ciudad + "|" + datos.CodigoAutorizacion
                            + "|" + datos.CodigoControl + "|" + datos.CodigoEmpresa + "|" + datos.CodigoProducto + "|" + datos.CodigoRecaudacion
                            + "|" + datos.Departamento + "|" + datos.Entidad + "|" + datos.Fecha + "|" + datos.Hora + "|" + datos.LoteDosificacion
                            + "|" + datos.Monto + "|" + datos.MontoCreditoFiscal + "|" + datos.NitFacturar + "|" + datos.NombreFacturar
                            + "|" + datos.NroRentaRecibo + "|" + datos.NumeroPago + "|" + datos.Operador + "|" + datos.OrigenTransaccion
                            + "|" + datos.Pais + "|" + datos.Secuencial + "|" + datos.Transaccion;
                    }
                    else
                    {
                        respuestaWS.CodError = 1;
                        respuestaWS.Descripcion = "Error:" + datos.Agencia + "|" + datos.Ciudad + "|" + datos.CodigoAutorizacion
                            + "|" + datos.CodigoControl + "|" + datos.CodigoEmpresa + "|" + datos.CodigoProducto + "|" + datos.CodigoRecaudacion
                            + "|" + datos.Departamento + "|" + datos.Entidad + "|" + datos.Fecha + "|" + datos.Hora + "|" + datos.LoteDosificacion
                            + "|" + datos.Monto + "|" + datos.MontoCreditoFiscal + "|" + datos.NitFacturar + "|" + datos.NombreFacturar
                            + "|" + datos.NroRentaRecibo + "|" + datos.NumeroPago + "|" + datos.Operador + "|" + datos.OrigenTransaccion
                            + "|" + datos.Pais + "|" + datos.Secuencial + "|" + datos.Transaccion;
                    }
                }
                else
                {
                    respuestaWS.CodError = 0;
                    respuestaWS.Descripcion = "OK REVERSION enviada:" + datos.Agencia + "|" + datos.Ciudad + "|" + datos.CodigoAutorizacion
                        + "|" + datos.CodigoControl + "|" + datos.CodigoEmpresa + "|" + datos.CodigoProducto + "|" + datos.CodigoRecaudacion
                        + "|" + datos.Departamento + "|" + datos.Entidad + "|" + datos.Fecha + "|" + datos.Hora + "|" + datos.LoteDosificacion
                        + "|" + datos.Monto + "|" + datos.MontoCreditoFiscal + "|" + datos.NitFacturar + "|" + datos.NombreFacturar
                        + "|" + datos.NroRentaRecibo + "|" + datos.NumeroPago + "|" + datos.Operador + "|" + datos.OrigenTransaccion
                        + "|" + datos.Pais + "|" + datos.Secuencial + "|" + datos.Transaccion;
                }
               
                               
            }
            else
            {

                respuestaWS.CodError = 99;
                respuestaWS.Descripcion = "Usuario o Contraseña erroneos";

            }

            return respuestaWS;
        }

       
    }
   
    [Serializable]
    public class WsTransaccion
    {
        public int CodigoEmpresa { get; set; }
        public string CodigoRecaudacion { get; set; }
        public string CodigoProducto { get; set; }
        public int NumeroPago { get; set; }
        public int Fecha { get; set; }
        public int Secuencial { get; set; }
        public int Hora { get; set; }
        public string OrigenTransaccion { get; set; }
        public int Pais { get; set; }
        public int Departamento { get; set; }
        public int Ciudad { get; set; }
        public string Entidad { get; set; }
        public string Agencia { get; set; }
        public int Operador { get; set; }
        public double Monto { get; set; }

        public int LoteDosificacion { get; set; }
        public string NroRentaRecibo { get; set; }
        public double MontoCreditoFiscal { get; set; }
        public string CodigoAutorizacion { get; set; }
        public string CodigoControl { get; set; }
        public string NitFacturar { get; set; }
        public string NombreFacturar { get; set; }
        public string Transaccion { get; set; }

        public WsTransaccion() { }


        public WsTransaccion(int _CodigoEmpresa, string _CodigoRecaudacion, string _CodigoProducto, int _NumeroPago, int _Fecha, int _Secuencial, int _Hora, string _OrigenTransaccion, int _Pais, int _Departamento, int _Ciudad, string _Entidad, string _Agencia, int _Operador, double _Monto, int _LoteDosificacion, string _NroRentaRecibo, double _MontoCreditoFiscal, string _CodigoAutorizacion, string _CodigoControl, string _NitFacturar, string _NombreFacturar, string _Transaccion)
        {
            CodigoEmpresa = _CodigoEmpresa;
            CodigoRecaudacion = _CodigoRecaudacion;
            CodigoProducto = _CodigoProducto;
            NumeroPago = _NumeroPago;
            Fecha = _Fecha;
            Secuencial = _Secuencial;
            Hora = _Hora;
            OrigenTransaccion = _OrigenTransaccion;
            Pais = _Pais;
            Departamento = _Departamento;
            Ciudad = _Ciudad;
            Entidad = _Entidad;
            Agencia = _Agencia;
            Operador = _Operador;
            Monto = _Monto;
            LoteDosificacion = _LoteDosificacion;
            NroRentaRecibo = _NroRentaRecibo;
            MontoCreditoFiscal = _MontoCreditoFiscal;
            CodigoAutorizacion = _CodigoAutorizacion;
            CodigoControl = _CodigoControl;
            NitFacturar = _NitFacturar;
            NombreFacturar = _NombreFacturar;
            Transaccion = _Transaccion;
        }
    }
    public class RespTransaccion
    {
        public int CodError { get; set; }
        public string Descripcion { get; set; }

        public RespTransaccion() { }

        public RespTransaccion(int _CodError, string _Descripcion)
        {
            CodError = _CodError;
            Descripcion = _Descripcion;
        }
    }
}

/*
codigoEmpresa	Num	9	Código único de empresa
codigoRecaudación	Char 	20	Código único de la recaudación
codigoProducto	Num	6	Código de producto
númeroPago	Num	9	Numero del pago por recaudación (1, 2, 3, N)
fecha	Num	8	AAAAMMDD (Fecha de la transacción)
secuencial	Num	8	Secuencial de la transacción diaria
hora	Num	6	HHMMSS, hora de la transacción
origenTransaccion	Char 	4	Origen de la transacción (Ver tabla de orígenes)
pais	Num	4	Codigo de pais donde se realizo la transacción (Ver tabla de paises)
departamento	Num	2	Código del departamento donde se efectúo la transacción (Ver tabla)
ciudad	Num	3	Código del ciudad donde se efectúo la transacción (Ver tabla)
entidad	Char 	4	Código de la entidad donde se efectuó la transacción (Ver tabla)
agencia	Char 	2	Código de la agencia donde se efectuó la transacción (Ver tabla)
operador	Num	6	Código del operador donde se efectuó la transacción (Ver tabla)
monto 	Num	7.2	Monto de la transacción
loteDosificacion	Num	6	Numero del lote de dosificación (solo para facturas)
nroRentaRecibo	Num	15	Numero de factura o recibo de la transacción
montoCreditoFiscal	Num	7.2	Cuando sea factura, válido para credito fiscal, SIEMPRE EN MONEDA NACIONAL Bolivianos
codigoAutorizacion	Char 	15	Codigo de autorizacion (Solo para facturas)
codigoControl	Char 	20	Codigo de control (solo para facturas)
nitFacturar	Char 	15	Nit a facturar (Solo para facturas)
nombreFacturar	Char 	60	Nombre a facturar (Solo para facturas)
Transacción	Char 	1	P=Pago, R=Reversión
*/