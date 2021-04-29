<%@ Page validateRequest="false" Language="C#" Title="CMS Form" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="form.aspx.cs" Inherits="Portal.CMS.Views.Admin.form" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://cdn.tiny.cloud/1/nn9xnln9qz2g6yfny9hzz3g8qvkwwwhjyuigntc4pv52eqt2/tinymce/5/tinymce.min.js" referrerpolicy="origin"></script>
    <section>
        <div id="ArticleForm" class="container" runat="server">
            <h1 class="d-block m-5 text-center"><asp:Label ID="Headline" runat="server" Text="" /></h1>
            <asp:HiddenField ID="ArticleID" runat="server" />
            <asp:HiddenField ID="CategoryID" runat="server" />
            <asp:HiddenField ID="AuthorID" runat="server" />
            <asp:HiddenField ID="ArticleSlug" runat="server" />
            <asp:HiddenField ID="IsAddNew" runat="server" />
            <div class="form-group">
                <label for="ArticleTitle">Title</label>
                <asp:TextBox ID="ArticleTitle" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="ArticleSubtitle">Subtitle</label>
                <asp:TextBox ID="ArticleSubtitle" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="ArticleBody">Body</label>
                <asp:TextBox ID="ArticleBody" CssClass="form-control" TextMode="MultiLine" runat="server" Rows="20"></asp:TextBox><br />
                <a id="add-image-button" class="ml-2 btn btn-primary btn-sm text-white font-weight-bold">Browse Uploads</a>
            </div>
            <div class="form-group">
                <label>Uploads <a id="add-file-button" class="ml-2 btn btn-primary btn-sm text-white font-weight-bold" style="height: 25px !important; line-height: 15px;">+</a></label>
                <div id="FileDropzone" class="form-control" runat="server"></div>
                <input type="file" id="file-upload-input" multiple style="display:none;" />
            </div>
            <div class="form-group">
                <label for="ArticleFeaturedImage">Featured Image</label>
                <asp:TextBox ID="ArticleFeaturedImage" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="ArticleExcerpt">Excerpt</label>
                <asp:TextBox ID="ArticleExcerpt" CssClass="form-control" TextMode="MultiLine" runat="server" Rows="5"></asp:TextBox>
            </div>
            <!--
            <div class="form-group">
                <label for="ArticleSlug">URL Slug</label>
                <asp:TextBox ID="ArticleSlugDisplay" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
            </div>
            -->
            <div class="form-group">
                <label for="ArticleTagDisplay">Tags <a id="add-tag-button" class="ml-2 btn btn-primary btn-sm text-white font-weight-bold" style="height: 25px !important; line-height: 15px;">+</a></label>
                <div ID="ArticleTagDisplay" class="form-control auto-height" runat="server"></div>
            </div>
            <div class="form-group">
                <label for="ArticlePublished">Published</label>
                <asp:TextBox ID="ArticlePublished" CssClass="form-control" TextMode="Date" runat="server"></asp:TextBox>
            </div>
            <div class="form-group mt-5 mb-5">
                <asp:Button ID="SaveButton" CssClass="btn btn-success btn-block" runat="server" Text="Submit" OnClick="SaveButton_Click" ValidationGroup="ArticleForm_ValidationGroup" Visible="true" />
            </div>
        </div>
        <div id="Success" class="container" runat="server">
            <div class="row">
                <div class="col-12-sm">
                    <h2 class="d-block p-3 m-3 text-center">It worked!</h2>
                </div>
            </div>
            <div class="row">
                <div class="col-12-sm">
                    <asp:HyperLink ID="LinkToDashboard" runat="server">Return to Dashboard</asp:HyperLink>
                </div>
            </div>
        </div>
    </section>
    <script>
        tinymce.init({ // WYSIWYG editor
          selector: '#MainContent_ArticleBody',
          height: 700,
          plugins: [
            'advlist autolink lists link image charmap print preview anchor',
            'searchreplace visualblocks code fullscreen',
            'insertdatetime media table paste wordcount'
          ],
          toolbar: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
            content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:14px }',
            paste_block_drop: true
        });

        let modal = {}, autocomplete, articleId, categoryId;
        $(document).ready(() => {
            articleId = $("#MainContent_ArticleID").val();
            categoryId = $("#MainContent_CategoryID").val();
        });

        $(document).on("click", "#add-file-button", function () {
            $("#file-upload-input").click();
        });

        $(document).on("change", "#file-upload-input", function () {
            uploadFiles($(this)[0].files);
        });

        $(document).on("click", "#add-tag-button", function () {
            modal = new Modal({
                title: 'Add Tag',
                content: '<input type="text" id="tagInput" value="" class="form-control"><input type="hidden" id="tagTarget" value="">',
                width: 500,
                height: 400,
                closeOnEsc: true,
                draggable: true,
                classes: [],
                buttons: [
                    {
                        id: '',
                        text: 'Add',
                        classes: ['button-green'],
                        onClick: function() {
                            // addTagToArticle(articleId, $("#tagTarget").val());
                            addTagToArticle(categoryId, articleId, $("#tagTarget").val(), $("#tagInput").val());
                            $("#tagInput").val("").focus();
                            $("#tagTarget").val("");
                        }
                    },
                    {
                        id: '',
                        text: 'Close',
                        classes: ['button-red'],
                        onClick: function() {
                            modal.destroy();
                        }
                    }
                ],
                open: function () {
                    $("#tagInput").focus();
                    autocomplete = new Autocomplete({
                        url: '/CMS/Views/Admin/form.aspx/FilterTagList?categoryId="' + categoryId + '"&input="__QUERY__"',
                        matchRequired: false,
                        input: '#tagInput',
                        target: '#tagTarget',
                        // height: null,
                        searchPlaceholder: '__QUERY__',
                        handleSelectItem: function(e) { return e; },
                        handleQueryData: function (data) { return data.d.map(i => ({ id: i.ID, display: i.DisplayName })) }
                    });
                },
                close: function () {
                    autocomplete.destroy();
                    autocomplete = null;
                }
            });
        });

        $(document).on("click", "#add-image-button", function () {
            modal = new Modal({
                // autoCreate: false,
                title: 'Add A File',
                content: '<div id="files"></div>',
                width: 600,
                height: 500,
                closeOnEsc: true,
                draggable: true,
                classes: [],
                buttons: [
                    {
                        id: '',
                        text: 'Cancel',
                        classes: ['button-red'],
                        onClick: function() {
                            modal.destroy();
                        }
                    }
                ],
                open: function () {
                    $.ajax({
                        url: '/CMS/Views/Admin/form.aspx/GetFileListForCategory',
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({
                            categoryId: categoryId
                        }),
                        success: (data) => { // console.log(data);
                            data.d.forEach(d => {
                                if (d.Type.includes("video")) {
                                    $("#files").append("<video class='thumbnail add-video'><source src='/vid/video/"+ d.ID +"' type='"+ d.Type +"'>Your browser does not support the video tag.</video>");
                                }
                                else {
                                    $("#files").append("<img src='/img/image/"+ d.ID +"' class='thumbnail add-image'>");
                                }
                            });
                        }
                    });
                },
                close: function () {
                }
            });
        });

        $(document).on("click", ".tag-remove-button", function () {
            let articleTagId = $(this).parent().attr('data-articletagid');
            removeTagFromArticle(articleId, articleTagId);
        });

        $(document).on("click", ".add-image", function () {
            // maybe move this functionality to a function...
            // if not in files dropzone, add it there...
            //// add to articlefiles also
            // allow same add method from file dropzone...
            let imageSrc = $(this).attr('src');
            let filePath = "/img/image/";
            let fileId = imageSrc.replace(filePath,"");

            if(typeof modal.destroy === 'function') modal.destroy();

            attachFileToArticle(fileId);

            addFileToBody(fileId, filePath);
        });

        $(document).on("click", ".add-video", function () {
            // maybe move this functionality to a function...
            // if not in files dropzone, add it there...
            //// add to articlefiles also
            // allow same add method from file dropzone...
            let videoSrc = $(this).find('source').attr('src');
            let filePath = "/vid/video/";
            let fileId = imageSrc.replace(filePath,"");

            if(typeof modal.destroy === 'function') modal.destroy();

            attachFileToArticle(fileId);

            addFileToBody(fileId, filePath);
        });

        $(document).on("keyup", "#MainContent_ArticleTitle", function () {
            $("#MainContent_ArticleSlug").val(slugify($(this).val()));
        });

        function addTagToArticle(categoryId, articleId, tagId, tagText) {
            $.ajax({
                url: "/CMS/Views/Admin/form.aspx/AddTagToArticle",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    categoryId: categoryId,
                    articleId: articleId,
                    tagId: tagId,
                    tagText: tagText
                }),
                success: (data) => {
                    displayTags(data.d);
                },
                error: (data) => { console.error(data); }
            });
        }

        $(document).on("dragover", "#MainContent_FileDropzone", function(e) {
            e.preventDefault();  
            e.stopPropagation();
            $(this).addClass('dragging');
        });

        $(document).on("dragleave", "#MainContent_FileDropzone", function(e) {
            e.preventDefault();  
            e.stopPropagation();
            $(this).removeClass('dragging');
        });

        $(document).on("drop", "#MainContent_FileDropzone", function(e) {
            e.preventDefault();  
            e.stopPropagation();
            uploadFiles(e.originalEvent.dataTransfer.files);
        });

        function uploadFiles(files) {
            // console.log(files);

            for (let i = 0; i < files.length; i++) {
                let formData = new FormData();
                formData.append('file', files[i]);
                formData.append('categoryId', categoryId);
                // formData.append('articleId', articleId);
                // console.log(formData);
                $.ajax({
                    url: "/CMS/Ajax/UploadFile.asmx/UploadFileForArticle",
                    type: "POST",
                    cache: false,
                    contentType: false,
                    // contentType: "application/json; charset=utf-8",
                    processData: false,
                    data: formData,
                    success: (data) => {
                        // UploadArticleFile
                        let json = JSON.parse(data); // console.log(json);

                        attachFileToArticle(json.ID);

                        addFileToBody(json.ID, json.Type);
                    },
                    error: (data) => { console.error(data); }
                });
            }
        }

        function attachFileToArticle(fileId) {
            let articleId = $("#MainContent_ArticleID").val();
            $.ajax({
                url: "/CMS/Views/Admin/form.aspx/AddFileToArticle",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    articleId: articleId,
                    fileId: fileId
                }),
                success: (data) => {
                    // console.log(data);
                },
                error: (data) => { console.error(data); }
            });
        }

        function addFileToBody(id, type) {
            if (type.includes('image/')) {
                displayImageThumbnail(id);
                $('button.tox-tbtn[title="Insert/edit image"]').click();
                setTimeout(() => {
                    $('div.tox-dialog input').first().val('/img/image/' + id).change().prop("readonly", true);
                }, 1);
            }
            else if (type.includes('video/')) {
                displayVideoThumbnail(id, type);
                $('button.tox-mbtn:nth-child(4)').click();
                setTimeout(() => {
                    $('div.tox-menu div[title="Media..."]').click();
                    setTimeout(() => {
                        $('div.tox-dialog input').first().val('/vid/video/' + id).change().prop("readonly", true);
                    }, 1);
                }, 1);
            }
        }

        function displayImageThumbnail(id) {
            if ($("#MainContent_FileDropzone").html().includes(id)) return false;
            $("#MainContent_FileDropzone").append("<img src='/img/image/"+ id +"' class='thumbnail'>");
        }

        function displayVideoThumbnail(id, type) {
            if ($("#MainContent_FileDropzone").html().includes(id)) return false;
            $("#MainContent_FileDropzone").append("<video class='thumbnail'><source src='/vid/video/"+ id +"' type='"+ type +"'>Your browser does not support the video tag.</video>");
        }

        function removeTagFromArticle(articleId, articleTagId) {
            $.ajax({
                url: "/CMS/Views/Admin/form.aspx/RemoveTagFromArticle",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    articleId: articleId,
                    articleTagId: articleTagId
                }),
                success: (data) => {
                    displayTags(data.d);
                },
                error: (data) => { console.error(data); }
            });
        }

        function displayTags(tags) {
            let tagDisplayHtml = "";

            tags.forEach((t) => {
                tagDisplayHtml += "<div class='tag-container' data-articletagid='" + t.ID + "'><span class='tag-button'>" + t.Tag.DisplayName + "</span><a class='tag-remove-button'></a></div>";
            });

            $("#MainContent_ArticleTagDisplay").html(tagDisplayHtml);
        }

        function slugify(string) {
          const a = 'àáâäæãåāăąçćčđďèéêëēėęěğǵḧîïíīįìłḿñńǹňôöòóœøōõőṕŕřßśšşșťțûüùúūǘůűųẃẍÿýžźż·/_,:;'
          const b = 'aaaaaaaaaacccddeeeeeeeegghiiiiiilmnnnnoooooooooprrsssssttuuuuuuuuuwxyyzzz------'
          const p = new RegExp(a.split('').join('|'), 'g')

          return string.toString().toLowerCase()
            .replace(/\s+/g, '-') // Replace spaces with -
            .replace(p, c => b.charAt(a.indexOf(c))) // Replace special characters
            .replace(/&/g, '-and-') // Replace & with 'and'
            .replace(/[^\w\-]+/g, '') // Remove all non-word characters
            .replace(/\-\-+/g, '-') // Replace multiple - with single -
            .replace(/^-+/, '') // Trim - from start of text
            .replace(/-+$/, '') // Trim - from end of text
        }
    </script>
</asp:Content>
 