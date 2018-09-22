using System;
using System.Linq;
using Children;
using Verse;

namespace HumanlikeLifeStages
{
    class ChildrenCrossMod
    {
        public readonly static HediffDef PregnancyDiscovered =
            DefDatabase<HediffDef>.GetNamedSilentFail("PregnancyDiscovered");


        public static bool isChildrenModOn()
        {
            return PregnancyDiscovered != null;
        }

        public static void Mated(Pawn donor, Pawn womb)
        {
            var actualWombGender = womb.gender;
            womb.gender = Gender.Female;

            var donorActualGender = donor.gender;
            donor.gender = Gender.Male;

            try
            {
                var assembly = typeof(BackstoryDef).Assembly;


                Type morePawnUtils = assembly.GetTypes().First(x => x.FullName.Contains("MorePawnUtil"));

                if (morePawnUtils == null)
                {
                    throw new Exception("Couldnt find Childern.MorePawnUtils in the assembly" +
                                        "<--- this is bad practice to control flow with an exception. Dont tell~");
                }

                bool didIt = false;
                foreach (var method in morePawnUtils.GetMethods().Where(aMethod => aMethod.Name.Contains("Loved")))
                {
                    var result = method.Invoke(null, new object[] {donor, womb, true});

                    if (result != null)
                    didIt = true;
                    break;
                }

                if (!didIt)
                {
                    throw new Exception("Coundnt find the Loved method in Children");
                }
            }
            catch (Exception)
            {
                //well that failed
                Mate.DefaultMate(donor, womb);
            }
            finally
            {
                donor.gender = donorActualGender;
                womb.gender = actualWombGender;
            }
        }
    }
}