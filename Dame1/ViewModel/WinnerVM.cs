using Dame1.Model;
using Dame1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dame1.ViewModel
{
    public class WinnerVM : BaseNotification
    {
        private GameLogic gameLogic;
        private Winner winner;

        public WinnerVM(GameLogic gameLogic, Winner winner)
        {
            this.gameLogic = gameLogic;
            this.winner = winner;
        }

        public Winner WinnerPlayer
        {
            get { return winner; }
            set
            {
                winner = value;
                NotifyPropertyChanged("WinnerPlayer");
            }
        }
    }
}
