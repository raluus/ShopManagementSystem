namespace ShopManagementSystem
{
    public class AssociationRule
    {
        public List<string> Antecedent { get; set; }
        public List<string> Consequent { get; set; }
        public double Confidence { get; set; }

        public double Support { get; set; }

        public AssociationRule(List<string> antecedent, List<string> consequent, double confidence,double support)
        {
            Antecedent = antecedent;
            Consequent = consequent;
            Confidence = confidence;
            Support = support;
        }

        public AssociationRule()
        {

        }
    }
}
