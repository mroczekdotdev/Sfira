$(document).ready(function () {
  $(document).on("click", ".UserMenu .avatar", function () {
    $(".UserMenu .items").show();
  });

  $(document).on('click', function (e) {
    if (!$(e.target).closest(".UserMenu").length) {
      $(".UserMenu .items").hide()
    }
  });
});
