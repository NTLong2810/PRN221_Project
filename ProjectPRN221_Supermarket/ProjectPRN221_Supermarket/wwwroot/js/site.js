"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/productHub").build();

connection.on("ReceiveChangeProduct", function () {
    console.log("Product updated");
    location.reload();
    // Refresh or update UI as needed
});

connection.start().then(function () {
    console.log("Connected to SignalR Hub");
}).catch(function (err) {
    console.error(err);
});