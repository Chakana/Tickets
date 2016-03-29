<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tickets.aspx.cs" Inherits="TNT_admin.Tickets" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="codigo" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:CommandField ShowEditButton="True" ShowSelectButton="True" />
            <asp:BoundField DataField="codigo" HeaderText="codigo" ReadOnly="True" SortExpression="codigo" />
            <asp:BoundField DataField="butaca" HeaderText="butaca" SortExpression="butaca" />
            <asp:BoundField DataField="id_evento" HeaderText="id_evento" SortExpression="id_evento" />
            <asp:BoundField DataField="valida" HeaderText="valida" SortExpression="valida" />
            <asp:CheckBoxField DataField="utilizada" HeaderText="utilizada" SortExpression="utilizada" />
            <asp:BoundField DataField="id_sector" HeaderText="id_sector" SortExpression="id_sector" />
            <asp:BoundField DataField="codigo_recaudacion" HeaderText="codigo_recaudacion" SortExpression="codigo_recaudacion" />
            <asp:BoundField DataField="fecha_uso" HeaderText="fecha_uso" SortExpression="fecha_uso" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:TNTConnectionString %>" DeleteCommand="DELETE FROM [Ticket] WHERE [codigo] = @original_codigo AND (([butaca] = @original_butaca) OR ([butaca] IS NULL AND @original_butaca IS NULL)) AND [id_evento] = @original_id_evento AND (([valida] = @original_valida) OR ([valida] IS NULL AND @original_valida IS NULL)) AND [utilizada] = @original_utilizada AND (([id_sector] = @original_id_sector) OR ([id_sector] IS NULL AND @original_id_sector IS NULL)) AND (([codigo_recaudacion] = @original_codigo_recaudacion) OR ([codigo_recaudacion] IS NULL AND @original_codigo_recaudacion IS NULL)) AND (([fecha_uso] = @original_fecha_uso) OR ([fecha_uso] IS NULL AND @original_fecha_uso IS NULL))" InsertCommand="INSERT INTO [Ticket] ([codigo], [butaca], [id_evento], [valida], [utilizada], [id_sector], [codigo_recaudacion], [fecha_uso]) VALUES (@codigo, @butaca, @id_evento, @valida, @utilizada, @id_sector, @codigo_recaudacion, @fecha_uso)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [Ticket]" UpdateCommand="UPDATE [Ticket] SET [butaca] = @butaca, [id_evento] = @id_evento, [valida] = @valida, [utilizada] = @utilizada, [id_sector] = @id_sector, [codigo_recaudacion] = @codigo_recaudacion, [fecha_uso] = @fecha_uso WHERE [codigo] = @original_codigo AND (([butaca] = @original_butaca) OR ([butaca] IS NULL AND @original_butaca IS NULL)) AND [id_evento] = @original_id_evento AND (([valida] = @original_valida) OR ([valida] IS NULL AND @original_valida IS NULL)) AND [utilizada] = @original_utilizada AND (([id_sector] = @original_id_sector) OR ([id_sector] IS NULL AND @original_id_sector IS NULL)) AND (([codigo_recaudacion] = @original_codigo_recaudacion) OR ([codigo_recaudacion] IS NULL AND @original_codigo_recaudacion IS NULL)) AND (([fecha_uso] = @original_fecha_uso) OR ([fecha_uso] IS NULL AND @original_fecha_uso IS NULL))">
        <DeleteParameters>
            <asp:Parameter Name="original_codigo" Type="String" />
            <asp:Parameter Name="original_butaca" Type="String" />
            <asp:Parameter Name="original_id_evento" Type="Int32" />
            <asp:Parameter Name="original_valida" Type="Byte" />
            <asp:Parameter Name="original_utilizada" Type="Boolean" />
            <asp:Parameter Name="original_id_sector" Type="Int32" />
            <asp:Parameter Name="original_codigo_recaudacion" Type="String" />
            <asp:Parameter Name="original_fecha_uso" Type="DateTime" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="codigo" Type="String" />
            <asp:Parameter Name="butaca" Type="String" />
            <asp:Parameter Name="id_evento" Type="Int32" />
            <asp:Parameter Name="valida" Type="Byte" />
            <asp:Parameter Name="utilizada" Type="Boolean" />
            <asp:Parameter Name="id_sector" Type="Int32" />
            <asp:Parameter Name="codigo_recaudacion" Type="String" />
            <asp:Parameter Name="fecha_uso" Type="DateTime" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="butaca" Type="String" />
            <asp:Parameter Name="id_evento" Type="Int32" />
            <asp:Parameter Name="valida" Type="Byte" />
            <asp:Parameter Name="utilizada" Type="Boolean" />
            <asp:Parameter Name="id_sector" Type="Int32" />
            <asp:Parameter Name="codigo_recaudacion" Type="String" />
            <asp:Parameter Name="fecha_uso" Type="DateTime" />
            <asp:Parameter Name="original_codigo" Type="String" />
            <asp:Parameter Name="original_butaca" Type="String" />
            <asp:Parameter Name="original_id_evento" Type="Int32" />
            <asp:Parameter Name="original_valida" Type="Byte" />
            <asp:Parameter Name="original_utilizada" Type="Boolean" />
            <asp:Parameter Name="original_id_sector" Type="Int32" />
            <asp:Parameter Name="original_codigo_recaudacion" Type="String" />
            <asp:Parameter Name="original_fecha_uso" Type="DateTime" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
