<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TNT_admin._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
   
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="jumbotron">
       <h1>TNT Admin Page</h1>
       <p>
           Pagina administrativa de TNT
       </p>
       <p>
           <a class="btn btn-lg btn-primary" runat="server" href="~/Account/Login" role="button">Iniciar sesión</a>
       </p>
   </div>
</asp:Content>
