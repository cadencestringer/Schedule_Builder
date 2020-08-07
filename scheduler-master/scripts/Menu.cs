using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
 //Checks and loads the specified scene
public class Menu : MonoBehaviour
{
    public void LoadByIndex(int sceneIndex)
    {         if (sceneIndex == 2 && Manager.m_numClasses < 5)
        {
            Debug.Log("Not enough classes.");
        }         else
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
     //Quits the program
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}
