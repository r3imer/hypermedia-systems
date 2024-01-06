using Reim.Htmx.Web;
using System.Text.Json;

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
    byte[] archive_file();
    //IArchiver get();
}

public class FakeCountArchiver(ContactsRepo db) : IArchiver {
    const double FINISHED = 20;
    int _count = 0;
    Status _status = Status.Waiting;

    public byte[] archive_file() {
        if (status() is Status.Complete) {
            var all = db.All();
            //var bytes = JsonSerializer.SerializeAsync()
            return all.ToJsonBytes();
        }
        return [];
    }

    public double progress() {
        if (_status is Status.Waiting) return 0;
        if (_status is Status.Complete) return 1;
        _count++;
        var r = (_count/FINISHED) switch {
            >1 => 1,
            <0 => 0,
            var x => x,
        };
        if (r == 1) { _status = Status.Complete; }
        return r;
    }

    public void reset() {
        _count = 0;
        _status = Status.Waiting;
    }

    public void run() {
        _count = 0;
        _status = Status.Running;
    }

    public Status status() => _status; 
}

public class FakeTimeArchiver(ContactsRepo db) : IArchiver {
    const double FINISHED = 14;
    DateTime? _started = null;

    public byte[] archive_file() {
        if (status() is Status.Complete) {
            var all = db.All();
            //var bytes = JsonSerializer.SerializeAsync()
            return all.ToJsonBytes();
        }
        return [];
    }

    public double progress() => _started switch {
        DateTime s => ((DateTime.UtcNow - s).TotalSeconds / FINISHED) switch {
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
