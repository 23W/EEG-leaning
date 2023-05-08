namespace EEGCore.Processing.Filtering
{
    public static class FilterExtension
    {
        public static void ProcessInplace(this IFilter filter, Data.Lead lead)
        {
            lead.Samples = filter.Process(lead.Samples);
        }

        public static void ProcessInplace(this IFilter filter, IEnumerable<Data.Lead> leads)
        {
            if (filter is FIRFilter originalFilter)
            {
                leads.AsParallel()
                     .ForAll(l => originalFilter.Clone().ProcessInplace(l));
            }
            else
            {
                foreach (var lead in leads)
                {
                    filter.ProcessInplace(lead);
                }
            }
        }
    }

    public static class FIRFilterExtensions
    {
        public static FIRFilter Clone(this FIRFilter filter)
        {
            var clone = new FIRFilter(filter.Window) { AutoUnshifting = filter.AutoUnshifting };
            return clone;
        }
    }
}
