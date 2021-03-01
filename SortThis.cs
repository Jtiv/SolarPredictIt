using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SortThis
{
    
    public static List<MarketData> List(List<MarketData> input)
    {
        Debug.Log("head of list reads" + input[0]);
        int c = input.Count;

        for (int i = 0; i < c - 1; i++)
        {
            int j = i;
            while (j > 0 && input[j - 1]._ID > input[j]._ID)
            {
                Swap(input[j], input[j - 1]);
                j = j - 1;
            }

        }

        Debug.Log("new head of list reads" + input[0]);

        return input;

    }


    private static void Swap(object a, object b)
    {
        var temp = a;
        a = b;
        b = temp;
    }
}
