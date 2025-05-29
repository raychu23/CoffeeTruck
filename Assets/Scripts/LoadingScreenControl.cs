using UnityEngine;
using System.Collections;

public class LoadingScreenControl : MonoBehaviour
{

    public GameObject truckHolder;
    public float xDistanceToSlide;
    public float moveSpeed;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("LoadingBehavior");
    }

    private IEnumerator LoadingBehavior()
    {
        Vector3 startingTruckPos = truckHolder.transform.position;
        Vector3 endTruckPos = new Vector3(startingTruckPos.x + xDistanceToSlide, startingTruckPos.y, startingTruckPos.z);

        // have our truck thing slide across the screen
        while (Vector3.Distance(truckHolder.transform.position, endTruckPos) > 1f)
        {
            truckHolder.transform.position = Vector3.Lerp(truckHolder.transform.position, endTruckPos, Time.deltaTime * moveSpeed);
            yield return false;
        }

        // when it gets there

        // pause for another second
        yield return new WaitForSeconds(1f);

        // destroy this prefab
        Destroy(this.gameObject);
    }
}
