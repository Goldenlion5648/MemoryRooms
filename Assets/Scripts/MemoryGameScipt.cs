using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGameScipt : MonoBehaviour
{
    GameObject originalCube;
    GameObject player;
    int offsetFromOriginal = -2;
    int offsetFromEachOther = 2;

    int currentDim = 3;
    int numFilledIn = 0;

    float showedTime =0 ;
    int numToAdd = -1;


    string cubePartName = "cubePart";
    string textPartName = "textNum";
    string filledInTag = "filledIn";

    bool isShowing = true;

    Color cubeColor = new Color(0, 0, 255);


    // Start is called before the first frame update
    void Start()
    {
        originalCube =  GameObject.Find("OGCube");

        //player = Instantiate(Sce,
        //            new Vector3(originalCube.transform.position.x + x * offsetFromEachOther,
        //            originalCube.transform.position.y + offsetFromOriginal + y * offsetFromEachOther,
        //            originalCube.transform.position.z),
        //            Quaternion.identity);

        setup();
    }

    void showMadePattern()
    {

    }

    void makePattern()
    {
        numFilledIn = 0;
        while(numFilledIn < currentDim * currentDim / 3 + numToAdd)
        {
            int randomTile = Random.Range(0, currentDim * currentDim);
            Transform currentObject = GameObject.Find(cubePartName + randomTile).transform;

            if (currentObject.tag.Contains(filledInTag) == false)
            {
                currentObject.tag = filledInTag;

                Renderer render = currentObject.GetComponent<Renderer>();
                render.material.color = cubeColor;
                numFilledIn++;

            }
        }
        showedTime = Time.time;

        Debug.Log("showedTime: " + showedTime);
    }


    void setup()
    {
        int total = 0;
        for (int y = currentDim; y >0 ; y--)
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
        makePattern();

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

        //if (isShowing == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit target;

                //Gizmos.DrawLine
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out target, 25) && target.collider.name.Contains(cubePartName))
                {
                Debug.DrawLine(Camera.main.transform.position, target.transform.position, Color.black, 5);

                    Renderer r = target.transform.gameObject.GetComponent<Renderer>();

                    r.material.SetColor("_Color", Color.blue);

                    Debug.Log("Changed Color");

                }
            }
        }
    }
}
