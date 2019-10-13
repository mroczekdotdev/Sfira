$(document).ready(function () {
  $(document).on("click", "button.follow", function () {
    var $this = $(this);
    var userId = $this.parents(".profile").data("username");

    $.ajax({
      type: "GET",
      url: userId + "/follow",
      success: function (result) {
        $this.html("Unfollow").removeClass("follow").addClass("unfollow").hide().fadeIn();
        $("#followersCount").html(result);
      },
    });
  });

  $(document).on("click", "button.unfollow", function () {
    var $this = $(this);
    var userId = $this.parents(".profile").data("username");

    $.ajax({
      type: "GET",
      url: userId + "/unfollow",
      success: function (result) {
        $this.html("Follow").removeClass("unfollow").addClass("follow").hide().fadeIn();
        $("#followersCount").html(result);
      },
    });
  });
});
