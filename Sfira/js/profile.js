$(document).ready(function () {
  $(document).on("click", ".profileActions .more", function () {
    var $this = $(this);
    $this.html($this.html() === "\u2022\u2022\u2022" ? '<i class="fas fa-chevron-right fa-fw"></i>' : "\u2022\u2022\u2022");
    $(".moreProfileActions").toggle();
    $(".primaryProfileActions").toggle();
  });

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
