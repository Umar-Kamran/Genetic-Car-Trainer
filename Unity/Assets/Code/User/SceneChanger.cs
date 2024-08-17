using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    #region Attributes

    /// <summary>
    /// stores the new car count
    /// </summary>
    private int CarCount;

    /// <summary>
    /// stores a reference to the text input field
    /// </summary>
    [SerializeField]
    private TMP_InputField CarCountInput;

    /// <summary>
    /// stores the new topology
    /// </summary>
    private uint[] Topology;

    /// <summary>
    /// stores a reference to the text input field
    /// </summary>
    [SerializeField]
    private TMP_InputField TopologyInput;

    /// <summary>
    /// indicates to destroy this object
    /// </summary>
    private bool destroyNow;
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this);
        destroyNow = false;
    }

    private void FixedUpdate()
    {
        if (destroyNow)
        {
            Destroy(GameObject.Find("destroyManager"));
        }
    }

    #region Methods
    /// <summary>
    /// Changes the scene to another track
    /// </summary>
    /// <param name="index">index of build scene</param>
    public void ChangeScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    /// <summary>
    /// When level changes, provides the ai manager with default values for topology and car count
    /// </summary>
    /// <param name="level"></param>
    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
            return;
        GameObject manager = GameObject.FindGameObjectWithTag("manager");
        manager.SetActive(false);   
        if (manager != null)
        {
            
            AIManager aIManager = manager.GetComponent<AIManager>();
            if (Topology != null && Topology.Length > 2)
            {
                aIManager.SetLayerShaping(Topology);
            }
            aIManager.SetCarCount(CarCount);
            aIManager.Init();
            manager.SetActive(true);
        }
        destroyNow = true;
    }

    /// <summary>
    /// sets the car count to inputted value
    /// </summary>
    public void SetCarCount()
    {
        try
        {
            string count = CarCountInput.text;
            CarCount = int.Parse(count);
        }
        catch (System.Exception)
        {

            CarCount = 120;
        }
    }

    /// <summary>
    /// sets the topology to the inputted topology, maintaing the correct input nodes and ouptut nodes needed
    /// </summary>
    public void SetTopology()
    {
        try
        {
            string topology = TopologyInput.text;
            string[] t = topology.Split(",");
            Topology = new uint[t.Length + 2];
            Topology[0] = 5;
            Topology[t.Length + 1] = 2;
            for (int i = 0; i < t.Length; i++)
            {
                Topology[i + 1] = uint.Parse(t[i]);
            }
        }
        catch (System.Exception)
        {

            Topology = null;
        }
    }
    #endregion
}
