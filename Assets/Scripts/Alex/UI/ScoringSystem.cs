using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ScoringSystem : MonoBehaviour
{
    public GameObject scoreText;
    public static int theScore;
    public GameObject objectiveCompletedText;
    public GameObject ringPhone;

    void Update()
    {
        scoreText.GetComponent<Text>().text = "Turn ground floor lights on: " + theScore + "/7";

        if (Input.GetKeyDown("="))
        {
            theScore = 7;
        }

        if (theScore >= 7)
        {
            objectiveCompletedText.SetActive(true);
            scoreText.SetActive(false);
            ringPhone.SetActive(true);
        }
    }
}