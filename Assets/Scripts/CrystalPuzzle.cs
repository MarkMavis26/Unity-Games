using UnityEngine;
using TMPro;
using System;

public class CrystalPuzzle : MonoBehaviour
{
    public TextMeshProUGUI puzzleInstructionText;

    public AudioClip victorySFX;
    public AudioSource audioSource;
    public GameObject greenCrystal;
    public GameObject blueCrystal;
    public GameObject redCrystal;

    public GameObject greenBasinSpawnPoint;
    public GameObject blueBasinSpawnPoint;
    public GameObject redBasinSpawnPoint;

    bool isHoldingRed;
    bool isHoldingGreen;
    bool isHoldingBlue;

    bool isNearRed;
    bool isNearGreen;
    bool isNearBlue;

    bool isNearGreenBasin;
    bool isNearBlueBasin;
    bool isNearRedBasin;

    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //picking up crystals
            if (isNearRed)
            {
                DropCrystals();
                isHoldingRed = true;
                puzzleInstructionText.text = "Red Crystal";
            }
            else if (isNearGreen)
            {
                DropCrystals();
                isHoldingGreen = true;
                puzzleInstructionText.text = "Green Crystal";
            }
            else if (isNearBlue)
            {
                DropCrystals();
                isHoldingBlue = true;
                puzzleInstructionText.text = "Blue Crystal";
            }

            //placing crystals
            if (isNearRedBasin && isHoldingSomething())
            {
                RemoveCrystalFromBasin(redBasinSpawnPoint.transform);
                if (isHoldingBlue)
                {
                    Instantiate(blueCrystal, redBasinSpawnPoint.transform.position, Quaternion.identity);
                }
                else if (isHoldingGreen)
                {
                    Instantiate(greenCrystal, redBasinSpawnPoint.transform.position, Quaternion.identity);
                }
                else if (isHoldingRed)
                {
                    Instantiate(redCrystal, redBasinSpawnPoint.transform.position, Quaternion.identity);
                }

                DropCrystals();
                CheckIfCorrect();
            }

            if (isNearBlueBasin && isHoldingSomething())
            {
                RemoveCrystalFromBasin(blueBasinSpawnPoint.transform);
                if (isHoldingBlue)
                {
                    Instantiate(blueCrystal, blueBasinSpawnPoint.transform.position, Quaternion.identity);
                }
                else if (isHoldingGreen)
                {
                    Instantiate(greenCrystal, blueBasinSpawnPoint.transform.position, Quaternion.identity);
                }
                else if (isHoldingRed)
                {
                    Instantiate(redCrystal, blueBasinSpawnPoint.transform.position, Quaternion.identity);
                }
                DropCrystals();
                CheckIfCorrect();

            }

            if (isNearGreenBasin && isHoldingSomething())
            {
                RemoveCrystalFromBasin(greenBasinSpawnPoint.transform);
                if (isHoldingBlue)
                {
                    Instantiate(blueCrystal, greenBasinSpawnPoint.transform.position, Quaternion.identity);
                }
                else if (isHoldingGreen)
                {
                    Instantiate(greenCrystal, greenBasinSpawnPoint.transform.position, Quaternion.identity);
                }
                else if (isHoldingRed)
                {
                    Instantiate(redCrystal, greenBasinSpawnPoint.transform.position, Quaternion.identity);
                }
                DropCrystals();
                CheckIfCorrect();

            }
        }
    }

    private bool isHoldingSomething()
    {
        if (isHoldingBlue || isHoldingGreen || isHoldingRed)
        {
            return true;
        }
        return false;
    }

    private void CheckIfCorrect()
    {
        bool isRedCorrect = redBasinSpawnPoint.transform.childCount > 0 && redBasinSpawnPoint.transform.GetChild(0).CompareTag("RedCrystal");
        bool isGreenCorrect = greenBasinSpawnPoint.transform.childCount > 0 && greenBasinSpawnPoint.transform.GetChild(0).CompareTag("GreenCrystal");
        bool isBlueCorrect = blueBasinSpawnPoint.transform.childCount > 0 && blueBasinSpawnPoint.transform.GetChild(0).CompareTag("BlueCrystal");

        if (isGreenCorrect && isRedCorrect && isBlueCorrect)
        {
            Debug.Log("✅ Puzzle Complete — Playing Audio");

            audioSource.PlayOneShot(victorySFX);

        }
    }

    private void DropCrystals()
    {
        isHoldingGreen = false;
        isHoldingBlue = false;
        isHoldingRed = false;

        puzzleInstructionText.text = "";
    }

    private void RemoveCrystalFromBasin(Transform basin)
    {
        if (basin.childCount != 0)
        {
            Destroy(basin.GetChild(0).gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //crystals
        if (other.gameObject.CompareTag("GreenCrystal"))
        {
            isNearGreen = true;

        }
        else if (other.gameObject.CompareTag("BlueCrystal"))
        {
            isNearBlue = true;
        }
        else if (other.gameObject.CompareTag("RedCrystal"))
        {
            isNearRed = true;
        }

        //basins
        if (other.gameObject.CompareTag("GreenBasin"))
        {
            isNearGreenBasin = true;
        }
        else if (other.gameObject.CompareTag("BlueBasin"))
        {
            isNearBlueBasin = true;
        }
        else if (other.gameObject.CompareTag("RedBasin"))
        {
            isNearRedBasin = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("GreenCrystal"))
        {
            isNearGreen = false;
        }
        else if (other.gameObject.CompareTag("BlueCrystal"))
        {
            isNearBlue = false;
        }
        else if (other.gameObject.CompareTag("RedCrystal"))
        {
            isNearRed = false;
        }

        //basins
        if (other.gameObject.CompareTag("GreenBasin"))
        {
            isNearGreenBasin = false;
        }
        else if (other.gameObject.CompareTag("BlueBasin"))
        {
            isNearBlueBasin = false;
        }
        else if (other.gameObject.CompareTag("RedBasin"))
        {
            isNearRedBasin = false;
        }
    }
}
