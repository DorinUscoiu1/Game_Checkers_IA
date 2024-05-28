using Dame1.Model;
using Dame1.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dame1.ViewModel
{
    public class GameVM
    {
        public ObservableCollection<ObservableCollection<SquareVM>> Board { get; set; }
        public GameLogic Game { get; set; }
        public ButtonVM Buttons { get; set; }

        public WinnerVM WinnerVM { get; set; }

        public TurnVM PlayerTurnVM { get; set; }

        public string RED_PIECE { get; set; }
        public string WHITE_PIECE { get; set; }
        private bool multipleJumps;
        public bool MultipleJumps
        {
            get { return multipleJumps; }
            set
            {
                if (multipleJumps != value)
                {
                    multipleJumps = value;
                    Game.MultipleJumps = value;
                    OnPropertyChanged(nameof(MultipleJumps));
                    UpdateCurrentState();
                }
            }

        }
        private string currentState;
        public string CurrentState
        {
            get { return currentState; }
            set
            {
                currentState = value;
                OnPropertyChanged(nameof(CurrentState));
            }
        }
        private void UpdateCurrentState()
        {
            if (MultipleJumps)
            {
                CurrentState = "Saritura multipla este permisa";
            }
            else
            {
                CurrentState = "Saritura multipla nu este permisa.";
            }
        }
        public GameVM()
        {
            ObservableCollection<ObservableCollection<Squares>> board = Helper.initBoard();
            Turn playerTurn = new Turn(PieceColor.Red);
            Winner winner = new Winner(0, 0);
            Game = new GameLogic(board, playerTurn, winner);
            Game.PropertyChanged += PropertyChanged;
            PlayerTurnVM = new TurnVM(Game, playerTurn);
            WinnerVM = new WinnerVM(Game, winner);
            Board = CellBoardToCellVMBoard(board);
            Buttons = new ButtonVM(Game);
            RED_PIECE = Helper.redPiece;
            WHITE_PIECE = Helper.whitePiece;
            MultipleJumps = false;
           UpdateCurrentState();
        }
        private ObservableCollection<ObservableCollection<SquareVM>> CellBoardToCellVMBoard(ObservableCollection<ObservableCollection<Squares>> board)
        {
            ObservableCollection<ObservableCollection<SquareVM>> result = new ObservableCollection<ObservableCollection<SquareVM>>();
            for (int i = 0; i < board.Count; i++)
            {
                ObservableCollection<SquareVM> line = new ObservableCollection<SquareVM>();
                for (int j = 0; j < board[i].Count; j++)
                {
                    Squares c = board[i][j];
                    SquareVM cell = new SquareVM(c, Game);
                    line.Add(cell);
                }
                result.Add(line);
            }
            return result;
        }
        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MultipleJumps")
            {
                MultipleJumps = Game.MultipleJumps;
                OnPropertyChanged(nameof(MultipleJumps));
            }
        }
        public event PropertyChangedEventHandler propertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OnMultipleJumpsChanged()
        {
           Game.MultipleJumps = MultipleJumps;
        }
    }
}
