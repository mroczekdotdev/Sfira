$(document).ready(function () {
  $(document).on("click", ".closeButton", function () {
    $(this).parents(".-closable").hide();
  });
});
