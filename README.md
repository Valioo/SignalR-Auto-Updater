# SignalR-Auto-Updater
Auto Updater in .NET 5 with SignalR

Checks for software updates, verifies client hardware id and client version(w & w/o beta);
When version is outdated, it [updates] the client software as well as adding the new update to the client database.

[Updates]: File is read in bytes and compressed. Then it is split into a random numbers of byte arrays(10-25).
Arrays are sent in random order to the client, assembled there and decompressed.

SSL is used to encrypt the connection.
HTTP requests are forwarded to https.
