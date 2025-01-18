# Turtle Challenge

## Usage

The challenge can run run two ways  
Navigate to root folder  
  
dotnet  

- dotnet run --project .\src\TurtleChallenge\TurtleChallenge.csproj game-settings moves

exe  

- .\TurtleChallenge.exe game-settings moves
  
To run tests  

- dotnet test

## Challenge

The console application provided satisfies the requirement to create a program that accepts two file paths containing game settings and uses these to create a board, mines, exit and navigates a turtle through the minefield.  
  
The solution also contains a test assembly containing UTs covering happy flow, error flow and edge cases.

## Game Configuration File Specification

### 1. Board Size

- **Format:**  
  `width x height`
- **Description:**  
  Defines the dimensions of the board.
- **Example:**  
  `5x4` means a board with a width of 5 and a height of 4.

---

### 2. Starting Position and Direction

- **Format:**  
  `x=<integer>, y=<integer>, dir=<direction>`
- **Description:**  
  Specifies the starting coordinates `(x, y)` of the player and their initial facing direction.
- **Valid Directions:**  
  - `North`
  - `East`
  - `South`
  - `West`
- **Example:**  
  `x=0, y=1, dir=North`  
  The player starts at `(0, 1)` facing North.

---

### 3. Exit Coordinates

- **Format:**  
  `x=<integer>, y=<integer>`
- **Description:**  
  Specifies the coordinates `(x, y)` of the exit point.
- **Example:**  
  `x=4, y=2`  
  The exit is located at `(4, 2)`.

---

### 4. Mines

- **Format:**  
  `mines=<x1,y1;x2,y2;...>`
- **Description:**  
  A semicolon-separated list of coordinates where mines are located.
- **Rules:**  
  - Each mine is specified by its `(x, y)` position.
  - Mines must be within the board boundaries.
- **Example:**  
  `mines=1,1;1,3;3,3`  
  Mines are at `(1, 1)`, `(1, 3)`, and `(3, 3)`.

---

#### Example games-settings File

```
5x4
x=0, y=1, dir=North
x=4, y=2
mines=1,1;1,3;3,3
```

## Move Command File Specification

### File Format

Each line represents a sequence of moves or commands for the application. The commands consist of the following:

- `r`: Rotate clockwise.
- `m`: Move forward.

The sequence of characters can contain both `r` and `m` commands, and each line in the file represents a distinct series of moves.

### 1. Command Sequence

- **Format:**  
  Each line contains a string of characters where:
  - `r` stands for rotating clockwise.
  - `m` stands for moving forward.

- **Description:**  
  Each line represents one series of commands to be executed by the application, starting from an initial position or state. The sequence is read and executed in order.

- **Valid Commands:**  
  - `r`: Rotate clockwise (e.g., if the object is facing North, it will rotate to East).
  - `m`: Move forward one step (e.g., if the object is facing North, it will move up by one unit on the grid).

#### Example moves File

```
mrmmmmrmm
mrmmrmrmm
mrrmmmrmm
rmmmrmmmm
```