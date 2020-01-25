// MATRIXMATH 0.2
// Kyle Alexander Buan
// Programmed for Numerical Methods class
// November 23, 2015

using System;


namespace Kyle_MatrixMath
{
    class Program
    {
        static SuperConsole Console = new SuperConsole();

        static Matrix MA, MB, MC, MT;

        static void Main(string[] args)
        {
            System.Console.Title = "Kyle's Matrix Math Program";

            char r;
            char r2 = 'N';

            Console.MainTitleText = "INTEGRATED MATRIX ARITHMETIC MODULES";
            Console.PromptText = "PLEASE ENTER YOUR CHOICE.";
            Console.MenuOptions = Menu.MAIN_MENU;
            Console.DisplayMode = SuperConsole.DISP_MENU;
            Console.Update();

            do
            {
                Console.MainTitleText = "INTEGRATED MATRIX ARITHMETIC MODULES";
                Console.PromptText = "PLEASE ENTER YOUR CHOICE.";
                Console.MenuOptions = Menu.MAIN_MENU;
                Console.DisplayMode = SuperConsole.DISP_MENU;
                r = Console.UpdateGetChar();

                switch (r)
                {
                    case 'I':
                        InputMenu();
                        break;
                    case 'M':
                        MathMenu();
                        break;
                    case 'O':
                        OutputMenu();
                        break;
                    case 'T':
                        TransferMenu();
                        break;
                    case 'A':
                        AboutMenu();
                        break;
                    case 'X':
                        Console.SetAlert("CONFIRMATION", "Do you really wish to exit\nthe program?", 1, Menu.OPT_YESNO);
                        r2 = Console.UpdateGetChar();
                        break;
                    default:
                        Console.SetAlert("ERROR", "Invalid selection", 2);
                        Console.UpdateGetChar();
                        break;
                }
            }
            while (r2 != 'Y');
        }

        static void ShowMatrix(Matrix M)
        {
            if (M == null)
            {
                Console.SetAlert("ERROR", "Matrix not yet initialized!", 2);
                Console.UpdateGetChar();
            }
            else
            {
                Console.MainTitleText = "VIEW MATRIX";
                Console.PromptText = "PRESS ANY KEY.";
                Console.DisplayMatrix = M;
                Console.DisplayMode = SuperConsole.DISP_MATRIX;
                Console.UpdateGetChar();
            }
        }

        static void InputMenu()
        {
            char r;

            int w = 0, h = 0;

            do
            {
                Console.MainTitleText = "INPUT MENU";
                Console.PromptText = "PLEASE ENTER YOUR CHOICE.";
                Console.MenuOptions = Menu.INPUT_MENU;
                Console.DisplayMode = SuperConsole.DISP_MENU;
                r = Console.UpdateGetChar();

                switch (r)
                {
                    case 'Q':
                        do
                        {
                            Console.SetAlert("New Matrix", "Matrix row size", 3);
                            h = Console.UpdateGetInt();
                            if (h == 0)
                            {
                                Console.SetAlert("ERROR", "Matrix rows must be at least 1.", 2);
                                Console.UpdateGetChar();
                            }
                        }
                        while (h == 0);
                        do
                        {
                            Console.SetAlert("New Matrix", "Matrix column size", 3);
                            w = Console.UpdateGetInt();
                            if (w == 0)
                            {
                                Console.SetAlert("ERROR", "Matrix columns must be at least 1.", 2);
                                Console.UpdateGetChar();
                            }
                        }
                        while (w == 0);

                        if (w == h)
                        {
                            Console.SetAlert("CREATE MATRIX A", "Do you want to make Matrix A\nan identity matrix?", 1, Menu.OPT_YESNO);
                            r = Console.UpdateGetChar();
                        }
                        if (r == 'Y' && w == h)
                            MA = new Matrix(h, w, true);
                        else
                            MA = new Matrix(h, w, false);
                        Console.SetAlert("SUCCESS", "Matrix A was successfuly created.", 0);
                        Console.UpdateGetChar();
                        break;
                    case 'W':
                        do
                        {
                            Console.SetAlert("New Matrix", "Matrix row size", 3);
                            h = Console.UpdateGetInt();
                            if (h == 0)
                            {
                                Console.SetAlert("ERROR", "Matrix rows must be at least 1.", 2);
                                Console.UpdateGetChar();
                            }
                        }
                        while (h == 0);
                        do
                        {
                            Console.SetAlert("New Matrix", "Matrix column size", 3);
                            w = Console.UpdateGetInt();
                            if (w == 0)
                            {
                                Console.SetAlert("ERROR", "Matrix columns must be at least 1.", 2);
                                Console.UpdateGetChar();
                            }
                        }
                        while (w == 0);

                        if (w == h)
                        {
                            Console.SetAlert("CREATE MATRIX A", "Do you want to make Matrix B\nan identity matrix?", 1, Menu.OPT_YESNO);
                            r = Console.UpdateGetChar();
                        }
                        if (r == 'Y' && w == h)
                            MB = new Matrix(h, w, true);
                        else
                            MB = new Matrix(h, w, false);
                        Console.SetAlert("SUCCESS", "Matrix B was successfuly created.", 0);
                        Console.UpdateGetChar();
                        break;
                    case 'A':
                        if (MA == null)
                        {
                            Console.SetAlert("ERROR", "Matrix A not yet initialized!", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            Console.MainTitleText = "EDIT MATRIX A";
                            Console.DisplayMatrix = MA;
                            Console.DisplayMode = SuperConsole.DISP_MATRIX;
                            string inp;
                            for (int y = 0; y < MA.MatrixHeight; y++)
                                for (int x = 0; x < MA.MatrixWidth; x++)
                                {
                                    Console.PromptText = "ENTER VALUE FOR [" + (y+1) + ", " + (x+1) + "] (X TO EXIT)";
                                    Console.MatrixColumnHighlight = x;
                                    Console.MatrixRowHighlight = y;
                                    inp = Console.UpdateGetString();
                                    if (inp.ToUpper() == "X")
                                    {
                                        y = MA.MatrixHeight;
                                        x = MA.MatrixWidth;
                                    }
                                    else
                                        MA.SetCell(y, x, Convert.ToDouble(inp));
                                }
                            Console.SetAlert("SUCCESS", "Matrix A successfuly edited.", 0);
                            Console.UpdateGetChar();
                            Console.MatrixRowHighlight = -1;
                            Console.MatrixColumnHighlight = -1;
                        }
                        break;
                    case 'S':
                        if (MB == null)
                        {
                            Console.SetAlert("ERROR", "Matrix B not yet initialized!", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            Console.MainTitleText = "EDIT MATRIX B";
                            Console.DisplayMatrix = MB;
                            Console.DisplayMode = SuperConsole.DISP_MATRIX;
                            string inp;
                            for (int y = 0; y < MB.MatrixHeight; y++)
                                for (int x = 0; x < MB.MatrixWidth; x++)
                                {
                                    Console.PromptText = "ENTER VALUE FOR [" + (y + 1) + ", " + (x + 1) + "] (X TO EXIT)";
                                    Console.MatrixColumnHighlight = x;
                                    Console.MatrixRowHighlight = y;
                                    inp = Console.UpdateGetString();
                                    if (inp.ToUpper() == "X")
                                    {
                                        y = MA.MatrixHeight;
                                        x = MA.MatrixWidth;
                                    }
                                    else
                                        MB.SetCell(y, x, Convert.ToDouble(inp));
                                }
                            Console.SetAlert("SUCCESS", "Matrix B successfuly edited.", 0);
                            Console.UpdateGetChar();
                            Console.MatrixRowHighlight = -1;
                            Console.MatrixColumnHighlight = -1;
                        }
                        break;
                    case 'D':
                        if (MA == null)
                        {
                            Console.SetAlert("ERROR", "Matrix A not yet initialized!", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            Console.MainTitleText = "EDIT MATRIX A CELL";
                            Console.DisplayMatrix = MA;
                            Console.DisplayMode = SuperConsole.DISP_MATRIX;

                            do
                            {
                                Console.PromptText = "ENTER ROW TO EDIT";
                                h = Console.UpdateGetInt() - 1;
                                if (h >= MA.MatrixHeight || h < 0)
                                {
                                    Console.SetAlert("ERROR", "Invalid row index!", 2);
                                    Console.UpdateGetChar();
                                }
                            }
                            while (h >= MA.MatrixHeight || h < 0);
                            Console.MatrixRowHighlight = h;

                            do
                            {
                                Console.PromptText = "ENTER COLUMN TO EDIT";
                                w = Console.UpdateGetInt() - 1;
                                if (w >= MA.MatrixWidth || w < 0)
                                {
                                    Console.SetAlert("ERROR", "Invalid column index!", 2);
                                    Console.UpdateGetChar();
                                }
                            }
                            while (w >= MA.MatrixWidth || w < 0);
                            Console.MatrixColumnHighlight = w;

                            Console.PromptText = "ENTER NEW VALUE FOR [" + (h+1) + ", " + (w+1) + "]";
                            MA.SetCell(h, w, Console.UpdateGetDouble());
                                
                            Console.SetAlert("SUCCESS", "Matrix A's cell [" + (h+1) + ", " + (w+1) + "]" + "\nsuccessfuly edited.", 0);
                            Console.UpdateGetChar();
                            Console.MatrixRowHighlight = -1;
                            Console.MatrixColumnHighlight = -1;
                        }
                        break;
                    case 'F':
                        if (MB == null)
                        {
                            Console.SetAlert("ERROR", "Matrix B not yet initialized!", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            Console.MainTitleText = "EDIT MATRIX B CELL";
                            Console.DisplayMatrix = MB;
                            Console.DisplayMode = SuperConsole.DISP_MATRIX;

                            do
                            {
                                Console.PromptText = "ENTER ROW TO EDIT";
                                h = Console.UpdateGetInt() - 1;
                                if (h >= MA.MatrixHeight || h < 0)
                                {
                                    Console.SetAlert("ERROR", "Invalid row index!", 2);
                                    Console.UpdateGetChar();
                                }
                            }
                            while (h >= MA.MatrixHeight || h < 0);
                            Console.MatrixRowHighlight = h;

                            do
                            {
                                Console.PromptText = "ENTER COLUMN TO EDIT";
                                w = Console.UpdateGetInt() - 1;
                                if (w >= MA.MatrixWidth || w < 0)
                                {
                                    Console.SetAlert("ERROR", "Invalid column index!", 2);
                                    Console.UpdateGetChar();
                                }
                            }
                            while (w >= MA.MatrixWidth || w < 0);
                            Console.MatrixColumnHighlight = w;

                            Console.PromptText = "ENTER NEW VALUE FOR [" + (h + 1) + ", " + (w + 1) + "]";
                            MB.SetCell(h, w, Console.UpdateGetDouble());

                            Console.SetAlert("SUCCESS", "Matrix B's cell [" + (h + 1) + ", " + (w + 1) + "]" + "\nsuccessfuly edited.", 0);
                            Console.UpdateGetChar();
                            Console.MatrixRowHighlight = -1;
                            Console.MatrixColumnHighlight = -1;
                        }
                        break;
                    case 'X':
                        r = 'X';
                        break;
                    default:
                        Console.SetAlert("ERROR", "Invalid selection", 2);
                        Console.UpdateGetChar();
                        break;
                }
            }
            while (r != 'X');
        }

        static void MathMenu()
        {
            char r;

            do
            {
                Console.MainTitleText = "MATH MENU";
                Console.PromptText = "PLEASE ENTER YOUR CHOICE.";
                Console.MenuOptions = Menu.MATH_MENU;
                Console.DisplayMode = SuperConsole.DISP_MENU;
                r = Console.UpdateGetChar();
                
                switch (r)
                {
                    case 'A':
                        if (MA == null)
                        {
                            Console.SetAlert("ERROR", "Matrix A not yet initialized.", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            if (MB == null)
                            {
                                Console.SetAlert("ERROR", "Matrix B not yet initialized.", 2);
                                Console.UpdateGetChar();
                            }
                            else
                            {
                                MC = MA.Add(MB);

                                if (MC.Error)
                                {
                                    Console.SetAlert("ERROR", MC.ErrorMessage, 2);
                                    Console.UpdateGetChar();
                                }
                                else
                                {
                                    Console.SetAlert("SUCCESS", "Operation successful.\nView result now?", 1, Menu.OPT_YESNO);
                                    r = Console.UpdateGetChar();
                                    if (r == 'Y')
                                        ShowMatrix(MC);
                                }
                            }
                        }
                        break;
                    case 'S':
                        if (MA == null)
                        {
                            Console.SetAlert("ERROR", "Matrix A not yet initialized.", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            if (MB == null)
                            {
                                Console.SetAlert("ERROR", "Matrix B not yet initialized.", 2);
                                Console.UpdateGetChar();
                            }
                            else
                            {
                                MC = MA.Subtract(MB);

                                if (MC.Error)
                                {
                                    Console.SetAlert("ERROR", MC.ErrorMessage, 2);
                                    Console.UpdateGetChar();
                                }
                                else
                                {
                                    Console.SetAlert("SUCCESS", "Operation successful.\nView result now?", 1, Menu.OPT_YESNO);
                                    r = Console.UpdateGetChar();
                                    if (r == 'Y')
                                        ShowMatrix(MC);
                                }
                            }
                        }
                        break;
                    case 'V':
                        if (MA == null)
                        {
                            Console.SetAlert("ERROR", "Matrix A not yet initialized.", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            if (MB == null)
                            {
                                Console.SetAlert("ERROR", "Matrix B not yet initialized.", 2);
                                Console.UpdateGetChar();
                            }
                            else
                            {
                                MC = MA.VectorMultiplication(MB);

                                if (MC.Error)
                                {
                                    Console.SetAlert("ERROR", MC.ErrorMessage, 2);
                                    Console.UpdateGetChar();
                                }
                                else
                                {
                                    Console.SetAlert("SUCCESS", "Operation successful.\nView result now?", 1, Menu.OPT_YESNO);
                                    r = Console.UpdateGetChar();
                                    if (r == 'Y')
                                        ShowMatrix(MC);
                                }
                            }
                        }
                        break;
                    case 'M':
                        if (MA == null)
                        {
                            Console.SetAlert("ERROR", "Matrix A not yet initialized.", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            Console.SetAlert("SCALAR MULTIPLICATION", "Scalar value", 3);
                            double v = Console.UpdateGetDouble();

                            MC = MA.ScalarMultiplication(v);

                            if (MC.Error)
                            {
                                Console.SetAlert("ERROR", MC.ErrorMessage, 2);
                                Console.UpdateGetChar();
                            }
                            else
                            {
                                Console.SetAlert("SUCCESS", "Operation successful.\nView result now?", 1, Menu.OPT_YESNO);
                                r = Console.UpdateGetChar();
                                if (r == 'Y')
                                    ShowMatrix(MC);
                            }

                        }
                        break;
                    case 'C':
                        if (MA == null)
                        {
                            Console.SetAlert("ERROR", "Matrix A not yet initialized.", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            if (MB == null)
                            {
                                Console.SetAlert("ERROR", "Matrix B not yet initialized.", 2);
                                Console.UpdateGetChar();
                            }
                            else
                            {
                                MC = MA.Augment(MB);

                                if (MC.Error)
                                {
                                    Console.SetAlert("ERROR", MC.ErrorMessage, 2);
                                    Console.UpdateGetChar();
                                }
                                else
                                {
                                    Console.SetAlert("SUCCESS", "Operation successful.\nView result now?", 1, Menu.OPT_YESNO);
                                    r = Console.UpdateGetChar();
                                    if (r == 'Y')
                                        ShowMatrix(MC);
                                }
                            }
                        }
                        break;
                    case 'T':
                        if (MA == null)
                        {
                            Console.SetAlert("ERROR", "Matrix A not yet initialized.", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {

                            MC = MA.Transpose();

                            if (MC.Error)
                            {
                                Console.SetAlert("ERROR", MC.ErrorMessage, 2);
                                Console.UpdateGetChar();
                            }
                            else
                            {
                                Console.SetAlert("SUCCESS", "Operation successful.\nView result now?", 1, Menu.OPT_YESNO);
                                r = Console.UpdateGetChar();
                                if (r == 'Y')
                                    ShowMatrix(MC);
                            }
                        }
                        break;
                    case 'G':
                        if (MA == null)
                        {
                            Console.SetAlert("ERROR", "Matrix A not yet initialized.", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            Matrix Temp = MA.GaussJordanElimination();

                            if (Temp.Error)
                            {
                                Console.SetAlert("ERROR", Temp.ErrorMessage, 2);
                                Console.UpdateGetChar();
                            }
                            else
                            {
                                // now resize the matrix
                                for (int y = 0; y < Temp.MatrixHeight; y++)
                                    Temp.SetCell(y, 0, Temp.GetCell(y, Temp.MatrixWidth - 1));

                                Temp.CopyTo(ref MC, 0, 1);
                                Console.SetAlert("SUCCESS", "Operation successful.\nView result now?", 1, Menu.OPT_YESNO);
                                r = Console.UpdateGetChar();
                                if (r == 'Y')
                                    ShowMatrix(MC);
                            }
                        }
                        break;
                    case 'I':
                        if (MA == null)
                        {
                            Console.SetAlert("ERROR", "Matrix A not yet initialized.", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {

                            MC = MA.Invert();

                            if (MC.Error)
                            {
                                Console.SetAlert("ERROR", MC.ErrorMessage, 2);
                                Console.UpdateGetChar();
                            }
                            else
                            {
                                Console.SetAlert("SUCCESS", "Operation successful.\nView result now?", 1, Menu.OPT_YESNO);
                                r = Console.UpdateGetChar();
                                if (r == 'Y')
                                    ShowMatrix(MC);
                            }
                        }
                        break;
                    case 'D':
                        if (MA == null)
                        {
                            Console.SetAlert("ERROR", "Matrix A not yet initialized.", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            if (MA.MatrixWidth == MA.MatrixHeight)
                            {
                                Console.SetAlert("MATRIX DETERMINANT", "The determinant is:\n" + MA.Determinant().ToString(), 0);
                                r = Console.UpdateGetChar();
                            }
                            else
                            {
                                Console.SetAlert("ERROR", "Matrix must be square, e.g.\nhas the same width and height.", 2);
                                Console.UpdateGetChar();
                            }
                        }
                        break;
                }
            }
            while (r != 'X');
        }

        static void OutputMenu()
        {
            char r;

            do
            {
                Console.MainTitleText = "OUTPUT MENU";
                Console.PromptText = "PLEASE ENTER YOUR CHOICE.";
                Console.MenuOptions = Menu.OUTPUT_MENU;
                Console.DisplayMode = SuperConsole.DISP_MENU;
                r = Console.UpdateGetChar();

                switch (r)
                {
                    case 'A':
                        ShowMatrix(MA);
                        break;
                    case 'B':
                        ShowMatrix(MB);
                        break;
                    case 'C':
                        ShowMatrix(MC);
                        break;
                    case 'X':
                        r = 'X';
                        break;
                    default:
                        Console.SetAlert("ERROR", "Invalid selection", 2);
                        Console.UpdateGetChar();
                        break;
                }
            }
            while (r != 'X');
        }

        static void TransferMenu()
        {
            char r;

            do
            {
                Console.MainTitleText = "TRANSFER MENU";
                Console.PromptText = "PLEASE ENTER YOUR CHOICE.";
                Console.MenuOptions = Menu.TRANSFER_MENU;
                Console.DisplayMode = SuperConsole.DISP_MENU;
                r = Console.UpdateGetChar();

                switch (r)
                {
                    case 'Q':
                        if (MA == null)
                        {
                            Console.SetAlert("ERROR", "Matrix A not yet initialized!", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            MA.CopyTo(ref MT);
                            Console.SetAlert("SUCCESS", "Matrix A copied to Matrix T.", 0);
                            Console.UpdateGetChar();
                        }
                        break;
                    case 'W':
                        if (MB == null)
                        {
                            Console.SetAlert("ERROR", "Matrix B not yet initialized!", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            MB.CopyTo(ref MT);
                            Console.SetAlert("SUCCESS", "Matrix B copied to Matrix T.", 0);
                            Console.UpdateGetChar();
                        }
                        break;
                    case 'E':
                        if (MC == null)
                        {
                            Console.SetAlert("ERROR", "Matrix C not yet initialized!", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            MC.CopyTo(ref MT);
                            Console.SetAlert("SUCCESS", "Matrix C copied to Matrix T.", 0);
                            Console.UpdateGetChar();
                        }
                        break;
                    case 'A':
                        if (MT == null)
                        {
                            Console.SetAlert("ERROR", "Matrix T not yet initialized!", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            MT.CopyTo(ref MA);
                            Console.SetAlert("SUCCESS", "Matrix T copied to Matrix A.", 0);
                            Console.UpdateGetChar();
                        }
                        break;
                    case 'S':
                        if (MT == null)
                        {
                            Console.SetAlert("ERROR", "Matrix T not yet initialized!", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            MT.CopyTo(ref MB);
                            Console.SetAlert("SUCCESS", "Matrix T copied to Matrix B.", 0);
                            Console.UpdateGetChar();
                        }
                        break;
                    case 'D':
                        if (MA == null)
                        {
                            Console.SetAlert("ERROR", "Matrix A not yet initialized!", 2);
                            Console.UpdateGetChar();
                        }
                        else
                        {
                            if (MB == null)
                            {
                                Console.SetAlert("ERROR", "Matrix B not yet initialized!", 2);
                                Console.UpdateGetChar();
                            }
                            else
                            {
                                MA.CopyTo(ref MT);
                                MB.CopyTo(ref MA);
                                MT.CopyTo(ref MB);
                                MT = null;
                                Console.SetAlert("SUCCESS", "Matrix A and Matrix B were swapped.", 0);
                                Console.UpdateGetChar();
                            }
                        }
                        break;
                    case 'X':
                        r = 'X';
                        break;
                    default:
                        Console.SetAlert("ERROR", "Invalid selection", 2);
                        Console.UpdateGetChar();
                        break;
                }
            }
            while (r != 'X');
            MT = null;
        }

        static void AboutMenu()
        {
            string ABOUT =
                "MATRIX MATH\n" +
                "version 1.0\n\n" +

                "Programmed in C# by Kyle Alexander Buan\n" +
                "For Numerical Methods class\n\n" +

                "Submitted to Prof. Manny Bernabe\n\n" +

                "Built on February 8, 2016; 01:49AM\n" +
                "using Microsoft Visual Studio 2015\n" +
                "for the .NET Framework 4 Client Profile\n\n" +

                "Using SuperConsole library by\n" + 
                "Kyle Alexander Buan\n\n" +
                
                "\"I never wanted to help her. But the me that time\n" +
                "did not want any enemies, so I didn't really have\n"+
                "a choice.\"";

            Console.SetAlert("ABOUT THIS PROGRAM", ABOUT, 1);
            Console.UpdateGetChar();
        }
    }
}
