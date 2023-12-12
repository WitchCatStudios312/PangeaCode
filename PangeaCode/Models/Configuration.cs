using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PangeaCode.Countries;
using PangeaCode.Formulas.Creators;

namespace PangeaCode.Models
{
    public class Configuration
    {
        public ICountry Country { get; private set; }

        public RateCalucationEngine Engine { get; private set; }

        public Configuration(ICountry country, RateCalucationEngine engine)
        {
            Country = country;
            Engine = engine;
        }
    }
}
