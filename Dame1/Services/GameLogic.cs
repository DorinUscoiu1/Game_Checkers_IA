using Dame1.Model;
using Dame1.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dame1.Services
{
    public class GameLogic: INotifyPropertyChanged
    {
        private ObservableCollection<ObservableCollection<Squares>> board;
        private Turn playerTurn;
        private Winner winner;
        private bool enableMultipleJumps = false;
        public bool MultipleJumps
        {
            get { return enableMultipleJumps; }
            set
            {
                enableMultipleJumps = value;
                OnPropertyChanged(nameof(MultipleJumps));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GameLogic(ObservableCollection<ObservableCollection<Squares>> board, Turn playerTurn, Winner winner)
        {
            this.board = board;
            this.playerTurn = playerTurn;
            this.winner = winner;
            this.winner.RedWins = Helper.getScore().RedWins;
            this.winner.WhiteWins = Helper.getScore().WhiteWins;
        }
        private void Switch(Squares square)
        {
            if (square.Piece.Color == PieceColor.Red)
            {
                Helper.Turn.PlayerColor = PieceColor.White;
                Helper.Turn.TurnImage = Helper.whitePiece;
                playerTurn.PlayerColor = PieceColor.White;
                playerTurn.TurnImage = Helper.whitePiece;
            }
            else
            {
                Helper.Turn.PlayerColor = PieceColor.Red;
                Helper.Turn.TurnImage = Helper.redPiece;
                playerTurn.PlayerColor = PieceColor.Red;
                playerTurn.TurnImage = Helper.redPiece;
            }
        }

        private void FindNeighbours(Squares square)
        {
            var neighboursToCheck = new HashSet<Tuple<int, int>>();

            Helper.NeighboursToBeChecked(square, neighboursToCheck);

            foreach (Tuple<int, int> neighbour in neighboursToCheck)
            {
                if (Helper.isInBounds(square.Row + neighbour.Item1, square.Column + neighbour.Item2))
                {
                    if (board[square.Row + neighbour.Item1][square.Column + neighbour.Item2].Piece == null)
                    {
                        if (!Helper.EMove)
                        {
                            Helper.CurrentNeighbours.Add(board[square.Row + neighbour.Item1][square.Column + neighbour.Item2], null);
                        }
                    }
                    else if (Helper.isInBounds(square.Row + neighbour.Item1 * 2, square.Column + neighbour.Item2 * 2) &&
                        board[square.Row + neighbour.Item1][square.Column + neighbour.Item2].Piece.Color != square.Piece.Color &&
                        board[square.Row + neighbour.Item1 * 2][square.Column + neighbour.Item2 * 2].Piece == null)
                    {
                        if (MultipleJumps)
                        {
                            Helper.CurrentNeighbours.Add(board[square.Row + neighbour.Item1 * 2][square.Column + neighbour.Item2 * 2], board[square.Row + neighbour.Item1][square.Column + neighbour.Item2]);
                            Helper.Path = true;
                        }
                        else if (!Helper.EMove)
                        {
                            Helper.CurrentNeighbours.Add(board[square.Row + neighbour.Item1 * 2][square.Column + neighbour.Item2 * 2], board[square.Row + neighbour.Item1][square.Column + neighbour.Item2]);
                            Helper.Path = true;
                            break;
                        }
                    }
                }
            }
        }

        private void DisplayMoves(Squares square)
        {
            if (Helper.Current != square)
            {
                if (Helper.Current != null)
                {
                    board[Helper.Current.Row][Helper.Current.Column].Texture = Helper.redSquare;

                    foreach (Squares selectedSquare in Helper.CurrentNeighbours.Keys)
                    {
                        selectedSquare.LegalSquareSymbol = null;
                    }
                    Helper.CurrentNeighbours.Clear();
                }

                FindNeighbours(square);

                if (Helper.EMove && !Helper.Path)
                {
                    Helper.EMove = false;
                    Switch(square);
                }
                else
                {

                    foreach (Squares neighbour in Helper.CurrentNeighbours.Keys)
                    {
                        board[neighbour.Row][neighbour.Column].LegalSquareSymbol = Helper.hintSquare;
                    }

                    Helper.Current = square;
                    Helper.Path = false;
                }
            }
            else
            {
                board[square.Row][square.Column].Texture = Helper.redSquare;

                foreach (Squares selectedSquare in Helper.CurrentNeighbours.Keys)
                {
                    selectedSquare.LegalSquareSymbol = null;
                }
                Helper.CurrentNeighbours.Clear();
                Helper.Current = null;
            }
        }
        public void ResetGame()
        {
            Helper.Reset(board);
        }

        public void SaveGame()
        {
            Helper.Save(board);
            SaveMultipleJumpsState();
        }

        public void LoadGame()
        {
            Helper.Load(board);
            LoadMultipleJumpsState();
            playerTurn.TurnImage = Helper.Turn.TurnImage;
        }

        public void About()
        {
            Helper.About();
        }
        public void Click(Squares square)
        {
                if ((Helper.Turn.PlayerColor == PieceColor.Red && square.Piece.Color == PieceColor.Red) ||
                    (Helper.Turn.PlayerColor == PieceColor.White && square.Piece.Color == PieceColor.White))
                {
                    DisplayMoves(square);
                }
        }

        public void Move(Squares square)
        {
            square.Piece = Helper.Current.Piece;
            square.Piece.Square = square;

            if (Helper.CurrentNeighbours[square] != null)
            {
                Helper.CurrentNeighbours[square].Piece = null;
                Helper.EMove = true;
            }
            else
            {
                Helper.EMove = false;
                Switch(Helper.Current);
            }

            board[Helper.Current.Row][Helper.Current.Column].Texture = Helper.redSquare;

            foreach (Squares selectedSquare in Helper.CurrentNeighbours.Keys)
            {
                selectedSquare.LegalSquareSymbol = null;
            }
            Helper.CurrentNeighbours.Clear();
            Helper.Current.Piece = null;
            Helper.Current = null;

            if (square.Piece.Type == PieceType.Regular)
            {
                if (square.Row == 0 && square.Piece.Color == PieceColor.Red)
                {
                    square.Piece.Type = PieceType.King;
                    square.Piece.Texture = Helper.redKingPiece;
                }
                else if (square.Row == board.Count - 1 && square.Piece.Color == PieceColor.White)
                {
                    square.Piece.Type = PieceType.King;
                    square.Piece.Texture = Helper.whiteKingPiece;
                }
            }
            if (Helper.EMove)
            {
                if (playerTurn.TurnImage == Helper.redPiece)
                {
                    Helper.CollectedWhitePieces++;
                }
                if (playerTurn.TurnImage == Helper.whitePiece)
                {
                    Helper.CollectedRedPieces++;
                }
                DisplayMoves(square);
            }

            if (Helper.CollectedRedPieces == 12 || Helper.CollectedWhitePieces == 12)
            {
                Finish();
            }
        }
        public void Finish()
        {
            Winner aux = Helper.getScore();
            if (Helper.CollectedRedPieces == 12)
            {
                MessageBox.Show("Congrats! White player has just won!");
                Helper.writeScore(aux.RedWins, ++aux.WhiteWins);
            }
            if (Helper.CollectedWhitePieces == 12)
            {
                MessageBox.Show("Congrats! Red player has just won!");
                Helper.writeScore(++aux.RedWins, aux.WhiteWins);
            }
            winner.RedWins = aux.RedWins;
            winner.WhiteWins = aux.WhiteWins;
            Helper.CollectedRedPieces = 0;
            Helper.CollectedWhitePieces = 0;
            Helper.Reset(board);
        }
        public void SaveMultipleJumpsState()
        {
            Helper.SaveMultipleJumpsState(MultipleJumps);
        }

        public void LoadMultipleJumpsState()
        {
            MultipleJumps = Helper.LoadMultipleJumpsState();
        }
    }
}
