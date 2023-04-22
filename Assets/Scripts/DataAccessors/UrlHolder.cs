using System.Collections;
using System.Collections.Generic;

namespace DataAccessors
{
    public class UrlHolder
    {
        public const string DATA_API_URL = "https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack";

        public string GetURL()
        {
            return DATA_API_URL;
        }
    }
}
