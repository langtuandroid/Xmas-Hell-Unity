using System.Collections;
using UnityEngine;

public class MenuCloud : MonoBehaviour
{
    // TODO: Find a way to display RandomRange properly in the editor
    public float RandomSpeedRangeMin;
    public float RandomSpeedRangeMax;
    public float RandomVerticalPositionRangeMin;
    public float RandomVerticalPositionRangeMax;
    public float RandomDelayMin;
    public float RandomDelayMax;

    private float _speedFactor;

	void Start ()
    {
        StartCoroutine(CrossScreenHorizontaly());
    }

    IEnumerator CrossScreenHorizontaly()
    {
        var factor = Random.value < .5 ? 1 : -1;
        var newPosition = transform.position;

        _speedFactor = Random.Range(RandomSpeedRangeMin, RandomSpeedRangeMax) * factor;

        newPosition.x = (factor < 0) ? 10 : -10;
        newPosition.y = Random.Range(RandomVerticalPositionRangeMin, RandomVerticalPositionRangeMax);
        transform.position = newPosition;

        yield return new WaitForSeconds(Random.Range(RandomDelayMin, RandomDelayMax));

        while ((_speedFactor < 0 && transform.position.x > -10) ||
              (_speedFactor > 0 && transform.position.x < 10))
        {
            newPosition.x += _speedFactor * Time.deltaTime;
            transform.position = newPosition;

            yield return null;
        }

        StartCoroutine("CrossScreenHorizontaly");
    }
}
