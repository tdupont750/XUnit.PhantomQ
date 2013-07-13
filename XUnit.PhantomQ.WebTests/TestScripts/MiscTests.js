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