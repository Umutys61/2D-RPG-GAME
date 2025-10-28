using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Vector3[] points;

    public Vector3[] Points => points;
    public Vector3 EntityPosition { get; set; }
    private bool GameStarted;

    public void Start()
    {
        EntityPosition = transform.position;
        GameStarted = true;
    }
    public Vector3 GetPosition(int pointIndex)
    {
        return  EntityPosition + points[pointIndex]; 
    }
    private void OnDrawGizmos()
    {
        if (GameStarted == false && transform.hasChanged)
        {
            EntityPosition = transform.position;

        }

    }


}
