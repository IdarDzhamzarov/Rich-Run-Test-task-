using UnityEngine;

public class Pathway: MonoBehaviour
{
    private int nextPoint = 1;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (var i = 0; i < transform.childCount - 1; i++)
        {
            var current = transform.GetChild(i).position;
            var next = transform.GetChild(i + 1).position;

            Gizmos.DrawLine(current, next);

            if (i == 0) Gizmos.DrawSphere(current, 0.3f);

            Gizmos.DrawSphere(next, 0.3f);
        }
    }

    public float GetTargetRotation(Vector3 position)
    {
        var forward = transform.GetChild(nextPoint).position - position;

        if (forward.magnitude < 0.1f) nextPoint += 1;
        return Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
    }

    public void Reset()
    {
        nextPoint = 1;
    }
}