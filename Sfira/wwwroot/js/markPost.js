$(document).ready(function () {
  $(document).on("click", "a.like", function () {
    var $this = $(this);
    var postId = $this.parents(".single-post").data("id");
    var link = postId + "/like";
    $.ajax({
      type: "GET",
      url: link,
      success: function (result) {
        $this.html('<i class="fas fa-heart fa-sm fa-fw"></i>' + result.likescount).removeClass("like").addClass("unlike").hide().fadeIn();
      }
    });
  });

  $(document).on("click", "a.unlike", function () {
    var $this = $(this);
    var postId = $this.parents(".single-post").data("id");
    var link = postId + "/unlike";
    $.ajax({
      type: "GET",
      url: link,
      success: function (result) {
        $this.html('<i class="far fa-heart fa-sm fa-fw"></i>' + result.likescount).removeClass("unlike").addClass("like").hide().fadeIn();
      }
    });
  });

  $(document).on("click", "a.favorite", function () {
    var $this = $(this);
    var postId = $this.parents(".single-post").data("id");
    var link = postId + "/favorite";
    $.ajax({
      type: "GET",
      url: link,
      success: function (result) {
        $this.html('<i class="fas fa-star fa-sm fa-fw"></i>' + result.favoritescount).removeClass("favorite").addClass("unfavorite").hide().fadeIn();
      }
    });
  });

  $(document).on("click", "a.unfavorite", function () {
    var $this = $(this);
    var postId = $this.parents(".single-post").data("id");
    var link = postId + "/unfavorite";
    $.ajax({
      type: "GET",
      url: link,
      success: function (result) {
        $this.html('<i class="far fa-star fa-sm fa-fw"></i>' + result.favoritescount).removeClass("unfavorite").addClass("favorite").hide().fadeIn();
      }
    });
  });
});
