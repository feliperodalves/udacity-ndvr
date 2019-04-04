using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{
	public GameObject player;
	public GameObject eventSystem;
	public GameObject startUI, restartUI, difficultUI;
	public GameObject startPoint, playPoint, restartPoint;

	public GameObject[] puzzleSpheres;
	public GameObject[] barris;
	private GameObject[] barrisOriginais;

	// How many times the orbs light up during the pattern display.
	private int puzzleLength;
	private int nBarris;
	private int erros;
	public GameObject sinalVerde;
	public GameObject sinalVermelho;
	public GameObject explosao;

	// How many seconds between the orbs light up during the pattern display.
	private float puzzleSpeed;

	// Variable for storing the order of the pattern display.
	private int[] puzzleOrder;

	// Variable for storing the index during the pattern display.
	private int currentDisplayIndex = 0;

	// Variable for storing the index the player is trying to solve.
	private int currentSolveIndex = 0;

	/* Uncomment the line below during 'A Little More Feedback!' lesson.*/
	public GameObject failAudioHolder;

	void Start()
	{
		// Update 'player' to be the camera's parent gameobject, i.e. 'GvrEditorEmulator' instead of the camera itself.
		// Required because GVR resets camera position to 0, 0, 0.
		player = player.transform.parent.gameObject;

		// Move the 'player' to the 'startPoint' position.
		player.transform.position = startPoint.transform.position;

		barrisOriginais = barris;
		nBarris = barris.Length;
		// Set the size of our array to the declared puzzle length.
//		puzzleOrder = new int[puzzleLength];

		// Create a random puzzle sequence.
//		GeneratePuzzleSequence();
	}

	// Create a random puzzle sequence.
	public void GeneratePuzzleSequence()
	{
		// Variable for storing a random number.
		int randomInt;
		// Loop as many times as the puzzle length.
		for (int i = 0; i < puzzleLength; i++)
		{
			// Generate a random number.
			randomInt = Random.Range(0, puzzleSpheres.Length);
			// Set the current index to the randomly generated number.
			puzzleOrder[i] = randomInt;
		}
	}

	// Begin the puzzle sequence.
	public void StartPuzzle()
	{
		// Disable the start UI.
		startUI.SetActive(false);
		// Move the player to the play position.
		erros = 0;
		iTween.MoveTo(player,
			iTween.Hash(
				"position", playPoint.transform.position,
				"time", 2,
				"easetype", "linear"
			)
		);
		// Disable the start UI.
		difficultUI.SetActive(true);
	}

	// Reset the puzzle sequence.
	public void ResetPuzzle()
	{
		// Enable the start UI.
		startUI.SetActive(true);
		// Disable the restart UI.
		restartUI.SetActive(false);
		// Move the player to the start position.
		player.transform.position = startPoint.transform.position;
		// Create a random puzzle sequence.
//		GeneratePuzzleSequence();
		barris = barrisOriginais;
		for (int i = 0; i < nBarris; i++) {
			barris[i].SetActive(true);
		}
	}

	// Disaplay the
	// Called from StartPuzzle() and invoked repeatingly.
	void DisplayPattern()
	{
		// If we haven't reached the end of the display pattern.
		if (currentDisplayIndex < puzzleOrder.Length)
		{
			Debug.Log("Display index " + (currentDisplayIndex+1) + ": Orb index " + puzzleOrder[currentDisplayIndex]);
			// Disable gaze input while displaying the pattern (prevents player from interacting with the orbs).
			eventSystem.SetActive(false);
			// Light up the orb at the current index.
			puzzleSpheres[puzzleOrder[currentDisplayIndex]].GetComponent<LightUp>().PatternLightUp(puzzleSpeed);
			// Move one to the next orb.
			currentDisplayIndex++;
		}
		// If we have reached the end of the display pattern.
		else
		{
			Debug.Log("End of puzzle display");
			// Renable gaze input when finished displaying the pattern (allows player to interacte with the orbs).
			eventSystem.SetActive(true);
			// Reset the index tracking the orb being lit up.
			currentDisplayIndex = 0;
			// Stop the pattern display.
			CancelInvoke();
		}
	}

	// Identify the index of the sphere the player selected.
	// Called from LightUp.PlayerSelection() method (see LightUp.cs script).
	public void PlayerSelection(GameObject sphere)
	{
		// Variable for storing the selected index.
		int selectedIndex = 0;
		// Loop throught the array to find the index of the selected sphere.
		for (int i = 0; i < puzzleSpheres.Length; i++)
		{
			// If the passed in sphere is the sphere at the index being checked.
			if (puzzleSpheres[i] == sphere)
			{
				Debug.Log("Looks like we hit sphere: " + i);
				// Update the index of the passed in sphere to be the same as the index being checked.
				selectedIndex = i;
			}
		}
		// Check if the sphere the player selected is correct.
		SolutionCheck(selectedIndex);
	}

	// Check if the sphere the player selected is correct.
	public void SolutionCheck(int playerSelectionIndex)
	{
		// If the sphere the player selected is the correct sphere.
		if (playerSelectionIndex == puzzleOrder[currentSolveIndex])
		{
			Debug.Log("Correct!  You've solved " + (currentSolveIndex+1) + " out of " + puzzleLength);
			// Update the tracker to check the next sphere.
			currentSolveIndex++;
			// If this was the last sphere in the pattern display...
			if (currentSolveIndex >= puzzleLength)
			{
				PuzzleSuccess();
			}
		}
		// If the sphere the player selected is the incorrect sphere.
		else
		{
			PuzzleFailure();
		}
	}

	// Do this when the player solves the puzzle.
	public void PuzzleTotalSuccess()
	{
		// Move the player to the restart position.
		iTween.MoveTo(player,
			iTween.Hash(
				"position", restartPoint.transform.position,
				"time", 2,
				"easetype", "linear"
			)
		);
		// Enable the restart UI.
		restartUI.SetActive(true);
		GvrAudioSource soundSuccess = restartUI.GetComponent<GvrAudioSource> ();
		soundSuccess.Play ();
	}

	// Do this when the player selects the wrong sphere.
	public void PuzzleFailure()
	{
		Debug.Log("You failed, resetting puzzle");
		// Get the GVR audio source component on the failAudioHolder and play the audio.
		/* Uncomment the line below during 'A Little More Feedback!' lesson.*/
		failAudioHolder.GetComponent<GvrAudioSource>().Play();
		// Reset the index the player is trying to solving.
		currentSolveIndex = 0;
		erros++;
		ParticleSystem vermelho = sinalVermelho.GetComponent<ParticleSystem> ();
		vermelho.Play ();
		// Begin the puzzle sequence.
		if (erros < 5) {
			StartCurrentPuzzle ();
		} else {
			StartPuzzle ();
		}
	}

//Criações Felipe
//Escolher a Dificuldade do Jogo
	public void difficultEasy()
	{
		puzzleSpeed = 1f;
		difficultUI.SetActive(false);
		StartCurrentPuzzle();
	}

	public void difficultHard()
	{
		puzzleSpeed = 0.5f;
		difficultUI.SetActive(false);
		StartCurrentPuzzle();
	}

	public void difficultExtraHard()
	{
		puzzleSpeed = 0.2f;
		difficultUI.SetActive(false);
		StartCurrentPuzzle();
	}
//Iniciar o Puzzle utilizando a quantidade de Barris Faltantes!
	public void StartCurrentPuzzle()
	{
		puzzleLength = nBarris - barris.Length + 1;
		puzzleOrder = new int[puzzleLength];
		GeneratePuzzleSequence();
		Debug.Log ("Tamanho do puzzle = " + puzzleLength);
		// Call the DisplayPattern() function repeatedly.
		CancelInvoke("DisplayPattern");
		InvokeRepeating("DisplayPattern", 3, puzzleSpeed);

		// Reset the index the player is trying to solving.
		currentSolveIndex = 0;
	}

	public void PuzzleSuccess()
	{
		ParticleSystem verde = sinalVerde.GetComponent<ParticleSystem> ();
		ParticleSystem explo = explosao.GetComponent<ParticleSystem> ();
		verde.Play();
		explosao.transform.position = barris [barris.Length-1].transform.position;
		explo.Play();
		GvrAudioSource soundExplosao = explosao.GetComponent<GvrAudioSource> ();
		soundExplosao.Play ();

		barris [barris.Length-1].SetActive(false);
		List<GameObject> list = new List<GameObject> (barris);
		list.Remove (barris [barris.Length-1]);
		barris = list.ToArray ();
		if (barris.Length >= 1) {
			StartCurrentPuzzle ();
		} else {
			PuzzleTotalSuccess ();
		}
	}

}


