document.getElementById("loginButton").addEventListener("click", event => {
    const username = document.getElementById("usernameInput").value;

    if (username) {
        $.post('/Index', function (result) {
            console.log('result', result);
        })
    }
    event.preventDefault();
});