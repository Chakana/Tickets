<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TiposEvento.aspx.cs" Inherits="TNT_admin.TiposEvento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:CommandField ShowEditButton="True" ShowSelectButton="True" />
            <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
            <asp:BoundField DataField="descripcion" HeaderText="descripcion" SortExpression="descripcion" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:TNTConnectionString %>" DeleteCommand="DELETE FROM [Tipos_evento] WHERE [id] = @original_id AND [descripcion] = @original_descripcion" InsertCommand="INSERT INTO [Tipos_evento] ([descripcion]) VALUES (@descripcion)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [Tipos_evento]" UpdateCommand="UPDATE [Tipos_evento] SET [descripcion] = @descripcion WHERE [id] = @original_id AND [descripcion] = @original_descripcion">
        <DeleteParameters>
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_descripcion" Type="String" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="descripcion" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="descripcion" Type="String" />
            <asp:Parameter Name="original_id" Type="Int32" />
            <asp:Parameter Name="original_descripcion" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
