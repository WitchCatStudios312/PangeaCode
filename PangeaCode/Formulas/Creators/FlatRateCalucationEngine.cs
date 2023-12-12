
namespace PangeaCode.Formulas.Creators
{
    // Concrete Creator: FlatRate
    public class FlatRateCalucationEngine : RateCalucationEngine
    {
        public override IFormula CreateFormula() //FactoryMethod
        {
            return new FlatRateFormula();
        }
    }
}
