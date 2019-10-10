$(document).ready(function () {
  $(document).on("click", ".CreateMessage .submit", function () {
    var form = $(this).parents(".CreateMessage");
    var chat = form.siblings(".Chat");
    var chatId = chat.attr("data-id");
    var messageBody = form.find(".body");
    var interlocutor = "";

    var model = {
      Body: messageBody.val(),
    };

    if (chatId == 0) {
      interlocutor = "?interlocutorid=" + chat.data("interlocutor");
    };

    $.ajax({
      type: "POST",
      url: "/chat/" + chatId + "/createmessage" + interlocutor,
      data: JSON.stringify(model),
      contentType: "application/json",
      success: function (result) {
        chat.attr("data-id", result);
        chatId = result;
        var messagesFeed = chat.find(".MessagesFeed");

        $.ajax({
          type: "GET",
          url: "/chat/" + chatId + "/messagesfeed",
          success: function (result) {
            messageBody.val("");
            messagesFeed.replaceWith(result);
          },
          error: function () {
          },
          complete: function () {
          },
        });
      },
    });
  });
});
