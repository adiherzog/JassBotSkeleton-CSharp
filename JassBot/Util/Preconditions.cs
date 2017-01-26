using System;

namespace JassBot.Util
{
    class Preconditions
    {
        public static void CheckNotNull(object something)
        {
            if(something == null)
            {
                throw new ArgumentNullException();
            }
        }

        public static void CheckArgument(bool argument)
        {
            if (argument != true)
            {
                throw new ArgumentException();
            }
        }
    }
}
