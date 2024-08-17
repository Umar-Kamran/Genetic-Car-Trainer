using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    #region Attributes
    /// <summary>
    /// Stores possible sprites for the cars
    /// </summary>
    [SerializeField]
    private Sprite[] carSprites;

    /// <summary>
    /// Stores the sprite for the current best car
    /// </summary>
    [SerializeField]
    private Sprite bestCarSprite;

    /// <summary>
    /// Stores the gameObject of the previous best car
    /// </summary>
    private GameObject lastBestCar;

    private static System.Random random = new System.Random();
    #endregion

    /// <summary>
    /// Changes the sprite of the best car, so it can be easily differentiated
    /// </summary>
    /// <param name="bestCar">Game object of the best car</param>
    public void setBestCar(GameObject bestCar)
    {
        if (lastBestCar != bestCar)
        {
            //resets last best car to have a regular car sprite
            if (lastBestCar != null)
            {
                lastBestCar.GetComponent<SpriteRenderer>().sprite = carSprites[random.Next(0, carSprites.Length)];
                lastBestCar.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
            lastBestCar = bestCar;
            lastBestCar.GetComponent<SpriteRenderer>().sprite = bestCarSprite;
            lastBestCar.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    /// <summary>
    /// Sets the sprites of all cars to a random base sprite
    /// </summary>
    /// <param name="cars">Array of car gameObjects</param>
    public void SetSprites(GameObject[] cars)
    {
        lastBestCar = null;
        foreach (GameObject car in cars)
            car.GetComponent<SpriteRenderer>().sprite = carSprites[random.Next(0, carSprites.Length)];
    }
}
