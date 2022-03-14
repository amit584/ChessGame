using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessEngine
{
    namespace ChessEngine
    {
        class ChessGame
        {
            public class Move
            {
                public int value;
                public int toCol;
                public int toRow;
                public int fromRow;
                public int fromCol;
                public Chessman[,] newBoard;

                public bool hasMoved { get; set; }
            }
            private bool[,] allowedMoves { set; get; }

            public Chessman[,] Chessmans { set; get; }//מערך של הכלים
            private Chessman selectedChessman;//הכלי הנבחר

            private const float TILE_SIZE = 1.0f;
            private const float TILE_OFFSET = 0.5f;

            private int selectionX = -1;
            private int selectionY = -1;

            public int[] EnPassantMove { set; get; }

            public static bool isWhiteTurn = true;//סימון תור השחקן
            private static int m_printBoardLevel = 100;

            public void Start()//הלוח במצב ראשוני
            {
                m_printBoardLevel = 99;
                SpawnAllChessmans();
                bool whiteMove = true;
                int moveCount = 0;
                while (true)
                {
                    moveCount++;
                    PrintBoard(Chessmans);
                    Move move = GetNextMove(whiteMove, 2, Chessmans);
                    Chessman cm = move.newBoard[move.toRow, move.toCol];
                    Console.WriteLine();
                    Console.WriteLine("Move #{3}: {0} to {1},{2}", cm.displayName, move.toRow, move.toCol, moveCount);
                    whiteMove = !whiteMove;
                    Chessman eaten = Chessmans[move.toRow, move.toCol];
                    if (eaten != null && eaten.GetType() == typeof(King))
                    {
                        Console.WriteLine("END!!!");
                       //Console.ReadKey();
                        break;
                    }
                    Chessmans = move.newBoard;
                }
            }
            public static void PrintBoard(Chessman[,] Board, int i_level = 99)
            {
                if (i_level < m_printBoardLevel) return;
                for (int row = 0; row < 8; row++)
                {
                    Console.Write("{0} |", row);
                    for (int col = 0; col < 8; col++)
                    {
                        if (Board[row, col] == null)
                            Console.Write(" " + " " + " " + "|");
                        else
                        {
                            string print = Board[row, col].displayName;
                            Console.Write(" " + print + " " + "|");
                        }
                    }
                    Console.WriteLine();
                }
            }
            public static void LogBoard(Chessman[,] Board, int i_level = 99)
            {

                if (i_level < m_printBoardLevel) return;
                string s="";

                for (int row = 0; row < 8; row++)
                {
                    s += row + " |";
                    for (int col = 0; col < 8; col++)
                    {
                        if (Board[row, col] == null)
                            s += " " + "  " + " " + "|";
                        else
                        {
                            string print = Board[row, col].displayName;
                            s += " " + print + " " + "|";
                        }
                    }
                    s += Environment.NewLine;
                }

                UnityEngine.Debug.Log(s);
            }

            private void SpawnAllChessmans()//יצירת מיקום להניח את הכלים
            {
                Chessmans = new Chessman[8, 8];
                Chessmans[0, 0] = new Rook(0, 0, false, "BR");
                Chessmans[0, 1] = new Knight(0, 1, false, "BN");
                Chessmans[0, 2] = new Bishop(0, 2, false, "BB");
                Chessmans[0, 4] = new King(0, 4, false, "BI");
                Chessmans[0, 3] = new Queen(0, 3, false, "BQ");
                Chessmans[0, 5] = new Bishop(0, 5, false, "BB");
                Chessmans[0, 6] = new Knight(0, 6, false, "BN");
                Chessmans[0, 7] = new Rook(0, 7, false, "BR");
                for (int i = 0; i < 8; i++)
                    Chessmans[1, i] = new Pawn(1, i, false, "BP");
                Chessmans[7, 0] = new Rook(7, 0, true, "WR");
                Chessmans[7, 1] = new Knight(7, 1, true, "WN");
                Chessmans[7, 2] = new Bishop(7, 2, true, "WB");
                Chessmans[7, 3] = new King(7, 3, true, "WI");
                Chessmans[7, 4] = new Queen(7, 4, true, "WQ");
                Chessmans[7, 5] = new Bishop(7, 5, true, "WB");
                Chessmans[7, 6] = new Knight(7, 6, true, "WN");
                Chessmans[7, 7] = new Rook(7, 7, true, "WR");
                for (int i = 0; i < 8; i++)
                    Chessmans[6, i] = new Pawn(6, i, true, "WP");
            }
  
            //public static List<Chessman> CloneActiveChessman(List<Chessman> OldActiveChessman)
            //{
            //    List<Chessman> NewActive = new List<Chessman>();
            //    for (int cmindex = 0; cmindex < OldActiveChessman.Count; cmindex++)
            //        NewActive[cmindex] = OldActiveChessman[cmindex].clone();
            //    return NewActive;
            //}

            public static Chessman[,] CloneBoard(Chessman[,] oldBoard)
            {
                Chessman[,] NewBoard = new Chessman[8, 8];
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        if (oldBoard[row, col] == null) continue;
                        NewBoard[row, col] = oldBoard[row, col].clone();
                    }
                }
                return NewBoard;
            }
 
            public static Chessman[,] DoMove(Chessman[,] board, int oldRow, int oldCol, int row, int col)
            {
                Chessman[,] NewBoard = CloneBoard(board);
                Chessman cmToMove = NewBoard[oldRow, oldCol];
                NewBoard[oldRow, oldCol] = null;//מחיקת הכלי מהלוח במקום הזה כי הוא עומד לזוז
                NewBoard[row, col] = cmToMove;//הכנסת הכלי למיקום החדש בלוח
                                              //        cmToMove.transform.position = GetTileCenter(row, col);
                cmToMove.SetPosition(row, col);//שינוי המיקום הפיזי
                return NewBoard;
            }

            public static Move GetNextMove(bool i_isWhiteTurn, int i_depth, Chessman[,] i_board)
            {
                LogBoard(i_board, i_depth);
               //Console.WriteLine("Depth: {0}, IsWhite: {1}", i_depth, i_isWhiteTurn);
                string colorname = "Black";
                int king = 20000;
                if (i_isWhiteTurn)
                {
                    colorname = "White";
                }
                Move selectedMove = new Move();
                if (i_depth < 0)
                {
                    selectedMove.value = EvaluateBoard(i_board);
                    
                    selectedMove.newBoard = CloneBoard(i_board);
                   
                    return selectedMove;
                }
                bool firstvalue = true;
                
                for (int boardrow = 0; boardrow < 8; boardrow++)
                {
                    for (int boardcol = 0; boardcol < 8; boardcol++)
                    {
                        Chessman cm = i_board[boardrow, boardcol];
                        if (cm == null || cm.m_isWhite == !i_isWhiteTurn) continue;
                        //Console.WriteLine("Checking move for Piece {0} at {1},{2}", cm.displayName, boardrow, boardcol);
                        bool[,] pms = cm.PossibleMove(i_board);
                        //Console.WriteLine("possible moves:");
                        int pmsnum = 0;
                        for (int pmsrow = 0; pmsrow < 8; pmsrow++)
                        {
                            for (int pmscol = 0; pmscol < 8; pmscol++)
                            {
                                if (!pms[pmsrow, pmscol])
                                    continue;
                                pmsnum++;
                                int fromrow = cm.m_currentRow;
                                int fromcol = cm.m_currentCol;
                                if (fromrow != boardrow || fromcol != boardcol)
                                {
                                    throw new Exception();
                                }
                                 Chessman[,] NewBoard = DoMove(i_board, boardrow, boardcol, pmsrow, pmscol);
                                //if (EvaluateBoard(NewBoard) <= selectedMove.value - king)
                                //    continue;//אם המצב שם את המלך בסיכון
                                if(EvaluateBoard(NewBoard) >= selectedMove.value + king)
                                {
                                    selectedMove.value = EvaluateBoard(NewBoard);
                                    selectedMove.newBoard = NewBoard;
                                    selectedMove.toCol = pmscol;
                                    selectedMove.toRow = pmsrow;
                                    selectedMove.hasMoved = true;
                                    selectedMove.fromCol = fromcol;
                                    selectedMove.fromRow = fromrow;
                                    return selectedMove;
                                }
                                Move newMove = GetNextMove(!i_isWhiteTurn, i_depth - 1, NewBoard);
                                Console.Write("{9}{8} at depth {5} next board after possible move #{2} for {6} from {3},{4} to {0},{1} value is {7}                               ",
                                    pmsrow, pmscol, pmsnum, boardrow, boardcol, i_depth, cm.displayName, newMove.value,
                                    colorname, '\r');
                                //PrintBoard(newMove.newBoard, i_depth);
                                
                                if (firstvalue)
                                {
                                    //bestBoardValue = newMove.value;
                                    selectedMove.fromCol = fromcol;
                                    selectedMove.fromRow = fromrow;
                                    selectedMove.value = newMove.value;
                                    selectedMove.newBoard = NewBoard;
                                    selectedMove.toCol = pmscol;
                                    selectedMove.toRow = pmsrow;
                                    selectedMove.hasMoved = true;
                                    if (newMove.hasMoved && selectedMove.newBoard[pmsrow, pmscol] == null)
                                    {
                                        throw new IndexOutOfRangeException();
                                    }
                                    
                                    firstvalue = false;
                                    continue;
                                }
                                
                                if (!i_isWhiteTurn) // black = computer = max
                                {
                                    //}
                                    if (newMove.value > selectedMove.value)
                                    {
                                        selectedMove.value = newMove.value;
                                        selectedMove.newBoard = NewBoard;
                                        selectedMove.toCol = pmscol;
                                        selectedMove.toRow = pmsrow;
                                        selectedMove.hasMoved = true;
                                        selectedMove.fromCol = fromcol;
                                        selectedMove.fromRow = fromrow;
                                     
                                    }
                                }
                                else
                                { 
                                    if (newMove.value < selectedMove.value)
                                    {
                                        selectedMove.fromCol = fromcol;
                                        selectedMove.fromRow = fromrow;
                                        selectedMove.value = newMove.value;
                                        selectedMove.newBoard = NewBoard;
                                        selectedMove.toCol = pmscol;
                                        selectedMove.toRow = pmsrow;
                                        selectedMove.hasMoved = true;
                                    }
                                }
                            }
                        }
                    }
                }
               
                return selectedMove;
            }

            private static int EvaluateBoard(Chessman[,] chessmans)// חישוב לוח לפי הערכת כלים בלוח הקיים  כרגע
            {
                int count = 0;
                int pmcount = 0;
                int piecesTotalValue = 0;
                int extrapoints = 0;
                for (int i = 0; i < 8; i++)//עוברים על מערך של הכלים המשתתפים כעת בלוח
                {
                    for (int col = 0; col < 8; col++)
                    {
                        Chessman cm = chessmans[i, col];
                        if (cm == null) continue;
                        // חישוב מספר מהלכים אפשריים של השחור-מחשב
                        if (!cm.m_isWhite)
                        {
                            if (cm.GetType() == typeof(Pawn))
                                if (cm.m_currentRow == 6)
                                    extrapoints += 60;
                            if (cm.m_currentRow == 3 || cm.m_currentRow == 4)
                                extrapoints += 20;
                            bool[,] allowedMoves = cm.PossibleMove(chessmans);//מערך של צעדים אפשריים לכלי
                            for (int amrow = 0; amrow < 8; amrow++)
                            {
                                for (int amcol = 0; amcol < 8; amcol++)
                                {
                                    if (allowedMoves[amrow, amcol])
                                    {
                                        pmcount++;
                                        if (chessmans[amrow,amcol] != null)
                                            extrapoints += 70;
                                    }
                                }
                            }
                            count++;
                            piecesTotalValue += cm.value();
                        }
                        else
                        {
                            piecesTotalValue -= cm.value();
                            if (cm.GetType() == typeof(Pawn))
                                if (cm.m_currentRow == 1)
                                    extrapoints -= 60;
                            if (cm.m_currentRow == 3 || cm.m_currentRow == 4)
                                extrapoints -= 20;
                        }
                    }
                }
                int SumVal = piecesTotalValue + count + pmcount + extrapoints;
                return SumVal;
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                ChessGame game = new ChessGame();
                game.Start();

            }
        }
    }
}
