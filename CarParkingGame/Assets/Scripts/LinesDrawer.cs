
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LinesDrawer : MonoBehaviour
{
    [SerializeField] UserInput userInput;
    [SerializeField] int interactableLayer;

    private Line _currentLine;
    private Route _currentRoute;

    RaycastDetector raycastDetector = new();

    //Events:
    public UnityAction<Route, List<Vector3>> OnParkLinkedToLine;

    private void Start()
    {
        userInput.OnMouseDown += OnMouseDownHandler;
        userInput.OnMouseMove += OnMouseMoveHandler;
        userInput.OnMouseUp += OnMouseUpHandler;

    }

    // Begin draw ---------
    private void OnMouseDownHandler()
    {
        ContactInfo contactInfo = raycastDetector.Raycast(interactableLayer);

        if (contactInfo.contacted)
        {
            bool isCar = contactInfo.collider.TryGetComponent(out Car _car);

            if (isCar && _car.route.isActive)
            {
                _currentRoute = _car.route;
                _currentLine = _currentRoute.line;
                _currentLine.Initialize();
            }
        }
    }

    // Draw ---------
    private void OnMouseMoveHandler()
    {
        if (_currentRoute != null)
        {
            ContactInfo contactInfo = raycastDetector.Raycast(interactableLayer);
            if (contactInfo.contacted)
            {
                Vector3 newPoint = contactInfo.point;

                _currentLine.AddPoint(newPoint);

                bool isPark = contactInfo.collider.TryGetComponent(out Park _park);
                if (isPark)
                {
                    Route parkRoute = _park.route;
                    if (parkRoute == _currentRoute)
                    {
                        _currentLine.AddPoint(contactInfo.transform.position);                        
                    }
                    else
                    {
                        _currentLine.Clear(); 
                    }

                    OnMouseUpHandler();
                }
            }
        }
    }
    // End draw ----------
    private void OnMouseUpHandler()
    {
        if (_currentRoute != null)
        {
            ContactInfo contactInfo = raycastDetector.Raycast(interactableLayer);
            if (contactInfo.contacted)
            {
                bool isPark = contactInfo.collider.TryGetComponent(out Park _park);
                if (_currentLine.pointsCount <2 || !isPark)
                {
                    _currentLine.Clear();
                }
                else
                {
                    OnParkLinkedToLine?.Invoke(_currentRoute, _currentLine.points);
                    _currentRoute.Disactivate();
                }
            }
            else
            {
                _currentLine.Clear();
            }
        }
        ResetDrawer();
    }
    private void ResetDrawer()
    {
        _currentRoute = null;
        _currentLine = null;
    }
}
