using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sys = System;

public class MemoryGameScipt : MonoBehaviour
{
    GameObject originalCube;
    GameObject player;
    int offsetFromOriginal = -2;
    int offsetFromEachOther = 2;

    int currentDim = 3;
    int numFilledIn = 0;

    float showedTime = 0;
    int numToAdd = -1;

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
        for (int i = 0; i < currentDim * currentDim; i++)
        {
            GameObject.Find(cubePartName + i).GetComponent<cubeProperties>().userClicked = false;
            GameObject.Find(cubePartName + i).transform.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
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

                Debug.Log("correct tile: " + currentObject.name);

            }
        }
        showedTime = Time.time;
        isShowing = true;

        Debug.Log("showedTime: " + showedTime);
    }


    void setup()
    {
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


                //t.anchor = TextAnchor.MiddleCenter;
                //t.transform.position = new Vector3(newCube.transform.position.x, newCube.transform.position.y, newCube.transform.position.z - .5f);
                //t.text = total.ToString();

                total += 1;
            }
        }

        Destroy(originalCube);
        makePattern();

    }

    void compareAnswer()
    {
        int numCorrect = 0;
        int total = 0;
        for (int y = currentDim; y > 0; y--)
        {
            for (int x = 0; x < currentDim; x++)
            {
                if (GameObject.Find(cubePartName + total).GetComponent<cubeProperties>().userClicked == true)
                {
                    if (GameObject.Find(cubePartName + total).GetComponent<cubeProperties>().isFilledIn == true)
                    {
                        numCorrect++;
                    }
                    else
                    {
                        //y = -1;
                        return;
                    }
                }
                else
                {
                    if (GameObject.Find(cubePartName + total).GetComponent<cubeProperties>().isFilledIn == false)
                    {
                        return;
                    }
                }

                total++;
            }
        }

        if (numCorrect >= numFilledIn)
        {
            numToAdd += 1;
            if (numToAdd == 2)
            {
                numToAdd = -1;
                currentDim += 1;
            }

            Debug.Log("Making new pattern");
            Debug.Log("numCorrect:" + numCorrect);
            Debug.Log("numFilledIn:" + numFilledIn);
            makePattern();

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
                GameObject.Find(cubePartName + i).transform.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
            }
            isShowing = false;
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

                        GameObject.Find(cubePartName + lastCubeLookedAtNum).GetComponent<Renderer>().material.mainTexture = Texture2D.whiteTexture;
                        Renderer rend = target.collider.gameObject.GetComponent<Renderer>();
                        target.collider.gameObject.GetComponent<cubeProperties>().isBeingLookedAt = true;
                    }

                    lastCubeLookedAtNum = sys.Int32.Parse(target.transform.name.Substring(cubePartName.Length));
                    GameObject.Find(cubePartName + lastCubeLookedAtNum).GetComponent<Renderer>().material.mainTexture = lookingAtTexture;
                    if (Input.GetMouseButtonDown(0))
                    {
                        //Debug.Log("Hit: " + target.transform.name);

                        target.collider.gameObject.GetComponent<cubeProperties>().userClicked =
                            !target.collider.gameObject.GetComponent<cubeProperties>().userClicked;
                        //Debug.DrawLine(Camera.main.transform.position, target.transform.position, Color.black, 5);

                        Renderer r = target.transform.gameObject.GetComponent<Renderer>();

                        if (r.material.GetColor("_Color") == Color.blue)
                        {
                            r.material.SetColor("_Color", Color.white);

                        }
                        else
                        {
                            r.material.SetColor("_Color", Color.blue);

                        }

                        Debug.Log("Changed Color of " + target.transform.name);
                    }
                }
                else
                {
                    //GameObject.Find(cubePartName + lastCubeLookedAtNum).GetComponent<Renderer>().material.mainTexture = Texture2D.whiteTexture;

                }

                compareAnswer();
            }
            else
            {
                GameObject.Find(cubePartName + lastCubeLookedAtNum).GetComponent<Renderer>().material.mainTexture = Texture2D.whiteTexture;

            }
        }


    }
}
