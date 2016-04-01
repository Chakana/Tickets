<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Empresas.aspx.cs" Inherits="TNT_admin.Empresas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" />
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
            <asp:BoundField DataField="nombre_empresa" HeaderText="nombre_empresa" SortExpression="nombre_empresa" />
            <asp:BoundField DataField="nit" HeaderText="nit" SortExpression="nit" />
            <asp:BoundField DataField="direccion" HeaderText="direccion" SortExpression="direccion" />
            <asp:BoundField DataField="representante_legal" HeaderText="representante_legal" SortExpression="representante_legal" />
            <asp:BoundField DataField="telefono" HeaderText="telefono" SortExpression="telefono" />
            <asp:BoundField DataField="fecha_registro" HeaderText="fecha_registro" SortExpression="fecha_registro" />
            <asp:BoundField DataField="id_usuario" HeaderText="id_usuario" SortExpression="id_usuario" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:TNTConnectionString %>" DeleteCommand="DELETE FROM [Empresas] WHERE [id] = @original_id AND [nombre_empresa] = @original_nombre_empresa AND [nit] = @original_nit AND [direccion] = @original_direccion AND [representante_legal] = @original_representante_legal AND [telefono] = @original_telefono AND (([fecha_registro] = @original_fecha_registro) OR ([fecha_registro] IS NULL AND @original_fecha_registro IS NULL)) AND [id_usuario] = @original_id_usuario" InsertCommand="INSERT INTO [Empresas] ([nombre_empresa], [nit], [direccion], [representante_legal], [telefono], [fecha_registro], [id_usuario]) VALUES (@nombre_empresa, @nit, @direccion, @representante_legal, @telefono, @fecha_registro, @id_usuario)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [Empresas]" UpdateCommand="UPDATE [Empresas] SET [nombre_empresa] = @nombre_empresa, [nit] = @nit, [direccion] = @direccion, [representante_legal] = @representante_legal, [telefono] = @telefono, [fecha_registro] = @fecha_registro, [id_usuario] = @id_usuario WHERE [id] = @original_id AND [nombre_empresa] = @original_nombre_empresa AND [nit] = @original_nit AND [direccion] = @original_direccion AND [representante_legal] = @original_representante_legal AND [telefono] = @original_telefono AND (([fecha_registro] = @original_fecha_registro) OR ([fecha_registro] IS NULL AND @original_fecha_registro IS NULL)) AND [id_usuario] = @original_id_usuario">
        <DeleteParameters>
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_nombre_empresa" Type="String" />
            <asp:Parameter Name="original_nit" Type="String" />
            <asp:Parameter Name="original_direccion" Type="String" />
            <asp:Parameter Name="original_representante_legal" Type="String" />
            <asp:Parameter Name="original_telefono" Type="String" />
            <asp:Parameter Name="original_fecha_registro" Type="DateTime" />
            <asp:Parameter Name="original_id_usuario" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="nombre_empresa" Type="String" />
            <asp:Parameter Name="nit" Type="String" />
            <asp:Parameter Name="direccion" Type="String" />
            <asp:Parameter Name="representante_legal" Type="String" />
            <asp:Parameter Name="telefono" Type="String" />
            <asp:Parameter Name="fecha_registro" Type="DateTime" />
            <asp:Parameter Name="id_usuario" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="nombre_empresa" Type="String" />
            <asp:Parameter Name="nit" Type="String" />
            <asp:Parameter Name="direccion" Type="String" />
            <asp:Parameter Name="representante_legal" Type="String" />
            <asp:Parameter Name="telefono" Type="String" />
            <asp:Parameter Name="fecha_registro" Type="DateTime" />
            <asp:Parameter Name="id_usuario" Type="Int32" />
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_nombre_empresa" Type="String" />
            <asp:Parameter Name="original_nit" Type="String" />
            <asp:Parameter Name="original_direccion" Type="String" />
            <asp:Parameter Name="original_representante_legal" Type="String" />
            <asp:Parameter Name="original_telefono" Type="String" />
            <asp:Parameter Name="original_fecha_registro" Type="DateTime" />
            <asp:Parameter Name="original_id_usuario" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
