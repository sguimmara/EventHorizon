using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour
{

    public int nOfStates = 25;
    public int columnSize = 5;

    Mesh mesh;

    // Use this for initialization
    void Start()
    {

    }

    public void Play(float duration, bool reversed)
    {
        mesh = GetComponent<MeshFilter>().mesh;

        Vector2[] result = new Vector2[mesh.uv.Length];

        for (int i = 0; i < mesh.uv.Length; i++)
        {
            Vector2 vNew = new Vector2((float)mesh.uv[i].x / columnSize,(float)mesh.uv[i].y / columnSize);
            result[i] = vNew;
        }

        mesh.uv = result;
        StartCoroutine(Animate(duration, reversed, mesh.uv));
    }

    IEnumerator Animate(float duration, bool reversed, Vector2[] originalSet)
    {
        float timestep = duration / nOfStates;

        for (int i = 0; i < nOfStates; i++)
        {
            Vector2[] result = new Vector2[originalSet.Length];

            int row = i % columnSize;
            int column = columnSize - i / columnSize;

            for (int j = 0; j < originalSet.Length; j++)
            {
                float newX = originalSet[j].x + row / (float)columnSize;
                float newY = originalSet[j].y + column / (float)columnSize;

                Vector2 vNew = new Vector2(newX, newY);
                result[j] = vNew;
            }

            mesh.uv = result;
            yield return new WaitForSeconds(timestep);
        }
    }
}
