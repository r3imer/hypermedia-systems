namespace Reim.Htmx.Archiver;

public enum Status {
    Waiting,
    Running,
    Complete
}

public interface IArchiver {
    Status status();
    double progress(); // [0,1]
    void run();
    void reset();
    string? archive_file();
    //IArchiver get();
}

public class FakeCountArchiver : IArchiver {
    const double FINISHED = 20;
    int? _count = null;

    public string? archive_file() => status() switch {
        Status.Complete => "", // TODO
        _ => null,
    };

    public double progress() => _count++ switch {
        int c => (c/FINISHED) switch {
            > 1 => 1,
            var x => x,
        },
        _ => 0
    };

    public void reset() {
        _count = null;
    }

    public void run() {
        _count = 0;
    }

    public Status status() => progress() switch {
        > 0 and < 1 => Status.Running,
        >= 1 => Status.Complete,
        _ => Status.Waiting,
    };
}

public class FakeTimeArchiver : IArchiver {
    DateTime? _started = null;

    public string? archive_file() => status() switch {
        Status.Complete => "", // TODO
        _ => null,
    };

    public double progress() => _started switch {
        DateTime s => ((DateTime.UtcNow - s).TotalSeconds / 15.0) switch {
            > 1 => 1,
            < 0 => 0,
            var x => x,
        },
        _ => 0,
    };

    public void reset() {
        _started = null;
    }

    public void run() {
        _started = DateTime.UtcNow;
    }

    public Status status() => progress() switch {
        > 0 and < 1 => Status.Running,
        >= 1 => Status.Complete,
        _ => Status.Waiting,
    };
}
