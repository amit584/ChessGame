using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessEngine
{
    abstract public class PieceColor
    {
        public abstract int lastRow();
        public abstract int firstRow();
        public abstract bool pastLast(int i_row);
        public abstract bool beforeFirst(int i_row);
        public abstract int direction();
        public int forward(int currentRow)
        {
            {
                int newRow = currentRow + direction();
                return newRow;
            }
        }
    }

    // from top (row 0) to bottom (row 7) 
    public class BlackPiece : PieceColor
    {
        public override int direction()
        {
            return +1;
        }

        public override int lastRow()
        {
            return 7;
        }

        public override int firstRow()
        {
            return 0;
        }

        public override bool pastLast(int i_row)
        {
            return i_row > 7;
        }
        public override bool beforeFirst(int i_row)
        {
            return i_row < 0;
        }
    }

    // from bottom (row 7) to top (row 0) 
    public class WhitePiece : PieceColor
    {
        public override int direction()
        {
            return -1;
        }

        public override int lastRow()
        {
            return 0;
        }

        public override int firstRow()
        {
            return 7;
        }

        public override bool pastLast(int i_row)
        {
            return i_row < 0;
        }
        public override bool beforeFirst(int i_row)
        {
            return i_row > 7;
        }
    }

    public abstract class Chessman
    {
        public string displayName { set; get; }
        public int m_currentRow { set; get; }
        public int m_currentCol { set; get; }
        public bool m_isWhite;
        public PieceColor m_color;

        public Chessman()
        { }
        public Chessman(int i_row, int i_col, bool i_isWhite, string i_displayName)
        {
            SetPosition(i_row, i_col);
            m_isWhite = i_isWhite;
            displayName = i_displayName;
            if (m_isWhite)
            {
                m_color = new WhitePiece();
            }
            else
            {
                m_color = new BlackPiece();
            }
        }
        public void SetPosition(int x, int y)
        {
            m_currentRow = x;
            m_currentCol = y;
        }

        public virtual bool[,] PossibleMove(Chessman[,] chessmans)//צעדים אפשריים שמשתנים לכל כלי
        {
            return new bool[8, 8];
        }
        public abstract Chessman clone();

        public abstract int value();

        protected void pmDiagnol(Chessman[,] chessmans, bool i_toLeft, bool i_toTop, bool[,] r)
        {
            int row = m_currentRow;
            int col = m_currentCol;
            int lastRow;
            int rowJump;
            int lastCol;
            int colJump;
            if (i_toTop)
            {
                lastRow = 0;
                rowJump = -1;
            }
            else
            {
                lastRow = 7;
                rowJump = 1;
            }
            if (i_toLeft)
            {
                lastCol = 0;
                colJump = -1;
            }
            else
            {
                lastCol = 7;
                colJump = 1;
            }

            while (row != lastRow && col != lastCol)
            {
                row += rowJump;
                col += colJump;
                Chessman c = chessmans[row, col];
                if (c == null)
                {
                    r[row, col] = true;
                }
                else
                {
                    if (m_isWhite != c.m_isWhite)
                    {
                        r[row, col] = true;
                    }
                    break;
                }
            };
        }
        protected void pmRow(Chessman[,] chessmans, int i_rowJump, int i_lastrow, bool[,] r)
        {
            int row = m_currentRow;
            while (row != i_lastrow)
            {
                row += i_rowJump;
                Chessman c = chessmans[row, m_currentCol];
                if (c == null)
                {
                    r[row, m_currentCol] = true;
                }
                else
                {
                    if (c.m_isWhite != m_isWhite)
                        r[row, m_currentCol] = true;
                    break;
                }
            }
        }

        protected void pmCol(Chessman[,] chessmans, int i_colJump, int i_lastCol, bool[,] r)
        {
            int col = m_currentCol;
            while (col != i_lastCol)
            {
                col += i_colJump;
                Chessman c = chessmans[m_currentRow, col];
                if (c == null)
                {
                    r[m_currentRow, col] = true;
                }
                else
                {
                    if (c.m_isWhite != m_isWhite)
                        r[m_currentRow, col] = true;
                    break;
                }
            }
        }


    }
}
