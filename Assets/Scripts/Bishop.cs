using UnityEngine;
using System.Collections;

public class Bishop : Chessman
{
	public override bool[,] PossibleMove ()
	{
		bool[,] r = new bool[8, 8];

		Chessman c;
		int i, j;

		// אלכסון שמאל קדימה
		i = CurrentX;
		j = CurrentY;
		while (true)
		{
			i--;
			j++;
			if (i < 0 || j >= 8)// בלוח
				break;

			c = BoardManager.Instance.Chessmans [i, j];
			if (c == null)//אין כלי שחוסם
				r [i, j] = true;//צעד חוקי
			else 
			{
				if (isWhite != c.isWhite)//הכלי אויב
					r [i, j] = true;//הצעד חוקי

				break;
			}
		}

		//אלכסון ימין קדימה
		i = CurrentX;
		j = CurrentY;
		while (true)
		{
			i++;
			j++;
			if (i >= 8 || j >= 8)
				break;

			c = BoardManager.Instance.Chessmans [i, j];
			if (c == null)
				r [i, j] = true;
			else 
			{
				if (isWhite != c.isWhite)
					r [i, j] = true;

				break;
			}
		}
			
		//אלכסון שמאל אחורה
		i = CurrentX;
		j = CurrentY;
		while (true)
		{
			i--;
			j--;
			if (i < 0 || j < 0)
				break;

			c = BoardManager.Instance.Chessmans [i, j];
			if (c == null)
				r [i, j] = true;
			else 
			{
				if (isWhite != c.isWhite)
					r [i, j] = true;

				break;
			}
		}

		//אלכסון ימין אחורה
		i = CurrentX;
		j = CurrentY;
		while (true)
		{
			i++;
			j--;
			if (i >= 8 || j < 0)
				break;

			c = BoardManager.Instance.Chessmans [i, j];
			if (c == null)
				r [i, j] = true;
			else 
			{
				if (isWhite != c.isWhite)
					r [i, j] = true;

				break;
			}
		}

		return r;
	}

}
