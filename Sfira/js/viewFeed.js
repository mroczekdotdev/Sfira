$(document).ready(function () {
  $(document).on("click", ".showPosts", function () {
    loadFeed($(this), "Posts");
  });

  $(document).on("click", ".showMedia", function () {
    loadFeed($(this), "Media");
  });

  $(document).on("click", ".showFollowers", function () {
    loadFeed($(this), "Followers");
  });

  function loadFeed(caller, feedName) {
    var profile = caller.parents(".profile");
    var userName = profile.data("username");
    var allFeeds = profile.siblings(".Feed")
    var feedToLoad = profile.siblings("." + feedName + "Feed");
    var activeMenuItem = profile.find(".profileMenu > .menuItem")

    if ($.trim(feedToLoad.html()) == "") {
      $.ajax({
        type: "GET",
        url: userName + "/" + feedName + "/",
        success: function (result) {
          feedToLoad.html(result);
        }
      });
    }

    allFeeds.hide();
    feedToLoad.show();
    activeMenuItem.removeClass("active");
    caller.addClass("active");
  };
});
