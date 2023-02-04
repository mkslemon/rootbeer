using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ggj.rootbeer
{
    [System.Serializable]
    public class FlavorProfile
    {
        public float Sweet { get; set; }
        public float Sour { get; set; }
        public float Salty { get; set; }
        public float Bitter { get; set; }

        public FlavorProfile(float sweet = 0f, float sour = 0f, float salty = 0f, float bitter = 0f)
        {
            Sweet = sweet;
            Sour = sour;
            Salty = salty;
            Bitter = bitter;
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
            Sweet = x[0];
            Sour = x[1];
            Salty = x[2];
            Bitter = x[3];
        }

        public void GenerateRandomProfile()
        {
            // consider distribution other than uniform for certain tastes
            Sweet = Random.value;
            Sour = Random.value;
            Salty = Random.value;
            Bitter = Random.value;
        }
        public float[] GetAsArray()
        {
            return new float[] { Sweet, Sour, Salty, Bitter };
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
            // returns distance
            return Mathf.Sqrt(this.GetAsArray().Zip(other.GetAsArray(), (t, o) => Mathf.Pow(t - o, 2)).Sum());
        }

        public static FlavorProfile GetAverages(IEnumerable<FlavorProfile> flavorProfiles) {
            (float sweet, float sour, float salty, float bitter) values = (0f, 0f, 0f, 0f);

            foreach (FlavorProfile flavorProfile in flavorProfiles) {
                values.sweet += flavorProfile.Sweet;
                values.sour += flavorProfile.Sour;
                values.salty += flavorProfile.Salty;
                values.bitter += flavorProfile.Bitter;
            }

            values.sweet /= flavorProfiles.Count();
            values.sour /= flavorProfiles.Count();
            values.salty /= flavorProfiles.Count();
            values.bitter /= flavorProfiles.Count();

            return new FlavorProfile(values.sweet, values.sour, values.salty, values.bitter);
        } 
    }
}
