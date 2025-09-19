# Island Defender
## About The Game
In Island Defender, you collect resources to build defenses to protect your keep from waves of enemies.

Game built in Unity/C# during a two-week game jam.

See [`Assets/_Scripts/`](Assets/_Scripts) for code.
See [Island Defender on itch.io](https://taco-snake-games.itch.io/island-survivors)

## Technical Highlights

### Enemy AI
- There are 7 enemies with different behaviors.
- Uses an inheritance-based state machine.

### Buildings
- There are 6 unique building.
- Buildings can be built on a 1D grid and require resources.

### Resource collection
- Plants, rocks, and trees spawn randomly on the island.
- They can be harvested and they're resources can be use to create buildings.

## What I’d improve

The enemy AI relies heavily on inheritance. This works fine for this small games, but it makes it difficult and complicated to extend the logic to more unique enemies (if I were to add more).

From a game design standpoint, I would create more unique enemies with different strengths and weaknesses. It can get repetitive.
