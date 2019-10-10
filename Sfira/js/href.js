$(document).ready(function () {
  $(document).on("click", ".href", function () {
    $(location).attr("href", $(this).attr("href"))
  });
});
