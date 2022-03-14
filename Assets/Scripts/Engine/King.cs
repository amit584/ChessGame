using ChessEngine;
using System.Collections;
using System;

namespace ChessEngine
{
    public class King : Chessman
    {
        public King(int i_row, int i_col, bool i_isWhite, string i_displayName) : base(i_row, i_col, i_isWhite, i_displayName) { }

        public override bool[,] PossibleMove(Chessman[,] chessmans)
        {
            bool[,] r = new bool[8, 8];

            Chessman c;
            int col, row;

            // קדימה
            row = m_color.forward(m_currentRow);
            if (!m_color.pastLast(row))
                col = checkCols(chessmans, r, row);

            // אחורה
            row = m_currentRow - m_color.direction();
            if (!m_color.beforeFirst(row))
                col = checkCols(chessmans, r, row);

            // צדדים
            col = m_currentCol - 1;
            if (col >= 0)
            {
                c = chessmans[m_currentRow, col];
                if (c == null || c.m_isWhite != m_isWhite)
                {
                    r[m_currentRow, col] = true;
                }
            }
            col = m_currentCol + 1;
            if (col <= 7)
            {
                c = chessmans[m_currentRow, col];
                if (c == null || c.m_isWhite != m_isWhite)
                {
                    r[m_currentRow, col] = true;
                }
            }
            return r;
        }

        private int checkCols(Chessman[,] chessmans, bool[,] r, int i_row)
        {
            int col;
            Chessman c;
            {
                for (col = m_currentCol - 1; col < m_currentCol + 2; col++)
                {
                    if (col >= 0 && col <= 7)
                    {
                        c = chessmans[i_row, col];
                        if (c == null || c.m_isWhite != m_isWhite)
                        {
                            r[i_row, col] = true;
                        }
                    }
                }
            }

            return col;
        }

        public override Chessman clone()
        {
            Chessman cm = new King(m_currentRow, m_currentCol, m_isWhite, displayName);
            return cm;
        }

        public override int value()
        {
            return 20000;
        }
    }
}
