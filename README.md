# war-simulation

A simulation of the two-player card game War written in C#. This program simulates 2,000 games, and tracks round-by-round data in a custom linked list. It exports the data gathered from each round into a JSON file. The file is analyzed with a python script, pulling information including total rounds, avg rounds per game, min/max rounds in a game, JackPot distributions, War scenario distributions, and player win ratio.

## RESULTS
| Stat | Value |
|---|---|
|Total rounds | 362144 |
|Games detected | 2000 |
|Avg rounds per game | 183 |
|Min rounds in a game | 16 |
|Max rounds in a game | 1252 |
|Aidan win rate | 50.0% |
|Computer win rate | 50.0% |
|War rate | 6.8% |
|Largest pot| 34 cards|

## Win distribution
| Player | Wins |
|---|---|
| Aidan | 181,070 |
| Computer | 181,074 |

## Pot size distribution
| Pot size | Occurrences |
|---|---|
| 2 cards | 338,726 |
| 10 cards | 21,866 |
| 18 cards | 1,444 |
| 26 cards | 106 |
| 34 cards | 2 |

## War chain distribution
| Wars chained | Occurrences |
|---|---|
| 0 | 337,664 |
| 1 | 22,786 |
| 2 | 1,571 |
| 3 | 118 |
| 4 | 5 |

## How the simulation works
- A 52 card deck is shuffled and dealt between two players (each player's hand having 26 cards)
- Each round both players flip the top card in their hand - highest rank wins both cards
- On tie (WAR) - each player puts 3 cards into the JackPot and flips a 4th card  to compare
- War continues until a player wins the pot and the game returns to its normal state
- The player who collects all 52 cards from the deck wins.
- Round simulation data is stored in a custom linked list and formatted/exported to a round.json file
- The parse.py python script analyzes the json file containing the rounds and their data

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
