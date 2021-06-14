namespace WebApi
{
    using System.Collections.Generic;

    public class AppSettings
    {
        /*
         {
  "ConnectionStrings": {
    "Default": "8U0gqCcA9Wi13CHQFJZujhQ+lIOamvORchmfpep5o2zfuSVZg7sa7AVN7gyTkSxqORfEJWIR7UTZ5n3vhf2UCOseV1wqbZY0LjS2eTWJ+71zD4lGURz/JZj8+2Pqzv3j"
  },
  "version": "测试version",
  "AppSettings": {
    "Str": "val",
    "num": 1,
    "arr": [
      1,
      2,
      3
    ],
    "subobj": {
      "a": "b"
    }
  }
}
         */

        public string Str { get; set; }

        public int num { get; set; }

        public List<int> arr { get; set; }

        public SubObj subobj { get; set; }
    }

    public class SubObj
    {
        public string a { get; set; }
    }
}
