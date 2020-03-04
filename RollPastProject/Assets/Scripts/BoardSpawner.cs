using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpawner : MonoBehaviour
{
    public GameObject GameController;
    public GameObject PieceWithHoles;
    public GameObject BlankPiece;
    public GameObject Ball;
    public GameObject Bar;
    public GameObject BarStore;

    private GameObject selectedCoinSpawn;
    private GameObject selectedPowerUpSpawn;

    public GameObject CoinSpawnLocation1;
    public GameObject CoinSpawnLocation2;
    public GameObject CoinSpawnLocation3;

    public GameObject PowerUpSpawnLocation1;
    public GameObject PowerUpSpawnLocation2;
    public GameObject PowerUpSpawnLocation3;

    public GameObject[] TwoHoles;
    public GameObject[] ThreeHoles;
    public GameObject[] FourHoles;
    public GameObject[] FiveHoles;

    private GameObject[] SelectedArray;

    MeshRenderer[] pieceWithHolesRenderer;
    MeshRenderer blankPieceRenderer;
    Renderer ball;
    Renderer bar;
    Renderer menuBar;

    public Texture[] boards;
    public Material[] balls;
    public Material[] bars;

    GameControllerScript gameControllerScript;

    /*
    public int boardStyle = 0;
    public int ballStyle = 0;
    public int barStyle = 0;
    */

    public int[] currentStyle = { 0, 0, 0 };

    public int numberOfHoles = 7;
    public int numberOfCoveredSpots = 5;
    public int state = 1;
    public float distanceFromBar = 30.0f;

    private readonly int boardIndex = 0;
    private readonly int ballIndex = 1;
    private readonly int barIndex = 2;

    bool onlyOnce = true;
    bool onFirstPiece = true;
    bool onMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        gameControllerScript = GameController.GetComponent<GameControllerScript>();

        this.onMenu = gameControllerScript.onMenu;

        pieceWithHolesRenderer = PieceWithHoles.transform.GetComponentsInChildren<MeshRenderer>();
        blankPieceRenderer = BlankPiece.transform.GetComponentInChildren<MeshRenderer>();

        ball = Ball.GetComponentInChildren<Renderer>();
        bar = Bar.GetComponent<Renderer>();
        if (onMenu) { menuBar = BarStore.GetComponent<Renderer>(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (onlyOnce)
        {
            currentStyle = gameControllerScript.currentEquipped;
            UpdateStyles(currentStyle);
            onlyOnce = false;
        }

        Spawning();
    }

    public void UpdateStyles(int[] styles)
    {
        this.currentStyle = styles;

        foreach (MeshRenderer r in pieceWithHolesRenderer)
        {
            r.sharedMaterial.SetTexture("_MainTex", boards[currentStyle[boardIndex]]);
        }

        blankPieceRenderer.sharedMaterial.SetTexture("_MainTex", boards[currentStyle[boardIndex]]);

        ball.material = balls[currentStyle[ballIndex]];
        bar.material = bars[currentStyle[barIndex]];
        if (onMenu) { menuBar.material = bars[currentStyle[barIndex]]; }
    }

    public void UpdateStyles(int updatedStyle, int storeNumber)
    {
        switch (storeNumber)
        {
            case 0:
                foreach (MeshRenderer r in pieceWithHolesRenderer)
                {
                    r.sharedMaterial.SetTexture("_MainTex", boards[updatedStyle]);
                }

                blankPieceRenderer.sharedMaterial.SetTexture("_MainTex", boards[updatedStyle]);
                break;
            case 1:
                ball.material = balls[currentStyle[ballIndex]];
                break;
            case 2:
                bar.material = bars[currentStyle[barIndex]];
                if (onMenu) { menuBar.material = bars[currentStyle[barIndex]]; }
                break;
            default:
                break;
        }
        
    }

    private void Spawning()
    {
        if (Vector3.Distance(transform.position, Bar.transform.position) < distanceFromBar)
        {
            SpawnBlankPiece();
            MoveSpawnner();
            if (onMenu) { SpawnHolePieceMenu(); }
            else
            {
                ArrayBasedOnState();
                SpawnHolePiece();
            }
            MoveSpawnner();
        }
    }

    private void ArrayBasedOnState()
    {
        switch (state)
        {
            case 1:
                SelectedArray = TwoHoles;
                break;
            case 2:
                SelectedArray = ThreeHoles;
                break;
            case 3:
                SelectedArray = FourHoles;
                break;
            case 4:
                SelectedArray = FiveHoles;
                break;
            default:
                SelectedArray = FiveHoles;
                break;
        }
    }

    private void MoveSpawnner()
    {
        Vector3 newPosition = transform.position;
        newPosition.y += 90.0f;
        transform.position = newPosition;
    }

    private void SpawnBlankPiece()
    {
        Instantiate(BlankPiece, transform.position, transform.rotation);

        if (!gameControllerScript.onMenu)
        {
            if (!onFirstPiece)
            {
                int whichCoinAreasToSpawn = UnityEngine.Random.Range(1, 5);
                bool spawnCoins = false;
                bool spawnPowerUp = false;

                switch (whichCoinAreasToSpawn)
                {
                    case 1:
                        selectedCoinSpawn = CoinSpawnLocation1;
                        spawnCoins = true;
                        break;
                    case 2:
                        selectedCoinSpawn = CoinSpawnLocation2;
                        spawnCoins = true;
                        break;
                    case 3:
                        selectedCoinSpawn = CoinSpawnLocation3;
                        spawnCoins = true;
                        break;
                    case 4:
                        //if (UnityEngine.Random.Range(1, 4) == 1)
                        //{
                            spawnPowerUp = true;
                        //}
                        break;

                }

                if (spawnCoins)
                {
                    Instantiate(selectedCoinSpawn, transform.position, selectedCoinSpawn.transform.rotation);
                } else if (spawnPowerUp)
                {
                    spawnPowerUp = false;
                    int powerUpPoint = UnityEngine.Random.Range(1, 3);

                    switch (powerUpPoint)
                    {
                        case 1:
                            selectedPowerUpSpawn = PowerUpSpawnLocation1;
                            break;
                        case 2:
                            selectedPowerUpSpawn = PowerUpSpawnLocation2;
                            break;
                        case 3:
                            selectedPowerUpSpawn = PowerUpSpawnLocation3;
                            break;
                    }

                    Instantiate(selectedPowerUpSpawn, transform.position, selectedPowerUpSpawn.transform.rotation);

                }


            } else
            {
                onFirstPiece = false;
            }
            
        }
    }

    private void SpawnHolePieceMenu()
    {
        GameObject holePieceObject = Instantiate(PieceWithHoles, transform.position, transform.rotation);

        GameObject[] coverColliders = new GameObject[numberOfHoles];
        GameObject[] covers = new GameObject[numberOfHoles];

        for(int i = 0; i < numberOfHoles; i++)
        {
            int coverIndex = 7 + i;
            coverColliders.SetValue(holePieceObject.transform.GetChild(i).gameObject, i);
            covers.SetValue(holePieceObject.transform.GetChild(coverIndex).gameObject, i);
        }

        int skipDeleting = UnityEngine.Random.Range(1, 7);
        int skipDeleting2 = skipDeleting - 1;

        for(int i = 0; i < numberOfHoles; i++)
        {
            HoleColliderScript currentCollider = coverColliders[i].GetComponent<HoleColliderScript>();

            currentCollider.GameControllerObject = GameController;

            if (i != skipDeleting && i != skipDeleting2)
            {
                Destroy(covers[i].gameObject);

                currentCollider.connectedCollider = coverColliders[skipDeleting];

            }else
            {
                currentCollider.noHole = true;

                if(i == skipDeleting2)
                {
                    HoleColliderScript colliderToConnect = coverColliders[skipDeleting].GetComponent<HoleColliderScript>();
                    colliderToConnect.connectedCollider = coverColliders[i].gameObject;
                    colliderToConnect.connected = true;
                }
            }
        }
    }

    private void SpawnHolePiece()
    {
        int randomState = UnityEngine.Random.Range(0, SelectedArray.Length);

        GameObject holePieceObject = Instantiate(SelectedArray[randomState], transform.position, transform.rotation);

        HoleColliderScript[] coverColliders = holePieceObject.transform.GetComponentsInChildren<HoleColliderScript>();

        int placementOfConnector = 0;

        for (int i = 0; i < coverColliders.Length; i++)
        {
            coverColliders[i].GameControllerObject = GameController;

            if (!coverColliders[i].connected && !coverColliders[i].noHole) { placementOfConnector = i; }
        }

        for (int j = 0; j < coverColliders.Length; j++)
        {
            if (j != placementOfConnector && coverColliders[j].noHole) { }
        }

        //Debug.Log("Number of states: " + numberOfStates + "Current state: " + currentState + "Number of covered holes: " + numberOfCoveredSpots + "holes that should be open are: " + testint + "check "+ checkingint);
        
    }
}