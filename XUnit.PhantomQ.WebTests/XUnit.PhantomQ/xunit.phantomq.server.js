var system = require('system'),
    page;

initialize();

function initialize() {
    if (system.args.length < 3) {
        console.log('ArgumentException: phantomq.js phantomq.html test.js dependency.js');
        phantom.exit(1);
        return;
    }

    page = require('webpage').create();
    page.onConsoleMessage = onConsoleMessage;
    page.open(system.args[1], onOpen);
}

function onConsoleMessage(msg) {
    console.log(msg);
}

function onOpen(status) {
    if (status !== "success") {
        console.log("Exception: Unable to access network");
        phantom.exit(1);
        return;
    }

    // Load all of the tests into the page in reverse order.
    for (var i = system.args.length - 1; i >= 2; i--) {
        var arg = system.args[i];
        page.evaluate(evaluateAddScript, arg);
    }

    var startResult = page.evaluate(evaluateStartPhantomQ);

    if (startResult == false) {
        console.log("Exception: Unable to start QUnit");
        phantom.exit(1);
        return;
    }

    waitFor(checkPhantomQComplete, onPhantomQComplete, 'checkPhantomQComplete');
}

function evaluateStartPhantomQ() {
    if (PhantomQ && typeof PhantomQ.start === 'function') {
        PhantomQ.start();
        return true;
    }
    return false;
}

function waitFor(doTest, onReady, msg, maxTimeout, timeout) {
    msg = msg || 'waitFor()';
    maxTimeout = maxTimeout || 3000;
    timeout = timeout || 100;

    var loopCount = 0,
        intervalId = setInterval(waitForInterval, timeout);

    function waitForInterval() {
        var result = doTest(),
            timeSpent = ++loopCount * timeout;

        if (result) {
            onReady();
            clearInterval(intervalId);
        } else if (timeSpent >= maxTimeout) {
            console.log('TimeoutException: ' + msg);
            phantom.exit(1);
        }
    }
}

function evaluateAddScript(src) {
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = src;
    document.body.appendChild(script);
}

function checkPhantomQComplete() {
    return page.evaluate(evaluateGetPhantomQComplete);
}

function evaluateGetPhantomQComplete() {
    return PhantomQ.isComplete;
}

function onPhantomQComplete() {
    var results = page.evaluate(evaluateGetPhantomQComplete);
    console.log(results);
    phantom.exit();
}

function evaluateGetPhantomQComplete() {
    return PhantomQ.results;
}