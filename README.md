# MultiRC Car Racing

A simple prototype of a racing game with Unity Netcode functionality for wireless multiplayer.

## Overview

The game is a top-down racing game built using Unity Engine, similar to RC car racing. Up to two players can race against each other in real time through Unity Netcode with an architecture to add support for additional players in future iterations. Race states, lap completion, and player rankings are synchronized through a server-authoritative networking model.

## Features 
- Multiplayer racing through Unity Netcode
- Server-authoritative race state
- Real-time player position synchronization
- Lap counting and checkpoint race progress tracking
- Dynamic race ranking system

## Technical Architecture 
- Engine: Unity 6 (6000.0.41f1)
- Language: C#
- Networking: Unity Netcode for GameObjects
- Architecture: Server-authoritative multiplayer
- Synchronization: NetworkVariables and RPCs
- Input: Unity Input System
- Physics: Unity Rigidbody-based vehicle movement

## Networking Design
- Server-authoritative architecture for race state management
- Client-owned vehicles process local input while synchronizing game state using NetworkVariables
- RPCs communicate race events and update shared game states
- Player rankings are calculated from lap count and checkpoint progression

## Engineering Challenges 
- Synchronizing player positioning and race progression across clients
- Implemented ownership-based client-side input system to ensure only the owning client controls their own car while maintaining server authority
- Maintained consistent player rankings, lap completion, and checkpoint progression across all networked clients
- Designed a dynamic player ranking position based on lap count and checkpoint progression

## Potential Features and Improvements 
- Support for an arbitrary amount of players
- Lobby-based matchmaking
- Dedicated server support
- Network latency compensation
- Player items
- AI-controlled opponents
