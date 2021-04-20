using System;
using System.Collections.Generic;
using System.IO;
using Moyasar;
using Moyasar.Models;
using Moyasar.Services;
using Newtonsoft.Json;

namespace MoyasarTest.Helpers
{   
    public static class FixtureHelper
    {
        public static string GetJsonObjectFixture(string filename, Dictionary<string, object> overrides = null)
        {
            var rawJson = File.ReadAllText(filename);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(rawJson);

            if (overrides == null)
            {
                return JsonConvert.SerializeObject(dict);
            }
            
            foreach (var pair in overrides)
            {
                dict[pair.Key] = pair.Value;
            }

            return JsonConvert.SerializeObject(dict);
        }
    }
}