using System;

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();
    }
}

class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

class Player
{
    public string Name { get; set; }
    public Position Position { get; set; }
    public int GemCount { get; set; } = 0;

    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
    }

    public void Move(char direction)
    {
        switch (direction)
        {
            case 'U': Position.Y--; break;
            case 'D': Position.Y++; break;
            case 'L': Position.X--; break;
            case 'R': Position.X++; break;
        }
    }
}

class Cell
{
    public string Occupant { get; set; } = "-";
}

class Board
{
    public Cell[,] Grid { get; private set; }
    private int size = 6;

    public Board()
    {
        Grid = new Cell[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Grid[i, j] = new Cell();
            }
        }
        // Initialize players, gems, and obstacles here for simplicity
        Grid[0, 0].Occupant = "P1"; // Top-left corner
        Grid[5, 5].Occupant = "P2"; // Bottom-right corner
        // Place gems and obstacles randomly or statically as per requirements
    }

    public void Display()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Console.Write(Grid[i, j].Occupant + " ");
            }
            Console.WriteLine();
        }
    }

    public bool IsValidMove(Player player, char direction)
    {
        // Implement logic to check if a move is valid (e.g., not moving into obstacles)
        return true; // Placeholder for compilation
    }

    public void CollectGem(Player player)
    {
        // Implement logic to check for gem collection
    }
}

class Game
{
    public Board Board { get; private set; }
    public Player Player1 { get; private set; }
    public Player Player2 { get; private set; }
    public Player CurrentTurn { get; private set; }
    private int TotalTurns = 0;

    public Game()
    {
        Board = new Board();
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        CurrentTurn = Player1;
    }

    public void Start()
    {
        while (!IsGameOver())
        {
            Board.Display();
            Console.WriteLine($"{CurrentTurn.Name}'s turn. Enter direction (U,D,L,R): ");
            char direction = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (Board.IsValidMove(CurrentTurn, direction))
            {
                CurrentTurn.Move(direction);
                Board.CollectGem(CurrentTurn);
                SwitchTurn();
            }
            else
            {
                Console.WriteLine("Invalid move. Try again.");
            }
            TotalTurns++;
        }
        AnnounceWinner();
    }

    private void SwitchTurn()
    {
        CurrentTurn = CurrentTurn == Player1 ? Player2 : Player1;
    }

    private bool IsGameOver()
    {
        return TotalTurns >= 30;
    }

    private void AnnounceWinner()
    {
        // Implement logic to announce winner based on GemCount
    }
}
