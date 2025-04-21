using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using GameCenter.Common.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace GameCenter.Common.Entities
{

    public enum LanguageEnum
    {
        English,
        French,
        Portuguese,
        Spanish
    }

    public class Translation<T> : Dictionary<LanguageEnum, T> {}



    public class MyEntity : Translation<MyEntityTranslation>
    {
        public DateTime CreateAt { get; set; }
    }

    public class MyEntityTranslation 
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}