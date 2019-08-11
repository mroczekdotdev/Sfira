//TODO: Merge functions.

$(document).ready(function () {
  $(document).on("click", "a.like", function () {
    var postid = $(this).data("id");
    var link = postid + "/like";
    var a = $(this);
    $.ajax({
      type: "GET",
      url: link,
      success: function (result) {
        a.html('<i class="fas fa-heart fa-sm fa-fw"></i>' + result.likescount).removeClass("like").addClass("unlike");
      }
    });
  });

  $(document).on("click", "a.unlike", function () {
    var postid = $(this).data("id");
    var link = postid + "/unlike";
    var a = $(this);
    $.ajax({
      type: "GET",
      url: link,
      success: function (result) {
        a.html('<i class="far fa-heart fa-sm fa-fw"></i>' + result.likescount).removeClass("unlike").addClass("like");
      }
    });
  });

  $(document).on("click", "a.favorite", function () {
    var postid = $(this).data("id");
    var link = postid + "/favorite";
    var a = $(this);
    $.ajax({
      type: "GET",
      url: link,
      success: function (result) {
        a.html('<i class="fas fa-star fa-sm fa-fw"></i>' + result.favoritescount).removeClass("favorite").addClass("unfavorite");
      }
    });
  });

  $(document).on("click", "a.unfavorite", function () {
    var postid = $(this).data("id");
    var link = postid + "/unfavorite";
    var a = $(this);
    $.ajax({
      type: "GET",
      url: link,
      success: function (result) {
        a.html('<i class="far fa-star fa-sm fa-fw"></i>' + result.favoritescount).removeClass("unfavorite").addClass("favorite");
      }
    });
  });
});
