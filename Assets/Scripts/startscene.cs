using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class startscene : MonoBehaviour
{

    public void loadbyindex(int sceneindex)
    {
        SceneManager.LoadScene(sceneindex);
    }

}
