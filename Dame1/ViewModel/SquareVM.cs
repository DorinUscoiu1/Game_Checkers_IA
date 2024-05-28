using Dame1.Services;
using Dame1.Model;
using Dame1.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dame1.ViewModel
{
    public class SquareVM : BaseNotification
    {
        private GameLogic gameLogic;
        private Squares genericSquare;
        private ICommand clickPieceCommand;
        private ICommand movePieceCommand;

        public SquareVM(Squares square, GameLogic gameLogic)
        {
            genericSquare = square;
            this.gameLogic = gameLogic;
        }

        public Squares GenericSquare
        {
            get
            {
                return genericSquare;
            }
            set
            {
                genericSquare = value;
                NotifyPropertyChanged("GenericSquare");
            }
        }

        public ICommand ClickPieceCommand
        {
            get
            {
                if (clickPieceCommand == null)
                {
                    clickPieceCommand = new RelayCommand<Squares>(gameLogic.Click);
                }
                return clickPieceCommand;
            }
        }

        public ICommand MovePieceCommand
        {
            get
            {
                if (movePieceCommand == null)
                {
                    movePieceCommand = new RelayCommand<Squares>(gameLogic.Move);
                }
                return movePieceCommand;
            }
        }
    }
}
