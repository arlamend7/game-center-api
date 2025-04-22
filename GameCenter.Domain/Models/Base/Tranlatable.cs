using System.Collections.Generic;

namespace GameCenter.Domain.Models.Base
{

    public enum LanguageEnum
    {
        English,
        French,
        Portuguese,
        Spanish
    }

    public class Translation<T> : Dictionary<LanguageEnum, T> {}
}