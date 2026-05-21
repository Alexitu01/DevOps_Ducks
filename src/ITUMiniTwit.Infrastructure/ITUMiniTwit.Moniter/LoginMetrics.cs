using Prometheus;
namespace ITUMiniTwit.Infrastructure.ITUMiniTwit.Moniter;
#pragma warning disable format
public class LoginMetrics : ILoginMetrics
#pragma warning restore format
{
    private readonly Counter _counter;
    public LoginMetrics()
    {
        _counter = Metrics.CreateCounter(
            "identity_login_attempts_total",
            "Total login attempts",
            new CounterConfiguration
            {
                LabelNames = new[] { "result" }
            });
    }

    public void RecordSuccess() =>
        _counter.WithLabels("success").Inc();

    public void RecordFailure() =>
        _counter.WithLabels("failure").Inc();
}
