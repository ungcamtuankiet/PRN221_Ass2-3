﻿"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/SignalRHub", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
    })
    .configureLogging(signalR.LogLevel.Debug)
    .withAutomaticReconnect()
    .build();

connection.on("RefreshData", function () {
    console.log("Refresh data called");
    location.reload();
});

connection.start().then(function () {
    console.log("Hub connected");
}).catch(function (err) {
    return console.error(err.toString());
});
