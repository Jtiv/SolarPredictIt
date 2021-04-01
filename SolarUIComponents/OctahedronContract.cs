using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctahedronContract : MonoBehaviour
{
    /// <summary>
    /// 
    ///     Floaty double-pyramid whos dimensions reflect the state of the contract
    ///     
    ///     Contract has 6 pts of data (best buy/sell no, best buy/sell yes, last trade, highest trade)
    ///     all scaling up to 1 making it easy for the data dummies (lookin at you, me).
    ///     
    ///     each vertex will reflect one of these points and scale its position from the center
    ///     relative to the deviation from the max of these entities.
    /// 
    /// </summary>


    private Mesh mesh;
    private MeshFilter meshfilter;
    private MeshRenderer MR;
    
    private Vector3[] Vertices; //6
    private Vector2[] UVs; //8
    private int[] Tris; //24
    private float W;
    private float H;

    [SerializeField]
    private float scaleFactor;

    public void OnEnable()
    {
        BasicOctahedron();
        Debug.Log("Octahedron Instantiated");
    }

    public void SetContract(Contract contract)
    {
        scaleFactor = 1f;
        AdjustOctahedron(Adjustment(contract._buySellPrices));
        Debug.Log("Octahedron Adjusted");
    }

    
    public void BasicOctahedron()
    {
        W = .5f;
        H = 1.5f;

        
        meshfilter = gameObject.AddComponent<MeshFilter>();
        MR = gameObject.AddComponent<MeshRenderer>();
        MR.material = new Material(Shader.Find("Standard"));
        gameObject.name = "octahedron";
        mesh = new Mesh();
        meshfilter.mesh = mesh;

        //lets makey the shapey!
        Vertices = new Vector3[6]
        {
            new Vector3(W, 0, 0),
            new Vector3(-W, 0, 0),
            new Vector3(0, H, 0),
            new Vector3(0, -H, 0),
            new Vector3(0, 0, W),
            new Vector3(0, 0, -W),
        };

        Tris = new int[24]
        {
          0,2,4,
          4,2,1,
          1,2,5,
          5,2,0,
          0,4,3,
          4,1,3,
          1,5,3,
          5,0,3,
        };

        UVs = new Vector2[Vertices.Length];

        for (int i = 0; i < UVs.Length; i++)
        {
            UVs[i] = new Vector2(Vertices[i].x, Vertices[i].z);
        }

        
        mesh.MarkDynamic();
        mesh.vertices = Vertices;
        mesh.uv = UVs;
        mesh.triangles = Tris;
        mesh.RecalculateNormals();

        
    }

    public void AdjustOctahedron(float[] adj)
    {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        for (int i = 0; i < adj.Length; i++)
        {
            Vertices[i] *= adj[i];
        }

        UVs = new Vector2[Vertices.Length];

        for (int i = 0; i < UVs.Length; i++)
        {
            UVs[i] = new Vector2(Vertices[i].x, Vertices[i].z);
        }

        mesh.MarkDynamic();
        mesh.vertices = Vertices;
        mesh.triangles = Tris;
        mesh.uv = UVs;
        mesh.RecalculateNormals();
    }

    private float[] Adjustment(string[] buysellprices)
    {
        float[] adjustments = new float[6];
        for (int i = 0; i < buysellprices.Length; i++)
        {
            //holy moly im a genius, this is so pretty
            //adjustments = is the buy/sell null? if so 0f if not parse the string to the value and scale it...

            adjustments[i] = buysellprices[i] is null ? 0f : (float.Parse(buysellprices[i]) + scaleFactor) * 10;

        }
        //rearrange to fit the vertices -- is this ugly?

        adjustments = new float[]
        {
            adjustments[3],
            adjustments[4],
            adjustments[0],
            adjustments[5],
            adjustments[1],
            adjustments[2],
        };


        return adjustments;
    }

}
