$(document).ready(function () {
  $(document).on("click", "span.comment", function () {
    var post = $(this).parents(".Post");
    var postId = post.data("id");
    var commentsFeed = post.find(".CommentsFeed");
    var commentCreate = post.find(".CommentCreate");
    var attachmentPreview = post.find(".attachment");
    var animationSpeed = "fast";

    if ($.trim(commentsFeed.html()) == "") {
      $.ajax({
        type: "GET",
        url: "/" + postId + "/comments",
        success: function (result) {
          commentCreate.slideDown(animationSpeed);
          commentsFeed.html(result).hide().slideDown(animationSpeed);
          attachmentPreview.toggleClass("bottom");
        }
      });
    }
    else {
      commentCreate.slideToggle(animationSpeed);
      commentsFeed.slideToggle(animationSpeed);
      attachmentPreview.toggleClass("bottom");
    }
  });

  $(document).on("click", ".CommentCreate button", function () {
    var post = $(this).parents(".Post");
    var postId = post.data("id");
    var body = post.find(".CommentCreate .body");
    var isCurrentUserAuthor = post.hasClass("-user");

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
        var action = post.find("span.comment");

        $.ajax({
          type: "GET",
          url: "/" + postId + "/comments/true",
          success: function (data, textStatus, jqXHR) {
            body.val("");
            comments.html(data);
            var commentsCount = jqXHR.getResponseHeader("Comments-Count");

            var icon;
            if (isCurrentUserAuthor) {
              icon = '<i class="far fa-comment-alt fa-sm fa-fw"></i>'
            }
            else {
              icon = '<i class="fas fa-comment-alt fa-sm fa-fw"></i>'
            }

            action.html(icon + '<span class="counter">' + commentsCount + "</span>").hide().fadeIn();
          },
        });
      },
    });
  });
});
