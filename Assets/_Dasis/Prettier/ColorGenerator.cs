using Dasis.DesignPattern;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Dasis.Prettier
{
    public class ColorGenerator : Singleton<ColorGenerator>
    {
        [SerializeField]
        private int numberOfColors;

        [SerializeField]
        private List<Color> colors = new List<Color>();

        public List<Color> Colors => colors;

        [Button]
        public void GenerateColors()
        {
            colors.Clear();
            for (int i = 0; i < numberOfColors; i++)
            {
                Color color = new Color
                {
                    r = Random.Range(0.3f, 1f),
                    g = Random.Range(0.3f, 1f),
                    b = Random.Range(0.3f, 1f),
                    a = 1,
                };
                colors.Add(color);
            }
        }
    }
}
