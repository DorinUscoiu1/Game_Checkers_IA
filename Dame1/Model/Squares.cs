using Dame1.Services;
using Dame1.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Dame1.Model
{
    public class Squares : INotifyPropertyChanged
    {
        private int row;
        private int column;
        private SquareShade shade;
        private string texture;
        private Piece piece;
        private string legalSquareSymbol;

        public event PropertyChangedEventHandler PropertyChanged;

        public Squares(int row, int column, SquareShade shade, Piece piece)
        {
            this.row = row;
            this.column = column;
            this.shade = shade;
            if (shade == SquareShade.Dark)
            {
                texture = Helper.redSquare;
            }
            else
            {
                texture = Helper.whiteSquare;
            }
            this.piece = piece;
        }

        public int Row
        {
            get
            {
                return row;
            }
        }

        public int Column
        {
            get
            {
                return column;
            }
        }

        public SquareShade Shade
        {
            get
            {
                return shade;
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

        public Piece Piece
        {
            get
            {
                return piece;
            }
            set
            {
                piece = value;
                NotifyPropertyChanged("Piece");
            }
        }

        public string LegalSquareSymbol
        {
            get
            {
                return legalSquareSymbol;
            }
            set
            {
                legalSquareSymbol = value;
                NotifyPropertyChanged("LegalSquareSymbol");
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
