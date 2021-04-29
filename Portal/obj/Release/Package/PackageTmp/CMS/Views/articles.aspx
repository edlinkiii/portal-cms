<%@ Page Language="C#" Title="CMS Article List" MasterPageFile="~/CMS/Views/CMS.Master" AutoEventWireup="true" CodeBehind="articles.aspx.cs" Inherits="Portal.CMS.Views.articles" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ArticleContent" runat="server">
    <section id="ArticleList">
        <div class="container">
            <div class="row">
                <div class="col-lg-9 col-md-10 col-sm-12" id="ArticleListContainer" runat="server"></div>
            </div>
        </div>
    </section>
    <script>
        let autocomplete = new Autocomplete({
            matchRequired: false,
            input: null,
            target: null,
            width: null,
            height: null,
            searchPlaceholder: '__QUERY__',
            url: "/CMS/Ajax/tags.aspx/FilterTagList?filter=__QUERY__",
        });
    </script>
</asp:Content>
