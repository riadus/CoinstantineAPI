using System;

namespace CoinstantineAPI.Data
{
    public interface IProfileItem
    {
        string Username { get; set; }
        bool Validated { get; set; }
        DateTime? ValidationDate { get; set; }
    }
}
