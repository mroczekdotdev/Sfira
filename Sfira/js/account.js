$(document).ready(function () {
  $(document).on("change", "#Input_Avatar", function () {
    var img = new Image();
    img.src = URL.createObjectURL($(this)[0].files[0]);

    $("#avatarPreview").css({
      "background-image": "url(" + img.src + ")",
      "background-position": "center",
      "background-size": "cover",
      "background-repeat": "no-repeat"
    });
  });

  $(document).on("change", "#Input_Cover", function () {
    var img = new Image();
    img.src = URL.createObjectURL($(this)[0].files[0]);

    $("#coverPreview").css({
      "background-image": "url(" + img.src + ")",
      "background-position": "center",
      "background-size": "cover",
      "background-repeat": "no-repeat"
    });
  });
});
