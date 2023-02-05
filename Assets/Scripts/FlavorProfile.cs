using System.Collections.Generic;
using System.Linq;
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
            Citrus = Random.value;
            Floral = Random.value;
            Sweet = Random.value;
            Exotic = Random.value;
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

            Debug.Log(this.GetAsArray());
            Debug.Log(other.GetAsArray());

            return Mathf.Sqrt(this.GetAsArray().Zip(other.GetAsArray(), (t, o) => Mathf.Pow(t - o, 2)).Sum());
        }

        public static FlavorProfile GetAverages(IEnumerable<FlavorProfile> flavorProfiles) {
            (float sweet, float sour, float salty, float bitter) values = (0f, 0f, 0f, 0f);

            foreach (FlavorProfile flavorProfile in flavorProfiles) {
                values.sweet += flavorProfile.Citrus;
                values.sour += flavorProfile.Floral;
                values.salty += flavorProfile.Sweet;
                values.bitter += flavorProfile.Exotic;
            }

            values.sweet /= flavorProfiles.Count();
            values.sour /= flavorProfiles.Count();
            values.salty /= flavorProfiles.Count();
            values.bitter /= flavorProfiles.Count();

            return new FlavorProfile(values.sweet, values.sour, values.salty, values.bitter);
        } 
    }
}
