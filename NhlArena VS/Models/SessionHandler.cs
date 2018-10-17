using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace NhlArena_VS.Models
{
    public static class SessionHandler    //SessionExtensions example name -> https://www.c-sharpcorner.com/article/session-state-in-asp-net-core/
    {
        //use it like this: HttpContext.Session.GetComplexData<int>("id");

        public static T GetComplexData<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static void SetComplexData(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static void RemoveComplexData(this ISession session, string key)
        {
            session.Remove(key);
        }
    }
}
