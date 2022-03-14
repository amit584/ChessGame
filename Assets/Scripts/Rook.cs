using UnityEngine;
using System.Collections;

public class Rook : Chessman
{
	public override bool[,] PossibleMove ()
	{
		bool[,] r = new bool[8, 8];

		Chessman c;
		int i;

		// ימינה
		i = CurrentX;
		while (true)
		{
			i++;
			if (i >= 8)//עברנו את הלוח
				break;

			c = BoardManager.Instance.Chessmans [i, CurrentY];//הכלי שלידנו
			if (c == null)//אין כלי מיממין
				r [i, CurrentY] = true;//הצעד חוקי
			else//יש כלי מימין
			{
				if (c.isWhite != isWhite)//הכלי אויב
					r [i, CurrentY] = true;//הצעד חוקי

				break;
			}
		}

		// שמאל
		i = CurrentX;
		while (true)
		{
			i--;
			if (i < 0)
				break;

			c = BoardManager.Instance.Chessmans [i, CurrentY];
			if (c == null)
				r [i, CurrentY] = true;
			else
			{
				if (c.isWhite != isWhite)
					r [i, CurrentY] = true;

				break;
			}
		}

		// קדימה
		i = CurrentY;
		while (true)
		{
			i++;
			if (i >= 8)
				break;

			c = BoardManager.Instance.Chessmans [CurrentX, i];
			if (c == null)
				r [CurrentX, i] = true;
			else
			{
				if (c.isWhite != isWhite)
					r [CurrentX, i] = true;

				break;
			}
		}

		// אחורה
		i = CurrentY;
		while (true)
		{
			i--;
			if (i < 0)
				break;

			c = BoardManager.Instance.Chessmans [CurrentX, i];
			if (c == null)
				r [CurrentX, i] = true;
			else
			{
				if (c.isWhite != isWhite)
					r [CurrentX, i] = true;

				break;
			}
		}

		return r;
	}

}
