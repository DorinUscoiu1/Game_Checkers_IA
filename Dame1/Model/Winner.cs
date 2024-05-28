using System;
using Dame1.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dame1.Model;

namespace Dame1.Model
{
    public class Winner : BaseNotification
    {
        private int Red_Wins;
        private int White_Wins;

        public Winner(int redWins, int whiteWins)
        {
            this.Red_Wins = redWins;
            this.White_Wins = whiteWins;
        }

        public int RedWins
        {
            get
            {
                return Red_Wins;
            }
            set
            {
                Red_Wins = value;
                NotifyPropertyChanged("RedWins");
            }
        }

        public int WhiteWins
        {
            get
            {
                return White_Wins;
            }
            set
            {
                White_Wins = value;
                NotifyPropertyChanged("WhiteWins");
            }
        }
    }
}
