using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dame1.Model;
using Dame1.Services;

namespace Dame1.ViewModel
{
    public class TurnVM : BaseNotification
    {
        private GameLogic gameLogic;
        private Turn playerTurn;

        public TurnVM(GameLogic gameLogic, Turn playerTurn)
        {
            this.gameLogic = gameLogic;
            this.playerTurn = playerTurn;
        }

        public Turn PlayerIcon
        {
            get
            {
                return playerTurn;
            }
            set
            {
                playerTurn = value;
                NotifyPropertyChanged("PlayerIcon");
            }
        }
    }
}
