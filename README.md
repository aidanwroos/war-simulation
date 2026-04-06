# war-simulation

A simulation of the two-player card game War written in C#. This program simulates 2,000 games, and tracks round-by-round data in a custom linked list. It exports the data gathered from each round into a JSON file. The file is analyzed with a python script, pulling information including total rounds, avg rounds per game, min/max rounds in a game, JackPot distributions, War scenario distributions, and player win ratio.

## RESULTS
| Stat | Value |
|---|---|
|Total rounds | 362144 |
|Games detected | 2000 |
|Avg rounds per game | 183.6 |
|Min rounds in a game | 16 |
|Max rounds in a game | 1252 |
|Aidan win rate | 50.0% |
|Computer win rate | 50.0% |
|War rate | 6.8% |
|Largest pot| 34 cards|

Wins {'Aidan': 181070, 'Computer': 181074}
Pot distribution {2: 338726, 10: 21866, 18: 1444, 26: 106, 34: 2}
War count distribution |{0: 337664, 1: 22786, 2: 1571, 3: 118, 4: 5}

## How the simulation works
- A 52 card deck is shuffled and dealt between two players (each player's hand having 26 cards)
- Each round both player flip the top card in their hand - highest card wins both
- On tie (WAR) - each player puts 3 cards into the JackPot and flips a 4th to compare
- War continues until a player wins the pot and the game returns to its normal state
- The player who collects all the cards from the deck wins.
- Round simulation data is stored in a custom linked list and formatted/exported to a round.json file

## Technologies involved
- C# using OOP paradigm
- Python for data analyis

# How to run simulation
- open cmd terminal or IDE
- ensure .NET installed
- git clone this repository
- navigate to \games directory
- run "dotnet run"
- run "python parser.py" on newly created json file
- view sim results
