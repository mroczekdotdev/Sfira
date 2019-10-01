$(document).ready(function () {
  $(document).on("click", ".showPosts", function () {
    loadFeed($(this), "posts");
  });

  $(document).on("click", ".showMedia", function () {
    loadFeed($(this), "media");
  });

  $(document).on("click", ".showFollowers", function () {
    loadFeed($(this), "followers");
  });

  function loadFeed(caller, feedName) {
    var parent = caller.parents(".profile");
    var userName = parent.data("username");
    var feed = parent.siblings("." + feedName + "-feed");
    var activeMenuItem = parent.find(".profileMenu > .menuItem")

    var activeFeed = $(".feed.active")

    if ($.trim(feed.html()) == "") {
      $.ajax({
        type: "GET",
        url: userName + "/" + feedName + "/",
        success: function (result) {
          feed.html(result);
        }
      });
    }
    activeFeed.removeClass("active").hide();
    feed.addClass("active").show();
    activeMenuItem.removeClass("active");
    caller.addClass("active");;
  }
});
