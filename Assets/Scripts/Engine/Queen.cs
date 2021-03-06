using ChessEngine;
using System.Collections;
using System;

namespace ChessEngine
{
    public class Queen : Chessman
    {
        public Queen(int i_row, int i_col, bool i_isWhite, string i_displayName) : base(i_row, i_col, i_isWhite, i_displayName) { }

        public override bool[,] PossibleMove(Chessman[,] chessmans)
        {
            bool[,] r = new bool[8, 8];

            pmDiagnol(chessmans, true, true, r);
            pmDiagnol(chessmans, true, false, r);
            pmDiagnol(chessmans, false, true, r);
            pmDiagnol(chessmans, false, false, r);
            pmRow(chessmans, -1, 0, r);
            pmRow(chessmans, 1, 7, r);
            pmCol(chessmans, 1, 7, r);
            pmCol(chessmans, -1, 0, r);
            return r;
        }
        public override Chessman clone()
        {
            Chessman cm = new Queen(m_currentRow, m_currentCol, m_isWhite, displayName);
            return cm;
        }

        public override int value()
        {
            int row = m_currentRow;
            int col = m_currentCol;
            int value = 900;
            switch (row)
            {
                case 0:
                    if (col == 0 || col == 7)
                        value += -20;
                    if (col == 1 || col == 2 || col ==5 || col == 6)
                        value += -10;
                    if (col == 3|| col == 4)
                        value += -5;
                    break;
                case 7:
                    if (col == 0 || col == 7)
                        value += -20;
                    if (col == 1 || col == 2 || col == 5 || col == 6)
                        value += -10;
                    if (col == 3 || col == 4)
                        value += -5;
                    break;
                case 1:
                    if (col == 0 || col == 7)
                        value += -10;
                    break;
                case 2:
                    if (col == 0 || col == 7)
                        value += -10;
                    if (col >1 || col <6)
                        value += 5;
                    break;
                case 3:
                    if (col == 0 || col == 7)
                        value += -5;
                    if (col > 1 || col < 6)
                        value += 5;
                    break;
                case 4:
                    if (col == 7)
                        value += -30;
                    if (col > 1 || col < 6)
                        value += 5;
                    break;
                case 5:
                    if (col == 0 || col == 7)
                        value += -10;
                    if (col > 0|| col <7)
                        value += 5;
                    break;
                case 6:
                    if (col == 0 || col == 7)
                        value += -10;
                    if (col == 2)
                        value += 5;
                    break;
            }
            return value;
        }
    }
}
