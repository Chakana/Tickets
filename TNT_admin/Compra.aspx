<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Compra.aspx.cs" Inherits="TNT_admin.Compra" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" />
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
            <asp:BoundField DataField="fecha_compra" HeaderText="fecha_compra" SortExpression="fecha_compra" />
            <asp:BoundField DataField="id_usuario_compra" HeaderText="id_usuario_compra" SortExpression="id_usuario_compra" />
            <asp:BoundField DataField="codigo_recaudacion" HeaderText="codigo_recaudacion" SortExpression="codigo_recaudacion" />
            <asp:BoundField DataField="pagado" HeaderText="pagado" SortExpression="pagado" />
            <asp:BoundField DataField="fecha_pago" HeaderText="fecha_pago" SortExpression="fecha_pago" />
            <asp:BoundField DataField="monto_cobrar" HeaderText="monto_cobrar" SortExpression="monto_cobrar" />
            <asp:BoundField DataField="numero_factura" HeaderText="numero_factura" SortExpression="numero_factura" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:TNTConnectionString %>" DeleteCommand="DELETE FROM [Compra] WHERE [id] = @original_id AND [fecha_compra] = @original_fecha_compra AND [id_usuario_compra] = @original_id_usuario_compra AND [codigo_recaudacion] = @original_codigo_recaudacion AND [pagado] = @original_pagado AND (([fecha_pago] = @original_fecha_pago) OR ([fecha_pago] IS NULL AND @original_fecha_pago IS NULL)) AND (([monto_cobrar] = @original_monto_cobrar) OR ([monto_cobrar] IS NULL AND @original_monto_cobrar IS NULL)) AND (([numero_factura] = @original_numero_factura) OR ([numero_factura] IS NULL AND @original_numero_factura IS NULL))" InsertCommand="INSERT INTO [Compra] ([fecha_compra], [id_usuario_compra], [codigo_recaudacion], [pagado], [fecha_pago], [monto_cobrar], [numero_factura]) VALUES (@fecha_compra, @id_usuario_compra, @codigo_recaudacion, @pagado, @fecha_pago, @monto_cobrar, @numero_factura)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [Compra]" UpdateCommand="UPDATE [Compra] SET [fecha_compra] = @fecha_compra, [id_usuario_compra] = @id_usuario_compra, [codigo_recaudacion] = @codigo_recaudacion, [pagado] = @pagado, [fecha_pago] = @fecha_pago, [monto_cobrar] = @monto_cobrar, [numero_factura] = @numero_factura WHERE [id] = @original_id AND [fecha_compra] = @original_fecha_compra AND [id_usuario_compra] = @original_id_usuario_compra AND [codigo_recaudacion] = @original_codigo_recaudacion AND [pagado] = @original_pagado AND (([fecha_pago] = @original_fecha_pago) OR ([fecha_pago] IS NULL AND @original_fecha_pago IS NULL)) AND (([monto_cobrar] = @original_monto_cobrar) OR ([monto_cobrar] IS NULL AND @original_monto_cobrar IS NULL)) AND (([numero_factura] = @original_numero_factura) OR ([numero_factura] IS NULL AND @original_numero_factura IS NULL))">
        <DeleteParameters>
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_fecha_compra" Type="DateTime" />
            <asp:Parameter Name="original_id_usuario_compra" Type="Int32" />
            <asp:Parameter Name="original_codigo_recaudacion" Type="String" />
            <asp:Parameter Name="original_pagado" Type="Byte" />
            <asp:Parameter Name="original_fecha_pago" Type="DateTime" />
            <asp:Parameter Name="original_monto_cobrar" Type="Decimal" />
            <asp:Parameter Name="original_numero_factura" Type="String" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="fecha_compra" Type="DateTime" />
            <asp:Parameter Name="id_usuario_compra" Type="Int32" />
            <asp:Parameter Name="codigo_recaudacion" Type="String" />
            <asp:Parameter Name="pagado" Type="Byte" />
            <asp:Parameter Name="fecha_pago" Type="DateTime" />
            <asp:Parameter Name="monto_cobrar" Type="Decimal" />
            <asp:Parameter Name="numero_factura" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="fecha_compra" Type="DateTime" />
            <asp:Parameter Name="id_usuario_compra" Type="Int32" />
            <asp:Parameter Name="codigo_recaudacion" Type="String" />
            <asp:Parameter Name="pagado" Type="Byte" />
            <asp:Parameter Name="fecha_pago" Type="DateTime" />
            <asp:Parameter Name="monto_cobrar" Type="Decimal" />
            <asp:Parameter Name="numero_factura" Type="String" />
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_fecha_compra" Type="DateTime" />
            <asp:Parameter Name="original_id_usuario_compra" Type="Int32" />
            <asp:Parameter Name="original_codigo_recaudacion" Type="String" />
            <asp:Parameter Name="original_pagado" Type="Byte" />
            <asp:Parameter Name="original_fecha_pago" Type="DateTime" />
            <asp:Parameter Name="original_monto_cobrar" Type="Decimal" />
            <asp:Parameter Name="original_numero_factura" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
