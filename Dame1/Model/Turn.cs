using Dame1.Services;
using Dame1.ViewModel;
using Dame1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Dame1.Model
{
    public class Turn : BaseNotification
    {
        private PieceColor color;
        private string image;

        public Turn(PieceColor color)
        {
            this.color = color;
            LoadImages();
        }
        private void LoadImages()
        {
            image = (color == PieceColor.Red) ? Helper.redPiece : Helper.whitePiece;
        }

        public PieceColor PlayerColor
        {
            get => color;
            set
            {
                color = value;
                NotifyPropertyChanged("PlayerColor");
            }
        }

        public string TurnImage
        {
            get => image;
            set
            {
                image = value;
                NotifyPropertyChanged("TurnImage");
            }
        }
    }
}
