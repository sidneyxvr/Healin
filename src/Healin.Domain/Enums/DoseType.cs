namespace Healin.Domain.Enums
{
    public enum DoseType : byte
    {
        First = 1,
        Second,
        Third,
        Reinforcement
    }

    public static class DoseTypeExtensions
    {
        public static string GetDescription(this DoseType doseType)
        {
            return doseType switch 
            { 
                DoseType.First          => "Primeira Dose",
                DoseType.Second         => "Segunda Dose",
                DoseType.Third          => "Terceira Dose",
                DoseType.Reinforcement  => "Reforço",
                _ => "Inválido"
            };
        }
    }
}
