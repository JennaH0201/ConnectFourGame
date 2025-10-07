# 🎮 ConnectFour Framework – IFN584 Group Project

Welcome to **ConnectFour**, a modular and extensible implementation of the classic two-player strategy game — developed by Team 14 for **IFN584: Software Development** at QUT.

This project demonstrates clean architecture, strategic object-oriented design, and robust gameplay mechanics, all delivered through a user-friendly C# console interface.

---

## 🧠 Project Overview

**ConnectFour** is a turn-based board game where players drop discs into a grid, aiming to connect four of their own symbols in a row — vertically, horizontally, or diagonally.

Our framework supports multiple game variants and modes, showcasing:

- Extensible architecture for future board games
- Special disc types with unique effects
- Human vs Human and Human vs Computer gameplay
- Undo/Redo, Save/Restore, and Spin mechanics
- Design pattern integration for maintainability and scalability

---

## 🧩 Game Variants Supported

| Game Variant     | Description                                                             |
|------------------|-------------------------------------------------------------------------|
| LineUp Classic   | Full-featured game with Ordinary, Boring, and Magnetic discs            |
| LineUp Basic     | Simplified version using only Ordinary discs and fixed grid size        |
| LineUp Spin      | Grid rotates 90° clockwise every 5 turns; gravity reapplied             |

---

## 👥 Team Members

| Name            | Student Number | Email                  | Role / Contribution               |
|-----------------|----------------|------------------------|-----------------------------------|
| Hamza Ateeq     | *Add ID*       | *Add Email*            | AI logic, disc placement          |
| Jaeeun Heo      | N1170519       | *Add Email*            | Undo/Redo system, testing         |
| Jennifer Ngo    | N1046724       | *Add Email*            | Save/Restore, help menu           |
| Philip Njoroge  | N1217634       | philip.njoroge@qut.edu.au | Grid logic, architecture, Spin |

---

## ✅ Assignment Requirements Checklist

| Requirement                 | Status| Notes                                                    |
|-----------------------------|---------|----------------------------------------------------------|
| Human vs Human mode         | ❌  | Validates moves and handles turn logic                |
| Human vs Computer mode      | ❌  | AI checks for winning move; else selects random move  |
| LineUp Classic              | ❌  | Includes all disc types and win logic                 |
| LineUp Basic                | ❌  | Ordinary discs only; fixed grid size                  |
| LineUp Spin                 | ❌  | Grid rotates every 5 turns; gravity reapplied         |
| Save and Restore            | ❌  | Game state saved to file and resumed accurately       |
| Undo and Redo               | ❌  | Full move history tracked; redo available after undo  |
| Help Menu                   | ❌  | Displays available commands and examples              |
| Modular Class Design        | ❌  | Follows OOP principles and design patterns            |
| Console Interface (.NET 8)  | ❌  | Text-based interface using ASCII/Unicode              |
| Design Patterns Applied     | ❌   | Template Method, Factory Method, Strategy, Command    |

---


## Project Configuration Files

This project includes two important configuration files to support development in **Visual Studio** and ensure smooth cross-platform compatibility:

### `.gitignore`

- **Purpose**: Hides files that are not relevant to the project (e.g. build artifacts, user-specific settings, temporary files).
- **Why it matters**: Keeps the repository clean and avoids cluttering version control with IDE-generated files.
- **Visual Studio users**: Prevents `.vs/`, `.user`, `.suo`, and other IDE-specific files from being tracked.
- **Cross-platform**: Ensures macOS and Windows users don’t commit OS-specific temp files.

### `.gitattributes`

- **Purpose**: Normalizes line endings and sets file handling rules across platforms.
- **Why it matters**: Prevents issues when switching between Windows (CRLF) and macOS/Linux (LF).
- **Visual Studio users**: Ensures consistent behavior when cloning or merging code.
- **Cross-platform**: Helps avoid merge conflicts due to line-ending differences.
