using UnityEngine;
using System.Collections;


namespace EventHorizon.Graphics
{
    public class Shockwave : GeometryFX
    {
        public float StartScale;
        public float EndScale;

        void Awake()
        {
            if (PlayOnAwake)
                Play();
        }

        IEnumerator Grow()
        {
            float f = 0;
            float t = 0;
            Vector3 originalScale = transform.localScale;
            Vector3 targetScale = originalScale * EndScale;
            Material originalMaterial = new Material(renderer.material);
            Color originalColor = originalMaterial.GetColor("_TintColor");
            Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0F);
            renderer.material = originalMaterial;

            while (f <= duration)
            {
                t = f / duration;
                transform.localScale = Vector3.Lerp(originalScale, targetScale, t);

                originalMaterial.SetColor("_TintColor", Color.Lerp(originalColor, targetColor, t));

                f += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            if (mode == FXmode.DestroyAtEnd)
                Destroy(gameObject);

            else if (mode == FXmode.Loop)
                Play();
        }

        public override void Play()
        {
            StartCoroutine(Grow());
        }
    }
}