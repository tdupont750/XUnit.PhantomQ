(function(require, phantom) {

    var url,
        timeout,
        page,
        system = require('system'),
        args = system.args,
        logs = [];

    init();

    function init() {
        if (args.length < 3) {
            console.log('ArgumentException: phantomq.js phantomq.html timeout test.js dependency.js');
            phantom.exit(1);
        }
        
        url = args[1];
        timeout = parseInt(args[2], 10);
        page = require('webpage').create();

        page.onConsoleMessage = onConsoleMessage;
        page.onCallback = onCallback;

        page.open(url, onOpen);
    }
    
    function onConsoleMessage(message) {
        logs.push(message);
    }

    function onCallback(event, data) {
        if (event !== 'QUnit.done') {
            return;
        }

        var result = data.qunitResult,
            failed = !result || !result.total || result.failed;

        if (result.total) {
            data.logs = logs;
            var json = JSON.stringify(data);
            console.log(json);
        } else {
            console.error('Exception: No tests were executed');
        }

        phantom.exit(failed ? 1 : 0);
    }

    function onOpen(status) {
        if (status !== 'success') {
            console.error('Exception: Unable to access network: ' + status);
            phantom.exit(1);
        }
        
        var result = page.evaluate(evalStopQUnit);
        if (!result) {
            console.error('Exception: The `QUnit` object is not present on this page.');
            phantom.exit(1);
        }
        
        // Init PhantomQ
        page.evaluate(evalInitPhantomQ);

        // Load all of the tests into the page in reverse order.
        for (var i = args.length - 1; i >= 3; i--) {
            var arg = args[i];
            page.evaluate(evalAddScript, arg);
        }

        // Start QUnit
        page.evaluate(evalStartQUnit);

        // Set Timeout
        setTimeout(onTimeout, timeout);
    }

    function onTimeout() {
        console.error('TimeoutException: The specified timeout of ' + timeout + ' seconds has expired. Aborting...');
        phantom.exit(1);
    }

    function evalStopQUnit() {
        if (QUnit === 'undefined' || !QUnit) {
            return false;
        }
        QUnit.config.autostart = false;
        QUnit.stop();
        return true;
    }

    function evalAddScript(src) {
        (function(s) {
            var script = document.createElement("script");
            script.type = "text/javascript";
            script.src = s;
            document.body.appendChild(script);
        })(src);
    }

    function evalStartQUnit() {
        QUnit.start();
    }

    function evalInitPhantomQ() {
        (function (q) {

            var testResults = {},
                currentTest = [];

            initPhantomQ();
            
            function initPhantomQ() {
                q.config.autostart = false;

                QUnit.testDone(onTestDone);
                QUnit.done(onDone);
                QUnit.log(onLog);
            }

            function onLog(details) {
                // Ignore passing assertions
                if (details.result) {
                    return;
                }

                var response = details.message || '';

                if (typeof details.expected !== 'undefined') {
                    if (response) {
                        response += ', ';
                    }

                    response += 'expected: ' + details.expected + ', but was: ' + details.actual;
                }

                if (details.source) {
                    response += "\n" + details.source;
                }

                currentTest.push('Failed assertion: ' + response);
            }

            function onTestDone(result) {
                var name = result.module
                        ? result.module + ': ' + result.name
                        : result.name,
                    success = result.failed === 0,
                    message = currentTest.join('\n    ');

                testResults[name] = {
                    success: success,
                    message: message
                };
                
                currentTest.length = 0;
            }

            function onDone(result) {
                if (typeof window.callPhantom === 'function') {
                    window.callPhantom('QUnit.done', {
                        qunitResult: result,
                        testResults: testResults
                    });
                }
            }
            
        })(QUnit);
    }

})(require, phantom);