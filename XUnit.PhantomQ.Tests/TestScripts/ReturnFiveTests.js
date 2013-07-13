test('ReturnFive Success', function() {
    var actual = ReturnFive();
    equal(actual, 5);
});

test('ReturnFive Fail', function () {
    var actual = ReturnFive();
    notEqual(actual, 6);
});