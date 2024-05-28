using Dame1.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dame1.Services
{
    public class Helper
    {
        public const char NO_PIECE = 'N';
        public const char WHITE_PIECE = 'W';
        public const char RED_PIECE = 'R';
        public const char RED_KING = 'T';
        public const char WHITE_KING = 'Q';
        public const char WHITE_TURN = '2';
        public const char RED_TURN = '1';

        public const int boardSize = 8;
        public static Squares Current { get; set; }
        private static Dictionary<Squares, Squares> currentNeighbours = new Dictionary<Squares, Squares>();
        private static Turn turn = new Turn(PieceColor.Red);
        private static bool extraMove = false;
        private static bool extraPath = false;
        private static int collectedRedPieces = 0;
        private static int collectedWhitePieces = 0;


        public static Dictionary<Squares, Squares> CurrentNeighbours
        {
            get
            {
                return currentNeighbours;
            }
            set
            {
                currentNeighbours = value;
            }
        }

        public static Turn Turn
        {
            get
            {
                return turn;
            }
            set
            {
                turn = value;
            }
        }

        public static bool EMove
        {
            get
            {
                return extraMove;
            }
            set
            {
                extraMove = value;
            }
        }

        public static bool Path
        {
            get
            {
                return extraPath;
            }
            set
            {
                extraPath = value;
            }
        }

        public static int CollectedWhitePieces
        {
            get { return collectedWhitePieces; }
            set { collectedWhitePieces = value; }
        }

        public static int CollectedRedPieces
        {
            get { return collectedRedPieces; }
            set { collectedRedPieces = value; }
        }
        public static ObservableCollection<ObservableCollection<Squares>> initBoard()
        {
            ObservableCollection<ObservableCollection<Squares>> board = new ObservableCollection<ObservableCollection<Squares>>();
            const int boardSize = 8;
            for (int i = 0; i < boardSize; i++)
            {
                board.Add(new ObservableCollection<Squares>());
                for (int j = 0; j < boardSize; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        board[i].Add(new Squares(i, j, SquareShade.Light, null));
                    }
                    else if (i < 3)
                    {
                        board[i].Add(new Squares(i, j, SquareShade.Dark, new Piece(PieceColor.White)));
                    }
                    else if (i > 4)
                    {
                        board[i].Add(new Squares(i, j, SquareShade.Dark, new Piece(PieceColor.Red)));
                    }
                    else
                    {
                        board[i].Add(new Squares(i, j, SquareShade.Dark, null));
                    }
                }
            }

            return board;
        }

        public static void ResetBoard(ObservableCollection<ObservableCollection<Squares>> squares)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        squares[i][j].Piece = null;
                    }
                    else
                        if (i< 3)
                    {
                        squares[i][j].Piece = new Piece(PieceColor.White);
                        squares[i][j].Piece.Square = squares[i][j];
                    }
                    else
                        if (i > 4)
                    {
                        squares[i][j].Piece = new Piece(PieceColor.Red);
                        squares[i][j].Piece.Square = squares[i][j];
                    }
                    else
                    {
                        squares[i][j].Piece = null;
                    }
                }
            }
        }
        public static bool isInBounds(int i, int j)
        {
            return i >= 0 && j >= 0 && i < boardSize && j < boardSize;
        }

        public static void NeighboursToBeChecked(Squares square, HashSet<Tuple<int, int>> neighboursToCheck)
        {
            if (square.Piece.Type == PieceType.King)
            {
                neighboursToCheck.Add(new Tuple<int, int>(-1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(-1, 1));
                neighboursToCheck.Add(new Tuple<int, int>(1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(1, 1));
            }
            else if (square.Piece.Color == PieceColor.Red)
            {
                neighboursToCheck.Add(new Tuple<int, int>(-1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(-1, 1));
            }
            else
            {
                neighboursToCheck.Add(new Tuple<int, int>(1, -1));
                neighboursToCheck.Add(new Tuple<int, int>(1, 1));
            }
        }
        public const string redSquare = "C:\\IA_anu2\\MVP\\Dame1\\Dame1\\RedSquare.png";
        public const string whiteSquare = "C:\\IA_anu2\\MVP\\Dame1\\Dame1\\WhiteSquare.png";
        public const string redPiece = "C:\\IA_anu2\\MVP\\Dame1\\Dame1\\RedPiece.jpg";
        public const string whitePiece = "C:\\IA_anu2\\MVP\\Dame1\\Dame1\\WhitePiece.png";
        public const string hintSquare = "C:\\IA_anu2\\MVP\\Dame1\\Dame1\\Hint.png";
        public const string redKingPiece = "C:\\IA_anu2\\MVP\\Dame1\\Dame1\\RedKing.png";
        public const string whiteKingPiece = "C:\\IA_anu2\\MVP\\Dame1\\Dame1\\WhiteKing.png";
        public static void Load(ObservableCollection<ObservableCollection<Squares>> squares)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            bool? answer = openDialog.ShowDialog();

            if (answer == true)
            {
                string path = openDialog.FileName;
                using (var reader = new StreamReader(path))
                {
                    string text;
                    text = reader.ReadLine();
                    if (text == RED_TURN.ToString())
                    {
                        Turn.PlayerColor = PieceColor.Red;
                        Turn.TurnImage = redPiece;
                    }
                    else
                    {
                        Turn.PlayerColor = PieceColor.White;
                        Turn.TurnImage = redPiece;
                    }
                    for (int i = 0; i < boardSize; i++)
                    {
                        text = reader.ReadLine();
                        for (int j = 0; j < boardSize; j++)
                        {
                            squares[i][j].LegalSquareSymbol = null;

                            if (text[j] == NO_PIECE)
                            {
                                squares[i][j].Piece = null;
                            }
                            else if (text[j] == RED_PIECE)
                            {
                                squares[i][j].Piece = new Piece(PieceColor.Red, PieceType.Regular);
                                squares[i][j].Piece.Square = squares[i][j];
                            }
                            else if (text[j] == RED_KING)
                            {
                                squares[i][j].Piece = new Piece(PieceColor.Red, PieceType.King);
                                squares[i][j].Piece.Square = squares[i][j];
                            }
                            else if (text[j] == WHITE_PIECE)
                            {
                                squares[i][j].Piece = new Piece(PieceColor.White, PieceType.Regular);
                                squares[i][j].Piece.Square = squares[i][j];
                            }
                            else if (text[j] == WHITE_KING)
                            {
                                squares[i][j].Piece = new Piece(PieceColor.White, PieceType.King);
                                squares[i][j].Piece.Square = squares[i][j];
                            }
                        }
                    }


                    foreach (var square in CurrentNeighbours.Keys)
                    {
                        square.LegalSquareSymbol = null;
                    }

                    CurrentNeighbours.Clear();

                    do
                    {
                        text = reader.ReadLine();
                        if (text == "-")
                        {
                            if (text.Length == 1)
                            {
                                break;
                            }
                            CurrentNeighbours.Add(squares[(int)char.GetNumericValue(text[0])][(int)char.GetNumericValue(text[1])], null);
                        }
                        else
                        {
                            CurrentNeighbours.Add(squares[(int)char.GetNumericValue(text[0])][(int)char.GetNumericValue(text[1])],
                                squares[(int)char.GetNumericValue(text[2])][(int)char.GetNumericValue(text[3])]);
                        }
                    } while (text != "-");

                }
            }
        }

        public static void Save(ObservableCollection<ObservableCollection<Squares>> squares)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            bool? answer = saveDialog.ShowDialog();
            if (answer == true)
            {
                var path = saveDialog.FileName;
                using (var writer = new StreamWriter(path))
                {
                    if (Turn.PlayerColor.Equals(PieceColor.Red))
                    {
                        writer.Write(RED_TURN);
                    }
                    else
                    {
                        writer.Write(WHITE_TURN);
                    }
                    writer.WriteLine();
                    foreach (var line in squares)
                    {
                        foreach (var square in line)
                        {
                            if (square.Piece == null)
                            {
                                writer.Write(NO_PIECE);
                            }
                            else if (square.Piece.Color.Equals(PieceColor.Red) && square.Piece.Type == PieceType.Regular)
                            {
                                writer.Write(RED_PIECE);
                            }
                            else if (square.Piece.Color.Equals(PieceColor.White) && square.Piece.Type == PieceType.Regular)
                            {
                                writer.Write(WHITE_PIECE);
                            }
                            else if (square.Piece.Color.Equals(PieceColor.White) && square.Piece.Type == PieceType.King)
                            {
                                writer.Write(WHITE_KING);
                            }
                            else if (square.Piece.Color.Equals(PieceColor.Red) && square.Piece.Type == PieceType.King)
                            {
                                writer.Write(RED_KING);
                            }
                        }
                        writer.WriteLine();
                    }


                    foreach (var square in CurrentNeighbours.Keys)
                    {
                        if (CurrentNeighbours[square] == null)
                        {
                            writer.Write(square.Row.ToString() + square.Column.ToString() + NO_PIECE);
                        }
                        else
                        {
                            writer.Write(square.Row.ToString() + square.Column.ToString() + CurrentNeighbours[square].Row.ToString() + CurrentNeighbours[square].Column.ToString());
                        }
                        writer.WriteLine();
                    }
                    writer.Write("-\n");
                }
            }
        }
        public static void Reset(ObservableCollection<ObservableCollection<Squares>> squares)
        {
            foreach (var square in CurrentNeighbours.Keys)
            {
                square.LegalSquareSymbol = null;
            }

            if (Current != null)
            {
                Current.Texture = redSquare;
            }

            currentNeighbours.Clear();
            Current = null;
            EMove = false;
            Path = false;
            CollectedWhitePieces = 0;
            CollectedRedPieces = 0;
            Turn.PlayerColor = PieceColor.Red;
            ResetBoard(squares);
        }
        public static void About()
        {
            string PATH = "C:\\IA_anu2\\MVP\\Dame1\\Dame1\\about.txt";

            using (var reader = new StreamReader(PATH))
            {
                MessageBox.Show(reader.ReadToEnd(), "About", MessageBoxButton.OKCancel);
            }
        }
        public static void writeScore(int r, int w)
        {
            string PATH = "C:\\IA_anu2\\MVP\\Dame1\\Dame1\\Wins.txt";
            using (var writer = new StreamWriter(PATH))
            {
                writer.WriteLine(r + "," + w);
            }
        }

        public static Winner getScore()
        {
            Winner aux = new Winner(0, 0);
            string PATH = "C:\\IA_anu2\\MVP\\Dame1\\Dame1\\Wins.txt";
            if (File.Exists(PATH))
            {
                    using (var reader = new StreamReader(PATH))
                    {
                        string line = reader.ReadLine();
                        var splitted = line.Split(',');
                        if (splitted.Length >= 2)
                        {
                            aux.RedWins = int.TryParse(splitted[0], out int redWins) ? redWins : 0;
                            aux.WhiteWins = int.TryParse(splitted[1], out int whiteWins) ? whiteWins : 0;
                        }
                    }
                
            }

            return aux;
        }
        private const string MULTIPLE_JUMPS_FILE_PATH = "C:\\IA_anu2\\MVP\\Dame1\\Dame1\\MultipleJumps.txt";

        public static void SaveMultipleJumpsState(bool state)
        {
            using (StreamWriter writer = new StreamWriter(MULTIPLE_JUMPS_FILE_PATH))
            {
                writer.WriteLine(state.ToString());
            }
        }

        public static bool LoadMultipleJumpsState()
        {
            bool state = false;
            if (File.Exists(MULTIPLE_JUMPS_FILE_PATH))
            {
                using (StreamReader reader = new StreamReader(MULTIPLE_JUMPS_FILE_PATH))
                {
                    string line = reader.ReadLine();
                    bool.TryParse(line, out state);
                }
            }
            return state;
        }
    }
}
