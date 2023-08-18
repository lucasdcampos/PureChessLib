using System;
using System.Security.Cryptography.X509Certificates;
using PureChess;

class Program
{
    static void Main()
    {
        bool execute = true;
        string uciPos = "";

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "Pure Chess Shell";
        

        Game.Instance = new Game();

        Game game = Game.Instance;

        game.state = GameState.Waiting;

        Write("{=Blue}Welcome to PureChess Terminal{/}\n");
        Write("Type 'help' for the list of commands\n");

        while (execute)
        {
            string command = Console.ReadLine();
            
            switch (command)
            {
                case "quit":
                    execute= false;
                    break;

                case "help":
                    Console.WriteLine(" ");
                    Console.WriteLine("PureChess is a basic and simple Chess Engine that can be used to play chess games.\n" +
                        "Currently, there is no AI in the engine, which means that the program only validates and calculates moves specified by a human – nothing more.\n" +
                        "This program does not have a GUI (Graphical User Interface) by default. It is up to you to choose one available online or proceed without one.\n" +
                        "The recommended GUI to use is the one provided by 'OpenChessTools', as it was designed with PureChess in mind.\n" +
                        "For more information, please visit the documentation page: https://openchessgame.org/purechess/docs");
                    Console.WriteLine(" ");
                    Console.WriteLine("Available commands:");
                    Console.WriteLine("'play': Start a new Game");
                    Console.WriteLine("'quit': Quit the Application");
                    Console.WriteLine("'draw': Draw a sketch of the board in the current position");
                    Console.WriteLine("'uci': Will initialize a new custom game");
                    Console.WriteLine("'clear': Clean the Console");
                    Console.WriteLine("'version': Will display the current software version");
                    Console.WriteLine("'debug': Turn on/off debug mode");
                    Console.WriteLine("'charmode': Switch between char mode (p) to sprite mode (♟)");
                    Console.WriteLine("'draw': Draw a sketch of the board in the current position");
                    Console.WriteLine("'stats': Will display some stats of the Game");
                    Console.WriteLine(" ");
                    Console.WriteLine("To make a move, first type 'play' to start a new Game, and then just specify your move respecting the following format (ex: e2e4)");


                    break;

                case "uci" or "UCI":
                    Console.WriteLine("uciok");
                    Game.Instance.engine.uciValidations[0] = true;
                    Game.Instance.state = GameState.Initializing;
                    break;

                case "play":
                    if(game.state != GameState.Playing)
                    {
                        Console.WriteLine("Starting new Game");
                        game.StartGame(Game.Instance.board.defaultPosition);
                    }
                    else
                    {
                        Console.WriteLine("Error: You are already in a game!");
                    }

                    break;

                case "stop":
                    if(Game.Instance.state == GameState.Playing)
                    {
                        Console.WriteLine("The game has ended. Type 'play' to start a new one");
                    }
                    Game.Instance.StopGame();
                    
                    break;

                case "debug":
                    Game.Instance.settings.debugMode = !Game.Instance.settings.debugMode;
                    Console.WriteLine("debugMode = " +  Game.Instance.settings.debugMode);
                    break;

                case "charmode":
                    Game.Instance.settings.charMode = !Game.Instance.settings.charMode;
                    Console.WriteLine("charMode = " + Game.Instance.settings.charMode);
                    break;

                case "graphicalboard":
                    Game.Instance.settings.graphicalBoard = !Game.Instance.settings.graphicalBoard;
                    Console.WriteLine("graphicalBoard = " + Game.Instance.settings.graphicalBoard);
                    break;

                case "draw":
                    if(game.state != GameState.Playing) { break; }
                    Game.Instance.board.DrawCurrentPosition();
                    break;
                case "testfen":
                    Console.WriteLine(Game.Instance.ConvertFENToSNA("r1bqkb1r/pppp1ppp/2n2n2/4p3/2B1P3/5N2/PPPP1PPP/RNBQK2R"));
                    break;
                case "clear":
                    Console.Clear();
                    break;

                case "ucipos":
                    Console.WriteLine(uciPos);
                    break;

                case "stats":
                    Console.WriteLine($"Game Status: {Game.Instance.state}");
                    if (Game.Instance.state == GameState.Playing) Console.WriteLine($"Current Position: {Game.Instance.currentPosition}");

                    break;

                default:
                    if(game.state == GameState.Playing) 
                    {
                        if (command.Length == 4)
                        {
                            Board board = Game.Instance.board;

                            char initialX = command[0];
                            char initialY = command[1];
                            char targetX = command[2];
                            char targetY = command[3];

                            int startIndex = Game.Instance.ConvertToIndex(initialX, initialY);
                            int targetIndex = Game.Instance.ConvertToIndex(targetX, targetY);

                            bool isValid = startIndex <= board.squares.Count && startIndex >= 0 && targetIndex <= board.squares.Count && targetIndex >= 0;

                            if (isValid)
                            {
                                if (!Game.Instance.engine.MakeMove(board.squares[startIndex], board.squares[targetIndex]))
                                {
                                    Console.WriteLine("Illegal move! Please, follow the rules and try again!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error: move format invalid! Please type in the following format (ex: e2e4)");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Invalid move, please type in the following format (ex: e2e4).");
                        }
                    }

                    else if(game.state == GameState.Initializing)
                    {
                        switch (command)
                        {
                            case "isready":
                                if (Game.Instance.engine.uciValidations[1])
                                {
                                    Console.WriteLine("readyok");
                                }

                                break;
                            case "ucinewgame":
                                Game.Instance.engine.uciValidations[1] = true;
                                break;
                            case "position":
                                Game.Instance.engine.uciValidations[2] = true;
                                Console.WriteLine("type position SNA string");
                                break;
                            case "go":
                                if (Game.Instance.engine.uciValidations[3])
                                {
                                    Game.Instance.StartGame(uciPos);
                                    uciPos = string.Empty;
                                    break;
                                }
                                
                                break;
                            case "stop":
                                Game.Instance.state = GameState.Waiting;
                                break;
                            default:
                                if (Game.Instance.engine.uciValidations[2])
                                {
                                    Game.Instance.engine.uciValidations[3] = true;
                                    uciPos = command;
                                    break;
                                    
                                }
                                break;
                        }
                    } 
                    else
                    {
                        Console.WriteLine("Unknown command, type 'help' for more information!");
                    }

                    break;
            }

        }
    }

    static void Write(string msg)
    {
        string[] ss = msg.Split('{', '}');
        ConsoleColor c;
        foreach (var s in ss)
            if (s.StartsWith("/"))
                Console.ResetColor();
            else if (s.StartsWith("=") && Enum.TryParse(s.Substring(1), out c))
                Console.ForegroundColor = c;
            else
                Console.Write(s);
    }
}
