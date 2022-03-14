using UnityEngine;
using System.Collections;

public class Knight : Chessman
{
	public override bool[,] PossibleMove ()
	{
		bool[,] r = new bool[8, 8];

		//קדימה שמאלה
		KnightMove(CurrentX - 1, CurrentY + 2,ref r);

		//קדימה ימינה
		KnightMove(CurrentX + 1, CurrentY + 2,ref r);

		//ימינה למעלה
		KnightMove(CurrentX + 2, CurrentY + 1,ref r);

		//ימינה למטה
		KnightMove(CurrentX + 2, CurrentY - 1,ref r);

		//אחורה שמאלה
		KnightMove(CurrentX - 1, CurrentY - 2,ref r);

		//אחורה ימינה
		KnightMove(CurrentX + 1, CurrentY - 2,ref r);

		//שמאלה למעלה
		KnightMove(CurrentX - 2, CurrentY + 1,ref r);

		//שמאלה למטה
		KnightMove(CurrentX - 2, CurrentY - 1,ref r);

		return r;
	}
    //public override Chessman clone()
    //{
    //    Chessman cm = new Knight();
    //    cm.SetPosition(CurrentX, CurrentY);
    //    cm.isWhite = isWhite;
    //    return cm;
    //}

    public void KnightMove(int x,int y,ref bool[,] r)
	{
		Chessman c;
		if(x >= 0 && x < 8 && y >= 0 && y < 8)
		{
			c = BoardManager.Instance.Chessmans [x, y];
			if (c == null)//אין כלי 
				r [x, y] = true;//צעד חוקי
			else if (isWhite != c.isWhite)//הכלי אויב   
				r [x, y] = true;//צעד חוקי
		}
	}
}
