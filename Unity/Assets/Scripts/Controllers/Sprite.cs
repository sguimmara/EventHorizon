using UnityEngine;
using System.Collections;


namespace EventHorizon.Graphics
{
    public enum SpriteMode { DestroyAtEnd, Loop, Once }

    public class Sprite : MonoBehaviour
    {
        public bool PlayOnAwake;
        public bool FixedRotation;
        public int height = 5;
        public int width = 5;
        public float duration;
        public bool reversed;
        public SpriteMode mode;

        Vector2[] originalSet;
        Mesh mesh;

        void Awake()
        {
  
            mesh = GetComponent<MeshFilter>().mesh;

            Vector2[] result = new Vector2[mesh.uv.Length];

            for (int i = 0; i < mesh.uv.Length; i++)
            {
                Vector2 vNew = new Vector2((float)mesh.uv[i].x / width, (float)mesh.uv[i].y / height);
                result[i] = vNew;
            }

            mesh.uv = result;
            originalSet = result;

            if (PlayOnAwake)
                Play();
        }

        public void Play()
        {
            StartCoroutine(Animate(duration, reversed, mode));
        }

        IEnumerator Animate(float duration, bool reversed, SpriteMode mode)
        {
            int nOfStates = height * width;

            float timestep = duration / nOfStates;
            int currentY = height;

            for (int i = 0; i < nOfStates; i++)
            {
                Vector2[] result = new Vector2[originalSet.Length];

                int currentX = i % width;

                if (currentX == 0)
                    currentY--;

                for (int j = 0; j < originalSet.Length; j++)
                {
                    float newX = originalSet[j].x + currentX / (float)width;
                    float newY = originalSet[j].y + currentY / (float)height;

                    Vector2 vNew = new Vector2(newX, newY);
                    result[j] = vNew;
                    //sphere.transform.position = new Vector3(vNew.x, vNew.y, 0);
                }

                mesh.uv = result;


                yield return new WaitForSeconds(timestep);
            }

            switch (mode)
            {
                case SpriteMode.DestroyAtEnd: Destroy(this.gameObject);
                    break;
                case SpriteMode.Loop: Play();
                    break;
                default:
                    break;
            }
        }

        public void Create(Vector3 position)
        {
            GameObject.Instantiate(gameObject, new Vector3(position.x, position.y, position.z - 0.1F), FixedRotation ? Quaternion.identity : Quaternion.Euler(0, 0, Random.Range(0, 360F)));
        }
    }

}