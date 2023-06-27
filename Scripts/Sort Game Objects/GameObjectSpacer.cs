using UnityEngine;

public enum SpacingDirection
{
    Horizontal,
    Vertical,
    Depth
}

public enum HorizontalAlignment
{
    Center,
    Left,
    Right
}

public class GameObjectSpacer : MonoBehaviour
{
    public float spacing = 0f;                                 // The desired spacing between objects
    public SpacingDirection spacingDirection = SpacingDirection.Horizontal;   // The direction of spacing
    public HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center; // The horizontal alignment of child objects

    private Transform gameObjectsParent;
    private float previousSpacing;

    private void OnValidate()
    {
        gameObjectsParent = transform;

        if (spacing != previousSpacing)
        {
            previousSpacing = spacing;
            SpaceGameObjects();
        }
    }

    public void SpaceGameObjects()
    {
        if (gameObjectsParent == null)
        {
            Debug.LogWarning("No game objects parent assigned.");
            return;
        }

        int childCount = gameObjectsParent.childCount;
        if (childCount == 0)
        {
            Debug.LogWarning("No child objects found in the game objects parent.");
            return;
        }

        // Calculate the total space required for all objects
        float totalSpace = (childCount - 1) * spacing;

        // Calculate the starting position
        Vector3 startPosition = transform.position;

        // Calculate the offset based on the alignment
        float alignmentOffset = CalculateAlignmentOffset(totalSpace);

        // Move the objects with equal spacing
        Vector3 currentPosition = startPosition;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = gameObjectsParent.GetChild(i);

            // Calculate the position for the current object
            float childSpace = GetChildSpace(child);
            Vector3 newPosition = GetNewPosition(currentPosition, childSpace, alignmentOffset);

            // Move the object to the new position
            child.position = newPosition;

            // Update the current position
            currentPosition += GetDirectionVector() * (childSpace + spacing);
        }
    }

    private float GetChildSpace(Transform child)
    {
        Renderer renderer = child.GetComponent<Renderer>();
        if (renderer != null)
        {
            return GetSpaceValue(renderer.bounds.size);
        }
        else
        {
            Debug.LogWarning("Child object does not have a Renderer component.");
            return 0f;
        }
    }

    private Vector3 GetNewPosition(Vector3 currentPosition, float childSpace, float alignmentOffset)
    {
        Vector3 directionVector = GetDirectionVector();
        Vector3 alignedPosition = currentPosition + alignmentOffset * directionVector;

        float halfChildSpace = childSpace / 2f;
        return alignedPosition + directionVector * halfChildSpace;
    }

    private float GetSpaceValue(Vector3 size)
    {
        switch (spacingDirection)
        {
            case SpacingDirection.Horizontal:
                return size.x;
            case SpacingDirection.Vertical:
                return size.y;
            case SpacingDirection.Depth:
                return size.z;
            default:
                return 0f;
        }
    }

    private Vector3 GetDirectionVector()
    {
        switch (spacingDirection)
        {
            case SpacingDirection.Horizontal:
                return Vector3.right;
            case SpacingDirection.Vertical:
                return Vector3.up;
            case SpacingDirection.Depth:
                return Vector3.forward;
            default:
                return Vector3.zero;
        }
    }

    private float CalculateAlignmentOffset(float totalSpace)
    {
        float alignmentOffset = 0f;

        switch (horizontalAlignment)
        {
            case HorizontalAlignment.Center:
                alignmentOffset = -totalSpace / 2f;
                break;
            case HorizontalAlignment.Left:
                alignmentOffset = 0f;
                break;
            case HorizontalAlignment.Right:
                alignmentOffset = -totalSpace;
                break;
        }

        return alignmentOffset;
    }
}
