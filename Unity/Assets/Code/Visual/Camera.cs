using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    #region Attributes

    [SerializeField]
    private CinemachineVirtualCamera cam;

    private bool FollowUser;

    [SerializeField]
    private Transform User;

    private Transform Car;

    [SerializeField]
    private float ZoomDelta;
    #endregion

    private void Awake()
    {
        FollowUser = false;
    }
    public void ChangeCar(GameObject car)
    {
        Car = car.transform;
        ChangeFollow();
    }

    public void PlayerMovement(bool FollowUser)
    {
        this.FollowUser = FollowUser;
        ChangeFollow();
    }

    private void ChangeFollow()
    {
        if (FollowUser)
            cam.Follow = User;
        else
            cam.Follow = Car;
    }
    
    public void ZoomCamera(bool ZoomIn)
    {
        float zoomAmount = ZoomIn ? -ZoomDelta : ZoomDelta;
        cam.m_Lens.OrthographicSize += zoomAmount;
        cam.m_Lens.OrthographicSize = Mathf.Clamp(cam.m_Lens.OrthographicSize, 1.5f,20f);
    }
}
