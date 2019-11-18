$(document).ready(function () {
  $(document).on("click", "span.like", function () {
    var $this = $(this);
    var postId = $this.parents(".Post").data("id");

    $.ajax({
      type: "GET",
      url: "/" + postId + "/like",
      success: function (result) {
        $this.html('<i class="fas fa-heart fa-sm fa-fw"></i><span class="counter">' + result.likescount + "</span>").removeClass("like").addClass("unlike").hide().fadeIn();
      }
    });
  });

  $(document).on("click", "span.unlike", function () {
    var $this = $(this);
    var postId = $this.parents(".Post").data("id");

    $.ajax({
      type: "GET",
      url: "/" + postId + "/unlike",
      success: function (result) {
        $this.html('<i class="far fa-heart fa-sm fa-fw"></i><span class="counter">' + result.likescount + "</span>").removeClass("unlike").addClass("like").hide().fadeIn();
      }
    });
  });

  $(document).on("click", "span.favorite", function () {
    var $this = $(this);
    var postId = $this.parents(".Post").data("id");

    $.ajax({
      type: "GET",
      url: "/" + postId + "/favorite",
      success: function (result) {
        $this.html('<i class="fas fa-star fa-sm fa-fw"></i><span class="counter">' + result.favoritescount + "</span>").removeClass("favorite").addClass("unfavorite").hide().fadeIn();
      }
    });
  });

  $(document).on("click", "span.unfavorite", function () {
    var $this = $(this);
    var postId = $this.parents(".Post").data("id");

    $.ajax({
      type: "GET",
      url: "/" + postId + "/unfavorite",
      success: function (result) {
        $this.html('<i class="far fa-star fa-sm fa-fw"></i><span class="counter">' + result.favoritescount + "</span>").removeClass("unfavorite").addClass("favorite").hide().fadeIn();
      }
    });
  });
});
