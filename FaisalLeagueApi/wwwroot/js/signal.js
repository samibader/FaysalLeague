"use strict";
var token;

function authorize() {
    var username = $("input#username").val();
    var password = $("input#password").val();
    
    var arr = { username: username, password: password };
    $.ajax
        ({
            type: "POST",
            url: "/api/login/Authenticate",
            dataType: 'json',
            async: false,
            //headers: {
            //    "Authorization": "Basic " + btoa(USERNAME + ":" + PASSWORD)
            //},
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(arr),
            success: function (data) {
                $("#tokenspan").text(data.token);
                token = data.token;
            }
        });
}


var connection = new signalR.HubConnectionBuilder().withUrl("/signalHub", { transport: signalR.HttpTransportType.WebSockets, accessTokenFactory: () => this.token, SkipNegotiation: true }).build();

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ReceiveMessagePrivate", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg ="Private says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});


connection.onclose(function (e) {
    var li = document.createElement("li");
    li.textContent = "client disconnected";
    document.getElementById("messagesList").appendChild(li);
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    //var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("answerButton").addEventListener("click", function (event) {
    var questionId = document.getElementById("questionIdInput").value;
    var choiceId = document.getElementById("choiceIdInput").value;
    connection.invoke("AnswerQuestion", questionId, choiceId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("getQuestionButton").addEventListener("click", function (event) {
    var questionId = document.getElementById("questionIdInput").value;
    connection.invoke("GetQuestion", questionId).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("sendPrivateButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessageToCaller", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

//document.getElementById("beginButton").addEventListener("click", function (event) {
//    connection.invoke("BeginTimer").catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});

document.getElementById("disconnectButton").addEventListener("click", function (event) {
    connection.invoke("Disconnect").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function opensocket() {
    connection.start()
        .catch(function (err) {
            return console.error(err.toString());
        });
}

function getconnectionid() {
    connection.invoke('getConnectionId')
        .then(function (connectionId) {
            console.log('connectionId = ' + connectionId);
        });
}
function getUserIdentifier() {
    connection.invoke('GetUserIdentifier')
        .then(function (userIdentifier) {
            console.log('userIdentifier = ' + userIdentifier);
        });
}