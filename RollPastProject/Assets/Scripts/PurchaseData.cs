using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PurchaseData
{
    public int[] ownBoards = { 1, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int[] ownBalls = { 1, 0, 0, 0, 0, 0 };
    public int[] ownBars = { 1, 0, 0, 0, 0, 0 };


    public PurchaseData()
    {
        ownBoards = new int[] { 1, 0, 0, 0, 0, 0 };
        ownBalls = new int[] { 1, 0, 0, 0, 0, 0 };
        ownBars = new int[] { 1, 0, 0, 0, 0, 0 };
    }

    public PurchaseData(int[] board, int[] ball, int[] bar)
    {
        ownBoards = board;
        ownBalls = ball;
        ownBars = bar;
    }

    public PurchaseData(int[] part, int setter)
    {
        switch (setter)
        {
            case 0:
                ownBoards = part;
                break;
            case 1:
                ownBalls = part;
                break;
            case 2:
                ownBars = part;
                break;
            default:
                break;
        }
    }
}
