using UnityEngine;

public class ArtifactTracer : MonoBehaviour
{
    public GameObject drawPrefab;
    private GameObject traceTrail;
    private Plane planeVec;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        planeVec = new Plane(Camera.main.transform.forward * -1, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            traceTrail = (GameObject)Instantiate(drawPrefab, this.transform.position, Quaternion.identity);
            traceTrail.transform.parent = GameObject.Find("DrawObjects").transform;

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float _dis;
            if (planeVec.Raycast(mouseRay, out _dis))
            {
                startPos = mouseRay.GetPoint(_dis);
            }
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float _dis;
            if (planeVec.Raycast(mouseRay, out _dis))
            {
                traceTrail.transform.position = mouseRay.GetPoint(_dis);
            }
        }
    }

    public void DestroyDrawPrefabs()
    {
        foreach(Transform draw in GameObject.Find("DrawObjects").transform)
        {
            GameObject.Destroy(draw.gameObject);
        }
    }
}
