$(document).ready(function () {
  $(document).on("click", "a.like", function () {
    var $this = $(this);
    var postId = $this.parents(".single-post").data("id");
    $.ajax({
      type: "GET",
      url: postId + "/like",
      success: function (result) {
        $this.html('<i class="fas fa-heart fa-sm fa-fw"></i>' + result.likescount).removeClass("like").addClass("unlike").hide().fadeIn();
      }
    });
  });

  $(document).on("click", "a.unlike", function () {
    var $this = $(this);
    var postId = $this.parents(".single-post").data("id");
    $.ajax({
      type: "GET",
      url: postId + "/unlike",
      success: function (result) {
        $this.html('<i class="far fa-heart fa-sm fa-fw"></i>' + result.likescount).removeClass("unlike").addClass("like").hide().fadeIn();
      }
    });
  });

  $(document).on("click", "a.favorite", function () {
    var $this = $(this);
    var postId = $this.parents(".single-post").data("id");
    $.ajax({
      type: "GET",
      url: postId + "/favorite",
      success: function (result) {
        $this.html('<i class="fas fa-star fa-sm fa-fw"></i>' + result.favoritescount).removeClass("favorite").addClass("unfavorite").hide().fadeIn();
      }
    });
  });

  $(document).on("click", "a.unfavorite", function () {
    var $this = $(this);
    var postId = $this.parents(".single-post").data("id");
    $.ajax({
      type: "GET",
      url: postId + "/unfavorite",
      success: function (result) {
        $this.html('<i class="far fa-star fa-sm fa-fw"></i>' + result.favoritescount).removeClass("unfavorite").addClass("favorite").hide().fadeIn();
      }
    });
  });
});
