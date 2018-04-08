using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SignPost : MonoBehaviour
{	
	public void ResetScene() 
	{
        SceneManager.UnloadSceneAsync("A Maze");
        SceneManager.LoadScene("A Maze");
    }
}