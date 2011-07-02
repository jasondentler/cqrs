using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Cqrs.Specs
{
    public class EventSourceHelper
    {

        private static ScenarioContext Context { get { return ScenarioContext.Current; } }

        public static void SetId<T>(Guid id, params string[] naturalId)
        {
            var key = BuildKey<T>(naturalId);
            Context[key] = id;
            Context[BuildKey<T>()] = id;
        }

        public static Guid GetId<T>(params string[] naturalId)
        {
            var key = BuildKey<T>(naturalId);
            return (Guid) Context[key];
        }

        private static string BuildKey<T>(params string[] naturalId)
        {
            return string.Join(";",
                               new[] {typeof (T).ToString()}
                                   .Union(naturalId));
        }

    }
}
