using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ParticleSetup : MonoBehaviour
{
    public InputAction setTarget;

    public static float gravity = 9.81f;

    private int _numParticles;
    private float _particleSpacing;
    private float _particleSize;

    private float smoothingRadius;

    [SerializeField]
    private Transform particlesParent;

    public int NumParticles
    {
        get { return _numParticles; }
        set
        {
            if (_numParticles == value) return;
            _numParticles = value;
            OnNumParticleChange?.Invoke(_numParticles);
        }
    }

    public float ParticleSize
    {
        get { return _particleSize; }
        set
        {
            if (_particleSize == value) return;
            _particleSize = value;
            OnParticleSizeChange?.Invoke(_particleSize);
        }
    }

    public float ParticleSpacing
    {
        get { return _particleSpacing; }
        set
        {
            if (_particleSpacing == value) return;
            _particleSpacing = value;
            OnParticleSpacingChange?.Invoke(_particleSpacing);
        }
    }

/*    public float SmoothingRadius
    {
        get { return _smoothingRadius; }
        set
        {
            if (_smoothingRadius == value) return;
            _smoothingRadius = value;
            OnSmoothingRadiusChange?.Invoke(_smoothingRadius);
        }
    }*/

    public delegate void OnNumParticleChangeDelegate(int newVal);
    public event OnNumParticleChangeDelegate OnNumParticleChange;

    public delegate void OnParticleSpacingChangeDelegate(float newVal);
    public event OnParticleSpacingChangeDelegate OnParticleSpacingChange;

    public delegate void OnParticleSizeChangeDelegate(float newVal);
    public event OnParticleSizeChangeDelegate OnParticleSizeChange;

/*    public delegate void OnSmoothingRadiusChangeDelegate(float newVal);
    public event OnSmoothingRadiusChangeDelegate OnSmoothingRadiusChange;*/

    // 
    private Vector3[] positions;

    [SerializeField]
    private GameObject sphereObject;

    private void Awake()
    {
        setTarget.performed += context => { OnSetTarget(context); };
    }

    public void OnSetTarget(InputAction.CallbackContext context)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        // Vector2 mousePosWorld = Camera.main.ScreenToViewportPoint(mousePos);
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast (ray, out hit, 100))
        {
            CalculateDensityAtPoint(hit.transform.position);
        }
        // Debug.Log(mousePosWorld);
    } 

    private void Start()
    {
        if (OnNumParticleChange == null) OnNumParticleChange += NumParticleChangeHandler;
        if (OnParticleSpacingChange == null) OnParticleSpacingChange += ParticleSpacingChangeHandler;
        if (OnParticleSizeChange == null) OnParticleSizeChange += ParticleSizeChangeHandler;
        // if (OnSmoothingRadiusChange == null) OnSmoothingRadiusChange += SmoothingRadiusChangeHandler;

        NumParticles = 16;
        ParticleSpacing = 0.2f;
    }

    private void GenerateParticles()
    {
        positions = new Vector3[NumParticles];
        int particlesPerRow = (int)Mathf.Sqrt(NumParticles);
        int particlesPerCol = (NumParticles - 1) / particlesPerRow + 1;

        sphereObject.transform.localScale = new Vector3(ParticleSize, ParticleSize, ParticleSize);
        float spacing = sphereObject.GetComponent<MeshRenderer>().bounds.size.x + ParticleSpacing;

        for (int i = 0; i < NumParticles; i++)
        {
            float x = (i % particlesPerRow - particlesPerRow / 2f + 0.5f) * spacing;
            float y = (i / particlesPerRow - particlesPerCol / 2f + 0.5f) * spacing;
            positions[i] = new Vector2(x, y);
        }

        foreach (Vector2 pos in positions)
        {
            Instantiate(sphereObject, pos, Quaternion.identity, particlesParent);
            // calculate density
            // 

        }
    }

    private float SmoothingKernel(float radius, float dist)
    {
        return Mathf.Exp(Mathf.Pow(dist, 2) * -1 / (2 * Mathf.Pow(radius, 2)));
    }

    private float CalculateDensity(Vector2 target, float mass = 1)
    {
        float density = 0;

        foreach (Vector2 pos in positions)
        {
            float distToTarget = (pos - target).magnitude; 
            // particle size != smoothing radius!
            float influence = SmoothingKernel(smoothingRadius, distToTarget);
            density += mass * influence;
        }

        return density;
    }

    private float CalculateDensityAtPoint(Vector2 point)
    {
        return CalculateDensity(point);
    }

    public void SetSmoothingRadius(Slider slider)
    {
        smoothingRadius = slider.value;
    }

    IEnumerator ResetParticles()
    {
        while (particlesParent.childCount > 0) {
            DestroyImmediate(particlesParent.GetChild(0).gameObject);
            yield return null;
        }
        Physics.autoSimulation = false;
        GenerateParticles();
    }

    public void TurnPhysicsOn()
    {
        Physics.autoSimulation = true;
    }

    private void NumParticleChangeHandler(int num)
    {
        StartCoroutine(ResetParticles());
    }

    private void ParticleSpacingChangeHandler(float num)
    {
        StartCoroutine(ResetParticles());
    }

    private void ParticleSizeChangeHandler(float num)
    {
        StartCoroutine(ResetParticles());
    }

    public void SetNumParticle(Slider slider)
    {
        NumParticles = Mathf.RoundToInt(slider.value);
    }

    public void SetParticleSpacing(Slider slider)
    {
        ParticleSpacing = slider.value;
    }

    public void SetParticleSize(Slider slider)
    {
        ParticleSize = slider.value;
    }

    void Update()
    {

    }

    public void OnEnable()
    {
        setTarget.Enable();
    }

    public void OnDisable()
    {
        setTarget.Disable();
    }
}
