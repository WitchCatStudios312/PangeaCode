
namespace PangeaCode.Formulas.Creators
{
    // Concrete Creator: FormulaB
    public class FormulaBCalucationEngine : RateCalucationEngine
    {
        public override IFormula CreateFormula() //FactoryMethod
        {
            return new FormulaB();
        }
    }
}
