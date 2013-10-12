using UnityEngine;
using System.Collections;

namespace EventHorizon.Graphics
{
    public enum FXmode { DestroyAtEnd, Loop, Once }

    public class Sprite : FX
    {
        public bool FixedRotation;
        public int height = 5;
        public int width = 5;

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

        public override void Play()
        {
            StartCoroutine(Animate(duration, mode));
        }

        IEnumerator Animate(float duration, FXmode mode)
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
                }

                mesh.uv = result;


                yield return new WaitForSeconds(timestep);
            }

            switch (mode)
            {
                case FXmode.DestroyAtEnd: Destroy(this.gameObject);
                    break;
                case FXmode.Loop: Play();
                    break;
                default:
                    break;
            }
        }

        public override void Create(Transform parent)
        {
            Vector3 position = parent.position;
            GameObject.Instantiate(gameObject, new Vector3(position.x, position.y, position.z - 0.1F), FixedRotation ? Quaternion.identity : Quaternion.Euler(0, 0, Random.Range(0, 360F)));
        }
    }

}