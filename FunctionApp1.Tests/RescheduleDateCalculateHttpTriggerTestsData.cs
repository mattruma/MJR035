using System;
using System.Collections;
using System.Collections.Generic;

namespace FunctionApp1.Tests
{
    public class RescheduleDateCalculateHttpTriggerTestsData : IEnumerable<object[]>
    {

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new DateTime(2019, 12, 18),
                "85026",
                new DateTime(2019, 12, 20)
            };

            yield return new object[] {
                new DateTime(2019, 12, 18),
                "75014",
                new DateTime(2019, 12, 21)
            };

            yield return new object[] {
                new DateTime(2019, 12, 18),
                "75063",
                new DateTime(2019, 12, 22)
            };

            yield return new object[] {
                new DateTime(2019, 12, 18),
                "76014",
                new DateTime(2019, 12, 23)
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
