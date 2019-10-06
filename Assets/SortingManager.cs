using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingManager : MonoBehaviour
{
    public Transform leftBlockPos;  //set the left most block position
    public Transform rightBlockPos; //set the right most block position
    [Range(0.05f, 0.5f)]
    public float minSpacingBetweenBlocks = 0.1f;   //this is the minimum spacint that should be maintained, default is 0.1
    public List<GameObject> blocks;

    public GameObject blockPrefab;
    GameObject blockParent;

    public GameObject pointerIndicator;
    GameObject pointerIndicator1;
    GameObject pointerIndicator2;

    [Range(0.05f, 1f)] public float swapSpeed = 0.25f;

    private void Start()
    {
        if(blockPrefab == null)
        {
            Debug.LogError("No block prefab is attached!");
        }
        else
        {
            blockParent = new GameObject("block Parent");
            StartCoroutine(AddBlocks());
        }
        pointerIndicator1 = Instantiate(pointerIndicator, gameObject.transform, false);
        pointerIndicator1.SetActive(false);
        pointerIndicator2 = Instantiate(pointerIndicator, gameObject.transform, false);
        pointerIndicator2.SetActive(false);
    }

    public void StartSorting()
    {
        //start the sorting process

        int sortId = 2;
        //sort id is for choosing between different sorting techniques
        switch(sortId)
        {
            default:
            case 1:
                //for BubbleSort
                StartCoroutine(BubbleSort());
                break;
            case 2:
                //for selection Sort
                StartCoroutine(SelectionSort());
                break;
            case 3:
                //quick Sort
                break;
        }
    }

    IEnumerator InsertionSort()
    {
        yield return null;  //to avoid not all code paths return a value error

        //change the list into an array first befor starting the swapping
        Block[] blocksArray = new Block[this.blocks.Count];

        //populate blocks array
        for (int i = 0; i < blocksArray.Length; i++)
        {
            blocksArray[i] = this.blocks[i].GetComponent<Block>();
        }

        //Enable the pointer indicators for visualization
        pointerIndicator1.SetActive(true);
        pointerIndicator2.SetActive(true);

        //do insertion sorte
        int key = 0, j = 0;
        for(int i = 1; i < blocksArray.Length; i++)
        {
            key = blocksArray[i].height;
            j = i - 1;

            while (j >= 0 && blocksArray[j].height > key)
            {
                blocksArray[j + 1]
            }
        }
    }

    IEnumerator SelectionSort()
    {
        yield return null;  //to avoid not all code paths return a value error

        //change the list into an array first befor starting the swapping
        Block[] blocksArray = new Block[this.blocks.Count];

        //populate blocks array
        for (int i = 0; i < blocksArray.Length; i++)
        {
            blocksArray[i] = this.blocks[i].GetComponent<Block>();
        }

        //Enable the pointer indicators for visualization
        pointerIndicator1.SetActive(true);
        pointerIndicator2.SetActive(true);

        //do selection sorte
        int minIndex = 0;
        for(int i = 0; i < blocksArray.Length - 1; i++)
        {
            minIndex = i;
            for(int j = i + 1; j < blocksArray.Length; j++)
            {
                pointerIndicator1.transform.position = new Vector3(blocksArray[j].positionX, pointerIndicator1.transform.position.y, pointerIndicator1.transform.position.z);
                pointerIndicator2.transform.position = new Vector3(blocksArray[minIndex].positionX, pointerIndicator2.transform.position.y, pointerIndicator2.transform.position.z);
                if (blocksArray[j].height < blocksArray[minIndex].height)
                {
                    minIndex = j;
                    
                }
            }
            swapping = true;
            StartCoroutine(Swap(minIndex, i, blocksArray));
            while (swapping)
                yield return null;
        }
    }

    IEnumerator BubbleSort()
    {
        yield return null;
        //change the list into an array first befor starting the swapping
        Block[] blocksArray = new Block[this.blocks.Count];

        //populate blocks array
        for (int i = 0; i < blocksArray.Length; i++)
        {
            blocksArray[i] = this.blocks[i].GetComponent<Block>();
        }

        //Enable the pointer indicators for visualization
        pointerIndicator1.SetActive(true);
        pointerIndicator2.SetActive(true);

        //do bubble sort
        for(int i = 0; i < blocksArray.Length; i++)
        {
            for(int j = 0; j < blocksArray.Length - 1; j++)
            {
                pointerIndicator1.transform.position = new Vector3(blocksArray[j].positionX, pointerIndicator1.transform.position.y, pointerIndicator1.transform.position.z);
                pointerIndicator2.transform.position = new Vector3(blocksArray[j+1].positionX, pointerIndicator2.transform.position.y, pointerIndicator2.transform.position.z);
                if(blocksArray[j].height > blocksArray[j + 1].height)
                {
                    swapping = true;
                    StartCoroutine(Swap(j, j + 1, blocksArray));
                    while (swapping)
                        yield return null;
                }
            }
        }
        yield return new WaitForSecondsRealtime(0.5f);
        //After sorting disable the pointer indicators
        pointerIndicator1.SetActive(false);
        pointerIndicator2.SetActive(false);
    }

    bool swapping = false;
    IEnumerator Swap(int i, int j, Block[] blocks)
    {
        yield return null;
        //swap the blocks
        Block temp = blocks[i];
        blocks[i] = blocks[j];
        blocks[j] = temp;
        //swap the position
        float tempX = blocks[i].positionX;
        blocks[i].positionX = blocks[j].positionX;
        yield return new WaitForSecondsRealtime(swapSpeed / 5);
        blocks[j].positionX = tempX;
        yield return new WaitForSecondsRealtime(swapSpeed / 2);
        swapping = false;   //swapping is completed
    }


    #region Block Adding and Alligning
    IEnumerator AddBlocks()
    {
        yield return null;
        //add 25 blocks
        for (int i = 0; i < 15; i++)
        {
            AddBlock();
            yield return new WaitForSecondsRealtime(0.1f);
        }
        Debug.Log("Sorting started");
        yield return new WaitForSecondsRealtime(1f);
        StartSorting();
    }
    
    public void AddBlock()
    {

        //it adds block of random height
        GameObject g = Instantiate(blockPrefab);
        Block b = g.GetComponent<Block>();
        int randomHeight = Random.Range(1, Block.maxHeight);
        b.height = randomHeight;

        //Add the block in the list
        blocks.Add(g);

        //After Adding every block realign/resize the blocks
        ReAlignBlocks();
    }

    public void ReAlignBlocks()
    {
        //It re-sizes and re-aligns the blocks
        int countOfBlocks = blocks.Count;
        //get the current spacing required between blocks for the countOfBlocks
        float currentSpacing = (rightBlockPos.position.x - leftBlockPos.position.x) / countOfBlocks;    
        //if the current spacing is < 1 then blocks will overlap so need to reduce the scale of blocks

        
        //float availableBlockSpace = (rightBlockPos.position.x - leftBlockPos.position.x) - (minSpacingBetweenBlocks * (countOfBlocks - 1));
        float newXScale = 1; //default is 1
        float spacing = minSpacingBetweenBlocks;

        if (currentSpacing < (1 + minSpacingBetweenBlocks))
        {
            float requiredEmptySpace = (countOfBlocks - 1) * minSpacingBetweenBlocks;
            float totalSpace = (rightBlockPos.position.x - leftBlockPos.position.x);
            newXScale = (totalSpace - requiredEmptySpace) / countOfBlocks;
        }
        else if(currentSpacing > (1 + minSpacingBetweenBlocks))
        {
            //in this case blocks can be placed with more than minimumspace gap
            //the max X scale for block is 1
            float totalBlockSpace = countOfBlocks * 1;
            float availableBlankSpace = (rightBlockPos.position.x - leftBlockPos.position.x) - totalBlockSpace;
            spacing = availableBlankSpace / (countOfBlocks > 1 ? (countOfBlocks - 1) : countOfBlocks);
        }


        float x_base = leftBlockPos.position.x;
        for (int index = 0; index < countOfBlocks; index++)
        {
            float xPos = x_base + (index * newXScale) + (spacing * index);
            blocks[index].GetComponent<Block>().positionX = xPos;   //set the position variable
            blocks[index].transform.position = new Vector3(xPos, blocks[index].transform.position.y, blocks[index].transform.position.z);
            blocks[index].transform.localScale = new Vector3(newXScale, blocks[index].transform.localScale.y, 1);
        }

        //when re-alligning the blocks make sure that the pointer indicator's scale is also same as of the blocks
        pointerIndicator1.transform.localScale = new Vector3(newXScale * 0.75f, pointerIndicator1.transform.localScale.y, 1);
        pointerIndicator2.transform.localScale = new Vector3(newXScale * 0.75f, pointerIndicator2.transform.localScale.y, 1);
    }
    #endregion

}
