using UnityEngine;
using System.Collections;

public class Queen : Chessman 
{
	public override bool[,] PossibleMove ()
	{
		bool[,] r = new bool[8, 8];

		Chessman c;
		int i,j;

		// ימין
		i = CurrentX;
		while (true)
		{
			i++;
			if (i >= 8)
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

		// למעלה
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

		// למטה
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

		// אלכסון שמאל קדימה
		i = CurrentX;
		j = CurrentY;
		while (true)
		{
			i--;
			j++;
			if (i < 0 || j >= 8)
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

		// אלכסון ימין קדימה
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

		// אלכסון שמאל אחורה
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

		//  אלכסון ימין אחורה
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
