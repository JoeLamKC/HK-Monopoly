using System;
using System.Windows;
using System.Windows.Input;

/************************************************
 * This class is responsible for the view of text mode
 * 
 * 
 * It is independent from the other 2 GUI console and
 * this class is used to handle such different places
 * 
 * This class is used update view (text) to the window and get input from player
 * Each input will be passed to MainGameLogic after "Enter" being pressed
 * 
 ***********************************************/

namespace HKMonopoly
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TextMode : Window
    {
        public static TextMode TextModeInstance;
        public TextMode()
        {
            InitializeComponent();
            TextModeInstance = this;
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                int input;
                if (Int32.TryParse(InputBox.Text, out input))
                {
                    MainGameLogic.Instnace.NextGameStage(input);
                }
                else
                {
                    DisplayBlock.Text += "[Wrong Input] Please Enter an integer input \n";
                }
                InputBox.Text = "";
            }
        }

        public void UpdateText(string stringToPrint)
        {
            DisplayBlock.Text += stringToPrint + "\n";
            DisplayBlock.Height += 15;
            if (DisplayBlock.Height >= TextBlockScrollviewer.Height)
                TextBlockScrollviewer.ScrollToBottom();
        }

        public void ClearText()
        {
            DisplayBlock.Text ="";
        }
    }
}
