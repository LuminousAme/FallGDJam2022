using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    [SerializeField] private float oilPerSecond = 1.0f;
    private float currentOil;

    private bool burning = false;
    private bool acutalBurning = false;

    private bool raised = false;
    private bool acutalRaised = false;

    [SerializeField] ParticleSystem flame;
    [SerializeField] Light flameLight;

    private float activeIntensity;
    private float targetIntensity;
    private float activeRange;
    private float targetRange;

    private Vector3 activePosition;
    private Vector3 targetPosition;


    // Start is called before the first frame update
    void Start()
    {
        activeIntensity = flameLight.intensity;
        targetIntensity = activeIntensity;
        activeRange = flameLight.range;
        targetRange = activeRange;
        activePosition = transform.localPosition;
        targetPosition = activePosition;
        flame.Stop();

        if (ProgressManager.instance != null) currentOil = ProgressManager.instance.GetCurrentOil();
        else currentOil = 0f;

        Change();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentOil > 0.0f && Input.GetMouseButton(0))
        {
            burning = true;
        }
        else burning = false;

        if(burning != acutalBurning)
        {
            Change();
        }

        if (Input.GetMouseButton(1))
        {
            raised = true;
        }
        else raised = false;

        if (raised != acutalRaised) Raise();

        if (burning) currentOil -= oilPerSecond * Time.deltaTime;
        if (ProgressManager.instance != null) ProgressManager.instance.SetCurrentOil(currentOil);

        flameLight.intensity = MathUtils.Lerp(flameLight.intensity, targetIntensity, 5.0f * Time.deltaTime);
        flameLight.range = MathUtils.Lerp(flameLight.range, targetRange, 2.0f * Time.deltaTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, 2.0f * Time.deltaTime);

        if (flame.isPlaying && !burning) flame.Stop(); 
    }

    private void Change()
    {
        if(burning)
        {
            targetIntensity = activeIntensity;
            flame.Play();
        }
        else
        {
            targetIntensity = 0.0f;
            flame.Stop();
        }
        acutalBurning = burning;
    }

    private void Raise()
    {
        if(raised)
        {
            targetRange = 2.0f * activeRange;
            targetPosition = activePosition + (0.25f * Vector3.up);
        }
        else
        {
            targetRange = activeRange;
            targetPosition = activePosition;
        }
        acutalRaised = raised;
    }
}
