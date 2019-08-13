
$(document).on("click", "a.like", function () {
  var postid = $(this).parents(".single-post").data("id");
  var link = postid + "/like";
  var a = $(this);
  $.ajax({
    type: "GET",
    url: link,
    success: function (result) {
      a.html('<i class="fas fa-heart fa-sm fa-fw"></i>' + result.likescount).removeClass("like").addClass("unlike").hide().fadeIn();
    }
  });
});

$(document).on("click", "a.unlike", function () {
  var postid = $(this).parents(".single-post").data("id");
  var link = postid + "/unlike";
  var a = $(this);
  $.ajax({
    type: "GET",
    url: link,
    success: function (result) {
      a.html('<i class="far fa-heart fa-sm fa-fw"></i>' + result.likescount).removeClass("unlike").addClass("like").hide().fadeIn();
    }
  });
});

$(document).on("click", "a.favorite", function () {
  var postid = $(this).parents(".single-post").data("id");
  var link = postid + "/favorite";
  var a = $(this);
  $.ajax({
    type: "GET",
    url: link,
    success: function (result) {
      a.html('<i class="fas fa-star fa-sm fa-fw"></i>' + result.favoritescount).removeClass("favorite").addClass("unfavorite").hide().fadeIn();
    }
  });
});

$(document).on("click", "a.unfavorite", function () {
  var postid = $(this).parents(".single-post").data("id");
  var link = postid + "/unfavorite";
  var a = $(this);
  $.ajax({
    type: "GET",
    url: link,
    success: function (result) {
      a.html('<i class="far fa-star fa-sm fa-fw"></i>' + result.favoritescount).removeClass("unfavorite").addClass("favorite").hide().fadeIn();
    }
  });
});

$(document).on("click", "a.comment", function () {
  var parent = $(this).parents(".single-post");
  var postid = parent.data("id");
  var link = "/comments/" + postid;
  var comments = parent.find(".comments");
  var createcomment = parent.find(".create-comment");
  if ($.trim(comments.html()) == '') {
    $.ajax({
      type: "GET",
      url: link,
      success: function (result) {
        createcomment.slideDown();
        comments.html(result).hide().slideDown();
      }
    });
  }
  else {
    createcomment.slideToggle();
    comments.slideToggle();
  }
});

$(document).on("click", ".create-comment button", function () {
  var parent = $(this).parents(".single-post");
  var postid = parent.data("id");
  var link = "/comment/create?postId=" + postid;
  var message = parent.find(".message");

  var model = {
    Message: message.val(),
  };

  $.ajax({
    url: link,
    type: "POST",
    data: JSON.stringify(model),
    contentType: "application/json",
    success: function (result) {
      if (result === "success") {
        var comments = parent.find(".comments");
        var a = parent.find("a.comment");
        $.ajax({
          type: "GET",
          url: "/comments/" + postid,
          success: function (result) {
            message.val("");
            comments.html(result);
            var commentscount = parent.find(".single-comment").length;
            a.html('<i class="fas fa-comment-alt fa-sm fa-fw"></i>' + commentscount).hide().fadeIn();
          }
        });
      }
      else {
        alert("Reply have to be at least 3 characters.");
      }
    }
  });
});
