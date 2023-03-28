namespace EEGCore.Data
{
    public enum LeadType
    {
        Unknown,
        Frontal,
        Central,
        Temporal,
        Parietal,
        Occipital,
    }

    public enum LeadCode
    {
        Fp1,
        Fpz,
        Fp2,

        F7,
        F3,
        Fz,
        F4,
        F8,

        T3,
        C3,
        Cz,
        C4,
        T4,

        T5,
        P3,
        Pz,
        P4,
        T6,

        O1,
        Oz,
        O2
    }

    // Spherical EEG coordinates in degrees
    public class EEGCoordinate
    {
        public double Alpha { get; set; } = 0;
        public double Beta { get; set; } = 0;
        public double R { get; set; } = 1;
    }

    public class EEGSchemeLeadInfo
    {
        public LeadType LeadType { get; set; } = LeadType.Unknown;

        public EEGCoordinate Coordinate { get; set; } = new EEGCoordinate();
    }

    public static class EEGScheme
    {
        public static Dictionary<LeadCode, EEGSchemeLeadInfo> Scheme1020 => new Dictionary<LeadCode, EEGSchemeLeadInfo>()
        {
            {
                LeadCode.Fp1,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Frontal,
                    Coordinate = new EEGCoordinate() { Alpha = 18 }
                }
            },
            {
                LeadCode.Fpz,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Frontal,
                    Coordinate = new EEGCoordinate()
                }
            },
            {
                LeadCode.Fp2,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Frontal,
                    Coordinate = new EEGCoordinate() { Alpha = -18 }
                }
            },

            {
                LeadCode.F7,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Frontal,
                    Coordinate = new EEGCoordinate() { Alpha = 54 }
                }
            },
            {
                LeadCode.F3,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Frontal,
                    Coordinate = new EEGCoordinate() { Alpha = 45, Beta = 35 }
                }
            },
            {
                LeadCode.Fz,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Frontal,
                    Coordinate = new EEGCoordinate() { Beta = 45 }
                }
            },
            {
                LeadCode.F4,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Frontal,
                    Coordinate = new EEGCoordinate() { Alpha = -45, Beta = 35 }
                }
            },
            {
                LeadCode.F8,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Frontal,
                    Coordinate = new EEGCoordinate() { Alpha = -54 }
                }
            },

            {
                LeadCode.T3,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Temporal,
                    Coordinate = new EEGCoordinate() { Alpha = 90 }
                }
            },
            {
                LeadCode.C3,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Central,
                    Coordinate = new EEGCoordinate() { Alpha = 90, Beta = 45 }
                }
            },
            {
                LeadCode.Cz,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Central,
                    Coordinate = new EEGCoordinate() { Beta = 90 }
                }
            },
            {
                LeadCode.C4,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Central,
                    Coordinate = new EEGCoordinate() { Alpha = -90, Beta = 45 }
                }
            },
            {
                LeadCode.T4,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Temporal,
                    Coordinate = new EEGCoordinate() { Alpha = -90 }
                }
            },

            {
                LeadCode.T5,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Temporal,
                    Coordinate = new EEGCoordinate() { Alpha = 126 }
                }
            },
            {
                LeadCode.P3,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Parietal,
                    Coordinate = new EEGCoordinate() { Alpha = 135, Beta = 35 }
                }
            },
            {
                LeadCode.Pz,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Parietal,
                    Coordinate = new EEGCoordinate() { Alpha = 180, Beta = 45}
                }
            },
            {
                LeadCode.P4,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Parietal,
                    Coordinate = new EEGCoordinate() { Alpha = -135, Beta = 35 }
                }
            },
            {
                LeadCode.T6,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Temporal,
                    Coordinate = new EEGCoordinate() { Alpha = -126 }
                }
            },

            {
                LeadCode.O1,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Occipital,
                    Coordinate = new EEGCoordinate() { Alpha = 162 }
                }
            },
            {
                LeadCode.Oz,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Occipital,
                    Coordinate = new EEGCoordinate() { Alpha = 180 }
                }
            },
            {
                LeadCode.O2,
                new EEGSchemeLeadInfo()
                {
                    LeadType = LeadType.Occipital,
                    Coordinate = new EEGCoordinate() { Alpha = -162 }
                }
            },
        };
    }

    public class EEGLead : Lead
    {
        public LeadType LeadType { get; set; } = LeadType.Unknown;
    }
}
