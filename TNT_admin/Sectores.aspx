<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Sectores.aspx.cs" Inherits="TNT_admin.Sectores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:CommandField ShowEditButton="True" />
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
            <asp:BoundField DataField="descripcion" HeaderText="descripcion" SortExpression="descripcion" />
            <asp:BoundField DataField="precio_unitario" HeaderText="precio_unitario" SortExpression="precio_unitario" />
            <asp:BoundField DataField="id_evento" HeaderText="id_evento" SortExpression="id_evento" />
            <asp:BoundField DataField="asientos_disponibles" HeaderText="asientos_disponibles" SortExpression="asientos_disponibles" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:TNTConnectionString %>" DeleteCommand="DELETE FROM [sectores] WHERE [id] = @original_id AND [descripcion] = @original_descripcion AND [precio_unitario] = @original_precio_unitario AND [id_evento] = @original_id_evento AND [asientos_disponibles] = @original_asientos_disponibles" InsertCommand="INSERT INTO [sectores] ([descripcion], [precio_unitario], [id_evento], [asientos_disponibles]) VALUES (@descripcion, @precio_unitario, @id_evento, @asientos_disponibles)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [sectores]" UpdateCommand="UPDATE [sectores] SET [descripcion] = @descripcion, [precio_unitario] = @precio_unitario, [id_evento] = @id_evento, [asientos_disponibles] = @asientos_disponibles WHERE [id] = @original_id AND [descripcion] = @original_descripcion AND [precio_unitario] = @original_precio_unitario AND [id_evento] = @original_id_evento AND [asientos_disponibles] = @original_asientos_disponibles">
        <DeleteParameters>
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_descripcion" Type="String" />
            <asp:Parameter Name="original_precio_unitario" Type="Decimal" />
            <asp:Parameter Name="original_id_evento" Type="Int32" />
            <asp:Parameter Name="original_asientos_disponibles" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="descripcion" Type="String" />
            <asp:Parameter Name="precio_unitario" Type="Decimal" />
            <asp:Parameter Name="id_evento" Type="Int32" />
            <asp:Parameter Name="asientos_disponibles" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="descripcion" Type="String" />
            <asp:Parameter Name="precio_unitario" Type="Decimal" />
            <asp:Parameter Name="id_evento" Type="Int32" />
            <asp:Parameter Name="asientos_disponibles" Type="Int32" />
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_descripcion" Type="String" />
            <asp:Parameter Name="original_precio_unitario" Type="Decimal" />
            <asp:Parameter Name="original_id_evento" Type="Int32" />
            <asp:Parameter Name="original_asientos_disponibles" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
