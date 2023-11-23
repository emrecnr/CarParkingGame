using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game Instance;

    [HideInInspector] public List<Route> readyRoutes = new();

    private int totalRoutes;

    public UnityAction<Route> OnCarEntersPark;
    public UnityAction OnCarCollision;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        totalRoutes = transform.GetComponentsInChildren<Route>().Length;
        OnCarEntersPark += OnCarEntersParkHandler;
        OnCarCollision += OnCarCollisionHandler;
    }
    private void OnCarCollisionHandler()
    {
        Debug.Log("GameOver");

        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        DOVirtual.DelayedCall(2f, () =>
        {
            SceneManager.LoadScene(currentLevel);

        });
    }
    private void OnCarEntersParkHandler(Route route)
    {
        route.car.StopDancingAnimation();
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        DOVirtual.DelayedCall(1.3f, () =>
        {
            if (nextLevel < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextLevel);
            }
            else
            {
                Debug.LogWarning("No next level to load");
            }
        });
    }
    public void RegisterRoute(Route route)
    {
        readyRoutes.Add(route);
        if (readyRoutes.Count == totalRoutes)
        {
            MoveAllCars();
        }
    }
    private void MoveAllCars()
    {
        foreach (Route route in readyRoutes)
        {
            route.car.Move(route.linePoints);
        }
    }
}
