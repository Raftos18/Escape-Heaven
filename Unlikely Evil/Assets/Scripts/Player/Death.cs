using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour
{
    public bool IsDead;
    public float ChangeColorRate = 1.0f;
    public float ChangeColorDelay = 0.2f;
    public bool CanRestart;

    Renderer renderer;
    private Color becomeWhite = new Color();
    private float[] color = new float[3] { 0, 0, 0 };

    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        CanRestart = false;
    }

    /// <summary>
    /// Coroutine for killing the player 
    /// </summary>
    /// <param name="material">The renderer's material</param>
    /// <returns></returns>
    public IEnumerator Kill()
    {
        IsDead = true;
        MainMenu.ShowScore = true;
        while (color[0] < 3)
        {
            yield return new WaitForSeconds(ChangeColorDelay);
            ChangeColor(renderer.material);
        }
        color[0] = 0;
        color[1] = 0;
        color[2] = 0;
        CanRestart = true;
    }

    /// <summary>
    /// Change the current color of the material
    /// </summary>
    /// <param name="material">The renderer's material</param>
    private void ChangeColor(Material material)
    {
        if (IsDead)
        {
            for (int i = 0; i < color.Length; i++)
                color[i] += (ChangeColorRate) * Time.deltaTime;

            becomeWhite.r = color[0];
            becomeWhite.b = color[1];
            becomeWhite.g = color[2];

            material.SetColor("_Color", becomeWhite);
        }
    }
}
