namespace ShopManagementSystem
{
    public class Apriori
    {
        private List<List<string>> transactions;
        private double minSupport;
        private double minConfidence;
        public List<AssociationRule> associationRules;

        public Apriori(List<List<string>> transactions, double minSupport, double minConfidence)
        {
            this.transactions = transactions;
            this.minSupport = minSupport;
            this.minConfidence = minConfidence;
            this.associationRules = new List<AssociationRule>();
        }

        public List<AssociationRule> DoApriori()
        {
            var frequentItemsets = GenerateFrequentItemsets(transactions, minSupport);

            Dictionary<string, int> candidateItemsets;
            List<List<string>> subsets;
            candidateItemsets = frequentItemsets;

            for (int i = 0; i < frequentItemsets.Count; i++)
            {
                candidateItemsets = GenerateCandidateItemsets(candidateItemsets);
                candidateItemsets = CalculateSupportCount(candidateItemsets, transactions, minSupport);

                foreach (KeyValuePair<string, int> itemset in candidateItemsets)
                {
                    subsets = GenerateSubsets(itemset.Key);

                    foreach (var subset in subsets)
                    {
                        GenerateAssociationRule(subset, itemset.Key);
                    }
                }
            }

            return associationRules;
        }

        private static Dictionary<string, int> GenerateFrequentItemsets(List<List<string>> transactions, double minSupport)
        {
            Dictionary<string, int> itemSupportCount = new Dictionary<string, int>();

            foreach (var transaction in transactions)
            {
                foreach (var item in transaction)
                {
                    if (itemSupportCount.ContainsKey(item))
                        itemSupportCount[item]++;
                    else
                        itemSupportCount[item] = 1;
                }
            }

            var frequentItemsets = itemSupportCount
                .Where(kv => kv.Value >= minSupport)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            return frequentItemsets;
        }

        private static Dictionary<string, int> GenerateCandidateItemsets(Dictionary<string, int> frequentItemsets)
        {
            Dictionary<string, int> candidateItemsets = new Dictionary<string, int>();

            List<string> frequentItems = frequentItemsets.Keys.ToList();
            int frequentItemsCount = frequentItems.Count;

            for (int i = 0; i < frequentItemsCount; i++)
            {
                for (int j = i + 1; j < frequentItemsCount; j++)
                {
                    List<string> itemset1 = frequentItems[i].Split(',').ToList();
                    List<string> itemset2 = frequentItems[j].Split(',').ToList();

                    if (itemset1.Take(itemset1.Count - 1).SequenceEqual(itemset2.Take(itemset2.Count - 1)))
                    {
                        List<string> mergedItemset = itemset1.Concat(new[] { itemset2.Last() }).ToList();
                        string mergedItemsetKey = string.Join(",", mergedItemset);

                        if (!candidateItemsets.ContainsKey(mergedItemsetKey))
                            candidateItemsets.Add(mergedItemsetKey, 0);
                    }
                }
            }

            return candidateItemsets;
        }

        private static Dictionary<string, int> CalculateSupportCount(Dictionary<string, int> candidateItemsets, List<List<string>> transactions, double minSupport)
        {
            Dictionary<string, int> goodCandidateItemsets = new Dictionary<string, int>();

            foreach (var transaction in transactions)
            {
                foreach (var candidateItemset in candidateItemsets.Keys.ToList())
                {
                    List<string> candidateItems = candidateItemset.Split(',').ToList();

                    if (candidateItems.All(transaction.Contains))
                    {
                        candidateItemsets[candidateItemset]++;
                    }
                }
            }

            goodCandidateItemsets = candidateItemsets
                .Where(kv => kv.Value >= minSupport)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            return goodCandidateItemsets;
        }

        private List<List<string>> GenerateSubsets(string itemset)
        {
            List<List<string>> subsets = new List<List<string>>();
            List<string> items = itemset.Split(',').ToList();
            int itemSize = items.Count;
            GenerateSubsetsRecursive(items, 0, new List<string>(), subsets, itemSize);
            return subsets;
        }

        private void GenerateSubsetsRecursive(List<string> items, int index, List<string> currentSubset, List<List<string>> subsets, int itemSize)
        {
            if (itemSize > 2)
            {
                if (currentSubset.Count > 0 && currentSubset.Count < items.Count && currentSubset.Count > 1)
                {
                    subsets.Add(currentSubset);
                }
            }
            else
            {
                if (currentSubset.Count > 0 && currentSubset.Count < items.Count)
                {
                    subsets.Add(currentSubset);
                }
            }

            for (int i = index; i < items.Count; i++)
            {
                List<string> newSubset = new(currentSubset)
                {
                    items[i]
                };
                GenerateSubsetsRecursive(items, i + 1, newSubset, subsets, itemSize);
            }
        }

        private void GenerateAssociationRule(List<string> subset, string itemset)
        {
            AssociationRule rule = new()
            {
                Antecedent = subset,
                Consequent = itemset.Split(',').Except(subset).ToList(),
                Confidence = CalculateConfidence(subset, itemset),
                Support = CalculateSupportCount(itemset)
            };

            if (rule.Confidence >= minConfidence)
                associationRules.Add(rule);
        }

        private double CalculateSupportCount(string item)
        {
            double count = 0;

            foreach (var transaction in transactions)
            {
                List<string> items = item.Split(',').ToList();

                if (items.All(transaction.Contains))
                {
                    count++;
                }
            }

            return count;
        }

        private double CalculateConfidence(List<string> subset, string itemset)
        {
            double confidence = 0;
            string subSet = string.Join(",", subset);

            confidence = CalculateSupportCount(itemset) / CalculateSupportCount(subSet);

            return confidence;
        }
    }
}
