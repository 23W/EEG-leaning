﻿namespace EEGCore.Data
{
    public enum LeadType
    {
        Unknown,
        Frontal,
        Central,
        Temporal,
        Parietal,
        Occipital,

        IndependenctComponent,
    }

    public class Lead
    {
        public string Name { get; set; } = string.Empty;

        public LeadType LeadType { get; set; } = LeadType.Unknown;

        public double[] Samples { get; set; } = new double[0];
    }

    public class Record
    {
        public string Name { get; set; } = string.Empty;

        public List<Lead> Leads { get; set; } = new List<Lead>();

        public double SampleRate { get; set; } = 128;

        public int LeadsCount => Leads.Count();

        public int Duration => Leads.FirstOrDefault()?.Samples.Length ?? 0;
    }

    public class ICARecord : Record
    {
    }
}