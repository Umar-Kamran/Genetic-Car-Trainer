using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inputs : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// stores whether the user is controlling the camera or it is automatic
    /// </summary>
    public bool UserMovesCamera{get; private set;}

    /// <summary>
    /// Speed to move the camera
    /// </summary>
    private float Speed;

    /// <summary>
    /// position of the object
    /// </summary>
    private Transform transf;

    /// <summary>
    /// Camera to move
    /// </summary>
    private MoveCamera moveCamera;

    /// <summary>
    /// ai manager of this scene
    /// </summary>
    private AIManager AIManager;

    /// <summary>
    /// stores all keycodes of numbers
    /// </summary>
    private List<KeyCode> Numbers;

    #endregion

    #region Unity Methods
    private void Awake()
    {
        //if menu scene, no initialisation required
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
            return;

        //initialises input object
        Numbers = new List<KeyCode>
        {
            KeyCode.Alpha0, KeyCode.Alpha1, 
            KeyCode.Alpha2, KeyCode.Alpha3, 
            KeyCode.Alpha4, KeyCode.Alpha5, 
            KeyCode.Alpha6, KeyCode.Alpha7, 
            KeyCode.Alpha8, KeyCode.Alpha9
        };
        transf = GetComponent<Transform>();
        UserMovesCamera = false;
        AIManager = GameObject.FindGameObjectWithTag("manager").GetComponent<AIManager>();
        moveCamera = GameObject.FindGameObjectWithTag("manager").GetComponent<MoveCamera>();
        Speed = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
            ExitPressed();

        //if in menu script, camera cannot move
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
            return;

        //sets either user mode or automatic camera mode
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UserMovesCamera = false;
            moveCamera.PlayerMovement(UserMovesCamera);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            UserMovesCamera = true;
            moveCamera.PlayerMovement(UserMovesCamera);
        }


        if (Input.mouseScrollDelta.y != 0)
            moveCamera.ZoomCamera(Input.mouseScrollDelta.y > 0);


        if (Input.GetKeyDown(KeyCode.Space))
            AIManager.SetForceEvolve();


        for (int i = 0; i < Numbers.Count; i++)
        {
            if (Input.GetKeyDown(Numbers[i]))
            {
                Time.timeScale = i;
                break;
            }
        }

        //May cause program to greatly slow down or crash so recommended not to use
        if (Input.GetKeyDown(KeyCode.Backspace))
            Time.timeScale = 30f;
        else if (Input.GetKeyUp(KeyCode.Backspace))
            Time.timeScale = 1f;
    }

    private void FixedUpdate()
    {
        //moves camera
        if (UserMovesCamera && SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(0))
        {
            if (Input.GetKey(KeyCode.W))
                transf.Translate(new Vector3(0, Speed, 0));
            if (Input.GetKey(KeyCode.S))
                transf.Translate(new Vector3(0, -Speed, 0));
            if (Input.GetKey(KeyCode.A))
                transf.Translate(new Vector3(-Speed, 0, 0));
            if (Input.GetKey(KeyCode.D))
                transf.Translate(new Vector3(Speed, 0, 0));
        }

    }
    #endregion

    #region Methods
    /// <summary>
    /// Script called when esc is pressed, either exits to menu or exits the application
    /// </summary>
    private void ExitPressed()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
            Application.Quit();
        else
            SceneManager.LoadScene(0);
    }

    #endregion
}
