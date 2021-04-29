<%@ Page Language="C#" Title="CMS Article" MasterPageFile="~/CMS/Views/CMS.Master" AutoEventWireup="true" CodeBehind="article.aspx.cs" Inherits="Portal.CMS.Views.article" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ArticleContent" runat="server">
    <article>
        <div class="text-center"><asp:Label ID="ArticleImage" runat="server"></asp:Label></div>
        <h1 class="text-center"><asp:Label ID="ArticleTitle" runat="server" Text="Label"></asp:Label></h1>
        <h2 class="text-center"><asp:Label ID="ArticleSubtitle" runat="server" Text="Label"></asp:Label></h2>
        <p class="text-justify"><asp:Label ID="ArticleBody" runat="server" Text="Label"></asp:Label></p>
        <h5 class="text-right"><asp:Label ID="ArticleAuthor" runat="server" Text="Label"></asp:Label></h5>
        <h6 class="text-right"><asp:Label ID="ArticlePublishedDate" runat="server" Text="Label"></asp:Label></h6>
        <div id="ArticleTagContainer" runat="server"></div>
    </article>
    <asp:Label ID="ArticleEditButton" runat="server"></asp:Label>
</asp:Content>
