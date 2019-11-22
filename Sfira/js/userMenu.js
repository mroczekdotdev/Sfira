$(document).ready(function () {
  $(document).on("click", ".UserMenu .avatar", function () {
    $(".UserMenu .items").show();
    $("#UserMenuOverlay").show();
  });

  $(document).on('click', "#UserMenuOverlay", function (e) {
    $(".UserMenu .items").hide()
    $(this).hide();
  });
});
