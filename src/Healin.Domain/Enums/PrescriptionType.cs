namespace Healin.Domain.Enums
{
    public enum PrescriptionType : byte
    {
        SimpleWhite = 1,
        SpecialWhite,
        Blue,
        Yellow
    }

    public static class PrescriptionTypeExtension 
    { 
        public static string GetDescription(this PrescriptionType prescriptionType)
        {
            return prescriptionType switch
            {
                PrescriptionType.SimpleWhite => "Branco Simples",
                PrescriptionType.SpecialWhite => "Branco Especial",
                PrescriptionType.Blue => "Azul",
                PrescriptionType.Yellow => "Amarelo",
                _ => "Inválido"
            };
        }
    }
}
