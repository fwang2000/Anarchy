using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(Image))]
public class BezierFollow : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private Transform[] routes;

    private int routeToGo;
    private float tParam;
    private Vector2 togglePosition;
    private float speedModifier;
    private int positionTracker;

    private void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 0.5f;
        positionTracker = 0;
        transform.position = routes[0].GetChild(0).position;
    }

    private void Update()
    {
        // Camera.main.ScreenToWorldPoint(Input.mousePosition);
        /*
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }*/
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {
        Vector2 p0 = routes[routeNumber].GetChild(0).position;
        Vector2 p1 = routes[routeNumber].GetChild(1).position;
        Vector2 p2 = routes[routeNumber].GetChild(2).position;
        Vector2 p3 = routes[routeNumber].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            togglePosition = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            transform.position = togglePosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;

        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
            routeToGo = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition();
    }

    private void SetDraggedPosition()
    {
        transform.position = Input.mousePosition;
    }

    private Vector2 FindClosestPoint()
    {
        if (Vector2.Distance(Input.mousePosition, routes[0].GetChild(positionTracker).position) <
            Vector2.Distance(Input.mousePosition, routes[0].GetChild(positionTracker + 1).position))
        {
            return routes[0].GetChild(positionTracker).position;
        }
        else
        {
            return routes[0].GetChild(positionTracker++).position;
        }
    }
}
