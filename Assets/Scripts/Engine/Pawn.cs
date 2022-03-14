using ChessEngine;
using System.Collections;
using System;
namespace ChessEngine
{
    public class Pawn : Chessman
    {
        public Pawn(int i_row, int i_col, bool i_isWhite, string i_displayName) : base(i_row, i_col, i_isWhite, i_displayName) { }

        public override bool[,] PossibleMove(Chessman[,] chessmans)
        {
            bool[,] r = new bool[8, 8];
            Chessman c, c2;

            int nextRow = m_color.forward(m_currentRow);
            if (!m_color.pastLast(nextRow)) // יש לאן להתקדם
            {
                // תקיפה לימין
                if (m_currentCol != 7)
                {
                    c = chessmans[nextRow, m_currentCol + 1];
                    if (c != null && c.m_isWhite != m_isWhite)
                    {
                        r[nextRow, m_currentCol + 1] = true;
                    }
                }

                // תקיפה לשמאל
                if (m_currentCol != 0)
                {
                    c = chessmans[nextRow, m_currentCol - 1];
                    if (c != null && c.m_isWhite != m_isWhite)
                    {
                        r[nextRow, m_currentCol - 1] = true;
                    }
                }

                // התקדמות רגילה אם פנוי
                c = chessmans[nextRow, m_currentCol];
                if (c == null)
                {
                    r[nextRow, m_currentCol] = true;
                }

                // ואולי אפשר להתקדם כפול 
                if (m_color.forward(m_color.firstRow()) == m_currentRow // רק אם אני עדיין בשורה השנייה = התחלה ועוד אחת 
                    && c == null // אין אף אחד בשורה הבאה - ראה למעלה
                    )
                {
                    int nextNextRow = m_color.forward(nextRow);
                    c2 = chessmans[nextNextRow, m_currentCol];
                    if (c2 == null)
                    {
                        r[nextNextRow, m_currentCol] = true;
                    }
                }
            }

            return r;//החזרת מערך בוליאני של המקומות האפשריים
        }

        public override Chessman clone()
        {
            Chessman cm = new Pawn(m_currentRow, m_currentCol, m_isWhite, displayName);
            // cm.name = name;
            return cm;
        }
        public override int value()
        {
            int row = m_currentRow;
            int col = m_currentCol;
            int value = 100;
            switch (row)
            {
                case 1:
                    value += 50;
                    break;
                case 2:
                    if (col == 0 || col == 1 || col == 6 || col == 7)
                       value += 10;
                    if (col == 2 || col == 5)
                       value += 20;
                    if (col == 3 || col == 4)
                       value += 30;
                        break;
                case 3:
                    if (col == 0 || col == 1 || col == 6 || col == 7)
                        value += 5;
                    if (col == 2 || col == 5)
                        value += 10;
                    if (col == 3 || col == 4)
                        value += 25;
                    break;
                case 4:
                    if (col == 3 || col == 4)
                        value += 20;
                    break;
                case 5:
                    if (col == 0 ||  col == 7)
                        value += 5;
                    if (col == 1 || col == 6)
                        value += -5;
                    if (col == 2 || col == 5)
                        value += -10;
                    break;
                case 6:
                    if (col == 0 || col == 7)
                        value += 5;
                    if (col == 1 || col == 2 || col == 5 || col == 6)
                        value += 10;
                    if (col == 3 || col == 4)
                        value += -20;
                    break;
            }
            return value;
        }
    }
}
