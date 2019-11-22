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
    var profile = caller.parents(".Profile");
    var userName = profile.data("username");
    var allFeeds = profile.siblings(".Feed")
    var feedToLoad = profile.siblings("." + feedName + "Feed");
    var activeMenuItem = profile.find(".feedsMenu > .menuItem")

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
