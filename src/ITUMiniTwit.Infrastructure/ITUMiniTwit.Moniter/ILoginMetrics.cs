namespace ITUMiniTwit.Infrastructure.ITUMiniTwit.Moniter;

public interface ILoginMetrics
{
    void RecordSuccess();
    void RecordFailure();
}