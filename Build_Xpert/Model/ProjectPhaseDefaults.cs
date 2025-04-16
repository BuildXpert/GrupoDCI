namespace Build_Xpert.Model
{
    public class ProjectPhaseDefaults
    {
        public static readonly List<string> PhaseNames = new List<string>
        {
            "Planificación",
            "Preparación de obra",
            "Cimentación y estructura",
            "Acabados",
            "Inspecciones de calidad"
        };


        public static List<ProjectPhase> GenerateDefaultPhases(int projectId)
        {
            return PhaseNames.Select(name => new ProjectPhase
            {
                PhaseName = name,
                ProjectId = projectId,
                CreatedAt = DateTime.Now
            }).ToList();
        }
    }
}
