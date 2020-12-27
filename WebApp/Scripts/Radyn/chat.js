
function setcolor(selector) {
    $(selector).css("background-color", "lightgoldenrodyellow");
}

function clearColor(selector) {
    $(selector).css("background-color", "transparent");
}

function GetStatus() {
    if ($("#newmessage_messagebody").val() == "")
        return "";
    return $("#newmessage_username").val() + " is typing ...";
}
function updateChatArea() {
    $.getJSON(
                "/chat/chat/GetMessages",
                {
                    receiver: $("#reciverUsename").val(),
                    date: new Date().getTime()
                },
                function (data) {
                    $("#message").html += "<img src='/Content/Images/Loading.gif' />" + "دریافت ...";
                    var x;
                    if (data.length > 0) {
                        for (x in data) {
                            $("#chatarea").append(
                            "<p><font color='green'><b>" + data[x].Username + "</b></font> :" + data[x].MessageBody + "  [" + data[x].Time + "]</p>");
                            window.document.title = data[x].Username + ":" + data[x].MessageBody;
                        };

                    }
                    var objDiv = document.getElementById("chatarea");
                    objDiv.scrollTop = objDiv.scrollHeight;
                });
    $.getJSON(
                "/chat/chat/GetOtherPeople/",
                {
                    receiver: $("#reciverUsename").val()
                },
                function (data) {
                    var x;

                    $("#others").html("");

                    if (data.length > 0) {
                        $("#others").slideDown('slow');
                        for (x in data) {
                            $("#others").append(
                            "<p><a href='/chat/chat/index?user=" + data[x].Username + "&sessionId=" + data[x].SessionId + "' target='_blank'>" +
                            data[x].Username + " پیغام جدیدی برای شما دارد </a>" + "<img src='/Content/Images/gif-new.gif' />" + "</p>");
                            window.document.title = data[x].Username + ":پیغام جدیدی برای شما دارد";

                        }
                    }
                    var objDiv = document.getElementById("chatarea");
                    objDiv.scrollTop = objDiv.scrollHeight;
                });
    $.get('/Chat/Chat/UserOnline',
                {
                    user: $("#reciverUsename").val(),
                    r: new Date().getTime()
                },
                function (data) {
                    $("#onlineUsers").html(data);
                });
    if ($("#reciverUsename").val() != "") {
        $.getJSON('/Chat/Chat/GetStatus',
                {
                    receiver: $("#reciverUsename").val(),
                    r: new Date().getTime()
                },
                function (m) {
                    $("#divStatus").html(m.message);
                });

        $.post('/Chat/Chat/SetStatus',
                {
                    receiver: $("#reciverUsename").val(),
                    status: GetStatus(),
                    r: new Date().getTime()
                },
                function (data) {

                });
    }
    $("#message").html("پیغام ها لود شدند");
}
function sendNewMessage() {
    var username = $("#newmessage_username").val();
    var msg = $("#newmessage_messagebody").val();
    $("#newmessage_messagebody").focus();
    if (msg != null && msg != "" && msg != "\n") {
        var time = new Date();
        $("#receiver").attr("disabled", "true");
        for (var i = 0; i < msg.search("\n"); i++) {
            msg = msg.replace("\n", "<br/>");
        }
        $("#chatarea").append("<p><b><font color='red'>" + username + "</font></b> : " + msg + " [" + time.getHours() + ":" + time.getMinutes() + ":" + time.getSeconds() + "]</p>");
        var objDiv = document.getElementById("chatarea");
        objDiv.scrollTop = objDiv.scrollHeight;
        $.post(
                "/chat/chat/AddMessage",
                {
                    username: username,
                    message: msg,
                    receiver: $("#reciverUsename").val(),
                    reciverId: $("#reciverId").val(),
                    sessionId: $("#sessionId").val(),
                    r: new Date().getTime()
                });
        updateChatArea();
    }
    $("#newmessage_messagebody").val("");
}

function newChat(newUser) {
    var user = $("#reciverUsename").val();
    if (user == "")
        window.location = '/chat/chat/Index?user=' + newUser;
    else {
        if (user == newUser)
            return;
        else
            window.open('/chat/chat/Index?user=' + newUser, '', 'width=600px,height=400px,scrollbars=yes,resizable=yes');
    }
}

function exit() {
    $.get('/Chat/Chat/ExitChat', {}, function (data) {
        $("#onlineUsers").html(data);
    });
}