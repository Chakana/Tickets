<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Personas.aspx.cs" Inherits="TNT_admin.Personas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
            <asp:BoundField DataField="nombre" HeaderText="nombre" SortExpression="nombre" />
            <asp:BoundField DataField="apellidos" HeaderText="apellidos" SortExpression="apellidos" />
            <asp:BoundField DataField="cedula_identidad" HeaderText="cedula_identidad" SortExpression="cedula_identidad" />
            <asp:BoundField DataField="fecha_nacimiento" HeaderText="fecha_nacimiento" SortExpression="fecha_nacimiento" />
            <asp:BoundField DataField="direccion" HeaderText="direccion" SortExpression="direccion" />
            <asp:BoundField DataField="fecha_registro" HeaderText="fecha_registro" SortExpression="fecha_registro" />
            <asp:BoundField DataField="fecha_modificacion" HeaderText="fecha_modificacion" SortExpression="fecha_modificacion" />
            <asp:BoundField DataField="numero_celular" HeaderText="numero_celular" SortExpression="numero_celular" />
            <asp:BoundField DataField="id_usuario" HeaderText="id_usuario" SortExpression="id_usuario" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:TNTConnectionString %>" DeleteCommand="DELETE FROM [Personas] WHERE [id] = @original_id AND [nombre] = @original_nombre AND [apellidos] = @original_apellidos AND [cedula_identidad] = @original_cedula_identidad AND (([fecha_nacimiento] = @original_fecha_nacimiento) OR ([fecha_nacimiento] IS NULL AND @original_fecha_nacimiento IS NULL)) AND [direccion] = @original_direccion AND [fecha_registro] = @original_fecha_registro AND [fecha_modificacion] = @original_fecha_modificacion AND [numero_celular] = @original_numero_celular AND (([id_usuario] = @original_id_usuario) OR ([id_usuario] IS NULL AND @original_id_usuario IS NULL))" InsertCommand="INSERT INTO [Personas] ([nombre], [apellidos], [cedula_identidad], [fecha_nacimiento], [direccion], [fecha_registro], [fecha_modificacion], [numero_celular], [id_usuario]) VALUES (@nombre, @apellidos, @cedula_identidad, @fecha_nacimiento, @direccion, @fecha_registro, @fecha_modificacion, @numero_celular, @id_usuario)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [Personas]" UpdateCommand="UPDATE [Personas] SET [nombre] = @nombre, [apellidos] = @apellidos, [cedula_identidad] = @cedula_identidad, [fecha_nacimiento] = @fecha_nacimiento, [direccion] = @direccion, [fecha_registro] = @fecha_registro, [fecha_modificacion] = @fecha_modificacion, [numero_celular] = @numero_celular, [id_usuario] = @id_usuario WHERE [id] = @original_id AND [nombre] = @original_nombre AND [apellidos] = @original_apellidos AND [cedula_identidad] = @original_cedula_identidad AND (([fecha_nacimiento] = @original_fecha_nacimiento) OR ([fecha_nacimiento] IS NULL AND @original_fecha_nacimiento IS NULL)) AND [direccion] = @original_direccion AND [fecha_registro] = @original_fecha_registro AND [fecha_modificacion] = @original_fecha_modificacion AND [numero_celular] = @original_numero_celular AND (([id_usuario] = @original_id_usuario) OR ([id_usuario] IS NULL AND @original_id_usuario IS NULL))">
        <DeleteParameters>
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_nombre" Type="String" />
            <asp:Parameter Name="original_apellidos" Type="String" />
            <asp:Parameter Name="original_cedula_identidad" Type="String" />
            <asp:Parameter DbType="Date" Name="original_fecha_nacimiento" />
            <asp:Parameter Name="original_direccion" Type="String" />
            <asp:Parameter Name="original_fecha_registro" Type="DateTime" />
            <asp:Parameter Name="original_fecha_modificacion" Type="DateTime" />
            <asp:Parameter Name="original_numero_celular" Type="String" />
            <asp:Parameter Name="original_id_usuario" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="nombre" Type="String" />
            <asp:Parameter Name="apellidos" Type="String" />
            <asp:Parameter Name="cedula_identidad" Type="String" />
            <asp:Parameter DbType="Date" Name="fecha_nacimiento" />
            <asp:Parameter Name="direccion" Type="String" />
            <asp:Parameter Name="fecha_registro" Type="DateTime" />
            <asp:Parameter Name="fecha_modificacion" Type="DateTime" />
            <asp:Parameter Name="numero_celular" Type="String" />
            <asp:Parameter Name="id_usuario" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="nombre" Type="String" />
            <asp:Parameter Name="apellidos" Type="String" />
            <asp:Parameter Name="cedula_identidad" Type="String" />
            <asp:Parameter DbType="Date" Name="fecha_nacimiento" />
            <asp:Parameter Name="direccion" Type="String" />
            <asp:Parameter Name="fecha_registro" Type="DateTime" />
            <asp:Parameter Name="fecha_modificacion" Type="DateTime" />
            <asp:Parameter Name="numero_celular" Type="String" />
            <asp:Parameter Name="id_usuario" Type="Int32" />
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_nombre" Type="String" />
            <asp:Parameter Name="original_apellidos" Type="String" />
            <asp:Parameter Name="original_cedula_identidad" Type="String" />
            <asp:Parameter DbType="Date" Name="original_fecha_nacimiento" />
            <asp:Parameter Name="original_direccion" Type="String" />
            <asp:Parameter Name="original_fecha_registro" Type="DateTime" />
            <asp:Parameter Name="original_fecha_modificacion" Type="DateTime" />
            <asp:Parameter Name="original_numero_celular" Type="String" />
            <asp:Parameter Name="original_id_usuario" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
