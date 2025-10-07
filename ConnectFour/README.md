# File Overview

| File/Class              | Purpose                                                                 |
|-------------------------|-------------------------------------------------------------------------|
| Program.cs              | Main entry point; handles menu, game mode selection, and game loop      |
| DrawGrid.cs             | Manages grid state, disc placement, win/draw checking, and display      |
| Disc.cs                 | Abstract base class for all disc types; includes factory method         |
| DiscOrdinary.cs         | Ordinary disc with no special effect                                    |
| DiscBoring.cs           | Boring disc that removes discs below and drops to bottom                |
| DiscMagnetic.cs         | Magnetic disc that lifts nearest ordinary disc below                    |
| GameInventory.cs        | Tracks disc counts, player names, game mode, and move counter           |
| GameState.cs            | Handles save/load logic using grid snapshots and inventory              |
| InputValidation.cs      | Validates user input and parses disc/column commands                    |
| PlayerComputer.cs       | AI logic for computer player; chooses winning or random moves           |
| PlayerTestRunner.cs     | Runs scripted test sequences for debugging or demo                      |

---

# How Files Relate to Each Other

- Program.cs orchestrates the game:
  - Uses InputValidation.cs to parse user input
  - Uses GameInventory.cs to track game state
  - Uses DrawGrid.cs to manage grid and check win/draw
  - Uses Disc.cs and its subclasses to create and apply disc effects
  - Uses GameState.cs to save/load game progress
  - Uses PlayerComputer.cs for AI moves
  - Uses PlayerTestRunner.cs for test mode

- DrawGrid.cs depends on:
  - Disc.cs and its subclasses for disc behavior
  - GameInventory.cs for disc availability and tracking

- Disc.cs uses:
  - Factory method to instantiate correct subclass (Ordinary, Boring, Magnetic)
  - Polymorphism to apply effects via ApplyEffect() override

- GameState.cs serializes:
  - DrawGrid and GameInventory into a savable format
  - Restores them for continued gameplay

---
