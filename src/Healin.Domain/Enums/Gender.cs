namespace Healin.Domain.Enums
{
    public enum Gender : byte
    {
        Male = 1,
        Female,
        Other
    }

    public static class GenderExtensions
    {
        public static string GetDescription(this Gender gender)
        {
            return gender switch
            {
                Gender.Female => "Feminino",
                Gender.Male => "Masculino",
                Gender.Other => "Outro",
                _ => throw new System.NotImplementedException()
            };
        }
    }
}
