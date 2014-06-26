test('Ternary Operator True', function () {
    var actual = true ? 1 : 2;
    equal(actual, 1);
});

test('Ternary Operator False', function () {
    var actual = false ? 1 : 2;
    equal(actual, 2);
});

test('Null-Coalescing Operator IsNotNull', function () {
    var x = 1;
    var actual = x || 2;
    equal(actual, 1);
});

test('Null-Coalescing Operator IsNull', function () {
    var x = null;
    var actual = x || 2;
    equal(actual, 2);
});

test('Failure One', function () {
    var x = 1;
    x.plusOne();
});

test('Failure Two', function () {
    var x = 2;
    x.plusTwo();
});

test('Logging Success', function () {
    console.log('hello world');
    equal(true, true);
});

test('Logging Failure', function () {
    console.log('goodnight moon');
    equal(true, false);
});