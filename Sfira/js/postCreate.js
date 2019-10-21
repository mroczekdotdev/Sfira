$(document).ready(function () {
  var attachmentPreview = $(".attachmentPreview");

  $(document).on("change", "#imageAttachment", function () {
    $this = $(this);
    $(".attachmentInput").attr("data-selected", "false");
    $this.attr("data-selected", "true");

    var img = new Image();
    img.src = URL.createObjectURL($this[0].files[0]);
    $("#imagePreview").css({
      "background-image": "url(" + img.src + ")",
      "background-position": "center",
      "background-size": "cover",
      "background-repeat": "no-repeat"
    });

    attachmentPreview.show();
  });

  $(document).on("click", ".sendPost", function () {
    var attachmentInput = $('.attachmentInput[data-selected="true"]');
    var body = $("#postBody").val();

    var form = new FormData();

    form.append(
      "post",
      new Blob(["post"], {
        type: "application/json"
      })
    );

    form.append("body", body);

    if (attachmentInput[0] !== undefined) {
      form.append("formFile", attachmentInput[0].files[0]);
    }

    $.ajax({
      type: "POST",
      url: "/post/create",
      data: form,
      contentType: false,
      processData: false,
      success: function () {
        $(".attachmentForm")[0].reset();
        $(".postForm")[0].reset();
        attachmentPreview.hide();
      },
    });
  });
});
