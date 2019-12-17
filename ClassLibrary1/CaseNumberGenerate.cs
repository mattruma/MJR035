using System;

namespace ClassLibrary1
{
    public class CaseNumberGenerate : ICaseNumberGenerate
    {
        private static readonly Random _random = new Random();

        public int Generate()
        {
            return _random.Next(10000, 999999);
        }
    }
}
