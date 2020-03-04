using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreScript : MonoBehaviour
{
    //public Camera camera;

    public GameControllerScript gameControllerScript;

    RectTransform storeRect;

    public GameObject animationObject;
    public GameObject PageDots;
    public GameObject StoreScreen;
    public GameObject Dot;

    DotScript[] dotsArray;

    Animator animator;

    public int numberOfPages = 2;
    readonly int buttonsPerPage = 3;
    readonly float spaceBetweenDots = 15.0f;

    public int storeNumber = 0;
    public int[] purchasedItems;
    public int[] priceOfItems = { 0, 10, 10, 20, 50, 100 };

    private int currentPage;

    public bool moving = false;
    public bool advanceRight = false;
    public bool advanceLeft = false;
    public bool farLeft = false;
    public bool farRight = false;
    private bool splitCenter = false;

    ButtonClickScript[] storeButtons;

    private void Start()
    {
        storeRect = GetComponent<RectTransform>();
        animationObject = transform.GetChild(0).transform.GetChild(0).gameObject;
        animator = animationObject.GetComponent<Animator>();

        storeButtons = new ButtonClickScript[buttonsPerPage*numberOfPages];
        dotsArray = new DotScript[numberOfPages];

        int index = 0;
        int priceIndex = 0;
        for (int i = 0; i < StoreScreen.transform.childCount; i++)
        {
            GameObject storeScreenChild = StoreScreen.transform.GetChild(i).gameObject;
            TextMeshProUGUI[] currentScreenPrices = storeScreenChild.transform.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (TextMeshProUGUI t in currentScreenPrices)
            {
                string currentText = t.text;
                t.SetText(currentText + " " + priceOfItems[priceIndex].ToString());
                priceIndex++;
            }

            for (int j = 0; j < buttonsPerPage; j++)
            {
                GameObject currentObject = storeScreenChild.transform.GetChild(j).gameObject;

                ButtonClickScript currentButton = currentObject.GetComponent<ButtonClickScript>();

                storeButtons.SetValue(currentButton, index);
                index++;
            }
        }

        int numberOfDotsPastCenter = 0;
        int indexOfCenter = 0;

        if (numberOfPages % 2 == 0)
        {
            splitCenter = false;
            indexOfCenter = numberOfPages / 2;
        } else
        {
            splitCenter = true;
            numberOfDotsPastCenter = numberOfPages - 1;
            indexOfCenter = numberOfDotsPastCenter / 2;
        }

        for (int k = 0; k < numberOfPages; k++)
        {
            float xPosition = 0;

            if (splitCenter)
            {
                if (k < indexOfCenter)
                {
                    xPosition = (indexOfCenter - k) * -spaceBetweenDots;
                }else if (k > indexOfCenter)
                {
                    xPosition = (k - indexOfCenter) * spaceBetweenDots;
                }
            } else
            {
                if (k < indexOfCenter)
                {
                    xPosition = (indexOfCenter - k) * -(spaceBetweenDots/2);
                } else if (k > indexOfCenter)
                {
                    xPosition = (k - indexOfCenter) * (spaceBetweenDots/2); 
                } else
                {
                    xPosition = (spaceBetweenDots/2);
                }
            }

            Vector3 positionOfNewDotWorld = PageDots.transform.TransformPoint(new Vector3(xPosition, 0.0f, 0.0f));

            GameObject newDot = Instantiate(Dot, Dot.transform.position, Dot.transform.rotation);
            newDot.transform.SetParent(PageDots.transform, false);// = PageDots;
            newDot.GetComponent<RectTransform>().SetPositionAndRotation(positionOfNewDotWorld, newDot.transform.rotation);
            dotsArray.SetValue(newDot.GetComponent<DotScript>(), k);
        }

        UpdatePageDot();

        switch (storeNumber)
        {
            case 0:
                purchasedItems = SaveSystem.LoadPurchase().ownBoards;
                break;
            case 1:
                purchasedItems = SaveSystem.LoadPurchase().ownBalls;
                break;
            case 2:
                purchasedItems = SaveSystem.LoadPurchase().ownBars;
                break;
            default:
                break;
        }

        if (purchasedItems.Length != priceOfItems.Length)
        {
            int[] tempArray = new int[priceOfItems.Length];
            
            for (int i = 0; i < priceOfItems.Length; i++)
            {
                if (purchasedItems.Length - i > 0)
                {
                    tempArray[i] = purchasedItems[i];
                } else
                {
                    tempArray[i] = 0;
                }
            }

            purchasedItems = new int[tempArray.Length];
            purchasedItems = tempArray;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateButtonText();
        DetectSwipe();
        DetectButtons();
    }

    private void UpdateButtonText()
    {
        for (int k = 0; k < storeButtons.Length; k++)
        {
            ButtonClickScript currentButton = storeButtons.GetValue(k) as ButtonClickScript;

            if (purchasedItems[k] == 1)
            {
                if (gameControllerScript.currentEquipped[storeNumber] == k)
                {
                    if (currentButton.textToPlace.Equals("Owned"))
                    {
                        currentButton.UpdateText("Equipped");
                        //gameControllerScript.currentEquipped[storeNumber] = k;
                        BoardSpawner boardSpawnner = GameObject.Find("Spawnner").GetComponent<BoardSpawner>();

                        boardSpawnner.UpdateStyles(k , storeNumber);
                    }else
                    {
                        currentButton.UpdateText("Equipped");
                    }
                }
                else
                {
                    currentButton.UpdateText("Owned");
                }
            }
            else
            {
                currentButton.UpdateText("Buy");
            }
        }
    }

    private void UpdatePageDot()
    {
        int index = 0;
        foreach(DotScript d in dotsArray)
        {
            if (index == currentPage)
            {
                d.Highlight = true;
            } else
            {
                d.Highlight = false;
            }
            d.UpdateHighLight();
            index++;
        }
    }

    private void DetectButtons()
    {
        int buttonIndex = 0;
        foreach (ButtonClickScript b in storeButtons)
        {
            if (b.clicked)
            {
                if (purchasedItems[buttonIndex] != 1)
                {
                    if (gameControllerScript.money >= priceOfItems[buttonIndex])
                    {
                        gameControllerScript.money -= priceOfItems[buttonIndex];
                        gameControllerScript.purchaseMade = true;
                        purchasedItems.SetValue(1, buttonIndex);
                        PurchaseData data = new PurchaseData(purchasedItems, storeNumber);
                        SaveSystem.SavePlayer(gameControllerScript);
                        SaveSystem.SavePurchase(data);
                        gameControllerScript.purchaseMade = false;
                    }
                }
                else
                {
                    if (gameControllerScript.currentEquipped[storeNumber] != buttonIndex)
                    {
                        gameControllerScript.currentEquipped[storeNumber] = buttonIndex;
                        SaveSystem.SavePlayer(gameControllerScript);
                    }
                }

                b.clicked = false;
            }
            buttonIndex++;
        }
    }

    private void DetectSwipe()
    {
        Animate();

        if (Input.touchCount > 0)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(storeRect, Input.GetTouch(0).position, Camera.main))
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
                    if (touchDelta.x > 3)
                    {
                        if (!farRight)
                        {
                            advanceRight = true;
                        }
                    }

                    if (touchDelta.x < -3)
                    {
                        if (!farLeft)
                        {
                            advanceLeft = true;
                        }
                    }
                }
            }
        }
    }

    private void Animate()
    {
        if (advanceLeft && farLeft)
        {
            advanceLeft = false;
        }
        else if (advanceRight && farRight)
        {
            advanceRight = false;
        }

        if (advanceRight && !advanceLeft && !moving)
        {
            animator.SetBool("Right", true);
            moving = true;
        }
        else if (advanceLeft && !advanceRight && !moving)
        {
            animator.SetBool("Left", true);
            moving = true;
        }
        else
        {
            if (moving)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("MenuDefaultPosition"))
                {
                    animator.SetBool("Right", false);
                    animator.SetBool("Left", false);

                    if (advanceRight)
                    {
                        currentPage--;
                    } else if ( advanceLeft)
                    {
                        currentPage++;
                    }

                    UpdatePageDot();

                    advanceLeft = false;
                    advanceRight = false;
                    moving = false;
                }
            }

            if (currentPage == numberOfPages -1)
            {
                farLeft = true;
                farRight = false;
            }
            else if (currentPage == 0)
            {
                farRight = true;
                farLeft = false;
            }
            else
            {
                farLeft = false;
                farRight = false;
            }
        }
    }
}