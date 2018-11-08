namespace SuperiorHackBase.Core.ProcessInteraction.Memory.Patterns
{
    public interface IPatternProcessor
    {
        ScanResult Process(IHackContext context, ScanResult result);
    }
}