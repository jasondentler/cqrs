using Ninject;
using TechTalk.SpecFlow;

namespace Example
{

    [Binding]
    public class Configuration
    {

        static Configuration()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private static ScenarioContext Context { get { return ScenarioContext.Current; } }

        [BeforeScenario("domain")]
        public void OnSetup()
        {
            var kernel = new StandardKernel(new CqrsModule());
            Context.Set<IKernel>(kernel);
        }

    }

}
