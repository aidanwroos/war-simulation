using System;
using System.Collections.Generic;

public class Card
{
	public string Suit { get; set; }
	public string Rank { get; set; }
	private static Dictionary<string, int> _rankValues;
	
	static Card()
	{
		_rankValues = new Dictionary<string, int>();
		
		for(int i = 2; i <= 10; i++)
			_rankValues.Add(i.ToString(), i);
		
		_rankValues.Add("Jack", 11);
		_rankValues.Add("Queen", 12);
		_rankValues.Add("King", 13);
		_rankValues.Add("Ace", 14);
	}
	
	//return rank value for given rank
	public int Value
	{
		get { return _rankValues[Rank]; }
	}
	
	//card constructor
	public Card(string suit, string rank)
	{
		Suit = suit;
		Rank = rank;
	}
	
	//prints the card details
	public override string ToString()
	{
		return $"{Rank} of {Suit} with value {Value}";
	}
}

public class Deck
{
	private List<Card> _cards; // A complete 52 card deck
	private static Random _random = new Random(); // created once, shared
	
	//Deck constructor
	public Deck()
	{
		_cards = new List<Card>();
		BuildDeck(); //builds deck of cards
	}
	
	//method function to build the 52 card deck
	private void BuildDeck()
	{
		string[] suits = {"Hearts", "Diamonds", "Clubs", "Spades"};
		string[] ranks = {"Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King"};
		
		// (4 * 13 = 52)
		foreach (string suit in suits)
			foreach (string rank in ranks)
				_cards.Add(new Card(suit, rank));
		
		Console.WriteLine($"Deck built with {_cards.Count} cards");
	}
	
	//draw a single card from the deck
	public Card Deal()
	{
		if(_cards.Count == 0)
			throw new Exception("No cards left in the deck!");
		
		Card card = _cards[0]; //draw from top of the deck
		_cards.RemoveAt(0);
		return card;           //return the card object
	}
	
	public void printDeck()
	{
		for(int i=0; i < _cards.Count; i++)
		{
			Console.WriteLine($"{_cards[i].ToString()}");
		}
	}
	
	//completely shuffle the deck
	public void Shuffle()
	{
		
		for(int i = _cards.Count - 1; i > 0; i--)
		{
			int j = _random.Next(0, i + 1); //pick random index
			Card temp = _cards[i];         //swap the cards
			_cards[i] = _cards[j];
			_cards[j] = temp;
		}
		Console.WriteLine("Deck is shuffled!");
	}
}

public class Player
{
	private string _name;
	private Queue<Card> _hand;
	
	//player name property (getters and setters essentially)
	public string Name
	{
		get {return _name;}
		set 
		{
			if(string.IsNullOrEmpty(value))
				throw new Exception("Player name can't be empty!");
			_name = value;
		}
	}
	
	//player hand property
	public Queue<Card> Hand
	{
		get {return _hand;}
	}
	
	//removes card and returns from the front automatically
	public Card flipCard()
	{	
		return _hand.Dequeue(); 
	}
	
	//player constructor
	public Player(string name)
	{
		Name = name;
		_hand = new Queue<Card>();
		Console.WriteLine($"Player with name {name} created!");
	}
	
	//draw card method
	public void AddCard(Card card)
	{
		_hand.Enqueue(card);
	}
	
	//return card count in player's hand
	public int CardCount
	{
		get { return _hand.Count; }
	}
	
	//display cards in player's hand
	public void PrintHand()
	{
		Console.WriteLine("\n" + Name + "\'s hand:");  
						  
		foreach(Card card in _hand)
			Console.WriteLine(card);
	}
	
	
}

public class Game
{
	private Deck _deck;
	private Player player1;
	private Player player2;
	private RoundLinkedList _rounds; //linked list of all rounds in the game
	
	public Game(string player1Name, string player2Name)
	{
		_deck = new Deck();
		_deck.Shuffle();
		player1 = new Player(player1Name);
		player2 = new Player(player2Name);
		_rounds = new RoundLinkedList(); //initialize the linked list
		DealCards();
	}
	//split the deck evenly between the two players
	public void DealCards()
	{
		for(int i=0; i < 26; i++)
		{
			player1.AddCard(_deck.Deal());
			player2.AddCard(_deck.Deal());
		}
	}
	
	public void Start()
	{
		int rounds = 0;
		while(player1.CardCount > 0 && player2.CardCount > 0)
		{
			PlayRound(rounds);
			rounds += 1; //increment number of rounds played (excluding war sub-rounds)
			
		}
		DeclareWinner();
	}
	
	public void DeclareWinner()
	{
		if(player1.CardCount == 0)
			Console.WriteLine($"\n{player2.Name} wins!");
		else
			Console.WriteLine($"\n{player1.Name} wins!");
		Console.WriteLine($"\n----Game Summary ({_rounds.Count} rounds) ----\n");
		
		_rounds.ExportToJson("rounds.json"); //export round data to JSON file
		_rounds.PrintAll();
	}
	
	private int PlayRound(int rounds)
	{
		//--------Data Collected---------
		int roundnumber = rounds;
		string winner = "";
		int warCount = 0;
		int potSize = 2;
		//-------------------------------
		
		Card card_player1, card_player2;
	
		//each player draws 1 card from top of their hand
		card_player1 = player1.flipCard();
		card_player2 = player2.flipCard();
		
		Console.WriteLine($"\n--------------Round:[{rounds}]----------------------");
		Console.WriteLine($"Player 1 card: {card_player1.ToString()}");
		Console.WriteLine($"Player 2 card: {card_player2.ToString()}\n");
		
		if(card_player1.Value > card_player2.Value)
		{
			winner = player1.Name;
			player1.AddCard(card_player1);
			player1.AddCard(card_player2);
			Console.WriteLine("Player 1 wins the round!");
		}
		else if (card_player1.Value < card_player2.Value)
		{
			winner = player2.Name;
			player2.AddCard(card_player2);
			player2.AddCard(card_player1);
			Console.WriteLine("Player 2 wins the round!");
		}
		else //war situation
		{
			Console.WriteLine("WAR!!\n");
			
			List<Card> JackPot = new List<Card>(); //combined jackpot
			JackPot.Add(card_player1);
			JackPot.Add(card_player2);
			
			bool warOver = false;
			while(!warOver)
			{
				warCount++;
				//first check if each player even has enough cards to do war with
				if(player1.CardCount < 4)
				{
					Console.WriteLine($"{player2.Name} wins the war - {player1.Name} doesn't have enough cards!");
					foreach (Card c in JackPot) player2.AddCard(c);
					winner = player2.Name;
					potSize = JackPot.Count;
					break; //exit war loop
				}
				if(player2.CardCount < 4)
				{
					Console.WriteLine($"{player1.Name} wins the war - {player2.Name} doesn't have enough cards!");
					foreach (Card c in JackPot) player1.AddCard(c);
					winner = player1.Name;
					potSize = JackPot.Count;
					break; //exit war loop
				}
				
				//each player draws 3 cards from their hand, and adds them to the pot
				Console.WriteLine("Each player draws 3 cards and adds to the pot...");
				for (int i=0; i < 3; i++)
				{
					JackPot.Add(player1.flipCard());
					JackPot.Add(player2.flipCard());
				}
				
				Card warCard1, warCard2;
				warCard1 = player1.flipCard();
				warCard2 = player2.flipCard();
				JackPot.Add(warCard1);
				JackPot.Add(warCard2);
				
				//show each player's war cards
				Console.WriteLine($"{player1.Name} war card: {warCard1}");
				Console.WriteLine($"{player2.Name} war card: {warCard2}");
				
				if (warCard1.Value > warCard2.Value)
				{
					winner = player1.Name;
					Console.WriteLine($"{player1.Name} wins the war and takes {JackPot.Count} cards from the pot!");
					foreach(Card c in JackPot) player1.AddCard(c);
					potSize = JackPot.Count;
					warOver = true;
				}
				else if (warCard2.Value > warCard1.Value)
				{
					winner = player2.Name;
					Console.WriteLine($"{player2.Name} wins the war and takes {JackPot.Count} cards from the pot!");
					foreach(Card c in JackPot) player2.AddCard(c);
					potSize = JackPot.Count;
					warOver = true;
				}
				else
				{
					Console.WriteLine("WAR AGAIN!!"); //tied, run another war
				}
			}
		}
		Console.WriteLine("----------------------------------------------");
		
		//compare both player's cards
			//if the cards have the same values:
				//1. each player draws 3 cards and returns the 4th card
				//2. compare each player's 4th card
				//3. if cards have same values:
					//(repeat steps 1-3)
			//whoever's card's value is smallest:
				//give other player all cards drawn in the round
		
		//int roundnumber = rounds;
		//string winner = "";
		//int warCount = 0;
		//int potSize = 2;
		Round round = new Round(roundnumber, card_player1, card_player2, winner, warCount, potSize);
		_rounds.AddRound(round);
		
		return 0;
	}
}

//[Structuring the simulation results here:]
//-----------------------------------------------------------------------------------------------------

//Structure holding information for a single round
public class Round
{
	public int RoundNumber { get; set;}
	public Card Player1Card { get; set; }
	public Card Player2Card { get; set; }
	public string Winner { get; set; }
	public int WarCount { get; set; }
	public int PotSize { get; set; }
	
	//constructor
	public Round(int roundNum, Card p1Card, Card p2Card, string winner, int wCount, int potSize)
	{
		//(data being collected from each round)
		RoundNumber = roundNum;  //round number
		Player1Card = p1Card;    //player1's card
		Player2Card = p2Card;    //player2's card
		Winner = winner;         //name of winner
		WarCount = wCount;       //number of war rounds
		PotSize = potSize;       //number of cards in pot
	}
	
	//to easily display the round data if we want to
	public override string ToString()
	{
		return $"Round: {RoundNumber}, P1 card: {Player1Card.ToString()}, P2 card: {Player2Card.ToString()}, Winner: {Winner}, PotSize: {PotSize}, War Count: {WarCount}"; 
	}
}

//A single node, holding 1 round and its information
public class RoundNode
{
	public Round Data { get; set; }
	public RoundNode Next { get; set; }
	
	//RoundNode constructor
	public RoundNode(Round data)
	{
		Data = data;
		Next = null; //no next node by default
	}
}

public class RoundLinkedList
{
	private RoundNode _head; //first node in the RoundLinkedList
	private RoundNode _tail; //last node
	public int Count { get; private set; }
	
	public void ExportToJson(string filename)
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.AppendLine("[");
		
		RoundNode current = _head;
		while(current != null)
		{
			Round r = current.Data;
			sb.AppendLine("  {");
			sb.AppendLine($"    \"roundNumber\": {r.RoundNumber},");
			sb.AppendLine($"    \"player1Card\": \"{r.Player1Card.Rank}\",");
			sb.AppendLine($"    \"player2Card\": \"{r.Player2Card.Rank}\",");
			sb.AppendLine($"    \"winner\": \"{r.Winner}\",");
			sb.AppendLine($"    \"warCount\": {r.WarCount},");
			sb.AppendLine($"    \"potSize\": {r.PotSize}");
			sb.Append(current.Next != null ? "  }," : "  }");
			sb.AppendLine();
			current = current.Next;
		}
		
		sb.AppendLine("]");
		System.IO.File.WriteAllText(filename, sb.ToString());
		Console.WriteLine($"Exported {Count} rounds to {filename}");
	}
	
	//linkedlist constructor
	public RoundLinkedList()
	{
		_head = null;
		_tail = null;
		Count = 0;
	}
	
	public void AddRound(Round round)
	{
		RoundNode newNode = new RoundNode(round);
		
		if(_head == null) //list is empty
		{
			_head = newNode;
			_tail = newNode;
		}
		else
		{
			_tail.Next = newNode;
			_tail = newNode;
		}
		
		Count += 1; //increment number of nodes in list
	}
	
	//print all the rounds in the game
	public void PrintAll()
	{
		RoundNode current = _head;
		while(current != null)
		{
			Console.WriteLine(current.Data); //prints the round's data
			current = current.Next; 
		}
	}
}

//-----------------------------------------------------------------------------------------------------
public class Program
{
	public static void Main()
	{
		Game game = new Game("Aidan", "Computer");
		game.Start();
	}
}
