<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Evento.aspx.cs" Inherits="TNT_admin.Evento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" />
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
            <asp:BoundField DataField="nombre_evento" HeaderText="nombre_evento" SortExpression="nombre_evento" />
            <asp:BoundField DataField="id_lugar" HeaderText="id_lugar" SortExpression="id_lugar" />
            <asp:BoundField DataField="fecha_hora_evento" HeaderText="fecha_hora_evento" SortExpression="fecha_hora_evento" />
            <asp:BoundField DataField="id_empresa" HeaderText="id_empresa" SortExpression="id_empresa" />
            <asp:BoundField DataField="id_tipo_evento" HeaderText="id_tipo_evento" SortExpression="id_tipo_evento" />
        </Columns>
</asp:GridView>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:TNTConnectionString %>" DeleteCommand="DELETE FROM [Eventos] WHERE [id] = @original_id AND [nombre_evento] = @original_nombre_evento AND [id_lugar] = @original_id_lugar AND [fecha_hora_evento] = @original_fecha_hora_evento AND [id_empresa] = @original_id_empresa AND [id_tipo_evento] = @original_id_tipo_evento" InsertCommand="INSERT INTO [Eventos] ([nombre_evento], [id_lugar], [fecha_hora_evento], [id_empresa], [id_tipo_evento]) VALUES (@nombre_evento, @id_lugar, @fecha_hora_evento, @id_empresa, @id_tipo_evento)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [id], [nombre_evento], [id_lugar], [fecha_hora_evento], [id_empresa], [id_tipo_evento] FROM [Eventos]" UpdateCommand="UPDATE [Eventos] SET [nombre_evento] = @nombre_evento, [id_lugar] = @id_lugar, [fecha_hora_evento] = @fecha_hora_evento, [id_empresa] = @id_empresa, [id_tipo_evento] = @id_tipo_evento WHERE [id] = @original_id AND [nombre_evento] = @original_nombre_evento AND [id_lugar] = @original_id_lugar AND [fecha_hora_evento] = @original_fecha_hora_evento AND [id_empresa] = @original_id_empresa AND [id_tipo_evento] = @original_id_tipo_evento">
    <DeleteParameters>
        <asp:Parameter Name="original_id" Type="Int32" />
        <asp:Parameter Name="original_nombre_evento" Type="String" />
        <asp:Parameter Name="original_id_lugar" Type="Int32" />
        <asp:Parameter Name="original_fecha_hora_evento" Type="DateTime" />
        <asp:Parameter Name="original_id_empresa" Type="Int32" />
        <asp:Parameter Name="original_id_tipo_evento" Type="Int32" />
    </DeleteParameters>
    <InsertParameters>
        <asp:Parameter Name="nombre_evento" Type="String" />
        <asp:Parameter Name="id_lugar" Type="Int32" />
        <asp:Parameter Name="fecha_hora_evento" Type="DateTime" />
        <asp:Parameter Name="id_empresa" Type="Int32" />
        <asp:Parameter Name="id_tipo_evento" Type="Int32" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="nombre_evento" Type="String" />
        <asp:Parameter Name="id_lugar" Type="Int32" />
        <asp:Parameter Name="fecha_hora_evento" Type="DateTime" />
        <asp:Parameter Name="id_empresa" Type="Int32" />
        <asp:Parameter Name="id_tipo_evento" Type="Int32" />
        <asp:Parameter Name="original_id" Type="Int32" />
        <asp:Parameter Name="original_nombre_evento" Type="String" />
        <asp:Parameter Name="original_id_lugar" Type="Int32" />
        <asp:Parameter Name="original_fecha_hora_evento" Type="DateTime" />
        <asp:Parameter Name="original_id_empresa" Type="Int32" />
        <asp:Parameter Name="original_id_tipo_evento" Type="Int32" />
    </UpdateParameters>
</asp:SqlDataSource>
</asp:Content>
