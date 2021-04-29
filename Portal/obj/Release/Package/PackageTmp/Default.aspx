<%@ Page Title="CB Portal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Portal._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>CodeBlue Portal</h1>
        <p class="lead">Where things happen.</p>
        <br />
        <p><a href="https://www.codeblue360.com/" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Stuff</h2>
            <p>
                Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel.
            </p>
        </div>
        <div class="col-md-4">
            <h2>Things</h2>
            <p>
                Tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel.
            </p>
        </div>
        <div class="col-md-4">
            <h2>Featured Press Releases</h2>
            <div id="featuredPressReleases" runat="server"></div>
            <a href="/cms/press-release">All Press Releases</a>
        </div>
    </div>

</asp:Content>
