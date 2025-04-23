using UnityEngine;
using System.Linq;

public class Worker : MonoBehaviour
{
    public BoxTypes boxToSort;
    public float moveSpeed = 2f;
    public Transform deposit;

    private Transform targetBox;
    private bool getBox = false;
    void Start()
    {
        FindClosestBox();
    }

    void Update()
    {
        if (targetBox != null && !getBox)
        {
            MoveToTarget();
        }
        if (getBox)
        {
            MoveToDeposit();
        }
    }

    void FindClosestBox()
    {
        BoxType[] allBoxes = FindObjectsOfType<BoxType>();
        BoxType[] matchingBoxes = allBoxes
            .Where(box => box.boxType == boxToSort)
            .ToArray();

        if (matchingBoxes.Length == 0)
        {
            Debug.Log("No matching boxes found.");
            return;
        }

        BoxType closest = matchingBoxes
            .OrderBy(box => Vector3.Distance(transform.position, box.transform.position))
            .First();
        Debug.Log("CLOSEST " + boxToSort + ": " + closest.gameObject.transform.GetChild(0).transform);
        targetBox = closest.gameObject.transform.GetChild(0).transform;
    }

    void MoveToTarget()
    {
        Vector3 targetPosition = new Vector3(targetBox.position.x, transform.position.y, targetBox.position.z); // ignore le Y
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > 0.75f)
        {
            // Regarde vers la direction sans incliner la tête
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);

            // Avance
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            Transform boxParent = targetBox.parent;
            boxParent.SetParent(transform);
            getBox = true;
        }
    }

    void MoveToDeposit()
    {
        Vector3 targetPosition = new Vector3(deposit.position.x, transform.position.y, deposit.position.z); // ignore le Y
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > 0.75f)
        {
            // Regarde vers la direction sans incliner la tête
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);

            // Avance
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            Transform boxParent = targetBox.parent;
            boxParent.gameObject.GetComponent<BoxType>().enabled = false;
            boxParent.SetParent(null);
            getBox = false;
            FindClosestBox();
        }
    }

}
