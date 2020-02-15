using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sys = System;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class MemoryGameScipt : MonoBehaviour
{
    GameObject originalCube;
    GameObject player;
    int offsetFromOriginal = -2;
    float offsetFromEachOther = 1.0F;//was 2

    public static int currentDim = 3;
    public static int currentLevel = 1;
    public static int highestLevel = 1;
    public static int lives = 3;
    public static int hintsRemaining = 3;
    int numFilledIn = 0;

    public Text levelText;
    public Text hintText;
    public Text livesText;
    public Text highestLevelText;
    public Text instructText;

    float showedTime = 0;
    int numToAdd = -1;

    Color defaultFloorColor = new Color(.3f, 0.0f, 0.7f, 1);
    Color hintColor = new Color(100, 180, 0);


    int lastCubeLookedAtNum = 0;


    string cubePartName = "cubePart";
    string textPartName = "textNum";
    string filledInTag = "filledIn";
    string userClicked = "userClicked";

    bool isShowing = true;

    Color cubeColor = new Color(0, 0, 255);

    Texture2D lookingAtTexture;


    // Start is called before the first frame update
    void Start()
    {
        originalCube = GameObject.Find("OGCube");
        originalCube.GetComponent<Renderer>().material.SetColor("_Color", Color.white);

        //player = Instantiate(Sce,
        //            new Vector3(originalCube.transform.position.x + x * offsetFromEachOther,
        //            originalCube.transform.position.y + offsetFromOriginal + y * offsetFromEachOther,
        //            originalCube.transform.position.z),
        //            Quaternion.identity);
        lookingAtTexture = (Texture2D)Resources.Load("squareOutline");


        setup();
    }

    void showMadePattern()
    {

    }

    void makePattern()
    {
       
        if(currentLevel > highestLevel)
        {
            highestLevel = currentLevel;
        }
        for (int i = 0; i < currentDim * currentDim; i++)
        {
            if (GameObject.Find(cubePartName + i) != null)
            {
                GameObject.Find(cubePartName + i).GetComponent<cubeProperties>().userClicked = false;
                GameObject.Find(cubePartName + i).GetComponent<cubeProperties>().isFilledIn = false;
                GameObject.Find(cubePartName + i).transform.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
            }
        }


        numFilledIn = 0;
        while (numFilledIn < currentDim * currentDim / 3 + numToAdd)
        {
            int randomTile = Random.Range(0, currentDim * currentDim);
            Transform currentObject = GameObject.Find(cubePartName + randomTile).transform;

            if (currentObject.GetComponent<cubeProperties>().isFilledIn == false)
            {
                currentObject.GetComponent<cubeProperties>().isFilledIn = true;
                //Debug.Log(currentObject.tag);

                Renderer render = currentObject.GetComponent<Renderer>();
                render.material.color = cubeColor;
                numFilledIn++;

                //Debug.Log("correct tile: " + currentObject.name);

            }
        }
        showedTime = Time.time;
        isShowing = true;

        //Debug.Log("showedTime: " + showedTime);

        levelText.text = "Level: " + currentLevel;
        highestLevelText.text = "Highest Level\nGotten To: " + highestLevel;
        hintText.text = "Hints Remaining: " + hintsRemaining;
        livesText.text = "Lives Remaining: " + lives;

    }

    void removeCubes()
    {

        //GameObject.FindObjectsOfType(Cube)
        for (int i = 0; i < currentDim * currentDim; i++)
        {
            Destroy(GameObject.Find(cubePartName + i));
        }
    }

    void setup()
    {
        lastCubeLookedAtNum = 0;
        int total = 0;
        for (int y = currentDim; y > 0; y--)
        {
            for (int x = 0; x < currentDim; x++)
            {
                GameObject text = new GameObject();
                //TextMesh t = text.AddComponent<TextMesh>();
                //t.fontSize = 14;

                GameObject newCube = Instantiate(originalCube,
                    new Vector3(originalCube.transform.position.x + x * offsetFromEachOther,
                    originalCube.transform.position.y + offsetFromOriginal + y * offsetFromEachOther,
                    originalCube.transform.position.z),
                    Quaternion.identity);

                newCube.name = cubePartName + total;
                newCube.name = cubePartName + total;

                //newCube.transform.tag = "";
                text.transform.parent = newCube.transform;

                text.name = textPartName + total;
                //t.color = new Color(0, 0, 0);
                newCube.GetComponent<Renderer>().material.mainTexture = lookingAtTexture;

                //t.anchor = TextAnchor.MiddleCenter;
                //t.transform.position = new Vector3(newCube.transform.position.x, newCube.transform.position.y, newCube.transform.position.z - .5f);
                //t.text = total.ToString();

                total += 1;
            }
        }
        //PlayerControls.rb.transform.position = 
        //    new Vector3(GameObject.Find(cubePartName + (currentDim * currentDim) / 2).transform.position.x,
        //    PlayerControls.rb.transform.position.y,
        //    PlayerControls.rb.transform.position.z);
        transform.GetComponent<Renderer>().material.color = defaultFloorColor;

        Destroy(originalCube);
        makePattern();

    }

    bool compareAnswer()
    {
        int numCorrect = 0;
        int total = 0;
        for (int y = currentDim; y > 0; y--)
        {
            for (int x = 0; x < currentDim; x++)
            {
                //Debug.Log(cubePartName + total + " userClicked: " +
                    //GameObject.Find(cubePartName + total).GetComponent<cubeProperties>().userClicked);
                //Debug.Log(cubePartName + total + " isFilledIn: " +
                    //GameObject.Find(cubePartName + total).GetComponent<cubeProperties>().isFilledIn);


                //Debug.Log("numCorrect: " + numCorrect);
                if (GameObject.Find(cubePartName + total).GetComponent<cubeProperties>().isFilledIn == true)
                {
                    if (GameObject.Find(cubePartName + total).GetComponent<cubeProperties>().userClicked == true)
                    {
                        numCorrect++;
                    }
                    else
                    {
                        //y = -1;
                        return false;
                    }
                }
                else
                {
                    if (GameObject.Find(cubePartName + total).GetComponent<cubeProperties>().userClicked == true)
                    {
                        return false;
                    }
                }

                total++;
            }
        }



        if (numCorrect == numFilledIn)
        {
            return true;

        }
        else
            return false;
    }

    void showPattern()
    {
        for (int i = 0; i < currentDim * currentDim; i++)
        {
            Renderer r = GameObject.Find(cubePartName + i).transform.gameObject.GetComponent<Renderer>();
            r.material.color = new Color(255, 255, 255);
            if (GameObject.Find(cubePartName + i).GetComponent<cubeProperties>().isFilledIn)
            {
                r.material.SetColor("_Color", Color.blue);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Time: " + Time.time);

        if (isShowing == true && Time.time - showedTime >= currentDim * currentDim * .3)
        {
            for (int i = 0; i < currentDim * currentDim; i++)
            {
                if (GameObject.Find(cubePartName + i).GetComponent<cubeProperties>().userClicked == false)
                {
                    GameObject.Find(cubePartName + i).transform.GetComponent<Renderer>().material.color = new Color(255, 255, 255);

                }
                else
                {
                    Renderer r = GameObject.Find(cubePartName + i).transform.gameObject.GetComponent<Renderer>();

                    r.material.SetColor("_Color", Color.blue);

                }
            }
            transform.GetComponent<Renderer>().material.color = defaultFloorColor;
            isShowing = false;
        }

        if (isShowing)
        {
            //GameObject.Find(cubePartName + lastCubeLookedAtNum).GetComponent<Renderer>().material.mainTexture = Texture2D.whiteTexture;

        }

        if (isShowing == false)
        {

            RaycastHit target;

            //Gizmos.DrawLine
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out target, 25))
            {
                //Debug.Log(target);
                if (target.collider.name.Contains(cubePartName))
                {
                    //Debug.Log("Looking at: " + target.transform.name);

                    if (lastCubeLookedAtNum != sys.Int32.Parse(target.transform.name.Substring(cubePartName.Length)))
                    {

                        //GameObject.Find(cubePartName + lastCubeLookedAtNum).GetComponent<Renderer>().material.mainTexture = Texture2D.whiteTexture;
                        Renderer rend = target.collider.gameObject.GetComponent<Renderer>();
                        target.collider.gameObject.GetComponent<cubeProperties>().isBeingLookedAt = true;
                    }

                    lastCubeLookedAtNum = sys.Int32.Parse(target.transform.name.Substring(cubePartName.Length));
                    //GameObject.Find(cubePartName + lastCubeLookedAtNum).GetComponent<Renderer>().material.mainTexture = lookingAtTexture;
                    if (Input.GetMouseButtonDown(0))
                    {
                        //Debug.Log("Hit: " + target.transform.name);

                        target.collider.gameObject.GetComponent<cubeProperties>().userClicked =
                            !target.collider.gameObject.GetComponent<cubeProperties>().userClicked;
                        //Debug.DrawLine(Camera.main.transform.position, target.transform.position, Color.black, 5);

                        Renderer r = target.transform.gameObject.GetComponent<Renderer>();

                        if (r.material.GetColor("_Color") == Color.blue)
                        {
                            r.material.color = new Color(255,255,255);

                        }
                        else
                        {
                            r.material.SetColor("_Color", Color.blue);

                        }

                        //Debug.Log("Changed Color of " + target.transform.name);
                        //compareAnswer();
                    }
                }

                //compareAnswer();
            }


            if (Input.GetMouseButtonDown(1))
            {
                bool isCorrect = compareAnswer();
                if (isCorrect)
                {
                    currentLevel += 1;
                    numToAdd += 1;
                    transform.GetComponent<Renderer>().material.color = new Color(0, 255, 0);

                    if (numToAdd == 2)
                    {
                        numToAdd = -1;
                        //removeCubes();
                        currentDim += 1;
                        //setup();
                        SceneManager.LoadScene("ClassicMemoryGame");
                    }
                    else
                    {
                        makePattern();

                    }
                }
                else
                {
                    lives -= 1;
                    if (lives <= 0)
                    {
                        currentDim = 3;
                        currentLevel = 0;
                        lives = 3;
                        hintsRemaining = 3;
                        SceneManager.LoadScene("ClassicMemoryGame");
                    }
                    else
                    {
                    transform.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
                        makePattern();
                    }


                    livesText.text = "Lives Remaining: " + lives;


                }

            }

            if (isShowing == false && hintsRemaining > 0 && Input.GetKey(KeyCode.UpArrow))
            {
                isShowing = true;
                transform.GetComponent<Renderer>().material.color = hintColor;

                showPattern();
                if(compareAnswer() == false)
                {
                    hintsRemaining -= 1;
                }
                hintText.text = "Hints Remaining: " + hintsRemaining;
                showedTime = Time.time;
            }
        }


    }
}
