using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhaustParticles : MonoBehaviour
{
    [SerializeField]
    private ArcadeKart arcadeKart;

    [SerializeField]
    private ParticleSystem[] exhaustParticles;

    [SerializeField]
    private BaseInput iInput;

    private Dictionary<ParticleSystem, float> _rateOverTimeMultipliers;

    private void Awake()
    {
        _rateOverTimeMultipliers = new Dictionary<ParticleSystem, float>();

        if (exhaustParticles != null )
        {
            foreach(var system in exhaustParticles )
            {
                _rateOverTimeMultipliers.Add(system, system.emission.rateOverTimeMultiplier);
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 inputs = iInput.GenerateInput();

        if (exhaustParticles != null )
        {
            foreach(var system in exhaustParticles )
            {
                var emissionMod = system.emission;
                float multiplier = _rateOverTimeMultipliers[system] * (1f + Mathf.Abs(inputs.y) * 2f);
                emissionMod.rateOverTimeMultiplier = multiplier;
            }
        }
    }
}
