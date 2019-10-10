$(document).ready(function () {
  $(document).on("click", ".profileActions .more", function () {
    var $this = $(this);
    $this.html($this.html() === "\u2022\u2022\u2022" ? '<i class="fas fa-chevron-right fa-fw"></i>' : "\u2022\u2022\u2022");
    $(".moreProfileActions").toggle();
    $(".primaryProfileActions").toggle();
  });
});
