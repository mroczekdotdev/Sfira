$(document).ready(function () {
  $(document).on("click", "a.comment", function () {
    var parent = $(this).parents(".single-post");
    var postid = parent.data("id");
    var comments = parent.find(".comments");
    var createComment = parent.find(".create-comment");
    if ($.trim(comments.html()) == "") {
      $.ajax({
        type: "GET",
        url: "/comments/" + postid,
        success: function (result) {
          createComment.slideDown();
          comments.html(result).hide().slideDown();
        }
      });
    }
    else {
      createComment.slideToggle();
      comments.slideToggle();
    }
  });

  $(document).on("click", ".create-comment button", function () {
    var parent = $(this).parents(".single-post");
    var postId = parent.data("id");
    var body = parent.find(".create-comment .body");

    var model = {
      Body: body.val(),
    };

    $.ajax({
      type: "POST",
      url: "/comment/create?postId=" + postId,
      data: JSON.stringify(model),
      contentType: "application/json",
      success: function () {
        var comments = parent.find(".comments");
        var a = parent.find("a.comment");
        $.ajax({
          type: "GET",
          url: "/comments/" + postId,
          success: function (result) {
            body.val("");
            comments.html(result);
            var commentsCount = parent.find(".single-comment").length;
            a.html('<i class="fas fa-comment-alt fa-sm fa-fw"></i>' + commentsCount).hide().fadeIn();
          },
          error: function () {
          },
          complete: function () {
          },
        });
      }
    });
  });
});
