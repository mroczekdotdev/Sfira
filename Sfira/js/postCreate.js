$(document).ready(function () {
  var attachmentPreview = $(".attachmentPreview");

  $(document).on("change", "#imageAttachment", function (e) {
    $(".attachmentInput").attr("data-selected", "false");
    e.target.setAttribute("data-selected", "true");

    var img = new Image;
    img.src = URL.createObjectURL(e.target.files[0]);
    $("#imagePreview").css({
      "background-image": "url(" + img.src + ")",
      "background-position": "center",
      "background-size": "cover",
      "background-repeat": "no-repeat",
    });

    attachmentPreview.show();
  });

  $(document).on("click", ".sendPost", function () {
    submitAttachmentForm();
  });

  function submitAttachmentForm() {
    var selected = $('.attachmentInput[data-selected="true"]');

    if (selected[0] == undefined) {
      submitPostForm();
    }
    else {
      var file = new FormData();
      file.append("file", selected[0].files[0]);

      $.ajax({
        type: "POST",
        url: "/attachment/create",
        data: file,
        processData: false,
        contentType: false,
        success: function (result) {
          submitPostForm(result)
        },
      });
    }
  }

  function submitPostForm(attachment) {
    var body = $("#postBody").val();

    var post = {
      body
    };

    if (attachment !== undefined) {
      post.attachment = attachment
    }

    $.ajax({
      type: "POST",
      url: "/post/create",
      data: JSON.stringify(post),
      contentType: "application/json; charset=UTF-8",
      success: function (result) {
        $(".attachmentForm")[0].reset();
        $(".postForm")[0].reset();
        attachmentPreview.hide();
      },
    });
  }
});
