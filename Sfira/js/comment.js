$(document).ready(function () {
  $(document).on("click", "a.comment", function () {
    var post = $(this).parents(".Post");
    var postId = post.data("id");
    var commentsFeed = post.find(".CommentsFeed");
    var commentCreate = post.find(".CommentCreate");
    var attachmentPreview = post.find(".attachment");

    if ($.trim(commentsFeed.html()) == "") {
      $.ajax({
        type: "GET",
        url: postId + "/comments",
        success: function (result) {
          commentCreate.slideDown();
          commentsFeed.replaceWith(result).hide().slideDown();
          attachmentPreview.toggleClass("bottom");
        }
      });
    }
    else {
      commentCreate.slideToggle();
      commentsFeed.slideToggle();
      attachmentPreview.toggleClass("bottom");
    }
  });

  $(document).on("click", ".CommentCreate button", function () {
    var post = $(this).parents(".Post");
    var postId = post.data("id");
    var body = post.find(".CommentCreate .body");

    var model = {
      Body: body.val(),
    };

    $.ajax({
      type: "POST",
      url: "/comment/create?postId=" + postId,
      data: JSON.stringify(model),
      contentType: "application/json",
      success: function () {
        var comments = post.find(".CommentsFeed");
        var a = post.find("a.comment");

        $.ajax({
          type: "GET",
          url: postId + "/comments",
          success: function (result) {
            body.val("");
            comments.replaceWith(result);
            var commentsCount = post.find(".Comment").length;
            a.html('<i class="fas fa-comment-alt fa-sm fa-fw"></i><span class="counter">' + commentsCount + "</span>").hide().fadeIn();
          },
        });
      },
    });
  });
});
