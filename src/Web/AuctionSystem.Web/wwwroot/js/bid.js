let currentUserId = $('#currentUserId').val();
let consoleId = $('#consoleId').val();

let connection =
    new signalR.HubConnectionBuilder()
        .withUrl('/bidHub')
        .build();

connection.start()
    .then(function () {
        connection.invoke('Setup', consoleId);
    })
    .catch(function (err) {
        return console.error(err.toString());
    });