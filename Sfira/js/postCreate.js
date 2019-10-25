$(document).ready(function () {
  $(document).on("change", "#imageAttachment", function () {
    $this = $(this);
    var parent = $this.parents(".PostCreate");
    var attachmentPreview = parent.find(".attachmentPreview");
    var imagePreview = parent.find("#imagePreview");
    var addAttachment = parent.find(".addAttachment");
    var removeAttachment = parent.find(".removeAttachment");

    $(".attachmentInput").attr("data-selected", "false");
    $this.attr("data-selected", "true");

    var img = new Image();
    img.src = URL.createObjectURL($this[0].files[0]);
    imagePreview.css({
      "background-image": "url(" + img.src + ")",
      "background-position": "center",
      "background-size": "cover",
      "background-repeat": "no-repeat"
    });

    addAttachment.hide();
    removeAttachment.show();
    attachmentPreview.show();
  });

  $(document).on("click", ".sendPost", function () {
    var parent = $(this).parents(".PostCreate");
    var attachmentPreview = parent.find(".attachmentPreview");
    var addAttachment = parent.find(".addAttachment");
    var removeAttachment = parent.find(".removeAttachment");
    var attachmentInput = parent.find('.attachmentInput[data-selected="true"]');
    var attachmentForm = parent.find(".attachmentForm");
    var postForm = parent.find(".postForm");
    var body = postForm.find("#postBody").val();

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
        postForm[0].reset();
        attachmentForm[0].reset();
        attachmentPreview.hide();
        removeAttachment.hide();
        addAttachment.show();
      },
    });
  });

  $(document).on("click", ".removeAttachment", function () {
    $this = $(this);
    var parent = $this.parents(".PostCreate");
    var attachmentPreview = parent.find(".attachmentPreview");
    var addAttachment = parent.find(".addAttachment");
    var attachmentForm = parent.find(".attachmentForm");

    $this.hide()
    addAttachment.show();
    attachmentForm[0].reset();
    attachmentPreview.hide();
  });
});
