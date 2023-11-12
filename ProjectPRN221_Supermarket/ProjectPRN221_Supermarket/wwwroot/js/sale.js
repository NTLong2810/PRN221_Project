"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/productHub").build();

connection.on("ReceiveSalesUpdate", function () {
    console.log("Sales updated");
    location.reload();
});

connection.on("ReceiveOutOfStockMessage", function (message) {
    console.log("Out of stock message received:", message);
    alert(message);  // Thông báo cho người dùng khi hết hàng
});


connection.start().then(function () {
    console.log("Connected to SignalR Hub");
}).catch(function (err) {
    console.error(err);
});