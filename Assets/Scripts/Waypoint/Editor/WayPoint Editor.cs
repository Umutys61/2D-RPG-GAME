using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Waypoint))]
public class WayPointEditor : Editor
{
    private Waypoint waypointTarget => target as Waypoint;

    private void OnSceneGUI()
    {
        if (waypointTarget.Points.Length <= 0)
        {
            return;
        }
        Handles.color = Color.red;
        for (int i = 0; i < waypointTarget.Points.Length; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 currentPoint = waypointTarget.EntityPosition + waypointTarget.Points[i];
            Vector3 newPosition = Handles.FreeMoveHandle(currentPoint, 0.5f, Vector3.one * 0.5f, Handles.SphereHandleCap);

            GUIStyle text=new GUIStyle();
            text.fontStyle = FontStyle.Bold;
            text.fontSize = 16;
            text.normal.textColor = Color.black;
            Vector3 textPos = new Vector3(0.2f, -0.2f);
            Handles.Label(waypointTarget.EntityPosition + waypointTarget.Points[i] + textPos,$"{i+1}",text);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Free Move");
                waypointTarget.Points[i] = newPosition - waypointTarget.EntityPosition;
            }

        }   
    }
}
