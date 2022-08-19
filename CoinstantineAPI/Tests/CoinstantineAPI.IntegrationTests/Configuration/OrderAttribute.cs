using System;

namespace CoinstantineAPI.IntegrationTests.Configuration
{
    /// <summary>
    /// Used by CustomOrderer
    /// </summary>
    public class OrderAttribute : Attribute
    {
        public int I { get; }

        public OrderAttribute(int i)
        {
            I = i;
        }
    }

}