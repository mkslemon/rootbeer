using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace ggj.rootbeer
{
    [System.Serializable]
    public class FlavorProfile
    {
        public float Citrus { get; set; }
        public float Floral { get; set; }
        public float Sweet { get; set; }
        public float Exotic { get; set; }

        public FlavorProfile(float citrus = 0f, float floral = 0f, float sweet = 0f, float exotic = 0f)
        {
            Citrus = citrus;
            Floral = floral;
            Sweet = sweet;
            Exotic = exotic;
        }
        public FlavorProfile(float[] x)
        {
            // ensure the length is correct
            // unfortunately this will need to be updated if the number of tastes changes
            int expectedLength = 4;
            if (x.Length != expectedLength)
            {
                throw new System.IndexOutOfRangeException("Incorrect number of tastes found in Flavor Profile. Expected: " + expectedLength + " Found: " + x.Length); // should only be possible if there is an issue with this code. Should instead be a unit test.
            }
            Citrus = x[0];
            Floral = x[1];
            Sweet = x[2];
            Exotic = x[3];
        }

        public void GenerateRandomProfile()
        {
            // consider distribution other than uniform for certain tastes
            Citrus = UnityEngine.Random.value;
            Floral = UnityEngine.Random.value;
            Sweet = UnityEngine.Random.value;
            Exotic = UnityEngine.Random.value;
        }
        public float[] GetAsArray()
        {
            return new float[] { Citrus, Floral, Sweet, Exotic };
        }

        public FlavorProfile GetAverage(FlavorProfile other)
        {
            // can always assume the length of the flavor profiles will be the same. No need to check.
            // get averages accross both arrays
            float[] fpAverage = this.GetAsArray().Zip(other.GetAsArray(), (t, o) => (t + o) / 2f).ToArray();
            return new FlavorProfile(fpAverage);
        }
        public float GetDistance(FlavorProfile other)
        {
            // TODO Dan would like to suggest using Cosine similarity not Euclidian distance!
            // returns distance

            List<float> myList = new List<float>();
            myList.AddRange(this.GetAsArray());
            List<float> otherList = new List<float>();
            otherList.AddRange(other.GetAsArray());
            Debug.Log(this.GetAsArray());
            Debug.Log(other.GetAsArray());


            //trying to return cosin sim between lists [-1,1] normalized to the [0,1] space
            //return (GetCosineSimilarity(myList, otherList)/2) + .5f;
           return Mathf.Sqrt(this.GetAsArray().Zip(other.GetAsArray(), (t, o) => Mathf.Pow(t - o, 2)).Sum()) / 2f; // constant for now todo
        }
        
        public override string ToString()
        {
            return "Citrus: " + Citrus + ", Floral: " + Floral + ", Sweet: " + Sweet + ", Exotic: " + Exotic;
        }

        //https://stackoverflow.com/questions/7560760/cosine-similarity-code-non-term-vectors idk what this does, but it's supposedly cosin similarity :V
        public static float GetCosineSimilarity(List<float> V1, List<float> V2)
        {
            int N = 0;
            N = ((V2.Count < V1.Count) ? V2.Count : V1.Count);
            float dot = 0.0f;
            float mag1 = 0.0f;
            float mag2 = 0.0f;
            for (int n = 0; n < N; n++)
            {
                dot += V1[n] * V2[n];
                mag1 += Mathf.Pow(V1[n], 2);
                mag2 += Mathf.Pow(V2[n], 2);
            }

            return dot / (Mathf.Sqrt(mag1) * Mathf.Sqrt(mag2));
        }

        public static FlavorProfile GetAverages(IEnumerable<FlavorProfile> flavorProfiles) {
            List<float[]> flavorArrays = flavorProfiles.Select(sel => sel.GetAsArray()).ToList();

            var AverageFlavor = Enumerable.Range(0, flavorArrays[0].Length).Select(i => flavorArrays.Select(flavor => flavor[i]).Sum() / flavorArrays.Count).ToArray();
            return new FlavorProfile(AverageFlavor);
        }
    }
}
