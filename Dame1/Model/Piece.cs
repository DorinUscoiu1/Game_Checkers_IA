using Dame1.Services;
using Dame1.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Media.TextFormatting;

namespace Dame1.Model
{
    public class Piece : INotifyPropertyChanged
    {
        private PieceColor color;
        private PieceType type;
        private string texture;
        private Squares square;

        public event PropertyChangedEventHandler PropertyChanged;

        public Piece(PieceColor color)
        {
            this.color = color;
            type = PieceType.Regular;
            if (color == PieceColor.Red)
            {
                texture = Helper.redPiece;
            }
            else
            {
                texture = Helper.whitePiece;
            }
        }

        public Piece(PieceColor color, PieceType type)
        {
            this.color = color;
            this.type = type;
            if (color == PieceColor.Red)
            {
                texture = Helper.redPiece;
            }
            else
            {
                texture = Helper.whitePiece;
            }
            if (type == PieceType.King && color == PieceColor.Red)
            {
                texture = Helper.redKingPiece;
            }
            if (type == PieceType.King && color == PieceColor.White)
            {
                texture = Helper.whiteKingPiece;
            }
        }
        public PieceColor Color
        {
            get
            {
                return color;
            }
        }
        public PieceType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                NotifyPropertyChanged("Type");
            }
        }

        public string Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
                NotifyPropertyChanged("Texture");
            }
        }

        public Squares Square
        {
            get
            {
                return square;
            }
            set
            {
                square = value;
                NotifyPropertyChanged("Square");
            }
        }
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
