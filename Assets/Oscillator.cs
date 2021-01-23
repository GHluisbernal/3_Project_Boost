using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float periodInSeconds = 3f;

    private Vector3 startingPossition;

    // Start is called before the first frame update
    void Start()
    {
        startingPossition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (periodInSeconds <= Mathf.Epsilon)
        {
            return;
        }
        var cycles = Time.time / periodInSeconds;
        const float tau = Mathf.PI * 2f;
        var rawSinWave = Mathf.Sin(cycles * tau); // -1 to 1

        var movementFactor = rawSinWave / 2f + 0.5f;
        var offset = movementVector * movementFactor;
        transform.position = startingPossition + offset;
    }
}
