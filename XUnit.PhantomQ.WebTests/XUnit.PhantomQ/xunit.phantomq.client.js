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
    }
            
    function start() {
        if (!self.isStarted) {
            self.isStarted = true;
            q.start();
        }
    }
            
    function onTestDone(result) {
        var success = result.failed === 0;
        self.results[result.name] = success;
    }
            
    function onDone() {
        var json = JSON.stringify(self.results);
        setResults(json);
    }
            
    function setResults(results) {
        if (!self.isComplete) {
            self.isComplete = true;
            self.results = results;
        }
    }

})(QUnit);