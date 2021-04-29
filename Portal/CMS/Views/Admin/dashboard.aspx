<%@ Page Language="C#" Title="Dashboard" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="Portal.CMS.Views.Admin.dashboard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <section>
        <div class="container">
            <div class="row">
                <div class="col-lg-12">
                    <h1 class="d-block p-5 m-5 text-center"><asp:Label ID="Headline" runat="server" Text="" /></h1>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 text-center mb-5">
                    <a href="add-new" class="btn btn-primary btn-block" role="button" title="Add New Article">Add New</a>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 table-responsive-xl">
                    <table id="ArticleTable" class="w-100 table table-striped">
                        <thead id="ArticleTableHeader" class="thead-dark" runat="server"></thead>
                        <tbody id="ArticleTableBody" class="" runat="server"></tbody>
                    </table>
                </div>
            </div>
        </div>
    </section>
    <script>
        $(document).on("change", ".switch.is-featured input", function () {
            let id = $(this).data("id");
            let checked = $(this).prop("checked");
            $.ajax({
                url: "/CMS/Views/Admin/dashboard.aspx/SetIsFeatured",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify({
                    articleId: id,
                    isFeatured: checked ? "true" : "false"
                }),
                success: (data) => {
                    console.log(data);
                    $(".switch.is-archived input[data-id='" + id + "']").attr('checked', false).removeAttr('checked');
                    $(this).attr('checked', checked);
                },
                error: (data) => {
                    console.log(data);
                }
            });
        });
        $(document).on("change", ".switch.is-archived input", function () {
            let id = $(this).data("id");
            let checked = $(this).prop("checked");
            $.ajax({
                url: "/CMS/Views/Admin/dashboard.aspx/SetIsArchived",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify({
                    articleId: id,
                    isArchived: checked ? "true" : "false"
                }),
                success: (data) => {
                    console.log(data);
                    $(".switch.is-featured input[data-id='" + id + "']").attr('checked', false).removeAttr('checked');
                    $(this).attr('checked', checked);
                },
                error: (data) => {
                    console.log(data);
                }
            });
        });
        $(document).on("click", "th.sortable", function () {
            window.location = "?order=" + $(this).attr("data-order") + "&dir=" + $(this).attr("data-dir");
        });
    </script>
</asp:Content>
