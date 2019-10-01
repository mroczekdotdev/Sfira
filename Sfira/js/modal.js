$(document).ready(function () {
  $("body").on("click", ".thumbnail", function () {
    var modal = $("#image-modal");
    modal.css("display", "block");
    modal.children(".modalContent").attr("src", $(this).data("fullresolutionimage"));
  });

  $(".modalClose").on("click", function () {
    $(this).parents(".modal").css("display", "none");
  });
});
