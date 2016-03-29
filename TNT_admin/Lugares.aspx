<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Lugares.aspx.cs" Inherits="TNT_admin.Lugares" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" />
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
            <asp:BoundField DataField="nombre_lugar" HeaderText="nombre_lugar" SortExpression="nombre_lugar" />
            <asp:BoundField DataField="direccion" HeaderText="direccion" SortExpression="direccion" />
            <asp:BoundField DataField="capacidad_maxima" HeaderText="capacidad_maxima" SortExpression="capacidad_maxima" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:TNTConnectionString %>" DeleteCommand="DELETE FROM [Lugares] WHERE [id] = @original_id AND [nombre_lugar] = @original_nombre_lugar AND [direccion] = @original_direccion AND [capacidad_maxima] = @original_capacidad_maxima" InsertCommand="INSERT INTO [Lugares] ([nombre_lugar], [direccion], [capacidad_maxima]) VALUES (@nombre_lugar, @direccion, @capacidad_maxima)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [Lugares]" UpdateCommand="UPDATE [Lugares] SET [nombre_lugar] = @nombre_lugar, [direccion] = @direccion, [capacidad_maxima] = @capacidad_maxima WHERE [id] = @original_id AND [nombre_lugar] = @original_nombre_lugar AND [direccion] = @original_direccion AND [capacidad_maxima] = @original_capacidad_maxima">
        <DeleteParameters>
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_nombre_lugar" Type="String" />
            <asp:Parameter Name="original_direccion" Type="String" />
            <asp:Parameter Name="original_capacidad_maxima" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="nombre_lugar" Type="String" />
            <asp:Parameter Name="direccion" Type="String" />
            <asp:Parameter Name="capacidad_maxima" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="nombre_lugar" Type="String" />
            <asp:Parameter Name="direccion" Type="String" />
            <asp:Parameter Name="capacidad_maxima" Type="Int32" />
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_nombre_lugar" Type="String" />
            <asp:Parameter Name="original_direccion" Type="String" />
            <asp:Parameter Name="original_capacidad_maxima" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
