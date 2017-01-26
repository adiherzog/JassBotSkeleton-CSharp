# C# Jass Bot Skeleton

This is a skeleton for writing a Jass (Swiss card game) bot for this Jass server: https://github.com/webplatformz/challenge

## Configuration

All important settings can be made in the class `JassBotApplication`:

* Server URI
* Tournament mode
* Session name
* Number of bot players
* Bot name
* Player names

Furthermore the Server URI can also be supplied as the first command line argument: `JassBot.exe ws://myjasschallengeserver.com:3333`.

## Run

The project uses C# 6. It's easiest to build it using Visual Studio 2015 (The free Community Edition should do).

## Implement Game Logic

Start by improving the implementation of `TrumpfChooser.cs` and `CardChooser.cs`.
