using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tnt_receptor_tst
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ServiceReference1.ws_tnt_receptorSoapClient cli = new ServiceReference1.ws_tnt_receptorSoapClient();
            ServiceReference1.WsTransaccion req = new ServiceReference1.WsTransaccion();
            req.Agencia = "AGE1";
            req.Ciudad = 1;
            req.CodigoAutorizacion = "1111010101";
            req.CodigoControl = "920292";
            req.CodigoEmpresa = 1;
            req.CodigoProducto = "1";
            req.CodigoRecaudacion = "WMIDYXFOPNF1ZIRLSYN6";
            req.Departamento = 1;
            req.Entidad = "BNB";
            req.Fecha = 20151129;
            req.Hora = 150000;
            req.LoteDosificacion = 1;
            req.Monto = 10;
            req.MontoCreditoFiscal = 1;
            req.NitFacturar = "202020202";
            req.NombreFacturar = "LEOQ";
            req.NroRentaRecibo = "123";
            req.NumeroPago = 1;
            req.Operador = 1;
            req.OrigenTransaccion = "CAJA";
            req.Pais = 1;
            req.Secuencial = 1;
            req.Transaccion = "TRA1";

            ServiceReference1.RespTransaccion resp = cli.datosTransaccion(req, "sintesis", "a1b1c1d1");
            TextBox1.Text = resp.CodError.ToString();
            TextBox2.Text = resp.Descripcion;
        }
    }
}