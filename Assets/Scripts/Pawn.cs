using UnityEngine;
using System.Collections;
using System;

public class Pawn : Chessman
{
	public override bool[,] PossibleMove ()
	{
		bool[,] r = new bool[8, 8];
		Chessman c, c2;

		// תור הלבן
		if (isWhite) 
		{
			//אלכסון שמאל
			if (CurrentX != 0 && CurrentY != 7) //לא במשבצת השמאלית ביותר ולא בשורה האחרונה
			{
              
                c = BoardManager.Instance.Chessmans [CurrentX - 1, CurrentY + 1];
				if (c != null && !c.isWhite)//אפשר לזוז אלכסון רק אם קיים שם כלי של יריב
					r [CurrentX - 1, CurrentY + 1] = true;//הצעד חוקי
			}

			//אלכסון ימין
			if (CurrentX != 7 && CurrentY != 7) //לא במשבצת ימנית סיותר ולא בשורה האחרונה
			{
                
                c = BoardManager.Instance.Chessmans [CurrentX + 1, CurrentY + 1];
				if (c != null && !c.isWhite)//אפשר לזוז אלכסון רק אם קיים שם כלי של יריב
                    r [CurrentX +1, CurrentY + 1] = true;//הצעד חוקי
			}

			//קדימה
			if (CurrentY != 7) //לא בשורה האחרונה
			{
				c = BoardManager.Instance.Chessmans [CurrentX, CurrentY + 1];
				if (c == null)//אם לא קיים כלי מולנו
					r [CurrentX, CurrentY + 1] = true;  //צעד חוקי
			}

			//קדימה בתור הראשון
			if (CurrentY == 1) //(במשבצת ההתחלתית שלנו ( בשורה הראשונה
			{
				c = BoardManager.Instance.Chessmans [CurrentX, CurrentY + 1];
				c2 = BoardManager.Instance.Chessmans [CurrentX, CurrentY + 2];
				if (c == null & c2 == null)//שתי משבצות קדימה פנויות
					r [CurrentX, CurrentY + 2] = true;//צעד חוקי
			}
		} 
        //תור השחור
		else 
		{
			//אלכסון שמאל
			if (CurrentX != 0 && CurrentY != 0) 
			{
               
                c = BoardManager.Instance.Chessmans [CurrentX - 1, CurrentY -1];
				if (c != null && c.isWhite)
					r [CurrentX - 1, CurrentY - 1] = true;
			}

			//אלכסון ימין
			if (CurrentX != 7 && CurrentY != 0) 
			{
             
                c = BoardManager.Instance.Chessmans [CurrentX + 1, CurrentY - 1];
				if (c != null && c.isWhite)
					r [CurrentX +1, CurrentY - 1] = true;
			}

			//קדימה
			if (CurrentY != 0) 
			{
				c = BoardManager.Instance.Chessmans [CurrentX, CurrentY - 1];
				if (c == null)
					r [CurrentX, CurrentY - 1] = true;
			}

			//קדימה בתור הראשון
			if (CurrentY == 6) 
			{
				c = BoardManager.Instance.Chessmans [CurrentX, CurrentY - 1];
				c2 = BoardManager.Instance.Chessmans [CurrentX, CurrentY - 2];
				if (c == null & c2 == null)
					r [CurrentX, CurrentY - 2] = true;
			}
		}

		return r;//החזרת מערך בוליאני של המקומות האפשריים
	}

}
