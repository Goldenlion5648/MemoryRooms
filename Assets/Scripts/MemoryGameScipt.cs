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
    int numToAdd = -1;


    string cubePartName = "cubePart";
    string textPartName = "textNum";
    string filledInTag = "filledIn";


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
        while(numFilledIn < currentDim * currentDim / 3 + numToAdd)
        {
            int randomTile = Random.Range(0, currentDim * currentDim);
            Transform currentObject = GameObject.Find(cubePartName + randomTile).transform;

            if (currentObject.tag.Contains(filledInTag) == false)
            {
                currentObject.tag += filledInTag;
                numFilledIn++;

            }
        }
    }


    void setup()
    {
        int total = 0;
        for (int y = currentDim; y >0 ; y--)
        {
            for (int x = 0; x < currentDim; x++)
            {
                GameObject text = new GameObject();
                TextMesh t = text.AddComponent<TextMesh>();
                t.fontSize = 14;

                GameObject newCube = Instantiate(originalCube,
                    new Vector3(originalCube.transform.position.x + x * offsetFromEachOther,
                    originalCube.transform.position.y + offsetFromOriginal + y * offsetFromEachOther,
                    originalCube.transform.position.z),
                    Quaternion.identity);

                newCube.name = cubePartName + total;

                text.name = textPartName + total;

                text.transform.parent = newCube.transform;

                t.anchor = TextAnchor.MiddleCenter;
                t.transform.position = new Vector3(newCube.transform.position.x, newCube.transform.position.y, newCube.transform.position.z - .5f);
                t.text = total.ToString();

                total += 1;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
