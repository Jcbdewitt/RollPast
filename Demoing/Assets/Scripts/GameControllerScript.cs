using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameControllerScript : MonoBehaviour
{
    
    public CameraRigScript cameraRigScript;

    public GameObject MenuPanel;
    public GameObject ShopPanel;
    public GameObject BackDropPanel;
    public GameObject BallPanel;
    public GameObject BarPanel;
    public GameObject SettingsPanel;
    public GameObject EndScreenPanel;
    public GameObject CornerPanel;
    public GameObject FinalText;
    public GameObject RowsPassed;
    public GameObject RowsScore;
    public GameObject ball;
    public GameObject bar;
    public GameObject SnapPoint;
    public GameObject LeftTouchPanel;
    public GameObject RightTouchPanel;

    public BoardSpawner spawnner;

    public TextMeshProUGUI scoreInCorner;
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI rowsPassedScore;
    public TextMeshProUGUI coinsCollectedScore;

    public ButtonClickScript playButton;
    public ButtonClickScript shopButton;
    public ButtonClickScript backDropButton;
    public ButtonClickScript backDropCloseButton;
    public ButtonClickScript[] backDropClosePanelButtons;
    public ButtonClickScript ballButton;
    public ButtonClickScript ballCloseButton;
    public ButtonClickScript[] ballClosePanelButtons;
    public ButtonClickScript barButton;
    public ButtonClickScript barCloseButton;
    public ButtonClickScript[] barClosePanelButtons;
    public ButtonClickScript closeShopButton;
    public ButtonClickScript[] closeShopPanelButton;
    public ButtonClickScript settingsButton;
    public ButtonClickScript closeSettingsButton;
    public ButtonClickScript[] closeSettingsPanelButton;
    public ButtonClickScript flipControlsButton;
    public ButtonClickScript resetButton;
    public ButtonClickScript restartButton;
    public ButtonClickScript menuButton;

    public BarRotateScript barRotate;

    public AdmobTest Ads;

    public int[] currentEquipped = { 0, 0, 0 };

    public float boostTime = 3f;
    public float magnetizedTime = 8.0f;

    public int money = 0;
    public int rowsPassed = 0;
    public int coinsCollected = 0;
    public int finalScoreValue = 0;
    public int lastRowsPassed = 0;
    public int holesUntilBump = 2;
    public int boostPowerUpBackUp = 0;
    public int magnetPowerUpBackUp = 0;

    private float lastTopSpeed = 0.0f;

    public bool onMenu = false;
    public bool shopOpen = false;
    public bool purchaseMade = false;
    public bool settingsOpened = false;
    public bool rotate = true;
    public bool resetingEverything = false;
    public bool waitALoop = true;
    public bool gameEnded = false;
    public bool musicStateChange = false;
    public bool musicMuted = false;
    public bool effectStateChange = false;
    public bool effectsMuted = false;

    public bool testBoost = false;

    bool onlyOnce = true;
    public bool boosting = false;
    bool returningToNormalSpeed = false;


    // Start is called before the first frame update
    void Awake()
    {
        money = SaveSystem.LoadPlayer().money;
        rotate = SaveSystem.LoadPlayer().rotate;
        musicMuted = SaveSystem.LoadPlayer().musicMuted;
        effectsMuted = SaveSystem.LoadPlayer().effectMuted;
        currentEquipped = SaveSystem.LoadPlayer().currentEquipped;
        if (!onMenu) { barRotate.nonFlippedControls = !rotate; }
    }

    void Update()
    {
        if (onMenu)
        {
            Menu();
        }
        else
        {
            Playing();
        }

        SetMutes();

        if (testBoost)
        {
            PowerUp(1);
            testBoost = false;
        }
    }

    private void SetMutes()
    {
        if (musicMuted)
        {
            cameraRigScript.muted = true;
        }
        else
        {
            cameraRigScript.muted = false;
        }

        if (!onMenu)
        {
            if (effectsMuted)
            {
                ball.transform.gameObject.GetComponent<BallScript>().muted = true;
            }
            else
            {
                ball.transform.gameObject.GetComponent<BallScript>().muted = false;
            }
        }

        if (musicStateChange)
        {
            musicStateChange = false;
            SaveSystem.SavePlayer(this);
            Debug.Log(SaveSystem.LoadPlayer().musicMuted);
        }

        if (effectStateChange)
        {
            effectStateChange = false;
            SaveSystem.SavePlayer(this);
        }
    }

    private void Menu()
    {

        scoreInCorner.SetText(money.ToString());

        if (!settingsOpened && !shopOpen)
        {
            MenuPanel.SetActive(true);
            ShopPanel.SetActive(false);
            BackDropPanel.SetActive(false);
            BallPanel.SetActive(false);
            BarPanel.SetActive(false);
            SettingsPanel.SetActive(false);

            if (playButton.clicked)
            {
                SceneManager.LoadScene("IcbGame");
            }

            if (shopButton.clicked)
            {
                shopButton.clicked = false;
                shopOpen = true;
            }

            if (settingsButton.clicked)
            {
                settingsButton.clicked = false;
                settingsOpened = true;
            }
        } else if (shopOpen)
        {
            if (backDropButton.clicked)
            {
                ShopPanel.SetActive(false);
                BackDropPanel.SetActive(true);

                if (backDropCloseButton.clicked || ClosePanelTouchCheck(backDropClosePanelButtons))
                {
                    backDropButton.clicked = false;
                    backDropCloseButton.clicked = false;
                    BackDropPanel.SetActive(false);
                }
            }else if (ballButton.clicked)
            {
                ShopPanel.SetActive(false);
                BallPanel.SetActive(true);

                if (ballCloseButton.clicked || ClosePanelTouchCheck(ballClosePanelButtons))
                {
                    ballButton.clicked = false;
                    ballCloseButton.clicked = false;
                    BallPanel.SetActive(false);
                }
            }
            else if (barButton.clicked)
            {
                ShopPanel.SetActive(false);
                BarPanel.SetActive(true);

                if (barCloseButton.clicked || ClosePanelTouchCheck(barClosePanelButtons))
                {
                    barButton.clicked = false;
                    barCloseButton.clicked = false;
                    BarPanel.SetActive(false);
                }
            } else if (closeShopButton.clicked || ClosePanelTouchCheck(closeShopPanelButton))
            {
                closeShopButton.clicked = false;
                shopOpen = false;
            }else
            {
                MenuPanel.SetActive(false);
                ShopPanel.SetActive(true);
                SettingsPanel.SetActive(false);
            }
        }
        else if (settingsOpened)
        {
            MenuPanel.SetActive(false);
            BackDropPanel.SetActive(false);
            SettingsPanel.SetActive(true);

            if (flipControlsButton.clicked)
            {
                rotate = !rotate;
                SaveSystem.SavePlayer(this);
                flipControlsButton.clicked = false;
            }

            if (resetButton.clicked)
            {
                resetingEverything = true;
                currentEquipped = new int[] { 0, 0, 0};
                SaveSystem.SavePlayer(this);
                PurchaseData resetData = new PurchaseData();
                SaveSystem.SavePurchase(resetData);
                resetingEverything = false;
                money = SaveSystem.LoadPlayer().money;
                GameObject.Find("Spawnner").GetComponent<BoardSpawner>().UpdateStyles(currentEquipped);
                BackDropPanel.SetActive(true);
                BackDropPanel.GetComponent<StoreScript>().purchasedItems = SaveSystem.LoadPurchase().ownBoards;
                BackDropPanel.SetActive(false);
                resetButton.clicked = false;
            }
            
            if (closeSettingsButton.clicked || ClosePanelTouchCheck(closeSettingsPanelButton))
            {
                closeSettingsButton.clicked = false;
                settingsOpened = false;
            }
            
            if (!waitALoop)
            {
                if (rotate)
                {
                    flipControlsButton.UpdateText("Rotate");
                }
                else
                {
                    flipControlsButton.UpdateText("Raise Sides");
                }
            }
            else
            {
                waitALoop = false;
            }
        }
    }

    private void Playing()
    {
        if (rowsPassed != lastRowsPassed)
        {
            IncreaseDiff();
        }

        if (boosting)
        {
            IncreaseDiff();
        }

        if (gameEnded)
        {
            finalScore.SetText((coinsCollected + rowsPassed).ToString());
            finalScoreValue = coinsCollected + rowsPassed;
            rowsPassedScore.SetText(rowsPassed.ToString());
            coinsCollectedScore.SetText(coinsCollected.ToString());

            EndScreenPanel.SetActive(true);
            CornerPanel.SetActive(false);

            cameraRigScript.moveCameraRig = false;
            barRotate.receiveInput = false;

            LeftTouchPanel.SetActive(false);
            RightTouchPanel.SetActive(false);

            

            if (onlyOnce)
            {
                onlyOnce = false;
                cameraRigScript.ChangeToLoseSong();
                BallScript ballScript = ball.GetComponent<BallScript>();
                Ads.bannerView.Show();
                bar.GetComponent<Rigidbody>().useGravity = true;
                bar.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                ballScript.SnapPoint = SnapPoint;
                ballScript.gameEnded = true;
            }

            if (restartButton.clicked)
            {
                int currentTotal = SaveSystem.LoadPlayer().money;
                SaveSystem.SavePlayer(this);
                Ads.DestroyAd();
                SceneManager.LoadScene("IcbGame");
                
            }
            else if (menuButton.clicked)
            {
                SaveSystem.SavePlayer(this);
                Ads.DestroyAd();
                SceneManager.LoadScene("Menu");
                
            }
        }
        else
        {
            EndScreenPanel.SetActive(false);

            barRotate.receiveInput = true;

        }

        scoreInCorner.SetText((coinsCollected + rowsPassed).ToString());

    }

    public void PowerUp(int power)
    {
        if (!effectsMuted) { ball.GetComponent<BallScript>().PowerSound(true); }

        switch (power)
        {
            case 0:
                if (boosting)
                {
                    boostPowerUpBackUp++;
                }else
                {
                    boosting = true;
                    StartCoroutine(PowerTime(boostTime, power));
                }
                break;
            case 1:
                if (ball.GetComponentInChildren<BallScript>().magnetized)
                {
                    magnetPowerUpBackUp++;
                } else
                {
                    ball.GetComponentInChildren<BallScript>().magnetized = true;
                    StartCoroutine(PowerTime(magnetizedTime, power));
                }
                break;
        }

    }

    public void PowerDown(int power)
    {

        if (!effectsMuted) { ball.GetComponent<BallScript>().PowerSound(false); }

        switch (power)
        {
            case 0:
                boosting = false;
                break;
            case 1:
                ball.GetComponentInChildren<BallScript>().magnetized = false;
                break;
        }
    }

    private void IncreaseDiff()
    {
        if (!boosting)
        {
            if (returningToNormalSpeed)
            {
                returningToNormalSpeed = false;
                cameraRigScript.speedFactor = lastTopSpeed;
            }

            if (rowsPassed % holesUntilBump == 0)
            {
                cameraRigScript.speedFactor += 0.5f;
            }

            lastRowsPassed = rowsPassed;
        } else
        {
            if (!returningToNormalSpeed)
            {
                returningToNormalSpeed = true;
                lastTopSpeed = cameraRigScript.speedFactor;
                Debug.Log(lastTopSpeed + " last speed");
                cameraRigScript.speedFactor = lastTopSpeed + 50f;
            }
        }
        
        if (rowsPassed > 10)
        {
            spawnner.state = 4;
        } else if (rowsPassed > 6)
        {
            spawnner.state = 3;
        } else if (rowsPassed > 3)
        {
            spawnner.state = 2;
        }

    }

    private bool ClosePanelTouchCheck(ButtonClickScript[] panels)
    {
        bool returningBool = false;
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].pressed)
            {
                returningBool = true;
                panels[i].pressed = false;
            }
        }

        return returningBool;
    }

    private IEnumerator PowerTime(float waitTime, int power)
    {

        yield return new WaitForSeconds(waitTime);

        if (power == 0)
        {
            if (boostPowerUpBackUp == 0)
            {
                PowerDown(power);
            } else
            {
                boostPowerUpBackUp--;
                StartCoroutine(PowerTime(waitTime, power));
            }
        } else
        {
            if (magnetPowerUpBackUp == 0)
            {
                PowerDown(power);
            }
            else
            {
                magnetPowerUpBackUp--;
                StartCoroutine(PowerTime(waitTime, power));
            }
        }
    }
}
