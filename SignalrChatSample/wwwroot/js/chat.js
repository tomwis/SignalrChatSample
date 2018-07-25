// The following sample code uses modern ECMAScript 6 features 
// that aren't supported in Internet Explorer 11.
// To convert the sample for environments that do not support ECMAScript 6, 
// such as Internet Explorer 11, use a transpiler such as 
// Babel at http://babeljs.io/. 
//
// See Es5-chat.js for a Babel transpiled version of the following code:

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.on("ReceiveMessage", (user, message) => {
    const msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    document.getElementById("messagesList").appendChild(createMessageLayout(user, msg));
});

connection.on("UpdateLoggedInUsers", (users) => {
    document.getElementById("userList").innerHTML = "";

    for (var i = 0; i < users.length; i++) {
        document.getElementById("userList").appendChild(createUserLayout(users[i]));
    }
});

function createMessageLayout(user, msg) {
    const currentUser = document.getElementById("ChatUser_Username").value;

    const li = document.createElement("li");
    li.className = "message-container";

    if (user != currentUser) {
        li.className += " message-container-others";
    }

    var userLayout = document.createElement("div");
    userLayout.className = "message-user";
    userLayout.textContent = user;

    var msgLayout = document.createElement("div");
    msgLayout.className = "message";
    msgLayout.textContent = msg;

    li.appendChild(userLayout);
    li.appendChild(msgLayout);

    return li;
}

function createUserLayout(user) {
    const li = document.createElement("li");
    li.textContent = user;
    return li;
}

function listLoggedInUsers() {
    const user = document.getElementById("ChatUser_Username").value;
    connection.invoke("AddLoggedInUser", user).catch(err => console.error(err.toString()));
}

connection.start()
    .then(r => {
        listLoggedInUsers();
    })
    .catch(err => console.error(err.toString()));

document.getElementById("sendButton").addEventListener("click", event => {

    const user = document.getElementById("ChatUser_Username").value;
    const message = document.getElementById("messageInput").value;
    document.getElementById("messageInput").value = "";

    connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));

    event.preventDefault();
});

document.getElementById("messageInput").addEventListener("keyup", event => {

    if (event.key !== "Enter") return;

    document.getElementById("sendButton").click();

    event.preventDefault();
});

window.addEventListener("beforeunload", function (e) {
    const user = document.getElementById("ChatUser_Username").value;
    connection.invoke("RemoveLoggedInUser", user).catch(err => console.error(err.toString()));
});
