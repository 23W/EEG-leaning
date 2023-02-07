namespace EEGCore.Processing.Filtering
{
    public static class FilterExtension
    {
        public static void ProcessInplace(this Filter filter, Data.Lead lead)
        {
            lead.Samples = filter.Process(lead.Samples);
        }
    }
}
