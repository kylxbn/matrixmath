// Matrix library
// Kyle Alexander Buan
// A part of MatrixMath
// February 7, 2016

using System;

namespace Kyle_MatrixMath
{
    class Matrix
    {
        double[,] Cells;
        public bool Error;
        public string ErrorMessage;
        public int border;

        public int MatrixWidth { get; }

        public int MatrixHeight { get; }

        public Matrix(int height, int width, bool identity = false)
        {
            Error = false;
            ErrorMessage = "";
            border = 0;

            MatrixWidth = width;
            MatrixHeight = height;
            Cells = new double[MatrixWidth, MatrixHeight];

            for (int y = 0; y < MatrixHeight; y++)
                for (int x = 0; x < MatrixWidth; x++)
                {
                    Cells[x, y] = (identity && x == y) ? 1 : 0;
                }
        }

        public double GetCell(int y, int x)
        {
            return Cells[x, y];
        }

        public void SetCell(int y, int x, double val)
        {
            Cells[x, y] = val;
        }

        public void CopyTo(ref Matrix M, int height = 0, int width = 0)
        {
            M = new Matrix(height == 0 ? this.MatrixHeight : height, width == 0 ? this.MatrixWidth : width);
            for (int y = 0; y < (height == 0 ? this.MatrixHeight : height); y++)
                for (int x = 0; x < (width == 0 ? this.MatrixWidth : width); x++)
                    M.SetCell(y, x, this.GetCell(y,x));
            M.border = this.border;
        }

        public Matrix Add(Matrix M)
        {
            Matrix Res = new Matrix(MatrixHeight, MatrixWidth);

            if (MatrixWidth == M.MatrixWidth && MatrixHeight == M.MatrixHeight)
            {
                for (int y = 0; y < MatrixHeight; y++)
                    for (int x = 0; x < MatrixWidth; x++)
                        Res.SetCell(y,x, this.GetCell(y, x) + M.GetCell(y, x));
            }
            else
            {
                Res.Error = true;
                Res.ErrorMessage = "The matrices must be of the same size.";
            }

            return Res;
        }

        public Matrix Subtract(Matrix M)
        {
            Matrix Res = new Matrix(MatrixHeight, MatrixWidth);

            if (MatrixWidth == M.MatrixWidth && MatrixHeight == M.MatrixHeight)
            {
                for (int y = 0; y < MatrixHeight; y++)
                    for (int x = 0; x < MatrixWidth; x++)
                        Res.SetCell(y, x, this.GetCell(y, x) - M.GetCell(y, x));
            }
            else
            {
                Res.Error = true;
                Res.ErrorMessage = "The matrices must be of the same size.";
            }

            return Res;
        }

        public double Determinant()
        {
            if (this.MatrixWidth == 1)
                return this.GetCell(0, 0);
            else
            {
                int sign = 1;
                double res = 0;
                for (int x = 0; x< this.MatrixWidth; x++)
                {
                    res += sign * (this.GetCell(0, x) * this.Minor(0, x).Determinant());
                    sign *= -1;
                }
                return res;
            }
        }

        private Matrix Minor(int ypos, int xpos)
        {
            Matrix Res = null;
            this.CopyTo(ref Res);

            // remove row
            for (int x = 0; x<Res.MatrixWidth; x++)
            {
                for (int y = ypos; y<(Res.MatrixHeight-1); y++)
                {
                    Res.SetCell(y, x, Res.GetCell(y + 1, x));
                }
            }

            // remove column
            for (int y = 0; y<Res.MatrixHeight; y++)
            {
                for (int x = xpos; x<(Res.MatrixWidth-1); x++)
                {
                    Res.SetCell(y, x, Res.GetCell(y, x + 1));
                }
            }

            Matrix Res2 = null;
            Res.CopyTo(ref Res2, Res.MatrixHeight - 1, Res.MatrixHeight - 1);

            return Res2;
        }

        public Matrix ScalarMultiplication(double n)
        {
            Matrix Res = new Matrix(MatrixHeight, MatrixWidth);

            for (int y = 0; y < MatrixHeight; y++)
                for (int x = 0; x < MatrixWidth; x++)
                    Res.SetCell(y, x, GetCell(y, x) * n);

            return Res;
        }

        public Matrix VectorMultiplication(Matrix M)
        {
            Matrix Res = new Matrix(this.MatrixHeight, M.MatrixWidth);

            if (this.MatrixWidth != M.MatrixHeight)
            {
                Res.Error = true;
                Res.ErrorMessage = "The matrix to multiply has\nan invalid width.";
            }
            else
            {
                double total;

                for (int leftRow = 0; leftRow < this.MatrixHeight; leftRow++)
                {
                    for (int rightColumn = 0; rightColumn < M.MatrixWidth; rightColumn++)
                    {
                        total = 0;
                        for (int rightCell = 0; rightCell < M.MatrixHeight; rightCell++)
                        {
                            total += this.GetCell(leftRow, rightCell) * M.GetCell(rightCell, rightColumn);
                        }
                        Res.SetCell(leftRow, rightColumn, total);
                    }
                }
            }

            return Res;
        }

        public Matrix Augment(Matrix M)
        {
            Matrix Res = new Matrix(this.MatrixHeight, this.MatrixWidth + M.MatrixWidth);

            if (this.MatrixHeight != M.MatrixHeight)
            {
                Res.Error = true;
                Res.ErrorMessage = "The matrices must have the same height.";
            }
            else
            {
                for (int y = 0; y<this.MatrixHeight; y++)
                {
                    for (int x = 0; x<this.MatrixWidth; x++)
                    {
                        Res.SetCell(y, x, this.GetCell(y, x));
                    }
                }
                for (int y = 0; y < M.MatrixHeight; y++)
                {
                    for (int x = 0; x < M.MatrixWidth; x++)
                    {
                        Res.SetCell(y, x+this.MatrixWidth, M.GetCell(y, x));
                    }
                }
                Res.border = this.MatrixWidth;
            }
            return Res;
        }

        public Matrix Transpose()
        {
            Matrix Res = new Matrix(this.MatrixWidth, this.MatrixHeight);

            for (int y = 0; y < this.MatrixHeight; y++)
                for (int x = 0; x < this.MatrixWidth; x++)
                    Res.SetCell(x, y, this.GetCell(y, x));

            return Res;
        }

// -----------------------------------------------------
// FROM HERE ON, we trudge onto the mysterious yet dangerous lands of...
// << T H E   E L E M E N T A R Y   M A T R I X   R O W   O P E R A T I O N S >>
// which even the bravest of warriors dare not enter.
// Are you brave enough?
// (And no... I'm not brave enough... But I have to plunge into the darkness
//  in order to save my beloved College Diploma from the hands of the
//  tester--The Overseer--which guides all who tries to pass through
//  THE GATE OF GRADUATION **thunderclap**)
// If so, then, prove thineself, o warrior, and show The Overseer!
// Peace be upon my code. 

        void SwapRows(int r1, int r2)
        {
            double[] temp = new double[this.MatrixWidth];
            for (int x = 0; x < this.MatrixWidth; x++)
            {
                temp[x] = this.GetCell(r1, x);
            }
            for (int x=0; x < this.MatrixWidth; x++)
            {
                this.SetCell(r1, x, this.GetCell(r2, x));
                this.SetCell(r2, x, temp[x]);
            }
        }
        
        void MultiplyRow(int r, double v)
        {
            for (int x = 0; x < this.MatrixWidth; x++)
            {
                this.SetCell(r, x, this.GetCell(r, x) * v);
            }
        }

        void DivideRow(int r, double v)
        {
            for (int x = 0; x < this.MatrixWidth; x++)
            {
                this.SetCell(r, x, this.GetCell(r, x) / v);
            }
        }

        void AddToRow(int r1, int r2)
        {
            for (int x = 0; x < this.MatrixWidth; x++)
            {
                this.SetCell(r2, x, this.GetCell(r1, x) + this.GetCell(r2, x));
            }
        }

// AND HERE IS THE DREADED
// << G A U S S - J O R D A N   E L I M I N A T I O N >> SUBROUTINE

        public Matrix GaussJordanElimination()
        {
            double maxInColumn;
            int maxInColumnPos;

            Matrix Res = null;
            Matrix Left = null;
            Matrix Final = null;
            this.CopyTo(ref Res);
            this.CopyTo(ref Left, 0, this.MatrixWidth - 1);
            Res.border = 0;
            if (Left.Determinant() != 0)
            {
                for (int x = 0; x < Res.MatrixWidth; x++)
                {
                    maxInColumn = 0;
                    maxInColumnPos = x;
                    for (int y = x; y < Res.MatrixHeight; y++)
                    {
                        if (Math.Abs(Res.GetCell(y, x)) > maxInColumn)
                        {
                            maxInColumn = Math.Abs(Res.GetCell(y, x));
                            maxInColumnPos = y;
                        }
                    }
                    if (x < Res.MatrixHeight)
                        Res.SwapRows(x, maxInColumnPos);

                    for (int y = x + 1; y < this.MatrixHeight; y++)
                    {
                        if (Res.GetCell(y, x) != 0)
                        {
                            if (Math.Abs(Res.GetCell(y, x)) <= Math.Abs(Res.GetCell(x, x)))
                                Res.MultiplyRow(y, -Res.GetCell(x, x) / Res.GetCell(y, x));
                            else
                                Res.DivideRow(y, -Res.GetCell(y, x) / Res.GetCell(x, x));
                            Res.AddToRow(x, y);
                        }
                    }
                }

                // ROEW IS NAU EN RUE ECKELLONE FOURME (ALLSOE NOEN ASS 'TRYANGOOLAR FOERM').
                for (int x = Res.MatrixHeight - 1; x >= 0; x--)
                {
                    for (int y = x - 1; y >= 0; y--)
                    {
                        if (Res.GetCell(y, x) != 0)
                        {
                            if (Math.Abs(Res.GetCell(y, x)) <= Math.Abs(Res.GetCell(x, x)))
                                Res.MultiplyRow(y, -Res.GetCell(x, x) / Res.GetCell(y, x));
                            else
                                Res.DivideRow(y, -Res.GetCell(y, x) / Res.GetCell(x, x));
                            Res.AddToRow(x, y);
                        }
                    }
                    // But we have to make this single number ONE.
                    Res.DivideRow(x, Res.GetCell(x, x));
                }
                Res.CopyTo(ref Final);
            }
            else
            {
                Final = new Matrix(1, 1);
                Final.Error = true;
                Final.ErrorMessage = "This matrix has a determinant of zero\nand therefore does not have an answer.";
            }
            return Final;
        }

        public Matrix Invert()
        {
            Matrix Temp = null;
            Matrix Final = new Matrix(1, 1);
            if (this.MatrixWidth != this.MatrixHeight)
            {
                Final.Error = true;
                Final.ErrorMessage = "Matrix must be square.";
            }
            else if (this.Determinant() == 0)
            {
                Final.Error = true;
                Final.ErrorMessage = "Matrix has no inverse because\nits determinant is zero.";
            }
            else
            {
                Matrix Combi = this.Augment(new Matrix(this.MatrixHeight, this.MatrixWidth, true));
                Temp = Combi.GaussJordanElimination();

                for (int y = 0; y < Temp.MatrixHeight; y++)
                    for (int x = 0; x < Temp.MatrixWidth/2; x++)
                        Temp.SetCell(y, x, Temp.GetCell(y, x + Temp.MatrixWidth / 2));

                Temp.CopyTo(ref Final, 0, Temp.MatrixWidth / 2);
            }
            return Final;
        }
    }
}
