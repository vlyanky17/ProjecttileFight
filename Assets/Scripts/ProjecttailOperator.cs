using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ProjecttailOperator : MonoBehaviour
{
    [SerializeField] private List<SphereTile> _spheres;
    [SerializeField] private List<PillarTile> _pillars;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Player _player;
    [SerializeField] private float _sphereSpeed;
    [SerializeField] private float _pillarSpeed;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _area;

    public SphereTile GetSphereFromPool()
    {
        var sphere = _spheres[0];
        if (sphere == null) return sphere;
        _spheres.Remove(sphere);
        sphere.transform.SetParent(_enemy.transform);
        sphere.transform.localPosition = Vector3.zero;
        sphere.gameObject.SetActive(true);
        sphere.Init(_sphereSpeed,this);
        return sphere;
    }

    public void GetSphereBack(SphereTile sphere)
    {
        _spheres.Add(sphere);
        sphere.transform.SetParent(transform);
        sphere.transform.localPosition = Vector3.zero;
        sphere.gameObject.SetActive(false);
    }

    public PillarTile GetPillarFromPool()
    {
        var pillar = _pillars[0];
        if (pillar == null) return pillar;
        _pillars.Remove(pillar);
        pillar.transform.SetParent(_enemy.transform);
        pillar.transform.localPosition = Vector3.zero;
        pillar.gameObject.SetActive(true);
        pillar.Init(_pillarSpeed, this);
        return pillar;
    }

    public void GetPillarBack(PillarTile pillar)
    {
        _pillars.Add(pillar);
        pillar.transform.SetParent(transform);
        pillar.transform.localPosition = Vector3.zero;
        pillar.gameObject.SetActive(false);
    }

    public void HitPlayer()
    {
        _player.REciveDamage(1);
    }

    public void UpdateHp(float hp)
    {
        _hpSlider.value = hp;
        if (hp == 0) CallDeathScreen();
    }

    private void CallDeathScreen()
    {
        _area.SetActive(false);
        _deathScreen.SetActive(true);
    }
}
