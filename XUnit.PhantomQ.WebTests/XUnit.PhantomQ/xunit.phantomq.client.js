var PhantomQ = (function (q) {

    var self = {
        start: start,
        isStarted: false,
        isComplete: false,
        results: {}
    };

    initialize();

    return self;

    function initialize() {
        // Prevent AutoStart
        q.config.autostart = false;
        // Wireup Listeners
        q.testDone(onTestDone);
        q.done(onDone);
        q.log(onLog);
    }

    function start() {
        if (!self.isStarted) {
            self.isStarted = true;
            q.start();
        }
    }

    function onTestDone(result) {
        var success = result.failed === 0;
        setResult(result.name, 'success', success);
    }

    function onDone() {
        var json = JSON.stringify(self.results);
        setResults(json);
    }

    function onLog(log) {
        setResult(log.name, 'message', log.message);
    }

    function setResult(name, key, value) {
        var result = self.results[name] || {};
        result[key] = value;
        self.results[name] = result;
    }

    function setResults(results) {
        if (!self.isComplete) {
            self.results = results;
            self.isComplete = true;
        }
    }

})(QUnit);