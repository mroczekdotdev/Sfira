$(document).ready(function () {
  $("body").on("click", ".Thumbnail", function () {
    var modal = $("#ImageModal");
    modal.css("display", "block");
    modal.children(".modalContent").attr("src", $(this).data("fullresolutionimage"));
  });

  $(".modalClose").on("click", function () {
    $(this).parents(".modal").hide();
  });
});
