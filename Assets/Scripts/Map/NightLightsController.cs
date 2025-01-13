using UnityEngine;
using System.Collections.Generic;

public class NightLightsController : MonoBehaviour
{
    public Light directionalLight; // Przypisz Directional Light
    public List<Light> nightLights; // Lista �wiate� do w��czania w nocy
    public List<ParticleSystem> torchParticles; // Lista system�w cz�steczkowych pochodni
    public float nightStartAngle = 180f; // K�t, po kt�rym zaczyna si� noc
    public float nightEndAngle = 360f; // K�t, po kt�rym ko�czy si� noc

    private void Update()
    {
        // Pobierz aktualny k�t rotacji Directional Light (w osi X)
        float lightAngle = directionalLight.transform.eulerAngles.x;

        // Sprawd�, czy jest noc (k�t pomi�dzy nightStartAngle a nightEndAngle)
        bool isNight = lightAngle >= nightStartAngle && lightAngle <= nightEndAngle;

        // W��cz lub wy��cz �wiat�a w zale�no�ci od pory dnia
        foreach (Light light in nightLights)
        {
            if (light != null) light.enabled = isNight;
        }

        // W��cz lub wy��cz modu�y Lights w systemach cz�steczkowych
        foreach (ParticleSystem ps in torchParticles)
        {
            if (ps != null)
            {
                // Pobierz modu� Lights systemu cz�steczkowego
                var lightsModule = ps.lights;
                lightsModule.enabled = isNight;
            }
        }
    }
}