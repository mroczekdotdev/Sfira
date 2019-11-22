$(document).ready(function () {
  $("body").on("click", ".Thumbnail", function () {
    var modal = $("#ImageModal");
    modal.css("display", "block");
    modal.children(".content").attr("src", $(this).data("fullresolutionimage"));
  });

  $("#ImageModal").on("click", function () {
    $(this).hide();
  });
});
