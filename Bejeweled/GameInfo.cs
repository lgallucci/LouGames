using System;
using System.Collections.Generic;
using System.Text;

namespace Bejeweled
{
    class GameInfo
    {
        /* Constructor */
        internal GameInfo()
        {
            topTenNames = new string[20];
            topTenScores = new int[20];
            place = -1;
        }

        /* Properties */
        private int cascadeCount;

        internal int CascadeCount
        {
            get { return cascadeCount; }
            set { cascadeCount = value; }
        }

        private bool gameOver;

        internal bool GameOver
        {
            get { return gameOver; }
            set { gameOver = value; }
        }

        private bool highScores;

        internal bool HighScores
        {
            get { return highScores; }
            set { highScores = value; }
        }

        private bool sound;

        internal bool Sound
        {
            get { return sound; }
            set { sound = value; }
        }

        private int hint;

        internal int Hint
        {
            get { return hint; }
            set { hint = value; }
        }

        private bool isRunning;

        internal bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }

        private int level;

        internal int Level
        {
            get { return level; }
            set { level = value; }
        }

        private bool levelUp;

        internal bool LevelUp
        {
            get { return levelUp; }
            set { levelUp = value; }
        }

        private int nextScore;

        internal int NextScore
        {
            get { return nextScore; }
            set { nextScore = value; }
        }

        private int place;

        internal int Place
        {
            get { return place; }
            set { place = value; }
        }

        private long score;

        internal long Score
        {
            get { return score; }
            set { score = value; }
        }

        private bool showHint;

        internal bool ShowHint
        {
            get { return showHint; }
            set { showHint = value; }
        }

        private int startScore;

        internal int StartScore
        {
            get { return startScore; }
            set { startScore = value; }
        }

        private string[] topTenNames;

        internal string[] TopTenNames
        {
            get { return topTenNames; }
            set { topTenNames = value; }
        }

        private int[] topTenScores;

        internal int[] TopTenScores
        {
            get { return topTenScores; }
            set { topTenScores = value; }
        }

        private int timerValue;

        internal int TimerValue
        {
            get { return timerValue; }
            set { timerValue = value; }
        }

        private bool isRetired;

        internal bool IsRetired
        {
            get { return isRetired; }
            set { isRetired = value; }
        }

        private int gameMode;

        internal int GameMode
        {
            get { return gameMode; }
            set { gameMode = value; }
        }
    }
}
