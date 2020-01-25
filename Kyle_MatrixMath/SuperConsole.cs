// SUPERCONSOLE 1.3
// Kyle Alexander Buan
// Made for MatrixMath
// November 23, 2015 - January 14, 2016

/*

HISTORY

1.0 First stable release
1.1 Fixed matrix width-height ordering
1.2 Augment separator is now available for matrix view
1.3 Input boxes made more prominent (when they wouldn't cover something important)
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Kyle_MatrixMath
{
    class SuperConsole
    {
        public const int DISP_TEXT = 0;
        public const int DISP_MENU = 1;
        public const int DISP_MATRIX = 2;
        const int DISP_ALERT = 3;

        char[,] Screen;
        int[,] ScreenForeground;
        int[,] ScreenBackground;
        
        int WindowWidth, WindowHeight;

        public string MainTitleText { get; set; }

        public string MainText { get; set; }

        public Dictionary<char, string> MenuOptions { get; set; }

        public Matrix DisplayMatrix { get; set; }

        public int MatrixColumnHighlight { get; set; }

        public int MatrixRowHighlight { get; set; }

        public int DisplayMode { get; set; }

        public string PromptText { get; set; }

        public string AlertTitle { get; set; }

        public string AlertText { get; set; }

        public bool AlertEnabled { get; set; }

        int AlertFG { get; set; }

        int AlertBG { get; set; }

        bool AlertTextBox { get; set; }
        int AlertTextBoxX, AlertTextBoxY;

        public Dictionary<char, string> AlertOptions;


        public SuperConsole()
        {
            ResetScreen();

            MainTitleText = "TITLE";
            MainText = "Main text contents";
            MenuOptions = new Dictionary<char, string>() { { 'A', "Item 1" }, { 'B', "Item 2" }, { 'C', "Item 3" } };
            DisplayMode = DISP_TEXT;
            PromptText = "Prompt text";
            AlertTitle = "Alert Title";
            AlertText = "Alert Text";
            AlertEnabled = false;
            AlertFG = (int)ConsoleColor.White;
            AlertBG = (int)ConsoleColor.Red;
            MatrixColumnHighlight = -1;
            MatrixRowHighlight = -1;
        }

        void ResetScreen()
        {
            WindowWidth = Console.WindowWidth;
            WindowHeight = Console.WindowHeight - 1;

            Screen = new char[WindowWidth, WindowHeight];
            ScreenForeground = new int[WindowWidth, WindowHeight];
            ScreenBackground = new int[WindowWidth, WindowHeight];

            for (int y = 0; y < WindowHeight; y++)
                for (int x = 0; x < WindowWidth; x++)
                {
                    Screen[x, y] = ' ';
                    ScreenForeground[x, y] = AlertEnabled ? (int)ConsoleColor.Gray : (int)ConsoleColor.White;
                    ScreenBackground[x, y] = AlertEnabled ? (int)ConsoleColor.Black : (int)ConsoleColor.DarkBlue;
                }
        }

        void DrawScreen()
        {
            if (DisplayMode == DISP_MATRIX)
            {
                int maxCellLength = 0;
                for (int y = 0; y < DisplayMatrix.MatrixHeight; y++)
                    for (int x = 0; x < DisplayMatrix.MatrixWidth; x++)
                        if (maxCellLength < DisplayMatrix.GetCell(y, x).ToString().Length)
                            maxCellLength = DisplayMatrix.GetCell(y, x).ToString().Length;

                maxCellLength++; // because we need spacing

                if (WindowWidth - 6 < maxCellLength * DisplayMatrix.MatrixWidth)
                    maxCellLength = (WindowWidth - 8) / DisplayMatrix.MatrixWidth;

                if (maxCellLength < 2 || DisplayMatrix.MatrixHeight >= (WindowHeight - 4))
                {
                    SetAlert("Error", "Matrix too big to display", 2);
                }
            }

            ResetScreen();

            // draw title bars and window borders
            for (int x = 0; x < WindowWidth; x++)
            {
                ScreenBackground[x, 0] = AlertEnabled ? (int)ConsoleColor.DarkCyan : (int)ConsoleColor.Cyan;
                ScreenForeground[x, 0] = (int)ConsoleColor.Black;
                Screen[x, 0] = '-';
                ScreenBackground[x, WindowHeight-2] = AlertEnabled ? (int)ConsoleColor.DarkCyan : (int)ConsoleColor.Cyan;
                ScreenForeground[x, WindowHeight-2] = (int)ConsoleColor.Black;
                Screen[x, WindowHeight-2] = '-';
                ScreenBackground[x, WindowHeight-1] = (int)ConsoleColor.Green;
                ScreenForeground[x, WindowHeight-1] = (int)ConsoleColor.Black;
                Screen[x, WindowHeight-1] = '-';
            }
            for (int y = 0; y < WindowHeight-1; y++)
            {
                ScreenBackground[0, y] = AlertEnabled ? (int)ConsoleColor.DarkCyan : (int)ConsoleColor.Cyan;
                ScreenForeground[0, y] = (int)ConsoleColor.Black;
                Screen[0, y] = '|';
                ScreenBackground[WindowWidth-1, y] = AlertEnabled ? (int)ConsoleColor.DarkCyan : (int)ConsoleColor.Cyan;
                ScreenForeground[WindowWidth-1, y] = (int)ConsoleColor.Black;
                Screen[WindowWidth-1, y] = '|';

            }

            WriteCentered(" " + MainTitleText + " ", 0);
            WriteUpperLeft(" " + PromptText + " ", 1, WindowHeight - 1);

            // draw content
            if (DisplayMode == DISP_TEXT)
                WriteUpperLeft(MainText, 2, 2);
            else if (DisplayMode == DISP_MENU)
            {
                int y = 2;
                int jump = (WindowHeight - 5) / MenuOptions.Count();
                foreach (var opt in MenuOptions)
                {
                    ScreenForeground[3, y] = (int)ConsoleColor.Black;
                    ScreenForeground[4, y] = (int)ConsoleColor.Black;
                    ScreenBackground[3, y] = AlertEnabled ? (int)ConsoleColor.DarkYellow : (int)ConsoleColor.Yellow;
                    ScreenBackground[4, y] = AlertEnabled ? (int)ConsoleColor.DarkYellow : (int)ConsoleColor.Yellow;
                    Screen[3, y] = opt.Key;
                    Screen[4, y] = '.';
                    WriteUpperLeft(opt.Value, 7, y);
                    y += jump;
                }
            }
            else if (DisplayMode == DISP_MATRIX)
            {
                int sign = 0, dec = 0, exp = 0;
                int maxCellLength = 0;
                for (int y = 0; y < DisplayMatrix.MatrixHeight; y++)
                    for (int x = 0; x < DisplayMatrix.MatrixWidth; x++)
                    {
                        if (maxCellLength < DisplayMatrix.GetCell(y, x).ToString().Length)
                            maxCellLength = DisplayMatrix.GetCell(y, x).ToString().Length;
                    }

                maxCellLength += 1;

                if (WindowWidth - 6 < maxCellLength * DisplayMatrix.MatrixWidth)
                    maxCellLength = (WindowWidth - 8) / DisplayMatrix.MatrixWidth;

                if (maxCellLength < 2 || DisplayMatrix.MatrixHeight < (WindowHeight - 4))
                {
                    // draw augment separator
                    if (DisplayMatrix.border > 0)
                        for (int y = 0; y < DisplayMatrix.MatrixHeight; y++)
                        {
                            Screen[3 + maxCellLength * DisplayMatrix.border, y + 2] = '|';
                        }

                    // draw left bracket
                    Screen[2, 2] = '/';
                    for (int y = 0; y < (DisplayMatrix.MatrixHeight - 2); y++)
                        Screen[2, y + 3] = '|';
                    Screen[2, DisplayMatrix.MatrixHeight + 1] = '\\';

                    // draw contents
                    double toDraw;
                    for (int y = 0; y < DisplayMatrix.MatrixHeight; y++)
                        for (int x = 0; x < DisplayMatrix.MatrixWidth; x++)
                        {
                            toDraw = DisplayMatrix.GetCell(y, x);
                            if (toDraw.ToString().Contains("-"))
                                sign = 2;
                            else
                                sign = 0;
                            if (toDraw.ToString().Contains("."))
                                dec = 1;
                            else
                                dec = 0;
                            if (toDraw.ToString().Contains("E"))
                                exp = 1;
                            else
                                exp = 0;
                            WriteRightAlign(DisplayMatrix.GetCell(y, x).ToString("G" + (maxCellLength - (dec+sign+exp))), 4 + maxCellLength * x, y + 2, maxCellLength - 1);
                        }

                    // draw right bracket
                    Screen[4 + (DisplayMatrix.MatrixWidth) * maxCellLength, 2] = '\\';
                    for (int y = 0; y < (DisplayMatrix.MatrixHeight - 2); y++)
                    {
                        Screen[4 + DisplayMatrix.MatrixWidth * maxCellLength, y + 3] = '|';
                    }
                    Screen[4 + DisplayMatrix.MatrixWidth * maxCellLength, DisplayMatrix.MatrixHeight + 1] = '/';

                    // draw highlights
                    if (MatrixRowHighlight >= 0)
                        for (int x = 1; x < WindowWidth - 1; x++)
                            ScreenBackground[x, MatrixRowHighlight + 2] = AlertEnabled ? (int)ConsoleColor.DarkBlue : (int)ConsoleColor.Blue;
                    if (MatrixColumnHighlight >= 0)
                        for (int y = 1; y < WindowHeight - 2; y++)
                            for (int x = maxCellLength * MatrixColumnHighlight + 4; x < maxCellLength * (MatrixColumnHighlight + 1) + 4; x++)
                                ScreenBackground[x, y] = AlertEnabled ? (int)ConsoleColor.DarkBlue : (int)ConsoleColor.Blue;
                    if (MatrixRowHighlight >= 0 && MatrixColumnHighlight >= 0)
                        for (int x = maxCellLength * MatrixColumnHighlight + 4; x < maxCellLength * (MatrixColumnHighlight + 1) + 4; x++)
                        {
                            ScreenBackground[x, MatrixRowHighlight + 2] = AlertEnabled ? (int)ConsoleColor.Gray : (int)ConsoleColor.White;
                            ScreenForeground[x, MatrixRowHighlight + 2] = (int)ConsoleColor.Black;
                        }
                }
            }

            if (AlertEnabled)
                DrawAlert();

        }

        public void SetAlert(string title, string message, int type, Dictionary<char, string> opt = null)
        {
            AlertTitle = " " + title + " ";
            AlertText = message;
            if (type == 0)
            {
                AlertFG = (int)ConsoleColor.Black;
                AlertBG = (int)ConsoleColor.Green;
            }
            if (type == 1)
            {
                AlertFG = (int)ConsoleColor.White;
                AlertBG = (int)ConsoleColor.DarkBlue;
                AlertTextBox = false;
            }
            if (type == 2)
            {
                AlertFG = (int)ConsoleColor.White;
                AlertBG = (int)ConsoleColor.Red;
                AlertTextBox = false;
            }
            if (type == 3)
            {
                AlertFG = (int)ConsoleColor.White;
                AlertBG = (int)ConsoleColor.DarkBlue;
                AlertTextBox = true;
            }
            AlertOptions = opt;
            if (AlertOptions != null)
                PromptText = "PLEASE ENTER YOUR CHOICE.";
            else
                PromptText = "PLEASE PRESS ANY KEY.";
            AlertEnabled = true;
        }

        void DrawAlert()
        {
            string[] lines = AlertText.Split('\n');
            int longestLine = 0;
            foreach (string l in lines)
            {
                if (longestLine < l.Length)
                    longestLine = l.Length;
            }
            if (AlertOptions != null)
            {
                foreach (string l in AlertOptions.Values)
                {
                    if (longestLine < l.Length + 3)
                        longestLine = l.Length + 3;
                }
            }
            if (AlertTitle.Length > longestLine)
                longestLine = AlertTitle.Length;

            int height = 3 + lines.Count() + (AlertTextBox ? 2 : 0);
            if (AlertOptions != null)
                height += AlertOptions.Count*2;
            int upper = WindowHeight / 2 - height / 2;
            int width = 4 + longestLine;
            int left = WindowWidth / 2 - (width) / 2;

            // clear space
            for (int y = upper; y < (upper+ height); y++)
                for (int x = left; x < (left + width); x++)
                {
                    Screen[x, y] = ' ';
                    ScreenForeground[x, y] = AlertFG;
                    ScreenBackground[x, y] = AlertBG;
                }

            // draw alert title bar
            for (int x = left; x < (left + width); x++)
            {
                Screen[x, upper] = '-';
                ScreenForeground[x, upper] = (int)ConsoleColor.Black;
                ScreenBackground[x, upper] = (int)ConsoleColor.Cyan;
            }
            // draw title text
            WriteCentered(AlertTitle, upper);
            // draw message
            for (int i = 0; i < lines.Count(); i++)
                WriteUpperLeft(lines[i], left + 2, upper + 2 + i);

            // draw options
            if (AlertOptions != null)
            {
                int y = upper + 3 + lines.Count();
                foreach (var opt in AlertOptions)
                {
                    ScreenForeground[left + 3, y] = (int)ConsoleColor.Black;
                    ScreenForeground[left + 4, y] = (int)ConsoleColor.Black;
                    ScreenBackground[left + 3, y] = (int)ConsoleColor.Yellow;
                    ScreenBackground[left + 4, y] = (int)ConsoleColor.Yellow;
                    Screen[left + 3, y] = opt.Key;
                    Screen[left + 4, y] = '.';
                    WriteUpperLeft(opt.Value, left + 7, y);
                    y+= 2;
                }

            }

            // draw text box
            if (AlertTextBox)
            {
                for (int x = left+1; x < (left+width-1); x++)
                {
                    ScreenForeground[x, upper + height - 2] = (int)ConsoleColor.White;
                    ScreenBackground[x, upper + height - 2] = (int)ConsoleColor.Black;
                }

                AlertTextBoxX = left + 1;
                AlertTextBoxY = upper + height - 2;
            }
        }

        void WriteCentered(string t, int y)
        {
            int offset = WindowWidth / 2 - t.Length / 2;
            int x = offset;
            foreach (char c in t)
            {
                Screen[x, y] = c;
                x++;
            }
        }

        void WriteUpperLeft(string t, int x, int y)
        {
            int origX = x;

            foreach (char c in t)
            {
                if (c == '\n')
                {
                    x = origX;
                    y++;
                }
                else
                {
                    Screen[x, y] = c;
                    x++;
                }
            }
        }

        void WriteRightAlign(string t, int x, int y, int l)
        {
            int offset = x + l - t.Length;
            foreach (char c in t)
            {
                Screen[offset, y] = c;
                offset++;
            }
        }

        public char UpdateGetChar()
        {
            //Console.Write('\r');
            Console.Clear();
            DrawScreen();

            int lastFG = ScreenForeground[0, 0];
            int lastBG = ScreenBackground[0, 0];
            Console.BackgroundColor = (ConsoleColor)lastBG;
            Console.ForegroundColor = (ConsoleColor)lastFG;

            string buf = "";

            for (int y = 0; y < WindowHeight; y++)
                for (int x = 0; x < WindowWidth; x++)
                {
                    if (ScreenForeground[x, y] == lastFG && ScreenBackground[x, y] == lastBG)
                        buf += Screen[x, y].ToString();
                    else
                    {
                        Console.Write(buf);
                        buf = Screen[x,y].ToString();
                        lastFG = ScreenForeground[x, y];
                        lastBG = ScreenBackground[x, y];
                        Console.BackgroundColor = (ConsoleColor)lastBG;
                        Console.ForegroundColor = (ConsoleColor)lastFG;
                    }
                }
            Console.Write(buf);

            if (AlertTextBox)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(AlertTextBoxX, AlertTextBoxY);
            }

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            AlertTextBox = false;
            AlertEnabled = false;
            return Console.ReadKey().KeyChar.ToString().ToUpper()[0];
        }

        public string UpdateGetString()
        {
            bool got = false;
            string inp = "";

            while (!got)
            {
                Console.Clear();
                DrawScreen();

                int lastFG = ScreenForeground[0, 0];
                int lastBG = ScreenBackground[0, 0];
                Console.BackgroundColor = (ConsoleColor)lastBG;
                Console.ForegroundColor = (ConsoleColor)lastFG;

                string buf = "";

                for (int y = 0; y < WindowHeight; y++)
                    for (int x = 0; x < WindowWidth; x++)
                    {
                        if (ScreenForeground[x, y] == lastFG && ScreenBackground[x, y] == lastBG)
                            buf += Screen[x, y].ToString();
                        else
                        {
                            Console.Write(buf);
                            buf = Screen[x, y].ToString();
                            lastFG = ScreenForeground[x, y];
                            lastBG = ScreenBackground[x, y];
                            Console.BackgroundColor = (ConsoleColor)lastBG;
                            Console.ForegroundColor = (ConsoleColor)lastFG;
                        }
                    }
                Console.Write(buf);

                if (AlertTextBox)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(AlertTextBoxX, AlertTextBoxY);
                }

                inp = Console.ReadLine();

                try
                {
                    if (inp.Trim().Length > 0)
                        got = true;
                    else
                        got = false;
                }
                catch
                {
                    got = false;
                }
            }
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            AlertTextBox = false;
            AlertEnabled = false;
            return inp;
        }

        public double UpdateGetDouble()
        {
            bool got = false;
            string inp;
            double res = 0;

            while (!got)
            {
                Console.Clear();
                DrawScreen();

                int lastFG = ScreenForeground[0, 0];
                int lastBG = ScreenBackground[0, 0];
                Console.BackgroundColor = (ConsoleColor)lastBG;
                Console.ForegroundColor = (ConsoleColor)lastFG;

                string buf = "";

                for (int y = 0; y < WindowHeight; y++)
                    for (int x = 0; x < WindowWidth; x++)
                    {
                        if (ScreenForeground[x, y] == lastFG && ScreenBackground[x, y] == lastBG)
                            buf += Screen[x, y].ToString();
                        else
                        {
                            Console.Write(buf);
                            buf = Screen[x, y].ToString();
                            lastFG = ScreenForeground[x, y];
                            lastBG = ScreenBackground[x, y];
                            Console.BackgroundColor = (ConsoleColor)lastBG;
                            Console.ForegroundColor = (ConsoleColor)lastFG;
                        }
                    }
                Console.Write(buf);

                if (AlertTextBox)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(AlertTextBoxX, AlertTextBoxY);
                }

                inp = Console.ReadLine();

                try
                {
                    res = Convert.ToDouble(inp);
                    got = true;
                }
                catch
                {
                    got = false;
                }
            }
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            AlertTextBox = false;
            AlertEnabled = false;
            return res;
        }
        public int UpdateGetInt()
        {
            bool got = false;
            string inp;
            int res = 0;

            while (!got)
            {
                Console.Clear();
                DrawScreen();

                int lastFG = ScreenForeground[0, 0];
                int lastBG = ScreenBackground[0, 0];
                Console.BackgroundColor = (ConsoleColor)lastBG;
                Console.ForegroundColor = (ConsoleColor)lastFG;

                string buf = "";

                for (int y = 0; y < WindowHeight; y++)
                    for (int x = 0; x < WindowWidth; x++)
                    {
                        if (ScreenForeground[x, y] == lastFG && ScreenBackground[x, y] == lastBG)
                            buf += Screen[x, y].ToString();
                        else
                        {
                            Console.Write(buf);
                            buf = Screen[x, y].ToString();
                            lastFG = ScreenForeground[x, y];
                            lastBG = ScreenBackground[x, y];
                            Console.BackgroundColor = (ConsoleColor)lastBG;
                            Console.ForegroundColor = (ConsoleColor)lastFG;
                        }
                    }
                Console.Write(buf);

                if (AlertTextBox)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(AlertTextBoxX, AlertTextBoxY);
                }

                inp = Console.ReadLine();

                try
                {
                    res = Convert.ToInt32(inp);
                    got = true;
                }
                catch
                {
                    got = false;
                }
            }

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            AlertTextBox = false;

            AlertEnabled = false;
            return res;
        }

        public void Update()
        {
            Console.Clear();
            DrawScreen();

            int lastFG = ScreenForeground[0, 0];
            int lastBG = ScreenBackground[0, 0];
            Console.BackgroundColor = (ConsoleColor)lastBG;
            Console.ForegroundColor = (ConsoleColor)lastFG;

            string buf = "";

            for (int y = 0; y < WindowHeight; y++)
                for (int x = 0; x < WindowWidth; x++)
                {
                    if (ScreenForeground[x, y] == lastFG && ScreenBackground[x, y] == lastBG)
                        buf += Screen[x, y].ToString();
                    else
                    {
                        Console.Write(buf);
                        buf = Screen[x, y].ToString();
                        lastFG = ScreenForeground[x, y];
                        lastBG = ScreenBackground[x, y];
                        Console.BackgroundColor = (ConsoleColor)lastBG;
                        Console.ForegroundColor = (ConsoleColor)lastFG;
                    }
                }
            Console.Write(buf);

            if (AlertTextBox)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(AlertTextBoxX, AlertTextBoxY);
            }

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            AlertTextBox = false;
            AlertEnabled = false;
        }

    }
}
