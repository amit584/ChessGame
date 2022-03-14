using UnityEngine;
using System.Collections;

public class King : Chessman
{
	public override bool[,] PossibleMove ()
	{
		bool[,] r = new bool[8, 8];

		Chessman c;
		int i, j;

		// למעלה
		i = CurrentX - 1;
		j = CurrentY + 1;
		if (CurrentY != 7) //אפשר להמשיך קדימה
		{
			for (int k = 0; k < 3; k++) 
			{
				if (i >= 0 || i < 8)
				{
					c = BoardManager.Instance.Chessmans [i, j];
					if (c == null)
						r [i, j] = true;
					else if (isWhite != c.isWhite)
						r [i, j] = true;
				}

				i++;
			}
		}

		// למטה
		i = CurrentX - 1;
		j = CurrentY - 1;
		if (CurrentY != 0) 
		{
			for (int k = 0; k < 3; k++) 
			{
				if (i >= 0 || i < 8)
				{
					c = BoardManager.Instance.Chessmans [i, j];
					if (c == null)
						r [i, j] = true;
					else if (isWhite != c.isWhite)
						r [i, j] = true;
				}

				i++;
			}
		}

		// שמאל
		if (CurrentX != 0) 
		{
			c = BoardManager.Instance.Chessmans [CurrentX - 1, CurrentY];
			if (c == null)
				r [CurrentX - 1, CurrentY] = true;
			else if (isWhite != c.isWhite)
				r [CurrentX - 1, CurrentY] = true;
		}

		// ימין
		if (CurrentX != 7) 
		{
			c = BoardManager.Instance.Chessmans [CurrentX + 1, CurrentY];
			if (c == null)
				r [CurrentX + 1, CurrentY] = true;
			else if (isWhite != c.isWhite)
				r [CurrentX + 1, CurrentY] = true;
		}
			
		return r;
	}

}
