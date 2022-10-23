using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLight : MonoBehaviour
{
    [SerializeField] float targetIntensity = 2f;
    [SerializeField] float targetRange = 20f;
    [SerializeField] float startingIntesnity = 0f;
    [SerializeField] float startingRange = 0f;
    [SerializeField] float insentitySpeed = 2f;
    [SerializeField] float rangeSpeed = 2f;
    [SerializeField] Light flameLight;

    // Start is called before the first frame update
    void Start()
    {
        flameLight.intensity = startingIntesnity;
        flameLight.range = startingRange;
    }

    // Update is called once per frame
    void Update()
    {
        flameLight.intensity = MathUtils.Lerp(flameLight.intensity, targetIntensity, insentitySpeed * Time.deltaTime);
        flameLight.range = MathUtils.Lerp(flameLight.range, targetRange, rangeSpeed * Time.deltaTime);
    }
}
