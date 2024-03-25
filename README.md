# Tic Tac Toe
<p align="center"> <img src=Images/Gameplay.gif> </p>

## About The Project


This is a simple example of a Tic Tac Toe game built in Unity, featuring implementation of the Dependency Injection framework [Zenject][zenject-url]

Key features:
- All dependencies are managed by the Zenject Container.
- Game progress and the current active player are stored in PlayerPrefs to handle emergency program stops. Data about board cells is saved by parsing a 2-dimensional array into a single string and restored upon loading.
- Utilizes primarily event-driven architecture, minimizing direct object calls.
- Game state resets upon scene reload, disposing of resources.


### Built With

- **Unity 2021.3.29f1** (support for older versions is not guaranteed)
- **Zenject v9.2.0**

### Usage

Clone this repository using Git into your local disk, then add the downloaded folder in Unity Hub as a project from disk.


### Gameplay

Each player takes turns pressing cells until one of them forms a line of 3 symbols (horizontally, vertically, or diagonally). Pressing "Start New Game" allows restarting at any time. After finishing the game, you can start a new game immediately!

## Roadmap

- [x] Change custom EventService to Zenject's Signals
- [ ] Visual rework


[zenject-url]: https://github.com/modesttree/Zenject
