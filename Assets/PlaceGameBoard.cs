using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class PlaceGameBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameBoard;
    public Text TextGameBoard;
    public ARGrid[,] Grids;
    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private bool placed = false;
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        TextGameBoard.gameObject.SetActive(true);
       
    }

    // Update is called once per frame

    void Update()
    {
        if(!placed)
        {

            if(Input.touchCount>0)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;

                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if(raycastManager.Raycast(touchPosition,hits,TrackableType.PlaneWithinPolygon))
                {
                    var hitPose = hits[0].pose;

                    gameBoard.SetActive(true);
                    gameBoard.transform.position = hitPose.position;

                  

                    placed = true;

                    planeManager.SetTrackablesActive(false);
                    TextGameBoard.gameObject.SetActive(false);

                }
            }
        }
        else
        {
            planeManager.SetTrackablesActive(false);
            TextGameBoard.gameObject.SetActive(false);
        }
    }
    public void AllowMoveGameBoard()
    {
        placed = false;
        planeManager.SetTrackablesActive(true);
        TextGameBoard.gameObject.SetActive(true);
    }
    public bool Placed()
    {
        return placed;
    }

}
