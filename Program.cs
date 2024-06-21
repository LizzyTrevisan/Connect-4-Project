using System.Text;

public class ConnectFourProject
{
    private GameBoard board;
    private Player player1;
    private Player player2;
    private Player currentPlayer;
    private ConsoleUI ui;

    public ConnectFourProject()
    {
        board = new GameBoard();
        ui = new ConsoleUI();
        player1 = new HumanPlayer('\u2B23', ui); // Human player Blue Emoji
        player2 = new ComputerPlayer('\u2B24', board); // Computer player Red Emoji
        currentPlayer = player1; // this is get Human to start the game
    }

    public void PlayGame()
    {
        while (true)
        {
            ui.PrintBoard(board);

            if (currentPlayer == player1)
            {
                Console.WriteLine("Your turn"); //Prints Human's turn
            }
            else
            {
                Console.WriteLine("PC turn ...");
                Thread.Sleep(2000); // it will provide 2 seconds delay for the computer's turn - reference of the source at the end
            }

            int column;
            while (true)
            {
                column = currentPlayer.GetMove(); // Get move from the current player
                if (board.MakeMove(currentPlayer.Symbol, column))
                {
                    break; // if a Valid move made, it will break out of the loop
                }
                Console.WriteLine("Invalid move, try again.");
            }

            if (board.CheckWin(currentPlayer.Symbol))//Set who won and accordingly to the emoji color.
            {
                ui.PrintBoard(board);
                SetWinColor(currentPlayer.Symbol);
                Console.WriteLine($"{currentPlayer.Symbol} wins!");
                Console.ResetColor(); //It will reset color of the emoji
                break; // the Game over function, current player wins
            }

            if (board.IsFull())
            {
                ui.PrintBoard(board);
                Console.WriteLine("It's a draw!");
                break; // Game over if statement in case the board is full and no winners
            }

            // this code will Switch to the other player
            currentPlayer = (currentPlayer == player1) ? player2 : player1;
        }
    }

    private void SetWinColor(char symbol) //SetWinColor fuction called above used for the winner if the Computer wins the result is in red otherwise Human won and it will be in blue.
    {
        if (symbol == '\u2B24')
        {
            Console.ForegroundColor = ConsoleColor.Red; //computer
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Blue; //Human
        }
    }


    public class GameBoard
    {
        private char[,] board;
        private const int rows = 6;
        private const int cols = 7;

        public GameBoard() //Gameboard function to locate the empty spaces and later on the emoji in the rows.
        {
            board = new char[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    board[r, c] = '.';
                }
            }
        }

        public bool MakeMove(char symbol, int col) // Make a move function using columns iteration
        {
            if (col < 0 || col >= cols)
                return false;

            for (int row = rows - 1; row >= 0; row--)
            {
                if (board[row, col] == '.')
                {
                    board[row, col] = symbol;
                    return true;
                }
            }
            return false;
        }

        public bool CheckWin(char symbol)
        {
            return CheckHorizontalWin(symbol) || CheckVerticalWin(symbol) || CheckDiagonalWin(symbol);
        }

        private bool CheckHorizontalWin(char symbol) //Checking the horizontal possibility win function iterating
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c <= cols - 4; c++) //  cols - 4 to check starting positions where at least four rows are remaining below the current row. The purpose is to avoid out-of-bounds indices in the array.
                {
                    if (board[r, c] == symbol && board[r, c + 1] == symbol && board[r, c + 2] == symbol && board[r, c + 3] == symbol)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckVerticalWin(char symbol)
        {
            for (int c = 0; c < cols; c++)
            {
                for (int r = 0; r <= rows - 4; r++) //Same refence for the cols -4, however, checking the rows.
                {
                    if (board[r, c] == symbol && board[r + 1, c] == symbol && board[r + 2, c] == symbol && board[r + 3, c] == symbol)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckDiagonalWin(char symbol)
        {
            for (int r = 0; r <= rows - 4; r++)
            {
                for (int c = 0; c <= cols - 4; c++)
                {
                    if (board[r, c] == symbol && board[r + 1, c + 1] == symbol && board[r + 2, c + 2] == symbol && board[r + 3, c + 3] == symbol)
                    {
                        return true;
                    }
                }

                for (int c = 3; c < cols; c++)
                {
                    if (board[r, c] == symbol && board[r + 1, c - 1] == symbol && board[r + 2, c - 2] == symbol && board[r + 3, c - 3] == symbol)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsFull()
        {
            for (int c = 0; c < cols; c++)
            {
                if (board[0, c] == '.')
                {
                    return false;
                }
            }
            return true;
        }

        public char[,] GetBoard()
        {
            return board;
        }
    }

    public abstract class Player
    {
        public char Symbol { get; private set; }

        protected Player(char symbol)
        {
            Symbol = symbol;
        }

        public abstract int GetMove();
    }

    public class HumanPlayer : Player
    {
        private ConsoleUI ui;

        public HumanPlayer(char symbol, ConsoleUI ui) : base(symbol)
        {
            this.ui = ui;
        }

        public override int GetMove()
        {
            return ui.GetMove();
        }
    }

    public class ComputerPlayer : Player
    {
        private GameBoard board;

        public ComputerPlayer(char symbol, GameBoard board) : base(symbol)
        {
            this.board = board;
        }

        public override int GetMove()
        {
            Random random = new Random();
            int col;
            do
            {
                col = random.Next(0, 6);
            } while (!IsColumnValid(col));
            return col;
        }

        private bool IsColumnValid(int col)
        {
            char[,] b = board.GetBoard();
            return b[0, col] == '.';
        }
    }

    public class ConsoleUI
    {
        public void PrintBoard(GameBoard board)
        {
            char[,] b = board.GetBoard();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Human Player: '\u2B23'"); //Colored Emoji - Blue chip
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("PC Player: '\u2B24'"); //Colored Emoji - Red Chip
            Console.ResetColor();
            Console.WriteLine("  0   1   2   3   4   5   6");
            Console.WriteLine("+---+---+---+---+---+---+---+");

            for (int r = 0; r < 6; r++) //grid 6 rows
            {
                Console.Write("|");
                for (int c = 0; c < 7; c++) //grid 7 columns
                {
                    switch (b[r, c])
                    {
                        case '\u2B23':
                            Console.ForegroundColor = ConsoleColor.Blue; //if the cell has the emoji \u2B23, Huma's Chip, it will print it in blue
                            break;
                        case '\u2B24':
                            Console.ForegroundColor = ConsoleColor.Red; //if te cell has the emoji '\u2B24', Computer Chip, it will print the emoji um red.
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                    Console.Write($" {b[r, c]} ");
                    Console.ResetColor();
                    Console.Write("|");
                }
                Console.WriteLine();
                Console.WriteLine("+---+---+---+---+---+---+---+");
            }
            Console.WriteLine();
        }

        public int GetMove()
        {
            Console.WriteLine("Enter a column (0-6): "); //Prints the instruction to the player
            int column;
            while (!int.TryParse(Console.ReadLine(), out column) || column < 0 || column > 6)
            {
                Console.WriteLine("Invalid input. Enter a column (0-6): "); //in case user enters an invalid number ti will display and ask to enter a number from 0-6
            }
            return column;
        }

        public void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }
    }


    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8; // Ensure the console uses UTF-8 encoding for the symbols to work
        ConnectFourProject game = new ConnectFourProject();
        game.PlayGame();
    }
}
/*REFERENCES:
1- OpenAI. (2023). ChatGPT (Mar 14 version) [Large language model]. https://chat.openai.com/chat - for lines 17  and 18 unicode emojis reference. 229 Get Move function and 54 Reset Color.
 
2 - https://learn.microsoft.com/en-us/dotnet/api/system.threading.thread.sleep?view=net-8.0 - Thread.Sleep()

3 - https://learn.microsoft.com/en-us/dotnet/api/system.console.foregroundcolor?view=net-8.0 - Foreground Color - Emojis

*/
