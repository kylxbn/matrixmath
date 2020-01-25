using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kyle_MatrixMath
{
    class Menu
    {
        public static Dictionary<char, string> MAIN_MENU = new Dictionary<char, string>()
        {
            {'I', "Input menu" },
            {'M', "Math menu" },
            {'O', "Output menu" },
            {'T', "Transfer menu" },
            {'A', "About this program" },
            {'X', "Exit program" }
        };
        public static Dictionary<char, string> INPUT_MENU = new Dictionary<char, string>()
        {
            {'Q', "Create Matrix A" },
            {'W', "Create Matrix B" },
            {'A', "Edit Matrix A"},
            {'S', "Edit Matrix B" },
            {'D', "Edit Matrix A's cell" },
            {'F', "Edit Matrix B's cell" },
            {'X', "Back to main menu" }
        };
        public static Dictionary<char, string> MATH_MENU = new Dictionary<char, string>()
        {
            {'A', "C <- A + B" },
            {'S', "C <- A - B" },
            {'V', "C <- A . B" },
            {'M', "C <- A x B" },
            {'D', "C <- |A|" },
            {'C', "C <- A augment B" },
            {'T', "C <- Transpose(A)" },
            {'I', "C <- A inverse" },
            {'G', "C <- GaussJordan(A)" },
            {'X', "Back to main menu" }
        };
        public static Dictionary<char, string> OUTPUT_MENU = new Dictionary<char, string>()
        {
            {'A', "Show Matrix A" },
            {'B', "Show Matrix B" },
            {'C', "Show Matrix C" },
            {'X', "Back to main menu" }
        };
        public static Dictionary<char, string> TRANSFER_MENU = new Dictionary<char, string>()
        {
            {'Q', "A --> T" },
            {'W', "B --> T" },
            {'E', "C --> T" },
            {'A', "T --> A" },
            {'S', "T --> B" },
            {'D', "A <-> B" },
            {'X', "Return to main menu" }
        };
        public static Dictionary<char, string> OPT_YESNO = new Dictionary<char, string>()
        {
            {'Y', "Yes" },
            {'*', "No" }
        };


    }
}
