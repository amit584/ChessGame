using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class BoardManager : MonoBehaviour
{
	public static BoardManager Instance{set;get;}
	private bool[,] allowedMoves{set;get;}

	public Chessman[,] Chessmans{ set; get;}//מערך של הכלים
	private Chessman selectedChessman;//הכלי הנבחר

	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;

	private int selectionX = -1;
	private int selectionY = -1;

	public List<GameObject> chessmanPrefabs;//רשימת הכלים בתור אובייקטים
	private List<GameObject> activeChessman;

    private Material previousMat;
	public Material selectedMat;
    
	public int[] EnPassantMove{ set; get;}

	private Quaternion orientation = Quaternion.Euler(0,180,0);

	public bool isWhiteTurn = true;//סימון תור השחקן
    public bool isHatzrahaAllowedLeft = false; //  האם חוקי להצריח
    public bool isHatzrahaAllowedRight = false;
    public bool isHatzrahaSelectedLeft = false; // האם השחקן בחר להצריח בתור זה
    public bool isHatzrahaSelectedRight = false; // האם השחקן בחר להצריח בתור זה
    public bool GameOver = false;
    string win = "";
    private void Start()//הלוח במצב ראשוני
	{
		Instance = this;
		SpawnAllChessmans ();
        Debug.Log("Your Turn ...");
    }

	private void Update()//הדכון הלוח בכל שינוי
	{
        if(GameOver)
        {
            if(win=="white")
                SceneManager.LoadScene(3);
            else
                SceneManager.LoadScene(4);
        }
        if (!isWhiteTurn)
        {
            Debug.Log("Computer says ...");
            ChessEngine.Chessman[,] switchboard = SwitchBoard(Chessmans);//העברה ללוח לפי המנוע
            ChessEngine.ChessEngine.ChessGame.LogBoard(switchboard, 101);
            ChessEngine.ChessEngine.ChessGame.Move SelectedMove = 
               ChessEngine.ChessEngine.ChessGame.GetNextMove(false, 2 , switchboard);//קריאה לפונקציי המחפשת צעד למחשב
            //שמירת הצעד שנעשה
            int ToX = SelectedMove.toCol;
            int ToY = 7 - SelectedMove.toRow;
            int FromY = 7 - SelectedMove.fromRow;
            int FromX = SelectedMove.fromCol;
            Debug.LogFormat("{1},{2} to {3},{4} ![{5},{6} to {7},{8}]", 0,
                FromY, FromX, ToY, ToX,
                SelectedMove.fromRow,SelectedMove.fromCol,SelectedMove.toRow,SelectedMove.toCol);
            selectedChessman = Chessmans[FromX, FromY];//בחירת הכלי שנבחר להזזה על מנת להזיז אותו פיזית 
            Chessman c = Chessmans[ToX, ToY];
            if (c != null && c.isWhite != isWhiteTurn)//אם קיים כלי במקום שאנחנו רוצים לזוז אליו והכלי הוא אויב
            {
                //אכילת כלי

                //אם הכלי שנאכל הוא המלך
                if (c.GetType() == typeof(King))
                {
                    EndGame();//נגמר המשחק
                    return;
                }
                //הכלי נעלם כשאוכלים אותו
                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }
            selectedChessman.transform.position = GetTileCenter(ToX, ToY);
            selectedChessman.SetPosition(ToX, ToY);//הכנסת הכלי למיקום החדש בלוח
            Chessmans[ToX, ToY] = selectedChessman;
            Chessmans[FromX, FromY] = null;//מחיקת הכלי מהלוח במקום הזה כי הוא עומד לזוז
            isWhiteTurn = !isWhiteTurn;//תור הבא
            Debug.LogFormat("{0} from {1},{2} to {3},{4} !", selectedChessman.name, FromY, FromX, ToY, ToX);
            Debug.Log("Your Turn ...");
            if(DangerKing(Chessmans, !isWhiteTurn))
                Debug.Log("check...");
            selectedChessman = null;
           
        }
        else
        {
            if (isHatzrahaSelectedLeft)
            {
                // השחקן בחר הצרחה - סוף תור
                isWhiteTurn = false;
                selectedChessman = null;
                isHatzrahaSelectedLeft = false;
                isHatzrahaAllowedLeft = false;
                return;
            }
            if (isHatzrahaSelectedRight)
            {
                // השחקן בחר הצרחה - סוף תור
                isWhiteTurn = false;
                selectedChessman = null;
                isHatzrahaSelectedRight = false;
                isHatzrahaAllowedRight = false;
                return;
            }
            UpdateSelection();
            DrawChessboard();

            if (Input.GetMouseButtonDown(0))//בלחיצת עכבר ימין
            {
                if (selectionX >= 0 && selectionY >= 0)//הלחיצה בתוך הלוח
                {
                    if (selectedChessman == null || !selectedChessman.isWhite)
                        // בחירת כלי
                        SelectChessman(selectionX, selectionY);
                    
                    else
                    {
                        MoveChessman(selectionX, selectionY);
                        if (DangerKing(Chessmans, isWhiteTurn))
                            Debug.Log("check...");

                    }
                }
            }
        }
    }

    public ChessEngine.Chessman[,] SwitchBoard(Chessman[,] OldBoard)//העברה ללוח לפי המנוע
    {
        ChessEngine.Chessman[,] NewBoard = new ChessEngine.Chessman[8, 8];
        for(int y = 0; y<8; y++)
        {
            for(int x = 0; x<8;x++)
            {
                Chessman cm = OldBoard[x, y];
                int row = 7 - y;
                int col = x;
                if (cm == null)
                    continue;
                if(cm.GetType() == typeof(Pawn))
                {
                    if (cm.isWhite)
                        NewBoard[row, col] = new ChessEngine.Pawn(row, col, true, "WP");
                    else
                        NewBoard[row, col] = new ChessEngine.Pawn(row, col, false, "BP");
                }
                if (cm.GetType() == typeof(Rook))
                {
                    if (cm.isWhite)
                        NewBoard[row, col] = new ChessEngine.Rook(row, col, true, "WR");
                    else
                        NewBoard[row, col] = new ChessEngine.Rook(row, col, false, "BR");
                }
                if (cm.GetType() == typeof(King))
                {
                    if (cm.isWhite)
                        NewBoard[row, col] = new ChessEngine.King(row, col, true, "WK");
                    else
                        NewBoard[row, col] = new ChessEngine.King(row, col, false, "BK");
                }
                if (cm.GetType() == typeof(Queen))
                {
                    if (cm.isWhite)
                        NewBoard[row, col] = new ChessEngine.Queen(row, col, true, "WQ");
                    else
                        NewBoard[row, col] = new ChessEngine.Queen(row, col, false, "BQ");
                }
                if (cm.GetType() == typeof(Knight))
                {
                    if (cm.isWhite)
                        NewBoard[row, col] = new ChessEngine.Knight(row, col, true, "WN");
                    else
                        NewBoard[row, col] = new ChessEngine.Knight(row, col, false, "BN");
                }
                if (cm.GetType() == typeof(Bishop))
                {
                    if (cm.isWhite)
                        NewBoard[row, col] = new ChessEngine.Bishop(row, col, true, "WB");
                    else
                        NewBoard[row, col] = new ChessEngine.Bishop(row, col, false, "BB");
                }
            }
        }
        return NewBoard;
    }

	private void SelectChessman(int x,int y)//בחירת כלי
	{
		if (Chessmans [x, y] == null)//אם לא קיים כלי במקום שנבחר
			return;

		if (Chessmans [x, y].isWhite != isWhiteTurn)//האם הכל לבן בתור של הלבן או הפוך
			return;

		bool hasAtleastOneMove = false;
		allowedMoves = Chessmans [x, y].PossibleMove (); //(אם לא אין צורך לחשב אפשרויות הזזה) אם קיימת אפשרות הזזה לכלי
		for (int i = 0; i < 8; i++)
			for (int j = 0; j < 8; j++)
				if (allowedMoves [i, j])
					hasAtleastOneMove = true;

		if (!hasAtleastOneMove)//אין אפשרות הזזה
			return;

		selectedChessman = Chessmans [x, y]; //בחירת הכלי
		previousMat = selectedChessman.GetComponent<MeshRenderer> ().material;
		selectedMat.mainTexture = previousMat.mainTexture;
		selectedChessman.GetComponent<MeshRenderer> ().material = selectedMat;
		BoardHighlights.Instance.HighlightAllowedMoves (allowedMoves);//עדכון סימון האריחים לפי שינוי בחירת הכלי
	}

	private void MoveChessman(int x,int y)//הזזת כלי
	{
		if (allowedMoves[x,y]) //רק אם התזוזה אפשרית
		{
			Chessman c = Chessmans [x, y];

			if (c != null && c.isWhite != isWhiteTurn)//אם קיים כלי במקום שאנחנו רוצים לזוז אליו והכלי הוא אויב
			{
				//אכילת כלי

				//אם הכלי שנאכל הוא המלך
				if (c.GetType () == typeof(King))
				{
					EndGame ();//נגמר המשחק
					return;
				}
                //הכלי נעלם כשאוכלים אותו
				activeChessman.Remove(c.gameObject);
				Destroy (c.gameObject);
			}
            
            if (selectedChessman.GetType () == typeof(Pawn))//אם אנחנו פיון
			{
				if (y == 7)//שורה אחרונה בתור לבן
				{
					activeChessman.Remove(selectedChessman.gameObject);//הריסת הפיון 
					Destroy (selectedChessman.gameObject);
					SpawnChessman (1, x, y);//החלפה במלכה
					selectedChessman = Chessmans [x, y];
				}
				else if (y == 0)//שורה אחרונה בתור שחור
				{
					activeChessman.Remove(selectedChessman.gameObject);//הריסת הפיון
					Destroy (selectedChessman.gameObject);
					SpawnChessman (7, x, y);//החלפה במלכה
					selectedChessman = Chessmans [x, y];
				}

			}
            //הזזת כלי
			Chessmans [selectedChessman.CurrentX, selectedChessman.CurrentY] = null;//מחיקת הכלי מהלוח במקום הזה כי הוא עומד לזוז
			selectedChessman.transform.position = GetTileCenter (x, y);
			selectedChessman.SetPosition (x, y);//הכנסת הכלי למיקום החדש בלוח
			Chessmans [x, y] = selectedChessman;
			isWhiteTurn = !isWhiteTurn;//תור הבא
		}

		selectedChessman.GetComponent<MeshRenderer> ().material = previousMat;
		BoardHighlights.Instance.Hidehighlights ();//הסתרת אריח מסומן אחרי ביטול סימון או הזזה
		selectedChessman = null;//ביטול בחירת הכלי אחרי הזזה
	}
    //בדיקות לצרחה
    //אם צריח זז
    public bool checkLeftRookHasMoved()
    {
        bool LeftRookHasMoved = false;
        if (Chessmans[0, 0] == null || Chessmans[0, 0].GetType() != typeof(Rook))
            LeftRookHasMoved = true;
        return LeftRookHasMoved;
    }
    public bool checkRightRookHasMoved()
    {
        bool RightRookHasMoved = false;
        if (Chessmans[7, 0] == null || Chessmans[7, 0].GetType() != typeof(Rook))
            RightRookHasMoved = true;
        return RightRookHasMoved;
    }
    //אם מלך זז
    public bool checkKingHasMoved()
    {
        bool KingHasMoved = false;
        if (Chessmans[4, 0] == null || Chessmans[4, 0].GetType() != typeof(King))
            KingHasMoved = true;
        return KingHasMoved;
    }
    //אם המקומות בין הצריח למלך ריקים
    public bool checknullsLeft()
    {
        bool freetogoLeft = false;
        if (Chessmans[1, 0] == null && Chessmans[2, 0] == null && Chessmans[3, 0] == null)
            freetogoLeft = true;
        return freetogoLeft;
    }
    public bool checknullsRight()
    {
        bool freetogoRight = false;
        if (Chessmans[5, 0] == null && Chessmans[6, 0] == null)
            freetogoRight = true;
        return freetogoRight;
    }
    //האם הצרחה חוקית
    public bool checkHatzrahaAllowedLeft()
    {
        if(!Instance.checkLeftRookHasMoved() && !Instance.checkKingHasMoved() && Instance.checknullsLeft())
            isHatzrahaAllowedLeft = true;
        return isHatzrahaAllowedLeft;
    }
    public bool checkHatzrahaAllowedRight()
    {
        if (!Instance.checkRightRookHasMoved() && !Instance.checkKingHasMoved() && Instance.checknullsRight())
            isHatzrahaAllowedRight = true;
        return isHatzrahaAllowedRight;
    }
    //בלחיצת כפתור הצרחה רק אם המהלך חוקי
    public void HatzrahaLeftClick()
    {
        if(Instance.checkHatzrahaAllowedLeft())
            Instance.Hatzraahaleft();
    }
    public  void Hatzraahaleft()
    {
        Chessman kingChessman =Chessmans[4, 0];
        Chessman RookChessman = Chessmans[0, 0];
        kingChessman.transform.position = GetTileCenter(2, 0);
        kingChessman.SetPosition(2, 0);//הכנסת הכלי למיקום החדש בלוח
        Chessmans[2, 0] = kingChessman;
        Chessmans[4, 0] = null;
        RookChessman.transform.position = GetTileCenter(3, 0);
        RookChessman.SetPosition(3, 0);//הכנסת הכלי למיקום החדש בלוח
        Chessmans[3, 0] = RookChessman;
        Chessmans[0, 0] = null;
        isHatzrahaSelectedLeft = true; //    update סמן ל
    }
    public void HatzraharightClick()
    {
        if (Instance.checkHatzrahaAllowedRight())
            Instance.HatzraahaRight();
    }
    public void HatzraahaRight()
    {
        Chessman kingChessman = Chessmans[4, 0];
        Chessman RookChessman = Chessmans[7, 0];
        kingChessman.transform.position = GetTileCenter(6, 0);
        kingChessman.SetPosition(6, 0);//הכנסת הכלי למיקום החדש בלוח
        Chessmans[6, 0] = kingChessman;
        Chessmans[7, 0] = null;
        RookChessman.transform.position = GetTileCenter(5, 0);
        RookChessman.SetPosition(5, 0);//הכנסת הכלי למיקום החדש בלוח
        Chessmans[5, 0] = RookChessman;
        Chessmans[7, 0] = null;
        isHatzrahaSelectedRight = true; //    update סמן ל
    }
    public static bool DangerKing(Chessman[,] board, bool iswhite)
    {
       for( int r=0; r<8;r++)
        {
            for(int c = 0; c<8;c++)
            {
                Chessman cm = board[r, c];
                if (cm == null) continue;
                bool[,] possible = cm.PossibleMove();
                for (int pmr = 0; pmr<8;pmr++)
                {
                    for(int pmc = 0; pmc < 8; pmc++)
                    {
                        if(possible[pmr,pmc] && board[pmr, pmc]!= null)
                        {
                            if (board[pmr, pmc].GetType() == typeof(King) && board[pmr, pmc].isWhite != iswhite)
                                return true;
                        }
                    }
                }
            }
        }
        return false;

    }

	private void UpdateSelection()//עדכון הבחירה
	{
		if (!Camera.main)
			return;

		RaycastHit hit;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 25.0f, LayerMask.GetMask ("ChessPlane"))) 
		{
			selectionX = (int)hit.point.x;
			selectionY = (int)hit.point.z;
		}
		else
		{
			selectionX = -1;
			selectionY = -1;
		}
	}

	private void SpawnChessman(int index,int row,int col)//יצירת מיקום להניח את הכלים
	{
		GameObject go = Instantiate (chessmanPrefabs [index], GetTileCenter(row,col), orientation) as GameObject;
		go.transform.SetParent (transform);
		Chessmans [row, col] = go.GetComponent<Chessman> ();//לקיחת החלקי הצ'סמני מהאוייבקט
		Chessmans [row, col].SetPosition (row, col);
		activeChessman.Add (go);//הוספה לכלים המשתתפים כעת
	}

	private void SpawnAllChessmans()//מיקום כל הכלים
	{
		activeChessman = new List<GameObject> ();
		Chessmans = new Chessman[8, 8];
		EnPassantMove = new int[2]{-1,-1};

		// לבן

		//מלך
		SpawnChessman (0,4,0);

		//מלכה
		SpawnChessman (1,3,0);

		//צריח
		SpawnChessman (2,0,0);
		SpawnChessman (2,7,0);

		//רץ
		SpawnChessman (3,2,0);
		SpawnChessman (3,5,0);

		//פרש
		SpawnChessman (4,1,0);
		SpawnChessman (4,6,0);

		//פיון
		for (int i = 0; i < 8; i++)
			SpawnChessman (5,i,1);

		// שחור

		//מלך
		SpawnChessman (6,4,7);

		//מלכה
		SpawnChessman (7,3,7);

		//רץ
		SpawnChessman (8,0,7);
		SpawnChessman (8,7,7);

		//צריח
		SpawnChessman (9,2,7);
		SpawnChessman (9,5,7);

		//פרש
		SpawnChessman (10,1,7);
		SpawnChessman (10,6,7);

		//פיון
		for (int i = 0; i < 8; i++)
			SpawnChessman (11,i,6);
	}

	private Vector3 GetTileCenter(int x,int y)//אמצע אריח
	{
		Vector3 origin = Vector3.zero;
		origin.x += (TILE_SIZE * x) + TILE_OFFSET;
		origin.z += (TILE_SIZE * y) + TILE_OFFSET;
		return origin;
	}

	private void DrawChessboard()//ציור הלוח
	{
		Vector3 widthLine = Vector3.right * 8;
		Vector3 heigthLine = Vector3.forward * 8;

		for (int i = 0; i <= 8; i++) 
		{
			Vector3 start = Vector3.forward * i;
			Debug.DrawLine (start, start + widthLine);
			for (int j = 0; j <= 8; j++) 
			{
				start = Vector3.right * j;
				Debug.DrawLine (start, start + heigthLine);
			}
		}

		//סימון האריח שנבחר
		if (selectionX >= 0 && selectionY >= 0)
		{	
			Debug.DrawLine (
				Vector3.forward * selectionY + Vector3.right * selectionX,
				Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

			Debug.DrawLine (
				Vector3.forward * (selectionY + 1 )+ Vector3.right * selectionX,
				Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
		}
	}

	private void EndGame()//סיום המשחק
	{
        if (isWhiteTurn)//תור הלבן
        {
            Debug.Log("White team wins");
            win =" white";
        }
        else//תור השחור
        {
            Debug.Log("Black team wins");
            win = "black";
        }
		foreach (GameObject go in activeChessman)
			Destroy (go);//מחחיקת הלוח
        GameOver = true;
		
	}
}