using System.Collections;
using UnityEngine;

public class ClimateManager : MonoBehaviour
{
    [Header("Visual References")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private ParticleSystem rainParticles;

    [Header("Sunny Day Settings")]
    [SerializeField] private Color sunnyLightColor = new Color(1.0f, 0.95f, 0.85f);
    [SerializeField] private float sunnyIntensity = 1.3f;
    [SerializeField] private Color sunnyFogColor = new Color(0.6f, 0.8f, 0.9f);
    [SerializeField] private float sunnyFogDensity = 0.005f;

    [Header("Storm Settings")]
    [SerializeField] private Color stormLightColor = new Color(0.2f, 0.25f, 0.35f);
    [SerializeField] private float stormIntensity = 0.15f;
    [SerializeField] private Color stormFogColor = new Color(0.08f, 0.1f, 0.15f);
    [SerializeField] private float stormFogDensity = 0.065f;
    [SerializeField] private float maxRainEmissionRate = 500f;

    [Header("Lightning Settings")]
    [SerializeField] private float minLightningInterval = 5f;
    [SerializeField] private float maxLightningInterval = 15f;

    private float _degradationFactor = 1.0f;
    private float _currentIntensity;
    private Color _currentLightColor;
    private float _currentFogDensity;
    private Color _currentFogColor;
    private float _currentRainEmission;

    private bool _isLightningFlashing = false;
    private Coroutine _lightningCoroutine;

    private void Start()
    {
        if (directionalLight == null)
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (var l in lights)
            {
                if (l.type == LightType.Directional)
                {
                    directionalLight = l;
                    break;
                }
            }
        }

        if (rainParticles == null)
        {
            ParticleSystem[] systems = FindObjectsOfType<ParticleSystem>();
            foreach (var ps in systems)
            {
                if (ps.name.ToLower().Contains("rain") || ps.name.ToLower().Contains("lluvia"))
                {
                    rainParticles = ps;
                    break;
                }
            }
        }

        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;

        _currentIntensity = sunnyIntensity;
        _currentLightColor = sunnyLightColor;
        _currentFogDensity = sunnyFogDensity;
        _currentFogColor = sunnyFogColor;
        _currentRainEmission = 0f;

        StatManager.OnStatChanged += UpdateClimateTarget;
        
        UpdateClimateTarget();
    }

    private void OnDestroy()
    {
        StatManager.OnStatChanged -= UpdateClimateTarget;
    }

    private void UpdateClimateTarget()
    {
        if (StatManager.Instance == null) return;

        float currentSum = StatManager.Instance.currentStrength + 
                            StatManager.Instance.currentShieldStat + 
                            StatManager.Instance.currentMaxHandSize;

        _degradationFactor = Mathf.Clamp01(currentSum / 25.0f);
        
        Debug.Log($"[ClimateManager] Stats total: {currentSum}/25 | Factor degradación: {_degradationFactor:F2}");
    }

    private void Update()
    {
        float lerpSpeed = Time.deltaTime * 0.5f;

        float targetIntensity = Mathf.Lerp(stormIntensity, sunnyIntensity, _degradationFactor);
        Color targetLightColor = Color.Lerp(stormLightColor, sunnyLightColor, _degradationFactor);
        float targetFogDensity = Mathf.Lerp(stormFogDensity, sunnyFogDensity, _degradationFactor);
        Color targetFogColor = Color.Lerp(stormFogColor, sunnyFogColor, _degradationFactor);
        float targetRainEmission = Mathf.Lerp(maxRainEmissionRate, 0f, _degradationFactor);

        _currentIntensity = Mathf.MoveTowards(_currentIntensity, targetIntensity, lerpSpeed);
        _currentLightColor = Color.Lerp(_currentLightColor, targetLightColor, Time.deltaTime);
        _currentFogDensity = Mathf.MoveTowards(_currentFogDensity, targetFogDensity, lerpSpeed * 0.05f);
        _currentFogColor = Color.Lerp(_currentFogColor, targetFogColor, Time.deltaTime);
        _currentRainEmission = Mathf.MoveTowards(_currentRainEmission, targetRainEmission, Time.deltaTime * 50f);

        if (directionalLight != null && !_isLightningFlashing)
        {
            directionalLight.intensity = _currentIntensity;
            directionalLight.color = _currentLightColor;
        }

        RenderSettings.fogDensity = _currentFogDensity;
        RenderSettings.fogColor = _currentFogColor;

        if (rainParticles != null)
        {
            var emission = rainParticles.emission;
            emission.rateOverTime = _currentRainEmission;

            if (_currentRainEmission > 1f && !rainParticles.isPlaying)
            {
                rainParticles.Play();
            }
            else if (_currentRainEmission <= 1f && rainParticles.isPlaying)
            {
                rainParticles.Stop();
            }
        }

        if (_degradationFactor <= 0.4f)
        {
            if (_lightningCoroutine == null)
            {
                _lightningCoroutine = StartCoroutine(LightningRoutine());
            }
        }
        else
        {
            if (_lightningCoroutine != null)
            {
                StopCoroutine(_lightningCoroutine);
                _lightningCoroutine = null;
            }
        }
    }

    private IEnumerator LightningRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minLightningInterval, maxLightningInterval);
            yield return new WaitForSeconds(delay);

            if (_degradationFactor <= 0.4f)
            {
                yield return StartCoroutine(TriggerLightningFlash());
            }
        }
    }

    private IEnumerator TriggerLightningFlash()
    {
        _isLightningFlashing = true;
        if (directionalLight == null) yield break;

        float originalIntensity = _currentIntensity;
        Color originalColor = _currentLightColor;

        directionalLight.color = Color.white;
        directionalLight.intensity = 2.5f;
        yield return new WaitForSeconds(0.04f);

        directionalLight.intensity = originalIntensity * 0.4f;
        yield return new WaitForSeconds(0.06f);

        directionalLight.intensity = 1.8f;
        yield return new WaitForSeconds(0.04f);

        directionalLight.color = originalColor;
        directionalLight.intensity = originalIntensity;

        _isLightningFlashing = false;
    }
}
