namespace Gizo.Application.Options;

public class IdentityConfigs
{
    public TimeSpan CodeExpirationDurationTime { get; set; }
    public bool UseSampleCode { get; set; }
    
    public void Deconstruct(out TimeSpan codeExpirationDurationTime, out bool useSampleCode)
    {
        codeExpirationDurationTime = CodeExpirationDurationTime;
        useSampleCode = UseSampleCode;
    }
}