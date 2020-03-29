using UnityEngine;

public class TileGraphArrow : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    
    public Tile From { get; set; }
    public Tile To { get; set; }

    private void Awake()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.alignment = LineAlignment.TransformZ;
        _lineRenderer.widthMultiplier = 0.05f;
    }

    // Start is called before the first frame update
    void Start()
    {
        var positions = new Vector3[]
        {
            new Vector3(From.transform.position.x, From.transform.position.y, From.transform.position.z - 1.0f),
            new Vector3(To.transform.position.x, To.transform.position.y, To.transform.position.z - 1.0f)
        };
        for(int i = 0; i < positions.Length; i++) positions[i] += new Vector3(0.5f, 0.5f);
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPositions(positions);
    }
}
