using ChessEngine;
using System.Collections;
using System;

namespace ChessEngine
{
    public class Knight : Chessman
    {
        public Knight(int i_row, int i_col, bool i_isWhite, string i_displayName) : base(i_row, i_col, i_isWhite, i_displayName) { }

        public override bool[,] PossibleMove(Chessman[,] chessmans)
        {
            bool[,] r = new bool[8, 8];

            //קדימה שמאלה
            KnightMove(m_currentRow - 1, m_currentCol + 2, ref r, chessmans);

            //קדימה ימינה
            KnightMove(m_currentRow + 1, m_currentCol + 2, ref r, chessmans);

            //ימינה למעלה
            KnightMove(m_currentRow + 2, m_currentCol + 1, ref r, chessmans);

            //ימינה למטה
            KnightMove(m_currentRow + 2, m_currentCol - 1, ref r, chessmans);

            //אחורה שמאלה
            KnightMove(m_currentRow - 1, m_currentCol - 2, ref r, chessmans);

            //אחורה ימינה
            KnightMove(m_currentRow + 1, m_currentCol - 2, ref r, chessmans);

            //שמאלה למעלה
            KnightMove(m_currentRow - 2, m_currentCol + 1, ref r, chessmans);

            //שמאלה למטה
            KnightMove(m_currentRow - 2, m_currentCol - 1, ref r, chessmans);

            return r;
        }
        public override Chessman clone()
        {
            Chessman cm = new Knight(m_currentRow, m_currentCol, m_isWhite, displayName);
            return cm;
        }

        public void KnightMove(int x, int y, ref bool[,] r, Chessman[,] board)
        {
            Chessman c;
            if (x >= 0 && x < 8 && y >= 0 && y < 8)
            {
                c = board[x, y];
                if (c == null)//אין כלי 
                    r[x, y] = true;//צעד חוקי
                else if (m_isWhite != c.m_isWhite)//הכלי אויב   
                    r[x, y] = true;//צעד חוקי
            }
        }

        public override int value()
        {
            int row = m_currentRow;
            int col = m_currentCol;
            int value = 320;
            switch (row)
            {
                case 0:
                    if (col == 0 || col == 7)
                        value += -50;
                    if (col >  1 && col < 6)
                        value += -30;
                    if (col == 1 || col == 6)
                        value += -40;
                    break;
                case 7:
                    if (col == 0 || col == 7)
                        value += -50;
                    if (col > 1 && col < 6)
                        value += -30;
                    if (col == 1 || col == 6)
                        value += -40;
                    break;
                case 1:
                    if (col == 0 || col == 7)
                        value += -40; 
                    if (col == 1 || col == 6)
                        value += -20;
                    break;
                case 2:
                    if (col == 0 || col == 7)
                        value += -30;
                    if (col == 2 || col == 5)
                        value += 10;
                    if (col == 3 || col == 4)
                        value += 15;
                    break;
                case 3:
                    if (col == 0 || col == 7)
                        value += -30;
                    if (col == 1 || col == 6 )
                        value += 5;
                    if (col == 2 || col == 5)
                        value += 15;
                    if (col == 3 || col == 4)
                        value += 20;
                    break;
                case 4:
                    if (col == 0 || col == 7)
                        value += -30;
                    if (col == 2 || col == 5)
                        value += 14;
                    if (col == 3 || col == 4)
                        value += 20;
                    break;
                case 5:
                    if (col == 0 || col == 7)
                        value += -30;
                    if (col == 1 || col == 6)
                        value += 5;
                    if (col == 2 || col == 5)
                        value += 10;
                    if (col == 3 || col == 4)
                        value += 15;
                    break;
                case 6:
                    if (col == 0 || col == 7)
                        value += -40;
                    if (col == 1 || col == 6)
                        value += -20;
                    if (col == 3 || col == 4)
                        value += 5;
                    break;
            }
            return value;
        }
    }
}
