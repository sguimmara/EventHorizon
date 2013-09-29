using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour
{

    public int rowSize = 5;
    public int columnSize = 5;
    int nOfStates;

    Mesh mesh;

    public void Play(float duration, bool reversed, bool destroyAtEnd)
    {
        nOfStates = rowSize * columnSize;
        mesh = GetComponent<MeshFilter>().mesh;

        Vector2[] result = new Vector2[mesh.uv.Length];

        for (int i = 0; i < mesh.uv.Length; i++)
        {
            Vector2 vNew = new Vector2((float)mesh.uv[i].x / rowSize,(float)mesh.uv[i].y / columnSize);
            result[i] = vNew;
        }

        mesh.uv = result;
        StartCoroutine(Animate(duration, reversed, mesh.uv, destroyAtEnd));
    }

    IEnumerator Animate(float duration, bool reversed, Vector2[] originalSet, bool destroyAtEnd)
    {        
        float timestep = duration / nOfStates;

        for (int i = 0; i < nOfStates; i++)
        {
            Vector2[] result = new Vector2[originalSet.Length];

            int row = i % rowSize;
            int column = columnSize - i / columnSize;

            for (int j = 0; j < originalSet.Length; j++)
            {
                float newX = originalSet[j].x + row / (float)rowSize;
                float newY = originalSet[j].y + column / (float)columnSize;

                Vector2 vNew = new Vector2(newX, newY);
                result[j] = vNew;
            }

            mesh.uv = result;
            yield return new WaitForSeconds(timestep);
        }

        if (destroyAtEnd)
            Destroy(this.gameObject);

    }
}
