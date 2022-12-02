using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int tenthsOfSecondsReverse;
        int matchesFound;
        int bestTimeOfSeconds = 0;
        bool isGameOver;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            tenthsOfSecondsReverse++;
            reverseTimeTextBlock.Text = GetReverseTimeToString(tenthsOfSecondsReverse);
            timeTextBlock.Text = GetTimeToString(tenthsOfSecondsElapsed);

            if (tenthsOfSecondsReverse == 20 || matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " -  Play again?";
                isGameOver = true;
            }

            if(matchesFound == 8)
            {
                if (bestTimeOfSeconds == 0 || bestTimeOfSeconds > tenthsOfSecondsElapsed)
                    bestTimeTextBlock.Text = GetTimeToString(tenthsOfSecondsElapsed);
            }
        }

        private string GetReverseTimeToString(int seconds) => (2 - (seconds / 10F)).ToString("0.0s");
        private string GetTimeToString(int seconds) => (seconds / 10F).ToString("0.0s");

        private void SetUpGame()
        {
            List<string> allAnimalEmoji = new List<string>()
            {
                "🦍", "🦍",
                "🐒", "🐒",
                "🐕", "🐕",
                "🐈", "🐈",
                "🐎", "🐎",
                "🐄", "🐄",
                "🐃", "🐃",
                "🦏", "🦏",
                "🐐", "🐐",
                "🦨", "🦨",
                "🐘", "🐘",
                "🐁", "🐁"
            };

            Random random = new Random();
            List<string> animalEmoji = new List<string>();
            
            for(int i = 0; i < 8; i++)
            {
                var randomNumber = random.Next(allAnimalEmoji.Count);
                int index = randomNumber % 2 == 1 ? randomNumber - 1 : randomNumber;
                animalEmoji.Add(allAnimalEmoji[index]);
                animalEmoji.Add(allAnimalEmoji[index + 1]);
                allAnimalEmoji.RemoveRange(index, 2);
            }

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (string.IsNullOrEmpty(textBlock.Name))
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Text = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }

            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
            tenthsOfSecondsReverse = 0;
            isGameOver = false;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        private void TextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBlock textBlock = (sender as TextBlock)!;

            if (!findingMatch)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = !findingMatch;
            }
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = !findingMatch;
                matchesFound++;
                tenthsOfSecondsReverse = 0;
            }
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = !findingMatch;
            }
        }

        private void timeTextBlock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (isGameOver)
            {
                SetUpGame();
            }
        }
    }
}
