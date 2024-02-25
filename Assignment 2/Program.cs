using System;


class Position
{
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

class Player
{
    public string Name { get; }
    public Position Position { get; set; }
    public int GemCount { get; set; }

    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
    }

    public void Move(char direction)
    {
        switch (char.ToUpper(direction))
        {
            case 'U':
                Position = new Position(Position.X - 1, Position.Y);
                break;
            case 'D':
                Position = new Position(Position.X + 1, Position.Y);
                break;
            case 'L':
                Position = new Position(Position.X, Position.Y - 1);
                break;
            case 'R':
                Position = new Position(Position.X, Position.Y + 1);
                break;
            default:
                Console.WriteLine("Invalid direction.");
                break;
        }
    }
}

class Cell
{
    public string Occupant { get; set; }

    public Cell(string occupant)
    {
        Occupant = occupant;
    }
}

class Board
{
    public Cell[,] Grid { get; }

    public Board()
    {
        Grid = new Cell[6, 6];
        InitializeBoard();
        PlaceObstacles();
        PlaceGems();
    }

    private void InitializeBoard()
    {
        // Initialize cells with empty spaces
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Grid[i, j] = new Cell("-");
            }
        }

        // Place players
        Grid[0, 0].Occupant = "P1";
        Grid[5, 5].Occupant = "P2";


        Random random = new Random();
        for (int i = 0; i < 6; i++)
        {
            int obstacleX = random.Next(6);
            int obstacleY = random.Next(6);
            if (Grid[obstacleX, obstacleY].Occupant == "-")
            {
                Grid[obstacleX, obstacleY].Occupant = "O";
            }
        }

        // Place gems (random positions)
        for (int i = 0; i < 6; i++)
        {
            int gemX = random.Next(6);
            int gemY = random.Next(6);
            if (Grid[gemX, gemY].Occupant == "-")
            {
                Grid[gemX, gemY].Occupant = "G";
            }
        }
    }

    public void Display()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Write(Grid[i, j].Occupant + " ");
            }
            Console.WriteLine();
        }
    }

    public bool IsValidMove(Player player, char direction)
    {
        int newX = player.Position.X;
        int newY = player.Position.Y;

        switch (char.ToUpper(direction))
        {
            case 'U':
                newX--;
                break;
            case 'D':
                newX++;
                break;
            case 'L':
                newY--;
                break;
            case 'R':
                newY++;
                break;
            default:
                break;
        }

        if (newX < 0 || newX >= 6 || newY < 0 || newY >= 6)
        {
            return false;
        }

        if (Grid[newX, newY].Occupant == "O")
        {
            return false;
        }

        return true;
    }
    public void MovePlayer(Player player, char direction)
    {
        if (IsValidMove(player, direction))
        {
            int newX = player.Position.X;
            int newY = player.Position.Y;
            switch (char.ToUpper(direction))
            {
                case 'U':
                    newX--;
                    break;
                case 'D':
                    newX++;
                    break;
                case 'L':
                    newY--;
                    break;
                case 'R':
                    newY++;
                    break;
                default:
                    break;
            }

            if (Grid[newX, newY].Occupant != "-" && Grid[newX, newY].Occupant != "G")
            {
                Console.WriteLine("Invalid Move. Try Again!");
                return;
            }

            Grid[player.Position.X, player.Position.Y].Occupant = "-";
            player.Move(direction);
            CollectGem(player);
            Grid[player.Position.X, player.Position.Y].Occupant = player.Name;
        }
        else
        {
            Console.WriteLine("Invalid Move.Try Again!");
        }
    }

    public void CollectGem(Player player)
    {
        if (Grid[player.Position.X, player.Position.Y].Occupant == "G")
        {
            player.GemCount++;
            Grid[player.Position.X, player.Position.Y].Occupant = "-";

        }

    }
    public void PlaceObstacles()
    {
        Random random = new Random();
        int obstacleCount = random.Next(3, 5);
        int placedObstacles = 0;

        while (placedObstacles < obstacleCount)
        {
            int x = random.Next(6);
            int y = random.Next(6);

            if ((x != 0 || y != 0) && (x != 5 || y != 5) && Grid[x, y].Occupant == "-")
            {
                Grid[x, y].Occupant = "O";
                placedObstacles++;
            }
        }
    }

    public void PlaceGems()
    {
        Random random = new Random();
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if ((i != 0 || j != 0) && (i != 5 || j != 5) && random.Next(10) < 3)
                {
                    Grid[i, j].Occupant = "G";
                }
            }
        }
    }
}

class Game
{
    private readonly Board Board;
    private readonly Player Player1;
    private readonly Player Player2;
    private Player CurrentTurn;
    private int TotalTurns;

    public Game()
    {
        Board = new Board();
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        CurrentTurn = Player1;
        TotalTurns = 0;
    }

    public void Start()
    {
        while (!IsGameOver())
        {
            Board.Display();
            Console.WriteLine($"Current Turn: {CurrentTurn.Name}");
            Console.Write("Enter move (U/D/L/R): ");
            char move = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            if (Board.IsValidMove(CurrentTurn, move))
            {
                Board.MovePlayer(CurrentTurn, move);
                Board.CollectGem(CurrentTurn);
                SwitchTurn();
                TotalTurns++;
            }
            else
            {
                Console.WriteLine("Invalid move. Try again.");
            }
        }
        Console.Clear();
        Board.Display();
        AnnounceWinner();
    }

    public void DisplayRemainingTurns()
    {
        int remainingTurns = 30 - TotalTurns;
        Console.WriteLine($"Turns Remaining: {remainingTurns}");
    }

    public void SwitchTurn()
    {
        CurrentTurn = CurrentTurn == Player1 ? Player2 : Player1;
    }



    private bool IsGameOver()
    {
        return TotalTurns >= 30;
    }

    private void AnnounceWinner()
    {
        if (Player1.GemCount > Player2.GemCount)
        {
            Console.WriteLine("Player P1 wins!");
        }
        else if (Player1.GemCount < Player2.GemCount)
        {
            Console.WriteLine("Player P2 wins!");
        }
        else
        {
            Console.WriteLine("It's a tie!");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();
    }
}
