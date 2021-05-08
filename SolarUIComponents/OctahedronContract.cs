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


    private MeshCollider meshcollider;
    private Mesh mesh;
    private MeshFilter meshfilter;
    private MeshRenderer MR;
    private Rigidbody rb;
    
    private Vector3[] Vertices; //6
    private Vector2[] UVs; //8
    private int[] Tris; //24
    private float W;
    private float H;

    [SerializeField]
    private float scaleFactor, planetGravResponseMod, degreesPerSecond, FloatingHeight;

    private Transform centerGrav;

    public void OnEnable()
    {
        BasicOctahedron();
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        meshcollider = gameObject.AddComponent<MeshCollider>();
        meshcollider.convex = true;
        Debug.Log("Octahedron Instantiated");
    }

    //remove on prod build
    private void OnDestroy()
    {
        Debug.Log("Oct Destroyed apparently");
    }

    private void Update()
    {
        if (centerGrav) Orbit();
    }

    public void SetContract(Contract contract)
    {
        scaleFactor = 1f;
        AdjustOctahedron(Adjustment(contract._buySellPrices));
        Debug.Log("Octahedron Adjusted");
    }

    public void SetGravPoint(Transform transform)
    {
        centerGrav = transform;
    }


    public void BasicOctahedron()
    {
        //set width and height here, but can be set from inspector if desired.
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

    public void AdjustOctahedron(float[] Adjustment)
    {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        for (int i = 0; i < Adjustment.Length; i++)
        {
            Vertices[i] *= Adjustment[i];
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
    
    public void Orbit()
    {

        Vector3 subjGravityDirection = (centerGrav.position - rb.position);
        Debug.DrawRay(rb.position, subjGravityDirection, Color.red); // <--- delete later
        float singleStep = planetGravResponseMod * Time.fixedDeltaTime;
        Vector3 reAngle = Vector3.RotateTowards(rb.transform.forward, -subjGravityDirection, singleStep, 0.0f);
        rb.transform.rotation = Quaternion.RotateTowards(rb.transform.rotation, Quaternion.Euler(reAngle), singleStep);

        RaycastHit hitinfo;
        if (Physics.Raycast(rb.position, subjGravityDirection, out hitinfo, 1500f, 1 << 8))
        {
            rb.transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.Self);

            if (hitinfo.distance > FloatingHeight)
            {
                rb.AddForce(subjGravityDirection.normalized * (100 / 2) * Time.fixedDeltaTime);
            }

            else
            {
                rb.AddForce(-subjGravityDirection.normalized * (100 / 1) * Time.fixedDeltaTime);
            }
        }

    }
}
