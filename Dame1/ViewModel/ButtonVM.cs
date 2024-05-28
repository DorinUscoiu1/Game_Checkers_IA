using Dame1.Command;
using Dame1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Dame1.Model;

namespace Dame1.ViewModel
{
    public class ButtonVM : BaseNotification
    {
        private GameLogic gameLogic;
        private ICommand resetCommand;
        private ICommand saveCommand;
        private ICommand aboutCommand;
        private ICommand loadCommand;

        public ButtonVM(GameLogic gameLogic)
        {
            this.gameLogic = gameLogic;
        }

        public ICommand ResetCommand
        {
            get
            {
                if (resetCommand == null)
                {
                    resetCommand = new CommandForButtons(gameLogic.ResetGame);
                }
                return resetCommand;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new CommandForButtons(gameLogic.SaveGame);
                }
                return saveCommand;
            }
        }

        public ICommand LoadCommand
        {
            get
            {
                if (loadCommand == null)
                {
                    loadCommand = new CommandForButtons(gameLogic.LoadGame);
                }
                return loadCommand;
            }
        }

        public ICommand AboutCommand
        {
            get
            {
                if (aboutCommand == null)
                {
                    aboutCommand = new CommandForButtons(gameLogic.About);
                }
                return aboutCommand;
            }
        }
    }
}
