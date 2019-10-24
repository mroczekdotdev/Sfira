$(document).ready(function () {
  $(document).on("click", ".Feed .Loader", function () {
    var $this = $(this);
    var link = $this.attr("data-link");
    var count = $this.attr("data-count");
    var cursor = $this.attr("data-cursor");

    $.ajax({
      type: "GET",
      url: link + count + "/" + cursor,
      success: function (data, textStatus, jqXHR) {
        $this.before(data);

        if (jqXHR.getResponseHeader("Loader-Keep") == "false") {
          $this.hide();
        }
        else {
          $this.attr("data-cursor", jqXHR.getResponseHeader("Loader-Cursor"));
        }
      }
    });
  });
});
